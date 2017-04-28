using System;

namespace Realms.KeyValueStorage
{
    /// <summary>
    /// A Realm instance (also referred to as a Realm) represents a Realm database wrapper making it easier to store key/value pairs.
    /// </summary>
    /// <seealso cref="IDisposable" />
    /// <remarks>
    /// <b>Warning</b>: Realm instances are not thread safe and can not be shared across threads.
    /// You must call <see cref="M:Realms.Realm.GetInstance(Realms.RealmConfigurationBase)" /> on each thread in which you want to interact with the Realm.
    /// </remarks>
    public class KeyValueRealm : IDisposable
    {
        #region Fields & Properties

        private readonly Realm _realm;
        private bool _disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueRealm"/> class.
        /// </summary>
        public KeyValueRealm()
        {
            _realm = Realm.GetInstance();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueRealm"/> class.
        /// </summary>
        /// <param name="realm">The realm.</param>
        public KeyValueRealm(Realm realm)
        {
            _realm = realm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueRealm"/> class.
        /// </summary>
        /// <param name="databasePath">The database path.</param>
        public KeyValueRealm(string databasePath)
        {
            _realm = Realm.GetInstance(databasePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueRealm"/> class.
        /// </summary>
        /// <param name="realmConfiguration">The realm configuration.</param>
        public KeyValueRealm(RealmConfigurationBase realmConfiguration)
        {
            _realm = Realm.GetInstance(realmConfiguration);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Factory for obtaining a <see cref="T:Realms.Realm" /> instance for this thread.
        /// </summary>
        /// <returns>
        /// A <see cref="T:Realms.Realm" /> instance.
        /// </returns>
        /// <exception cref="T:Realms.Exceptions.RealmFileAccessErrorException">Thrown if the file system returns an error preventing file creation.</exception>
        public static KeyValueRealm GetInstance()
        {
            return new KeyValueRealm();
        }

        /// <summary>
        /// Factory for obtaining a <see cref="T:Realms.Realm" /> instance for this thread.
        /// </summary>
        /// <param name="databasePath">Path to the realm, must be a valid full path for the current platform, relative subdirectory, or just filename.</param>
        /// <returns>
        /// A <see cref="T:Realms.Realm" /> instance.
        /// </returns>
        /// <exception cref="T:Realms.Exceptions.RealmFileAccessErrorException">Thrown if the file system returns an error preventing file creation.</exception>
        /// <remarks>
        /// If you specify a relative path, sandboxing by the OS may cause failure if you specify anything other than a subdirectory.
        /// </remarks>
        public static KeyValueRealm GetInstance(string databasePath)
        {
            return new KeyValueRealm(databasePath);
        }

        /// <summary>
        /// Factory for obtaining a <see cref="T:Realms.Realm" /> instance for this thread.
        /// </summary>
        /// <param name="realmConfiguration">The realm configuration.</param>
        /// <returns>
        /// A <see cref="T:Realms.Realm" /> instance.
        /// </returns>
        /// <exception cref="T:Realms.Exceptions.RealmFileAccessErrorException">Thrown if the file system returns an error preventing file creation.</exception>
        public static KeyValueRealm GetInstance(RealmConfigurationBase realmConfiguration)
        {
            return new KeyValueRealm(realmConfiguration);
        }

        /// <summary>
        /// Retrieves a <code>KeyValueItem</code> based on the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>KeyValueItem if it exists or null otherwise. 
        /// If an expiration was configured and is past the current time, it will also return null.</returns>
        public virtual KeyValueItem Get(string key)
        {
            var keyValueItem = _realm.Find<KeyValueItem>(key);

            if (keyValueItem?.ExpiresOn != null && keyValueItem.ExpiresOn.Value > DateTimeOffset.Now)
            {
                Remove(keyValueItem);
                return null;
            }

            return keyValueItem;
        }

        /// <summary>
        /// Retrieves a strongly typed value corresponding to a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value corresponding to the key if it exists or null otherwise. 
        /// If an expiration was configured and is past the current time, it will also return null.</returns>
        public virtual T Get<T>(string key)
        {
            return (T)Convert.ChangeType(Get(key).Value, typeof(T));
        }

        /// <summary>
        /// Persists the specified <code>KeyValueItem</code>, updating it if the key already exists.
        /// </summary>
        /// <param name="keyValueItem">The key value item.</param>
        public virtual void Set(KeyValueItem keyValueItem)
        {
            _realm.Write(() => _realm.Add(keyValueItem, true));
        }

        /// <summary>
        /// Persists the specified key, value and expiration, updating it if the key already exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiresOn">The expire date after which this key is no longer valid.</param>
        public virtual void Set(string key, object value, DateTimeOffset? expiresOn = null)
        {
            var keyValueItem = new KeyValueItem(key, value, expiresOn);
            Set(keyValueItem);
        }

        /// <summary>
        /// Removes the specified key value item.
        /// </summary>
        /// <param name="keyValueItem">The key value item to remove.</param>
        public virtual void Remove(KeyValueItem keyValueItem)
        {
            _realm.Write(() => Remove(keyValueItem));
        }

        /// <summary>
        /// Removes the specified key value item.
        /// </summary>
        /// <param name="key">The key.</param>
        public virtual void Remove(string key)
        {
            var item = Get(key);

            if (item != null)
            {
                Remove(item);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _realm.Dispose();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
