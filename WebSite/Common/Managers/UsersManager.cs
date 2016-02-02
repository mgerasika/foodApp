using System;
using System.Collections.Generic;
using System.Linq;
using FoodApp.Common.Model;

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
            return res;
        }

        public List<ngUserModel> GetUniqueUsers() {
            List<ngUserModel> res = new List<ngUserModel>();

            List<ngUserModel> users = GetItems();
            foreach (ngUserModel user in users) {
                ngUserModel userInResult = res.SingleOrDefault(r => r.Column == user.Column);
                if (null == userInResult) {
                    res.Add(user);
                }
            }

            return res;
        }

        internal ngUserModel GetUserByEmail(string email) {
            ngUserModel res = GetUserByEmail(GetUsers(), email);
            return res;
        }

        internal ngUserModel GetUserByEmail(List<ngUserModel> users,string email)
        {
            ngUserModel res = null;
            foreach (ngUserModel item in users)
            {
                if (item.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    res = item;
                    break;
                }
            }
            return res;
        }

        internal ngUserModel GetUserById(string id) {
            ngUserModel res = null;
            List<ngUserModel> users = GetUsers();
            foreach (ngUserModel item in users) {
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