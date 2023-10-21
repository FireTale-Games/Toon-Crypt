using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FT.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Editor.Items
{
    public class DatasheetLoaderEditor
    {
        private readonly List<IEnumerator> _downloadRequests = new();
        private readonly ItemDatabase _itemDatabase;
        private List<(DataTable, Type)> _loadedTables;

        public DatasheetLoaderEditor(ItemDatabase itemDatabase) =>
            _itemDatabase = itemDatabase;

        public void DownloadMultiple(List<(string, Action<byte[]>)> requests,  List<(DataTable, Type)> loadedTables)
        {
            _loadedTables = loadedTables;
            
            EditorApplication.update -= ProcessDownloads;
            EditorApplication.update += ProcessDownloads;

            foreach ((string uri, Action<byte[]> onData) in requests) 
                _downloadRequests.Add(GetRequest(uri, onData));
        }

        public void DownloadSingle((string, Action<byte[]>) request, List<(DataTable, Type)> loadedTables)
        {
            _loadedTables = loadedTables;
            
            EditorApplication.update -= ProcessDownloads;
            EditorApplication.update += ProcessDownloads;
            
            _downloadRequests.Add(GetRequest(request.Item1, request.Item2));
        }
        
        private void ProcessDownloads()
        {
            _downloadRequests.RemoveAll(request => !request.MoveNext());
            if (_downloadRequests.Any()) 
                return;
            
            EditorApplication.update -= ProcessDownloads;
        }

        private IEnumerator GetRequest(string uri, Action<byte[]> onData)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            Debug.Log("Web request " + uri);
            webRequest.SendWebRequest();

            while (!webRequest.isDone)
                yield return null;
            
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("CSV download: Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("CSV download: HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("CSV download:\nReceived: " + webRequest.downloadHandler.text);
                    onData.Invoke(webRequest.downloadHandler.data);
                    if (_itemDatabase != null)
                        LoadTables();
                    break;
            }
        }

        private void LoadTables()
        {
            foreach ((DataTable table, Type type) in _loadedTables)
            {
                Debug.Log($"Loading table {table.TableName}");
                _itemDatabase.Load(table, type);
            }
            
            EditorUtility.SetDirty(_itemDatabase);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}