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

       

        internal ngUsersSettingsModel GetUserSettings(string userId) {
            return GetItem(userId);
        }

        internal ngUsersSettingsModel EnsureUserSettings(string userId) {
            ngUsersSettingsModel res = GetUserSettings(userId);
            if (null == res)
            {
                res = new ngUsersSettingsModel();
                res.UserId = userId;
                UserSettingsManager.Inst.Add(res);
            }
            return res;
        }
    }
}