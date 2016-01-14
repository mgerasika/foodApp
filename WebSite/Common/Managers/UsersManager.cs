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
                res.AddRange(CreateDefaultUsers());
                Save();
            }

            return res;
        }

        private List<ngUserModel> CreateDefaultUsers() {
            List<ngUserModel> res = new List<ngUserModel>();
            res.Add(new ngUserModel {Name = "Олена К", Column = 5, Email = "okapij@darwinsgrove.com", Id = Guid.Parse("01258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Олена М", Column = 6, Email = "omartsinets@darwinsgrove.com", Id = Guid.Parse("02258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Ірина Х", Column = 7, Email = "ikhariv@darwinsgrove.com", Id = Guid.Parse("03258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Андрій П", Column = 8, Email = "apodanovskiy@darwinsgrove.com", Id = Guid.Parse("04258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Андрій Ч", Column = 9, Email = "achulyk@darwinsgrove.com", Id = Guid.Parse("05258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Андрій М", Column = 10, Email = "amamchur@darwinsgrove.com", Id = Guid.Parse("06258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Юра Г", Column = 11, Email = "haba.yura@darwinsgrove.com", Id = Guid.Parse("07258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Володя П", Column = 12, Email = "vpabyrivskyy@darwinsgrove.com", Id = Guid.Parse("08258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Андрій Б", Column = 13, Email = "aborovyi@darwinsgrove.com", Id = Guid.Parse("09258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Іван П", Column = 14, Email = "iplotytsia@darwinsgrove.com", Id = Guid.Parse("10258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Василь З", Column = 15, Email = "vzadorozhnyy@darwinsgrove.com", Id = Guid.Parse("11258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Марк К", Column = 16, Email = "mkapij@darwinsgrove.com", Id = Guid.Parse("12258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "̳Міша Г", Column = 17, Email = "mherasika@darwinsgrove.com", Id = Guid.Parse("47258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "̳Міша Г", Column = 17, Email = "mgerasika@gmail.com", Id = Guid.Parse("47258346-5281-4FD7-9234-D5EB994C534C").ToString()});
            res.Add(new ngUserModel {Name = "Андрій Д", Column = 18, Email = "adombr@darwinsgrove.com", Id = Guid.Parse("13258346-5281-4FD7-9234-D5EB994C534C").ToString()});


            return res;
        }

        public void Fix() {
            //fix user id
            List<ngUserModel> defaultUsers = CreateDefaultUsers();
            List<ngUserModel> users = GetUsers();
            foreach (ngUserModel user in users) {
                ngUserModel defaultUser = GetUserByEmail(defaultUsers,user.Email);
                if (null != defaultUser) {
                    user.Id = defaultUser.Id;
                    user.Column = defaultUser.Column;
                }
            }

            foreach (ngUserModel defaultUser in defaultUsers) {
                ngUserModel user = GetUserByEmail(users,defaultUser.Email);
                if (user == null) {
                    users.Add(defaultUser);
                }
            }
            Save();
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