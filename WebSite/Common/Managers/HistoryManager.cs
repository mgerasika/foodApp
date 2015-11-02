namespace FoodApp.Common
{
    public class HistoryManager : ManagerBase<HistoryModel>
    {
        public static HistoryManager Inst = new HistoryManager();

        protected override string FileName {
            get { return "history.json"; }
        }

        protected override string GetId(HistoryModel obj) {
            return obj.UserId;
        }

        public void CreateFake() {
        }
    }
}