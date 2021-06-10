using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaUtils
{
    public class GenericCache<TKey, TValue>
    {
        private int maxItems;
        private List<TKey> keyQueue = new List<TKey>();
        private Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        public GenericCache(int maxItems)
        {
            this.maxItems = maxItems;
        }

        public TValue Get(TKey key)
        {
            Refresh(key);

            if (dict.ContainsKey(key))
                return dict[key];

            return default(TValue);
        }
        
        public void Set(TKey key, TValue value)
        {
            dict[key] = value;
            //keyQueue.Enqueue(key);
            keyQueue.Insert(0, key);
            Resize();
        }

        private void Resize()
        {
            while (keyQueue.Count > maxItems)
            {
                //dict.Remove(keyQueue.Dequeue());
                dict.Remove(keyQueue.Last());
                keyQueue.RemoveAt(keyQueue.Count - 1);
            }
        }

        private void Refresh(TKey key)
        {
            var index = keyQueue.IndexOf(key);

            if (index >= 0)
            {
                keyQueue.RemoveAt(index);
                keyQueue.Insert(0, key);
            }
        }

    }
}