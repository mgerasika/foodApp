using System;
using System.Collections.Generic;

namespace FoodApp.Common
{
    public class UsersManager : ManagerBase<ngUserModel>
    {
        public static UsersManager Inst = new UsersManager();

        protected override string FileName
        {
            get { return "users.json"; }
        }

        protected override string GetId(ngUserModel obj) {
            return obj.UserId;
        }

        public List<ngUserModel> GetUsers() {
            List<ngUserModel> res = base.GetItems();
            if (res.Count == 0) {
                res.Add(new ngUserModel { Name = "Олена К", UserId = "_chk2m", Email = "okapij@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Олена М", UserId = "_ciyn3", Email = "omartsinets@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Ірина", UserId = "_ckd7g", Email = "ikhariv@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Андрій П", UserId = "_clrrx", Email = "apodanovskiy@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Андрій Ч", UserId = "_cyevm", Email = "achulyk@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Aндрій М", UserId = "_cztg3", Email = "amamchur@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Юра", UserId = "_d180g", Email = "haba.yura@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Володя", UserId = "_d2mkx", Email = "vpabyrivskyy@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Андрій Б", UserId = "_cssly", Email = "aborovyi@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Іван П", UserId = "_cu76f", Email = "iplotytsia@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Василь З", UserId = "_cvlqs", Email = "vzadorozhnyy@darwinsgrove.com" });
                res.Add(new ngUserModel {Name = "Міша Г", UserId = "_cx0b9", Email = "mherasika@darwinsgrove.com"});
                res.Add(new ngUserModel { Name = "Міша Г", UserId = "_cx0b9", Email = "mgerasika@gmail.com" });
                Save();
            }
            return res;
        }

        internal ngUserModel GetUserByEmail(string email) {
            ngUserModel res = null;
            foreach (ngUserModel item in GetUsers()) {
                if (item.Email.Equals(email,StringComparison.OrdinalIgnoreCase)) {
                    res = item;
                    break;
                }
            }
            return res;
        }

        internal bool HasEmail(string email) {
            bool res = GetUserByEmail(email) != null;
            return res;
        }
    }
}