using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace FT.Tools.CSV
{
    /// Read CSV-formatted data from a file or TextReader
    public class CSVReader : IDisposable
    {
        private const string LF = "\n";
        private string currentLine = "";
        
        /// This reader will read all of the CSV data
        private readonly BinaryReader reader;

        private CSVReader(byte[] csvData)
        {
            if (csvData == null)
                throw new ArgumentNullException($"Null byte[] passed to CSVReader");
            reader = new BinaryReader(new MemoryStream(csvData));
        }
        
        /// Read the next row from the CSV data
        /// <returns>A list of objects read from the row, or null if there is no next row</returns>
        private List<object> ReadRow()
        {
            // ReadLine() will return null if there's no next line
            if (reader.BaseStream.Position >= reader.BaseStream.Length)
                return null;

            StringBuilder builder = new();

            // Read the next line
            while (reader.BaseStream.Position < reader.BaseStream.Length && (!builder.ToString().EndsWith(LF)))
            {
                char c = reader.ReadChar();
                builder.Append(c);
            }

            currentLine = builder.ToString().Trim();

            // Build the list of objects in the line
            List<object> objects = new();
            while (currentLine != "")
                objects.Add(ReadNextObject());
            return objects;
        }
        
        /// Read the next object from the currentLine string
        /// <returns>The next object in the currentLine string</returns>
        private object ReadNextObject()
        {
            if (currentLine == null)
                return null;

            // Check to see if the next value is quoted
            bool quoted = currentLine.StartsWith("\"");

            // Find the end of the next value
            int i = 0;
            int len = currentLine.Length;
            bool foundEnd = false;
            while (!foundEnd && i <= len)
            {
                // Check if we've hit the end of the string
                if ((!quoted && i == len)
                    || (!quoted && currentLine.Substring(i, 1) == ",")
                    || (quoted && i == len - 1 && currentLine.EndsWith("\""))
                    || (quoted && currentLine.Substring(i, 2) == "\","))
                    foundEnd = true;
                else
                    i++;
            }
            if (quoted)
            {
                if (i > len || !currentLine.Substring(i, 1).StartsWith("\""))
                    throw new FormatException("Invalid CSV format: " + currentLine[..i]);
                i++;
            }
            string nextObjectString = currentLine[..i].Replace("\"\"", "\"");

            currentLine = i < len ? currentLine[(i + 1)..] : "";

            if (!quoted) 
                return nextObjectString;
            
            if (nextObjectString.StartsWith("\""))
                nextObjectString = nextObjectString[1..];
            if (nextObjectString.EndsWith("\""))
                nextObjectString = nextObjectString[..^1];
            return nextObjectString;
        }
        
        /// Read the row data read using repeated ReadRow() calls and build a DataColumnCollection with types and column names
        /// <param name="headerRow">True if the first row contains headers</param>
        /// <returns>System.Data.DataTable object populated with the row data</returns>
        private DataTable CreateDataTable(bool headerRow)
        {
            // Read the CSV data into rows
            List<List<object>> rows = new();
            while (ReadRow() is { } readRow)
                rows.Add(readRow);

            // The types and names (if headerRow is true) will be stored in these lists
            List<Type> columnTypes = new();
            List<string> columnNames = new();

            // Read the column names from the header row (if there is one)
            if (headerRow) columnNames.AddRange(rows[0].Select(name => name.ToString()));

            // Read the column types from each row in the list of rows
            bool headerRead = false;
            foreach (List<object> row in rows)
                if (headerRead || !headerRow)
                    for (int i = 0; i < row.Count; i++)
                        // If we're adding a new column to the columnTypes list, use its type.
                        // Otherwise, find the common type between the one that's there and the new row.
                        if (columnTypes.Count < i + 1)
                            columnTypes.Add(row[i].GetType());
                        else
                            columnTypes[i] = StringConverter.FindCommonType(columnTypes[i], row[i].GetType());
                else
                    headerRead = true;

            // Create the table and add the columns
            DataTable table = new();
            for (int i = 0; i < columnTypes.Count; i++)
            {
                table.Columns.Add();
                table.Columns[i].DataType = columnTypes[i];
                if (i < columnNames.Count)
                    table.Columns[i].ColumnName = columnNames[i];
            }

            // Add the data from the rows
            headerRead = false;
            foreach (List<object> row in rows)
                if (headerRead || !headerRow)
                {
                    DataRow dataRow = table.NewRow();
                    for (int i = 0; i < row.Count; i++)
                        dataRow[i] = row[i];
                    table.Rows.Add(dataRow);
                }
                else
                    headerRead = true;

            return table;
        }

        public static DataTable ReadCSVData(byte[] data, bool headerRow)
        {
            using CSVReader reader = new(data);
            return reader.CreateDataTable(headerRow);
        }
        

        #region IDisposable Members

        public void Dispose()
        {
            if (reader == null) return;
            try
            {
                reader.Close();
            }
            catch
            {
                // ignored
            }
        }

        #endregion
    }
}