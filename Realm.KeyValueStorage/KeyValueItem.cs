using System;

namespace Realms.KeyValueStorage
{
    public class KeyValueItem : RealmObject, IComparable, IComparable<KeyValueItem>
    {
        [PrimaryKey]
        public string Key { get; set; }

        [Indexed]
        public object Value { get; set; }

        public DateTimeOffset? ExpiresOn { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueItem"/> class.
        /// </summary>
        public KeyValueItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueItem"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expiresOn">The expire date after which this key is no longer valid.</param>
        public KeyValueItem(string key, object value, DateTimeOffset? expiresOn = null)
        {
            Key = key;
            Value = value;
            ExpiresOn = expiresOn;
        }

        #endregion

        #region Equality & Comparison Methods

        public int CompareTo(KeyValueItem other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Key, other.Key, StringComparison.Ordinal);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is KeyValueItem)) throw new ArgumentException($"Object must be of type {nameof(KeyValueItem)}");
            return CompareTo((KeyValueItem) obj);
        }

        protected bool Equals(KeyValueItem other)
        {
            return base.Equals(other) && string.Equals(Key, other.Key) && Equals(Value, other.Value) && ExpiresOn.Equals(other.ExpiresOn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((KeyValueItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ExpiresOn.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{Key}={Value}";
        }

        #endregion
    }
}
