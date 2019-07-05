using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string output = ""
                + "0 : Quit\r\n"
                + "1 : KuSerial Test\r\n"
                + "2 : KuSocket Test\r\n"
                + "4 : KuFile Test\r\n"
                + "5 : KuUtil Test\r\n"
                + "9 : Print Test\r\n"
                + "请输入序号选择测试项:\r\n\r\n";
            Console.Write(output);
            while(true)
            {
                char c = Console.ReadKey(true).KeyChar;
                try
                {
                    switch (c)
                    {
                        case '0':
                            return;
                        case '1':
                            new SerialTester().Start();
                            break;
                        case '2':
                            new SocketTester().Start();
                            break;
                        case '4':
                            new FileTester().Start();
                            break;
                        case '5':
                            new UtilTester().Start();
                            break;
                        case '9':
                            new PrintTester().Start();
                            break;
                        default:
                            Console.WriteLine("You press key " + c);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}
