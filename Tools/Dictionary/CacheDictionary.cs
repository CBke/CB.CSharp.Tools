using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tools.Dictionary
{
    public class CacheDictionary<TKey, Tvalue> : IDictionary<TKey, Tvalue>
    {
        private ConcurrentDictionary<TKey, Tvalue> IntDict { get; set; } = new ConcurrentDictionary<TKey, Tvalue>();
        public Func<TKey, Tvalue> Calulate { get; set; }

        public CacheDictionary(Func<TKey, Tvalue> Calulate)
        {
            this.Calulate = Calulate;
        }

        public Tvalue this[TKey key]
        {
            get
            {
                if (!IntDict.ContainsKey(key))
                    IntDict[key] = Calulate(key);

                return IntDict[key];
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<Tvalue> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<TKey, Tvalue> item)
        {
            throw new NotImplementedException();
        }

        public void Add(TKey key, Tvalue value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            IntDict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, Tvalue> item) => true;

        public bool ContainsKey(TKey key) => true;

        public void CopyTo(KeyValuePair<TKey, Tvalue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, Tvalue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, Tvalue> item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out Tvalue value)
        {
            value = IntDict[key];
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}