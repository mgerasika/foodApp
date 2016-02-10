using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using FoodApp.Client;
using FoodApp.Common;
using FoodApp.Common.Managers;
using FoodApp.Common.Model;

namespace FoodApp.Controllers.api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public abstract class ApiControllerBase : System.Web.Http.ApiController 
    {
    }
}