using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models.API
{
    public class UsersApiClient : ApiClient
    {
        public async Task<bool> IsRegistered(string number)
        {
            while (true)
            {
                try
                {
                    var response = await SendPostAsync(ApiStrings.IsRegistered, new { Number = number });

                    if (response.IsSuccessStatusCode) return true;

                    return false;
                }
                catch { }
            }
        }

    }
}
