using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Reply
    {

        public string command;
        public string UserId, FIO, email, log, OrderId, material, DateofEnd, AdressofDeliv;
        public int error; 
    }//Класс ответа к Диме после запроса к бд
}
