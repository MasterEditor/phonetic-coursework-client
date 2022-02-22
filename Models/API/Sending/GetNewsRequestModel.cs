using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models.API.Sending
{
    public class GetNewsRequestModel
    {
        public int Page { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public string SearchString { get; set; }

        public SearchType SearchType { get; set; }
    }
}
