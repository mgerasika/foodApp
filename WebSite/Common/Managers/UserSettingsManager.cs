using System.Diagnostics;

namespace FoodApp.Common
{
    public class UserSettingsManager : ManagerBase<ngUsersSettingsModel>
    {
        public static UserSettingsManager Inst = new UserSettingsManager();
        protected override string FileName
        {
            get { return "userSettings.json"; }
        }

        protected override string GetId(ngUsersSettingsModel obj) {
            return obj.UserId;
        }

        public void Fix() {
            foreach (ngUsersSettingsModel item in GetItems()) {
                ngUserModel user = UsersManager.Inst.GetUserByEmail(item.Email);
                Debug.Assert(null != user);
                item.UserId = user.Id;
            }
            Save();
        }


        internal ngUsersSettingsModel GetUserSettings(ngUserModel user)
        {
            return GetItem(user.Id);
        }

        internal ngUsersSettingsModel EnsureUserSettings(ngUserModel user) {
            ngUsersSettingsModel res = GetUserSettings(user);
            if (null == res)
            {
                res = new ngUsersSettingsModel();
                res.Email = user.Email;
                res.UserId = user.Id;
                UserSettingsManager.Inst.Add(res);
            }
            return res;
        }
    }
}