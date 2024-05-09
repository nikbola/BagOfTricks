using System.Collections.Generic;

namespace BagOfTricks.Extensions 
{
    public static class DictionaryExtensions 
    {
        public static bool UpdateKey<T1, T2>(this Dictionary<T1, T2> dict, T1 oldKey, T1 newKey) 
        {
            if (dict.ContainsKey(newKey))
                return false;

            if (!dict.TryGetValue(oldKey, out T2 value))
                return false;

            dict.Remove(oldKey);
            dict.Add(newKey, value);
            return true;
        }
    }
}