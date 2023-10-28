using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FT.Tools.Extensions
{
    public static class ExtensionMethods
    {
        public static T Parse<T>(this DataRow self, string name)
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
            return (T)typeConverter.ConvertFromInvariantString(self[name].ToString());
        }

        public static void AddIfNotNull<T>(this List<T> self, T value)
        {
            if (value == null)
                return;
            self.Add(value);
        }

        public static void RaycastHits(this List<RaycastResult> self, Vector2 mousePosition) => 
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) {position = mousePosition}, self);
    }
}