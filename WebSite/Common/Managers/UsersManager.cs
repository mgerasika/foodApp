using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodApp.Common {
    public class UsersManager : ManagerBase<ngUserModel> {
        public static UsersManager Inst = new UsersManager();

        protected override string FileName {
            get { return "users.json"; }
        }

        protected override string GetId(ngUserModel obj) {
            return obj.Id;
        }

        public List<ngUserModel> GetUsers() {
            List<ngUserModel> res = GetItems();
            if (res.Count == 0) {
                res.Add(new ngUserModel {Name = "����� �", Column = 5, Email = "okapij@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "����� �", Column = 6, Email = "omartsinets@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "�����", Column = 7, Email = "ikhariv@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "����� �", Column = 8, Email = "apodanovskiy@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "����� �", Column = 9, Email = "achulyk@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "A���� �", Column = 10, Email = "amamchur@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "���", Column = 11, Email = "haba.yura@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "������", Column = 12, Email = "vpabyrivskyy@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "����� �", Column = 13, Email = "aborovyi@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "���� �", Column = 14, Email = "iplotytsia@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "������ �", Column = 15, Email = "vzadorozhnyy@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "̳�� �", Column = 16, Email = "mherasika@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "̳�� �", Column = 16, Email = "mgerasika@gmail.com"});
                res.Add(new ngUserModel {Name = "A���� �", Column = 17, Email = "adombr@darwinsgrove.com"});
                res.Add(new ngUserModel {Name = "���� �", Column = 18, Email = "mkapij@darwinsgrove.com"});
                Save();
            }

            return res;
        }

        public void Fix() {
            //update - all users must have id
            foreach (ngUserModel user in GetUsers()) {
                if (string.IsNullOrEmpty(user.Id)) {
                    if (user.Email.Equals("mherasika@darwinsgrove.com") || user.Email.Equals("mgerasika@gmail.com")) {
                        user.Id = Guid.Parse("47258346-5281-4FD7-9234-D5EB994C534C").ToString();
                    }
                    else {
                        user.Id = Guid.NewGuid().ToString();
                    }
                }
            }
            Save();
        }

        public List<ngUserModel> GetUniqueUsers() {
            List<ngUserModel> res = new List<ngUserModel>();

            foreach (ngUserModel user in GetItems()) {
                ngUserModel userInResult = res.SingleOrDefault(r => r.Column == user.Column);
                if (null == userInResult) {
                    res.Add(user);
                }
            }

            return res;
        }

        internal ngUserModel GetUserByEmail(string email) {
            ngUserModel res = null;
            foreach (ngUserModel item in GetUsers()) {
                if (item.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) {
                    res = item;
                    break;
                }
            }
            return res;
        }

        internal ngUserModel GetUserById(string id) {
            ngUserModel res = null;
            foreach (ngUserModel item in GetUsers()) {
                if (item.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) {
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