using System.Collections;
using System.Net;
using System.Net.NetworkInformation;

namespace Ku.util
{
    public class KuNet
    {
        public static IList PortListUsed()
        {
            //获取本地计算机的网络连接和通信统计数据的信息
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            //返回本地计算机上的所有Tcp监听程序
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();
            //返回本地计算机上的所有UDP监听程序
            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();
            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
            IList allPorts = new ArrayList();
            foreach (IPEndPoint ep in ipsTCP) allPorts.Add(ep.Port);
            foreach (IPEndPoint ep in ipsUDP) allPorts.Add(ep.Port);
            foreach (TcpConnectionInformation conn in tcpConnInfoArray) allPorts.Add(conn.LocalEndPoint.Port);
            return allPorts;
        }
        public static bool IsPortUsed(int port)
        {
            return PortListUsed().IndexOf(port) >= 0;
        }
        public static int AvailablePort(int min = 5000, int max = IPEndPoint.MaxPort)
        {
            for (int i = min; i < max; i++)
                if (!IsPortUsed(i)) return i;
            return -1;
        }
    }
}
