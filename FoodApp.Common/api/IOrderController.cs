using System.Collections.Generic;
using FoodApp.Common.Model;
using FoodApp.Common.Url;

namespace FoodApp.Common.api {
    public interface IOrderController {
        IList<IList<ngOrderEntry>> GetAllOrders(string userId);

        IList<ngOrderEntry> GetOrders(string userId, int day);

        bool Delete(string userId, int day, string foodId);
    }
}