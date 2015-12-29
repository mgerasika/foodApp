using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FoodApp.Common
{
    public class HistoryManager : ManagerBase<ngHistoryModel>
    {
        public static HistoryManager Inst = new HistoryManager();

        protected override string FileName {
            get { return "history.json"; }
        }

        protected override string GetId(ngHistoryModel obj) {
            return obj.UserId;
        }

        public void CreateFake() {
        }

        public void Fix()
        {
            foreach (ngHistoryModel item in GetItems())
            {
                ngUserModel user = UsersManager.Inst.GetUserByEmail(item.Email);
                Debug.Assert(null != user);
                item.UserId = user.Id;
            }
            Save();
        }

        internal ngHistoryModel GetHistoryModelByUser(ngUserModel user) {
            return GetItem(user.Email);
        }

        internal bool HasAnyEntry(DateTime dt) {
            bool res = false;
            foreach (ngUserModel user in UsersManager.Inst.GetUsers()) {
                ngHistoryModel model = GetHistoryModelByUser(user);
                if (null != model) {
                    var entries = model.GetEntriesByDate(dt);
                    if (entries.Count > 0) {
                        res = true;
                        break;
                    }
                }
            }
            return res;
        }
    }
}