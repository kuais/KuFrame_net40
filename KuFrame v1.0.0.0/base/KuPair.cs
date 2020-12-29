using System.Collections.Generic;

namespace Ku
{
    public class KuPair<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }
        public KuPair(K key, V value)
        {
            this.Key = key;
            this.Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            var newObj = obj as KuPair<K, V>;
            if (newObj == null) return false;
            if (newObj.Key.Equals(Key) && newObj.Value.Equals(Value))
                return true;
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            var hashCode = 206514262;
            hashCode = hashCode * -1521134295 + EqualityComparer<K>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<V>.Default.GetHashCode(Value);
            return hashCode;
        }
    }
}
