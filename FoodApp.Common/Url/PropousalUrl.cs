namespace FoodApp.Common.Url {
    public class PropousalUrl {
        public const string c_sGetPropousal = "api/propousal";
        public const string c_sGetPropousalByDay = c_sGetPropousal + "/{userId}/{dayOfWeek}/";
        public const string c_sGetPropousals = c_sGetPropousal + "/{userId}/";
        public const string c_sBuy = c_sGetPropousal + "/{userId}/{dayOfWeek}/";
    }
}