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
    }
}
