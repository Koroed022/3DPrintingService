using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        static ServerObj server; // сервер
        static Thread listenThread; // поток для прослушивания
        static Socket lol = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
;
        static void Main(string[] args)
        {
            try
            {
                server = new ServerObj();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.CloseAll();//отключение всех клиентов и остановка сервера
               // Console.WriteLine(ex.Message);
            }
        }
    }
}
