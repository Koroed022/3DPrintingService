using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;


namespace Server
{
    class ServerObj
    {
        static TcpListener tcpListener;//Слушаем
        List<ClientObj> clients = new List<ClientObj>(); // все подключения
        protected internal void AddConnection(ClientObj clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObj client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
        // прослушивание входящих подключений
        protected internal void Listen()
        {
            string Host = System.Net.Dns.GetHostName();
            string IP = System.Net.Dns.GetHostByName(Host).AddressList[0].ToString();
            IPAddress localaddr = IPAddress.Parse("0.0.0.0");
            try
            {
                tcpListener = new TcpListener(localaddr, 9999);
                tcpListener.Start();
                Console.WriteLine("Server is online. IP address: " + IP);
                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    ClientObj clientObject = new ClientObj(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CloseAll();
            }
        }

        // отключение всех клиентов
        protected internal void CloseAll()
        {
            tcpListener.Stop(); //остановка сервера
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }
}