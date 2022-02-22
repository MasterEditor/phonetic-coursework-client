using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models.API.Sending
{
    class ConfirmPasswordRecoveryRequestModel : ConfirmRegistrationRequestModel
    {
        public string Password { get; set; }
    }
}
