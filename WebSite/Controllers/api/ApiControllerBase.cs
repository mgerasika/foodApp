using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;

namespace FoodApp.Controllers.api
{
    public abstract class ApiControllerBase<T> : ApiController where T : ngModelBase
    {
        protected abstract ManagerBase<T> GetManager();

        [HttpGet]
        public IEnumerable<T> Get()
        {
            List<T> items = GetManager().GetItems();
            return items;
        }

        /*
        [HttpGet]
        public T Get(string id)
        {
            T item = GetManager().GetItem(id);
            return item;
        }

        [HttpPost]
        public void Post([FromBody] T value)
        {
            GetManager().AddItem(value);
        }

        [HttpPut]
        public void Put(string id, [FromBody] T value)
        {
            GetManager().EditItem(id, value);
        }

        [HttpDelete]
        public void Delete(string id)
        {
            GetManager().Delete(id);
        }
         * */
    }
}