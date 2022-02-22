using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models.API
{
    public class HttpResult<T>
    {
        public T ResultObject { get; set; }

        public int StatusCode { get; set; }
    }
}
