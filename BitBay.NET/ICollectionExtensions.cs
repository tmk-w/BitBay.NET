using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace BitBay.NET
{
    public static class ICollectionExtensions
    {
        public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var kvp in dict)
            {
                if (kvp.Value != null)
                {
                    nameValueCollection.Add(kvp.Key.ToString(), kvp.Value.ToString());
                }
            }

            return nameValueCollection;
        }

        public static string ToQueryString(this NameValueCollection nvcCollection)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            foreach (string s in nvcCollection)
            {
                queryString.Add(s, nvcCollection[s]);
            }

            return queryString.ToString();
        }
    }
}
