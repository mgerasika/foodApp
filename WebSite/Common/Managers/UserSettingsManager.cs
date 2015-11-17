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
            return obj.Email;
        }

       

        internal ngUsersSettingsModel GetUserSettings(string email) {
            return GetItem(email);
        }

        internal ngUsersSettingsModel EnsureUserSettings(string email) {
            ngUsersSettingsModel res = GetUserSettings(email);
            if (null == res)
            {
                res = new ngUsersSettingsModel();
                res.Email = email;
                UserSettingsManager.Inst.Add(res);
            }
            return res;
        }
    }
}