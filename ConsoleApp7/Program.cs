using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {
            using(Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                // 1 вариант
                string hostName = "mail.ru";
                int hostPort = 80; // HTTP
                //socket.Connect(hostName, hostPort);
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Disconnect(true);

                // 2 вариант
                IPHostEntry ipAddr = Dns.GetHostEntry(hostName);
                // создание конечной точки
                IPEndPoint endPoint = new IPEndPoint(ipAddr.AddressList[0], hostPort);
                socket.Connect(endPoint);

                if (socket.Connected)
                {
                    // посылка сообщения на удаленный хост
                    socket.Send(Encoding.UTF8.GetBytes("GET /index.html HTTP 1.1\r\n"));

                    // прием сообщения (ответа) от удаленного хоста
                    byte[] buf = new byte[4 * 1024];
                    int recSize = socket.Receive(buf);
                    Console.WriteLine("Receive size {0} bytes", recSize);
                    Console.WriteLine(Encoding.UTF8.GetString(buf, 0, recSize));

                    // закрыть открытый канал связи
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                else
                {
                    Console.WriteLine("Не удалось соединиться с удаленным узлом");
                }
            }

            Console.ReadLine();
        }
    }
}
