using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json;
namespace Server
{
    class ClientObj
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;

        TcpClient client;
        ServerObj server; // объект сервера
        public ClientObj(TcpClient tcpClient, ServerObj serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }
        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                string Command = GetMessage();
              
                dynamic stuff = JsonConvert.DeserializeObject(Command);  //Парсим JSON через либу 
                //ЛИБА Json.net
                //Обработка

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }
        private Reply ProcessRequest(dynamic stuff)
        {
            Reply Ans=new Reply();//Обьект класса для возврата ответа от бд(с данными и ошибками внутри)
            switch (stuff.command)
            {
                case "Login"://login
                    {
                        String login = stuff.Login;
                        String Passw = stuff.Pass;
                        ///Запрос к бд
                        Ans.error = 0;
                        Ans.UserId = "";
                        return Ans;

                    }

                case "Registration"://Регистрация
                    {
                        String login = stuff.Login;
                        String Passw = stuff.Pass;
                        String Tel = stuff.Tel;
                        String Fio = stuff.FIO;
                        //Запрос к БД
                        Ans.error = 0;
                        Ans.UserId = "";
                        return Ans;

                    }

                case "GetOrder"://Вернуть заказ по OrderId 
                    {
                        String OrederId = stuff.OrderId;
                        //Запрос к бд
                        //Заполнить Ans
                        return Ans;
                    }
                case "AddOrder"://Добавить заказ
                    {
                        String UserId = stuff.UserId;
                        String File = stuff.File;
                        String MaterialId = stuff.MaterialId;
                        //и так далее 
                        //Запрос к бд
                        return Ans;

                    }
                case "GetOrdersOfUser"://Вернуть все заказы юзера
                    {
                        String UserId = stuff.UserId;
                        //Запрос к бд

                        return Ans;
                    }
                case "DeleteOrder":// Удалить заказ 
                    {
                        String OrederId = stuff.OrderId;
                        //Запрос к бд

                        return Ans;
                    }
                case "EditOrder"://Изменение заказа
                    {
                        String OrederId = stuff.OrderId;
                        String File = stuff.File;
                        String MaterialId = stuff.MaterialId;
                        //и так далее 
                        //Запрос к бд
                        return Ans;
                    }
                case "EditUser"://Изменение юзера
                    {
                        String login = stuff.Login;
                        String Passw = stuff.Pass;
                        String Tel = stuff.Tel;
                        String Fio = stuff.FIO;
                        //Запрос к БД
                        return Ans;
                    }
            }
                    Ans.error = -9999999;
                    return Ans;


        }



       
        private string GetMessage() // чтение входящего сообщения и преобразование в строку
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;

            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);
            return builder.ToString();
        }
        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}