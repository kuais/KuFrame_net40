using Microsoft.Win32.SafeHandles;
using System;

namespace Ku
{
    public abstract class KuSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public KuSafeHandle(IntPtr handle) : base(true)
        {
            SetHandle(handle);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            return ((KuSafeHandle)obj).handle.Equals(handle);
        }

        public override int GetHashCode()
        {
            return handle.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, handle);
        }

        public static bool operator ==(KuSafeHandle value1, KuSafeHandle value2)
        {
            if (Equals(value1, null) && Equals(value2, null)) return true;
            if (Equals(value1, null) || Equals(value2, null)) return false;
            return value1.handle == value2.handle;
        }

        public static bool operator !=(KuSafeHandle value1, KuSafeHandle value2)
        {
            if (Equals(value1, null) && Equals(value2, null)) return false;
            if (Equals(value1, null) || Equals(value2, null)) return true;
            return value1.handle != value2.handle;
        }
    }
}
