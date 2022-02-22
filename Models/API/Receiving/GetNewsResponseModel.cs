using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models.API.Receiving
{
    public class GetNewsResponseModel
    {
        public int TotalPages { get; set; }
        
        public List<NewsArticle> News { get; set; }
    }
}
