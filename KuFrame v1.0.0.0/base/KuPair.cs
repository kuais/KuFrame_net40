using System.Collections.Generic;

namespace Ku
{
    public class KuPair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public KuPair(TKey key, TValue value)
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
            if (obj == null) return false;
            var newObj = obj as KuPair<TKey, TValue>;
            if (newObj == null) return false;
            if ((newObj.Key.Equals(this.Key)) && (newObj.Value.Equals(this.Value)))
            {
                return true;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            var hashCode = 206514262;
            hashCode = hashCode * -1521134295 + EqualityComparer<TKey>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<TValue>.Default.GetHashCode(Value);
            return hashCode;
        }
    }
}
