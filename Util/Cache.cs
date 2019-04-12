using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Util
{
    public class Cache
    {
        private static System.Web.Caching.Cache cache = HttpRuntime.Cache;

        public static T GetCache<T>(string key) where T : class
        {

            if (cache.Get(key) != null)
                return (T)cache[key];

            return null;
        }

        public static void WriteCache<T>(string key, T value) where T : class
        {
            cache.Insert(key, value);
        }

        public static void WriteCache<T>(string key, T value, DateTime expireTime) where T : class
        {
            cache.Insert(key, value, null, expireTime, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public static void RemoveCache(string key)
        {
            cache.Remove(key);
        }

        public static void RemoveCache()
        {
            IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            while (CacheEnum.MoveNext())
                cache.Remove(CacheEnum.Key.ToString());
        }

    }
}
