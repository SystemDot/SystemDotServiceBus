using System.Collections.Concurrent;

namespace SystemDot.Messaging.Storage
{
    public static class ConcurrentDictionaryExtensions
    {
        public static void Remove<TKey, TItem>(this ConcurrentDictionary<TKey, TItem> dictionary, TKey id)
        {
            TItem temp;
            dictionary.TryRemove(id, out temp);
        }

        public static void AddOrUpdate<TKey, TItem>(this ConcurrentDictionary<TKey, TItem> dictionary, TKey id, TItem item)
        {
            dictionary.AddOrUpdate(id, item, (_, __) => item);
        }
        
        public static void UpdateIfExists<TKey, TItem>(this ConcurrentDictionary<TKey, TItem> dictionary, TKey id, TItem item)
        {
            TItem temp;
            if (dictionary.TryGetValue(id, out temp)) dictionary[id] = item;
        }
    }
}