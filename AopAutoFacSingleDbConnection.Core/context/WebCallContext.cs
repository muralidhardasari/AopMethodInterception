using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace AopMethodInterception.Core
{
    public static class WebCallContext
    {
        private const string DictName = "dict";

        public static object GetData(string name)
        {
            HttpContext httpContext = HttpContext.Current;

            if (httpContext != null)
            {
                return httpContext.Items[name];
            }

            var dict = (Dictionary<string, object>)CallContext.GetData(DictName);
            if (dict == null)
            {
                return null;
            }

            lock (dict)
            {
                object o;
                if (dict.TryGetValue(name, out o))
                {
                    return o;
                }
            }

            return null;
        }

        public static void SetData(string name, object value)
        {
           HttpContext httpContext = HttpContext.Current;

            if (httpContext != null)
            {
                httpContext.Items[name] = value;
            }
            else
            {
                var dict = (Dictionary<string, object>)CallContext.GetData(DictName);
                if (dict == null)
                {
                    dict = new Dictionary<string, object>();
                    CallContext.SetData(DictName, dict);
                }

                lock (dict)
                {
                    dict[name] = value;
                }
            }
        }

        /// <summary>
        /// Remove an item from the call context
        /// </summary>
        /// <param name="name">key of the item that must be removed</param>
        /// <param name="dispose">true (default) disposes an item if it has the IDisposable interface</param>
        /// <remarks>Set dispose to false when the item is this and calling from the Dispose method</remarks>
        public static void FreeNamedDataSlot(string name, bool dispose = true)
        {
           HttpContext httpContext = HttpContext.Current;

            object itemToFree = GetData(name);
            if (itemToFree != null)
            {
               if (httpContext != null)
                {
                    httpContext.Items.Remove(name);
                }
                else
                {
                    var skyDict = (Dictionary<string, object>)CallContext.GetData(DictName);
                    lock (skyDict)
                    {
                        skyDict.Remove(name);
                    }
                }
            }

            // Dispose disposable items.
            if (dispose)
            {
                var disposableItem = itemToFree as IDisposable;
                if (disposableItem != null)
                {
                    disposableItem.Dispose();
                }
            }
        }

        public static void FreeNamedDataSlot()
        {
            HttpContext httpContext = HttpContext.Current;

            // Removing keys while iterating over a dictionary throws an exception, so first put all keys in a list
            List<string> keys;
            if (httpContext != null)
            {
                keys = httpContext.Items.Keys.Cast<string>().ToList();
            }
            else
            {
                var skyDict = (Dictionary<string, object>)CallContext.GetData(DictName);
                lock (skyDict)
                {
                    keys = skyDict.Keys.ToList();
                }
            }

            foreach (var key in keys)
            {
                FreeNamedDataSlot(key);
            }
        }
    }
}
