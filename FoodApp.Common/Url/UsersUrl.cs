using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class UsersUrl : UrlBase {
        private const string c_sUsersPrefix = "api/users/";
        public const string c_sGetUsers = c_sUsersPrefix + "users/";
        public static UsersUrl Inst = new UsersUrl();
        public string GetUsers()
        {
            string res = FormatUrl(c_sGetUsers);
            return res;
        }

        private UsersUrl() {
            
        }
    }
}