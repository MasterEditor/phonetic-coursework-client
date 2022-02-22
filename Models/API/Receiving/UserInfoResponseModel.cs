using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models.API.Receiving
{
    public class UserInfoResponseModel
    {
        public string Number { get; set; }

        public int Minutes { get; set; }

        public int SMS { get; set; }

        public int Internet { get; set; }

        public double Balance { get; set; }

        public int AllMinutes { get; set; }

        public int AllSMS { get; set; }

        public int AllInternet { get; set; }
    }
}
