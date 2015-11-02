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

        public void Save() {
            SaveToFile();
        }
    }
}