using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Threading;
using System.Data;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Cache
{

    /// <summary>
    /// Provides application-wide (that is, session-agnostic) cache for DataTable objects. The cache
    /// is keyed with fully parameterized DbCommand instances.
    /// </summary>
    public static class DataSourceCache
    {
        #region Serialization
        [XmlSchemaProvider("GenerateSchema")]
        internal sealed class CDataWrapper : IXmlSerializable
        {
            public static implicit operator string(CDataWrapper value)
            {
                return value == null ? null : value.Value;
            }

            public static implicit operator CDataWrapper(string value)
            {
                return value == null ? null : new CDataWrapper { Value = value };
            }

            public System.Xml.Schema.XmlSchema GetSchema()
            {
                return null;
            }

            // return "xs:string" as the type in scheme generation
            public static XmlQualifiedName GenerateSchema(XmlSchemaSet xs)
            {
                return XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String).QualifiedName;
            }

            // "" => <Node/>
            // "Foo" => <Node><![CDATA[Foo]]></Node>
            public void WriteXml(XmlWriter writer)
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    writer.WriteCData(Value);
                }
            }

            // <Node/> => ""
            // <Node></Node> => ""
            // <Node>Foo</Node> => "Foo"
            // <Node><![CDATA[Foo]]></Node> => "Foo"
            public void ReadXml(XmlReader reader)
            {
                if (reader.IsEmptyElement)
                {
                    Value = "";
                }
                else
                {
                    reader.Read();

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.EndElement:
                            Value = ""; // empty after all...
                            break;
                        case XmlNodeType.Text:
                        case XmlNodeType.CDATA:
                            Value = reader.ReadContentAsString();
                            break;
                        default:
                            throw new InvalidOperationException("Expected text/cdata");
                    }
                }
            }

            // underlying value
            public string Value { get; set; }
            public override string ToString()
            {
                return Value;
            }
        }

        [DataContract]
        internal sealed class SerializableCacheKey
        {
            public string Query { get; set; }

            [DataMember(Name = "Query", EmitDefaultValue = false)]
            public CDataWrapper QueryCData
            {
                get { return Query; }
                set { Query = value; }
            }

            [DataMember]
            public List<Tuple<string, object>> Params { get; set; }

            public SerializableCacheKey()
            {
                this.Params = new List<Tuple<string, object>>();
            }

            public static implicit operator CacheKey(SerializableCacheKey value)
            {
                return value == null ? null : new CacheKey(value.Query, value.Params.ToArray());
            }

            public static implicit operator SerializableCacheKey(CacheKey value)
            {
                return value == null ? null : new SerializableCacheKey() { Query = value.Query, Params = value.Params.ToList() };
            }
        }
        #endregion Serialization

        [Serializable]
        public sealed class CacheKey
        {
            private readonly string _query;
            private readonly Tuple<string, object>[] _params;
            private readonly string _uniqueID;

            public string Query { get { return _query; } }
            public Tuple<string, object>[] Params { get { return _params; } }
            public string UniqueID { get { return _uniqueID; } }

            public CacheKey(DbCommand command)
            {
                _query = command.CommandText;
                _params = command.Parameters.Cast<DbParameter>()
                    .Select(x => new Tuple<string, object>(x.ParameterName, x.Value)).ToArray();

                _uniqueID = ComputeUniqueID();
            }

            public CacheKey(string query, Tuple<string, object>[] parameters)
            {
                _query = query;
                _params = (parameters ?? new Tuple<string, object>[0]);

                _uniqueID = ComputeUniqueID();
            }

            private string ComputeUniqueID()
            {
                StringBuilder buffer = new StringBuilder();
                buffer.Append(_query ?? "<no query>").Append("\0");
                foreach (Tuple<string, object> parameter in _params)
                {
                    buffer.Append(parameter.Item1)
                        .Append("\n")
                        .Append(parameter.Item2)
                        .Append("\0");
                }
                using (MD5 md5 = MD5.Create())
                {
                    return string.Join(
                        "",
                        md5.ComputeHash(Encoding.UTF8.GetBytes(buffer.ToString()))
                            .Select(x => x.ToString("X2"))
                            .ToArray()
                        );
                }
            }

            public override int GetHashCode()
            {
                return (_query ?? "<empty query>").GetHashCode()
                    ^ _params.Aggregate<Tuple<string, object>, int>(0, (result, x) => result ^ (x.Item1 ?? "<no name>").GetHashCode() ^ (x.Item2 ?? "<no value>").GetHashCode());
            }

            public override bool Equals(object obj)
            {
                CacheKey other = obj as CacheKey;
                if (object.ReferenceEquals(other, null))
                    return base.Equals(obj);

                if (this._query != other._query)
                    return false;

                if (this._params.Length != other._params.Length)
                    return false;

                for (int index = 0; index < this._params.Length; index++)
                {
                    if (this._params[index].Item1 != other._params[index].Item1
                        || !object.Equals(this._params[index].Item2, other._params[index].Item2))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public sealed class CacheValue
        {
#if CACHE_USE_WEAK_REFERENCES
            private readonly WeakReference _reference;
#else
            private readonly DataTable _table;
#endif
            public DateTime LastAccess { get; private set; }

            public CacheValue(DataTable table, bool initializeLastAccess = true)
            {
                if (initializeLastAccess)
                    LastAccess = DateTime.Now;

#if CACHE_USE_WEAK_REFERENCES
                _reference = new WeakReference(table);
#else
                _table = table;
#endif
            }

            public DataTable GetValue(bool updateLastAccess = true)
            {
                if (updateLastAccess)
                    LastAccess = DateTime.Now;

#if CACHE_USE_WEAK_REFERENCES
                object target = _reference.Target;
                return target as DataTable;
#else
                return _table;
#endif
            }
        }

        private static readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private static readonly Dictionary<CacheKey, CacheValue> _cache = new Dictionary<CacheKey, CacheValue>();
        private static readonly List<CacheKey> _cacheableKeys;

        /// <summary>
        /// Load the cacheable keys collection from the disk file (if exists)
        /// </summary>
        static DataSourceCache()
        {
            List<SerializableCacheKey> cacheableKeys = new List<SerializableCacheKey>();
            try
            {
                string fileName = ConfigurationManager.AppSettings["DataSourceCacheState"];
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(List<SerializableCacheKey>));
                    using (FileStream stream = File.OpenRead(fileName))
                    {
                        cacheableKeys = (List<SerializableCacheKey>)serializer.ReadObject(stream);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                _cacheableKeys = cacheableKeys.Select(x => (CacheKey)x).ToList();
            }
        }

        /// <summary>
        /// Get the current contents of the cache, complete with cache keys (queries + parameters)
        /// and cached data.
        /// </summary>
        public static KeyValuePair<CacheKey, CacheValue>[] Entries
        {
            get
            {
                _cacheLock.EnterReadLock();
                try
                {
                    return _cache.ToArray();
                }
                finally
                {
                    _cacheLock.ExitReadLock();
                }
            }
        }

        public static void RemoveCacheEntries(Func<CacheKey, bool> filter)
        {
            RemoveCacheEntries(Entries.Select(x => x.Key).Where(x => filter(x)));
        }

        public static void RemoveCacheEntries(IEnumerable<CacheKey> entries)
        {
            CacheKey[] toRemove = entries.ToArray();

            _cacheLock.EnterWriteLock();
            try
            {
                foreach (CacheKey key in toRemove)
                    _cache.Remove(key);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        public static void RemoveCacheEntry(CacheKey key)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _cache.Remove(key);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Get the collection of queries scheduled for bulk pre-loading.
        /// </summary>
        public static CacheKey[] CacheableKeys
        {
            get
            {
                lock (_cacheableKeys)
                {
                    return _cacheableKeys.ToArray();
                }
            }
        }

        /// <summary>
        /// Reset the contents of the cache. This operation does not affect the collection of
        /// queries scheduled for bulk pre-loading.
        /// </summary>
        public static void Clear()
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _cache.Clear();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Reset the collection of queries scheduled for bulk pre-loading. Does not affect the
        /// in-memory cache.
        /// </summary>
        public static void Flush()
        {
            lock (_cacheableKeys)
            {
                _cacheableKeys.Clear();
                WriteCacheableKeyCollection();
            }
        }

        /// <summary>
        /// Remove the specified entry from the collection of queries scheduled for bulk pre-loading.
        /// This operation also removes the specified entry from the in-memory cache (if applicable).
        /// </summary>
        public static void Forget(CacheKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            RemoveCacheableKey(key);

            _cacheLock.EnterWriteLock();
            try
            {
                _cache.Remove(key);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        public static bool TryGet(DbCommand command, out DataTable data)
        {
            _cacheLock.EnterReadLock();
            try
            {
                CacheValue value;
                if (_cache.TryGetValue(new CacheKey(command), out value)
                    && (data = value.GetValue()) != null)
                {
                    return true;
                }
                data = null;
                return false;
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public static void Put(DbCommand command, DataTable data, bool initializeLastAccess = true)
        {
            CacheKey key = new CacheKey(command);

            _cacheLock.EnterWriteLock();
            try
            {
                _cache[key] = new CacheValue(data, initializeLastAccess);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }

            AddCacheableKey(key);
        }

        private static void AddCacheableKey(CacheKey key)
        {
            lock (_cacheableKeys)
            {
                if (_cacheableKeys.Contains(key))
                    return;

                _cacheableKeys.Add(key);
                WriteCacheableKeyCollection();
            }
        }

        private static void RemoveCacheableKey(CacheKey key)
        {
            lock (_cacheableKeys)
            {
                int index = _cacheableKeys.IndexOf(key);
                if (index < 0)
                    return;

                _cacheableKeys.RemoveAt(index);
                WriteCacheableKeyCollection();
            }
        }

        /// <summary>
        /// ATTENTION: the calling code must guarantee that the collection won't change
        /// while the operation is inprogress by obtaining and holding appropriate locks.
        /// </summary>
        private static void WriteCacheableKeyCollection()
        {
            string fileName = ConfigurationManager.AppSettings["DataSourceCacheState"];
            if (string.IsNullOrEmpty(fileName))
                return;

            List<SerializableCacheKey> cacheableKeys = _cacheableKeys.Select(x => (SerializableCacheKey)x).ToList();

            DataContractSerializer serializer = new DataContractSerializer(typeof(List<SerializableCacheKey>));
            using (XmlWriter writer = XmlWriter.Create(fileName, new XmlWriterSettings() { Indent = true }))
            {
                serializer.WriteObject(writer, cacheableKeys);
            }
        }
    }

}
