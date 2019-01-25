using Ku.util;
using System;

namespace Test
{
    class UtilTester
    {
        public UtilTester()
        {

        }

        internal void Start()
        {
            string path;
            try
            {
                path =  KuReg.GetAutoRun("骨灰寄存架通讯后台");
                KuDll kuDll = new KuDll();
                kuDll.Load("KuFrame.dll");
                kuDll.GetClass("Ku.KuLog");
                kuDll.NewInstance("");
                kuDll.CallMethod("LogInfo", "Test");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
