using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Common;
using FoodApp.Common.Managers;

namespace FoodApp.Controllers.api {
    /*
          ngUserModel user = UsersManager.Inst.GetUserByEmail("mgerasika@gmail.com");
          string orderId = OrderManager.Inst.GetOrderId(user,3);
          if (!MoneyManager.Inst.CanBuy(user, 100,orderId)) {
              MoneyManager.Inst.Add(user,200);
          }
          if (MoneyManager.Inst.CanBuy(user, 10,orderId)) {
              MoneyManager.Inst.Buy(user, 10, orderId);
          }

          if (MoneyManager.Inst.CanBuy(user, 10,orderId))
          {
              MoneyManager.Inst.Buy(user, 10, orderId);
          }

          if (MoneyManager.Inst.CanRefund(user, orderId))
          {
              MoneyManager.Inst.Refund(user,orderId);
          }
          if (MoneyManager.Inst.CanRefund(user, orderId))
          {
              MoneyManager.Inst.Refund(user, orderId);
          }
           * */

    public class MoneyController : ApiControllerBase, IMoneyController {
        [HttpGet]
        [Route(MoneyUrl.c_sAddMoney)]
        public bool AddMoney(string userId, decimal val) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            MoneyManager.Inst.Add(user, val);
            return true;
        }


        [HttpGet]
        [Route(MoneyUrl.c_sRemoveMoney)]
        public bool RemoveMoney(string userId, decimal val) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            MoneyManager.Inst.Remove(user, val);
            return true;
        }

        [HttpGet]
        [Route(MoneyUrl.c_sGetMoney)]
        public decimal GetMoney(string userId) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            decimal res = MoneyManager.Inst.GetMoney(user);
            return res;
        }


        [HttpGet]
        [Route(MoneyUrl.c_sGetUsers)]
        public List<ngUserModel> GetUsers() {
            List<ngUserModel> res = new List<ngUserModel>();
            List<ngUserModel> users = UsersManager.Inst.GetUsers();
            foreach (ngUserModel user in users) {
                if (user.Email.Contains("darwin")) {
                    res.Add(user);
                }
            }
            return res;
        }

        [HttpGet]
        [Route(MoneyUrl.c_sBuy)]
        public bool Buy(string userId, int day) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            string orderId = OrderManager.Inst.GetOrderId(user, day);
            decimal total = OrderManager.Inst.CalcTotal(user, day);

            bool res = false;
            if (MoneyManager.Inst.CanBuy(user, total, orderId)) {
                MoneyManager.Inst.Buy(user, total, orderId);
                res = true;
            }
            return res;
        }

        [HttpGet]
        [Route(MoneyUrl.c_sRefund)]
        public bool Refund(string userId, int day) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            string orderId = OrderManager.Inst.GetOrderId(user, day);
            decimal total = OrderManager.Inst.CalcTotal(user, day);

            bool res = false;
            if (MoneyManager.Inst.CanRefund(user, orderId)) {
                MoneyManager.Inst.Refund(user, orderId);
                res = true;
            }
            return res;
        }

        [HttpGet]
        [Route(MoneyUrl.c_sCanBuy)]
        public bool CanBuy(string userId, int day) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            string orderId = OrderManager.Inst.GetOrderId(user, day);
            decimal total = OrderManager.Inst.CalcTotal(user, day);

            bool res = false;
            if (MoneyManager.Inst.CanBuy(user, total, orderId)) {
                res = true;
            }
            return res;
        }

        [HttpGet]
        [Route(MoneyUrl.c_sCanRefund)]
        public bool CanRefund(string userId, int day) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            string orderId = OrderManager.Inst.GetOrderId(user, day);

            bool res = false;
            if (MoneyManager.Inst.CanRefund(user, orderId)) {
                res = true;
            }
            return res;
        }
    }
}