using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class JsUsersApi : JsApiBase {


        public void GetUsers(JsHandler<JsArray<ngUserModel>> handler) {
            string url = UsersUrl.Inst.GetUsers();
            SendGet(url, delegate(JsArray<ngUserModel> args) {
                if (null != handler) {
                    handler(args);
                }
            });
        }

        public JsUsersApi(string serverUrl, string userId)
            : base(serverUrl, userId) {
        }
    }
}