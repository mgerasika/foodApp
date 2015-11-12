using System;
using System.Collections.Generic;

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

        internal ngHistoryModel GetHistoryModelByUserId(string userId) {
            return GetItem(userId);
        }

        internal bool HasAnyEntry(DateTime dt) {
            bool res = false;
            foreach (ngUserModel users in UsersManager.Inst.GetUsers()) {
                ngHistoryModel model = GetHistoryModelByUserId(users.UserId);
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