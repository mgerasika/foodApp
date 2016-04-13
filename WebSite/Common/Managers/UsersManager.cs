using System;
using System.Collections.Generic;
using System.Linq;
using FoodApp.Common.Parser;

namespace FoodApp.Common.Managers {
    public class UsersManager : ManagerBase<ngUserModel> {
        public static UsersManager Inst = new UsersManager();

        private bool isInit;

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

        internal ngUserModel GetUserByEmail(List<ngUserModel> users, string email) {
            ngUserModel res = null;
            foreach (ngUserModel item in users) {
                if (item.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) {
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

        public void Init() {
            if (!isInit) {
                lock (_lockObj) {
                    if (!isInit) {
                        ExcelTable excelTable = ExcelManager.Inst.Doc.GetExcelTable(0);
                        ExcelRow usersRow = null;
                        foreach (ExcelRow row in excelTable.Rows) {
                            if (row.Name != null && row.Name.ToLower().Contains("страви")) {
                                usersRow = row;
                                break;
                            }
                        }

                        foreach (ExcelCell cell in usersRow.Cells) {
                            uint column = cell.Column;
                            string text = cell.GetEntry().Value;

                            ngUserModel user = GetUserBySpreadSheetName(text);
                            if (null != user) {
                                user.Column = column;
                            }
                        }
                        Save();

                        isInit = true;
                    }
                }
            }
        }

        private ngUserModel GetUserBySpreadSheetName(string spreadSheetVal) {
            ngUserModel res = null;
            List<ngUserModel> users = GetUsers();
            foreach (ngUserModel user in users) {
                if (user.Name.Equals(spreadSheetVal, StringComparison.OrdinalIgnoreCase)) {
                    res = user;
                    break;
                }
            }
            return res;
        }
    }
}