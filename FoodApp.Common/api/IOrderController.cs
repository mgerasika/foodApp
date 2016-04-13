using System.Collections.Generic;

namespace FoodApp.Common {
    public interface IOrderController {
        IList<IList<ngOrderEntry>> GetAllOrders(string userId);

        IList<ngOrderEntry> GetOrders(string userId, int day);

        bool Delete(string userId, int day, string foodId);
    }
}