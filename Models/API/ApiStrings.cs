using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Client.Models.API
{
    public class ApiStrings
    {
        public const string Api = "/api";

        public const string Users = "/users";

        public const string News = "/news";

        public const string Tariffs = "/tariffs";

        public const string Promotions = "/promotions";

        public const string Payments = "/payments";

        public const string Consumptions = "/consumptions";

        public const string Services = "/services";

        public const string Storage = "/storage";

        public const string Statistics = "/statistics";

        public const string Feedback = "/feedback";

        public const string IsRegistered = Api + Users + "/IsRegistered";

        public const string Register = Api + Users + "/Register";

        public const string ConfirmRegistration = Api + Users + "/ConfirmRegistration";

        public const string Login = Api + Users + "/Login";

        public const string RecoverPassword = Api + Users + "/RecoverPassword";

        public const string ConfirmPasswordRecovery = Api + Users + "/ConfirmPasswordRecovery";

        public const string ChangePassword = Api + Users + "/changePassword";

        public const string GetUserInfo = Api + Users + "/getUserInfo";

        public const string GetAllUsers = Api + Users + "/getAll";

        public const string GetNews = Api + News + "/getList";

        public const string GetOneNews = Api + News + "/get";

        public const string RemoveArticle = Api + News + "/remove";

        public const string EditArticle = Api + News + "/edit";

        public const string AddArticle = Api + News + "/add";

        public const string GetTariffs = Api + Tariffs + "/getList";

        public const string GetAllTariffs = Api + Tariffs + "/getAll";

        public const string GetMyTariff = Api + Tariffs + "/get";

        public const string ChangeMyTariff = Api + Tariffs + "/changeTariff";

        public const string RemoveTariff = Api + Tariffs + "/remove";

        public const string EditTariff = Api + Tariffs + "/edit";

        public const string AddTariff = Api + Tariffs + "/add";

        public const string GetPromotions = Api + Promotions + "/getList";

        public const string RemovePromotion = Api + Promotions + "/remove";

        public const string AddPromotion = Api + Promotions + "/add";

        public const string Recharge = Api + Payments + "/recharge";

        public const string GetLastOperations = Api + Payments + "/getList";

        public const string GetLastConsumptions = Api + Consumptions + "/getList";

        public const string GetMyServices = Api + Services + "/getMyList";

        public const string GetServices = Api + Services + "/getList";

        public const string SubscribeToService  = Api + Services + "/subscribe";

        public const string UnsubscribeFromService = Api + Services + "/unsubscribe";

        public const string GetAllServices = Api + Services + "/getAll";

        public const string RemoveService = Api + Services + "/remove";

        public const string AddService = Api + Services + "/add";

        public const string EditService = Api + Services + "/edit";

        public const string GetAllImages = Api + Storage + "/getAllImages";

        public const string RemoveImage = Api + Storage + "/removeImage";

        public const string AddImage = Api + Storage + "/addImage";

        public const string GetStatistics = Api + Statistics + "/getList";

        public const string SendFeedback = Api + Feedback + "/send";
    }
}
