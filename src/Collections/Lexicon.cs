using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BagOfTricks.Collections 
{
    public class Lexicon<Key, Value> : IDictionary<Key, List<Value>>
    {
        private readonly Dictionary<Key, List<Value>> _dictionary = new();

        public List<Value> this[Key key] { get => _dictionary[key]; set => _dictionary[key] = value; }

        public ICollection<Key> Keys => _dictionary.Keys;

        public ICollection<List<Value>> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public void Add(Key key, List<Value> value)
        {
            _dictionary.Add(key, value);
        }

        public void Add(KeyValuePair<Key, List<Value>> item)
        {
            _dictionary.Add(item.Key, item.Value);
        }

        public void Add(Key key, Value value)
        {
            if (!_dictionary.TryGetValue(key, out List<Value> list))
            {
                list.Add(value);
            }
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<Key, List<Value>> item)
        {
            return _dictionary.Contains(item);
        }

        public bool ContainsKey(Key key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<Key, List<Value>>[] array, int arrayIndex)
        {
            
        }

        public IEnumerator<KeyValuePair<Key, List<Value>>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool Remove(Key key)
        {
            return _dictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<Key, List<Value>> item)
        {
            return _dictionary.Remove(item.Key);
        }

        public bool TryGetValue(Key key, out List<Value> value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }
    }
}