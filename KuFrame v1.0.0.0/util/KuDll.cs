using System;
using System.Reflection;

namespace Ku.util
{
    /// <summary>
    /// 动态加载DLL
    /// </summary>
    public class KuDll
    {
        #region 声明动态载入DLL的参数
        Assembly _assembly;
        object _object;                                  //要调用的类的实例
        Type _type;                                   //要调用的类的类型
        #endregion

        #region 属性
        public Assembly CurrentAssembly { get => _assembly; }
        public Type CurrentClass { get => _type; }
        public object CurrentInstance { get => _object; }
        #endregion

        public KuDll(){}

        /// <summary>
        /// 加载dll文件
        /// </summary>
        /// <param name="path">dll文件路径</param>
        public KuDll Load(string path)
        {
            _assembly = Assembly.LoadFrom(path) ?? throw new KuDllException(DLLError.AssemblyInvalid);
            return this;
        }

        /// <summary>
        /// 使用指定的类名称，获取类
        /// </summary>
        /// <param name="className">类名,含命名空间</param>
        /// <returns>实例化的类对象</returns>
        public KuDll GetClass(string className)
        {
            if (_assembly == null) throw new KuDllException(DLLError.AssemblyInvalid);
            //获取该类的类型     
            _type = _assembly.GetType(className) ?? throw new KuDllException(DLLError.ClassNotFound);
            return this;
        }

        /// <summary>
        /// 获取对象实例
        /// </summary>
        /// <param name="args">构造函数所需的参数</param>
        /// <returns>实例化的类对象</returns>
        public object NewInstance(params object[] args)
        {
            if (_type == null) throw new KuDllException(DLLError.ClassNotFound);
            return _object = Activator.CreateInstance(_type, args) ?? throw new KuDllException(DLLError.InstanceInvalid); ;
        }
        /// <summary>
        /// 获取对象实例
        /// </summary>
        /// <param name="className">指定类名</param>
        /// <param name="args">构造函数所需的参数</param>
        /// <returns></returns>
        public object NewInstanceWithClass(string className, params object[] args)
        {
            GetClass(className);
            if (_type == null) throw new KuDllException(DLLError.ClassNotFound);
            return _object = Activator.CreateInstance(_type, args) ?? throw new KuDllException(DLLError.InstanceInvalid); ;
        }

        /// <summary>
        /// 调用dll里的方法
        /// </summary>
        /// <param name="MethodName">方法名称</param>
        /// <param name="args">参数</param>
        /// <returns>函数执行结果</returns>
        public object CallMethod(string MethodName, params Object[] args)
        {
            if (_type == null) throw new KuDllException(DLLError.ClassNotFound);
            object o = _type.InvokeMember(MethodName, BindingFlags.InvokeMethod, null, _object, args);
            return o;
        }
    }

    public enum DLLError
    {
        Unknow = -1,
        AssemblyInvalid = -2,
        ClassNotFound = -3,
        InstanceInvalid = -4,
    }

    [Serializable]
    public class KuDllException : Exception
    {
        public DLLError Error { get; private set; } = DLLError.Unknow;
        public KuDllException(string message) : base(message) { }
        public KuDllException(DLLError e) : this(GetErrorString(e))
        {
            this.HResult = (int)e;
            this.Error = e;
        }

        private static string GetErrorString(DLLError e)
        {
            switch (e)
            {
                case DLLError.AssemblyInvalid:
                    return "DLL加载失败";
                case DLLError.ClassNotFound:
                    return "未找到类";
                case DLLError.InstanceInvalid:
                    return "创建对象失败";
                case DLLError.Unknow:
                default:
                    return "未知错误";
            }
        }
    }
}
