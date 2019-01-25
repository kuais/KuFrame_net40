using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace Ku.util
{
    public class KuReg
    {
        /// <summary>
        /// 获取注册表键值
        /// </summary>
        /// <param name="Root">注册表根键</param>
        /// <param name="RegAddress">路径</param>
        /// <param name="KeyName">名称</param>
        /// <param name="DefaultValue">若失败取此默认值</param>
        /// <returns></returns>
        public static object Load(RegistryKey Root, string RegAddress, string KeyName, object DefaultValue)
        {
            object Value = DefaultValue;
            string[] strAddress = RegAddress.Split('\\');
            int lenth = strAddress.Length;
            RegistryKey[] Key = new RegistryKey[lenth + 1];
            Key[0] = Root;
            for (int i = 0; i < lenth; i++)
            {
                Key[i + 1] = Key[i].CreateSubKey(strAddress[i]);
            }
            Value = Key[lenth].GetValue(KeyName);
            Key[lenth].Close();
            return Value;
        }

        public static void Save(RegistryKey Root, string RegAddress, string KeyName, string KeyValue, RegistryValueKind ValueKind = RegistryValueKind.String)
        {
            string[] strAddress = RegAddress.Split('\\');
            int lenth = strAddress.Length;
            RegistryKey[] Key = new RegistryKey[lenth + 1];
            Key[0] = Root;
            for (int i = 0; i < lenth; i++)
            {
                Key[i + 1] = Key[i].CreateSubKey(strAddress[i]);
            }
            Key[lenth].SetValue(KeyName, KeyValue, ValueKind);
            Key[lenth].Close();
        }

        /// <summary>
        /// 删除键值
        /// </summary>
        /// <param name="Root">注册表根键</param>
        /// <param name="RegAddress">路径</param>
        /// <param name="KeyName">名称</param>
        /// <returns></returns>
        public static void Delete(RegistryKey Root, string RegAddress, string KeyName)
        {
            string[] strAddress = RegAddress.Split('\\');
            int lenth = strAddress.Length;
            RegistryKey[] Key = new RegistryKey[lenth + 1];
            Key[0] = Root;
            for (int i = 0; i < lenth; i++)
            {
                Key[i + 1] = Key[i].CreateSubKey(strAddress[i]);
            }
            Key[lenth].DeleteSubKeyTree(KeyName);
            Key[lenth].Close();
        }

        /// <summary>  
        /// 执行注册表导入  .reg文件
        /// </summary>  
        /// <param name="regPath">注册表文件路径</param>  
        public static void Run(string path)
        {
            if (File.Exists(path))
            {
                path = @"""" + path + @"""";
                path = string.Format(" /s {0}", path);
                Process.Start("regedit", path);
            }
        }

        /// <summary>
        /// 设置开机启动，适用于WinXP
        /// </summary>
        /// <param name="keyName">启动程序名称</param>
        /// <param name="filePath">启动程序路径</param>
        public static void SetAutoRun(string keyName, string filePath)
        {
            string path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            KuReg.Save(Registry.LocalMachine, path, keyName, filePath);
        }

        /// <summary>
        /// 除开机启动项，适用于WinXP
        /// </summary>
        /// <param name="keyName">启动程序名称</param>
        public static void DelAutoRun(string keyName)
        {
            string path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            KuReg.Delete(Registry.LocalMachine, path, keyName);
        }

        /// <summary>
        /// 获取自动启动项的值
        /// </summary>
        /// <param name="keyName">启动项名称</param>
        /// <returns>启动项路径</returns>
        public static string GetAutoRun(string keyName)
        {
            string path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            return KuReg.Load(Registry.LocalMachine, path, keyName, "")?.ToString();
        }

    }
}
