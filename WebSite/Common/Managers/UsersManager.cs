using System;
using System.Collections.Generic;
using System.Linq;

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
            return obj.Email;
        }

        public List<ngUserModel> GetUsers() {
            List<ngUserModel> res = base.GetItems();
            if (res.Count == 0) {
                res.Add(new ngUserModel { Name = "Олена К", Column = 5, Email = "okapij@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Олена М", Column =6, Email = "omartsinets@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Ірина", Column = 7, Email = "ikhariv@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Андрій П", Column =8, Email = "apodanovskiy@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Андрій Ч", Column = 9, Email = "achulyk@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Aндрій М", Column = 10, Email = "amamchur@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Юра", Column = 11, Email = "haba.yura@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Володя", Column = 12, Email = "vpabyrivskyy@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Андрій Б", Column = 13, Email = "aborovyi@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Іван П", Column = 14, Email = "iplotytsia@darwinsgrove.com" });
                res.Add(new ngUserModel { Name = "Василь З", Column = 15, Email = "vzadorozhnyy@darwinsgrove.com" });
                res.Add(new ngUserModel {Name = "Міша Г", Column = 16, Email = "mherasika@darwinsgrove.com"});
                res.Add(new ngUserModel { Name = "Міша Г", Column = 16, Email = "mgerasika@gmail.com" });
                res.Add(new ngUserModel { Name = "Aндрій Д", Column = 17, Email = "adombr@darwinsgrove.com" });
                Save();
            }
            return res;
        }

        public List<ngUserModel> GetUniqueUsers()
        {
            List<ngUserModel> res = new List<ngUserModel>();

            foreach (ngUserModel user in base.GetItems())
            {
                ngUserModel userInResult = res.SingleOrDefault(r => r.Column == user.Column);
                if (null == userInResult)
                {
                    res.Add(user);
                }
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