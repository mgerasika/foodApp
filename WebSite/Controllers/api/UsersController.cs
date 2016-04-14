using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Common;
using FoodApp.Common.Managers;

namespace FoodApp.Controllers.api {
    public class UsersController : ApiControllerBase, IMoneyController {
        [HttpGet]
        [Route(UsersUrl.c_sGetUsers)]
        public List<ngUserModel> GetUsers()
        {
            List<ngUserModel> users = UsersManager.Inst.GetUsers();
            return users;
        }
    }
}