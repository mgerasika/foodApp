using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FoodApp.Client
{
    public class ErrorApiController :
        ApiController
    {
        [HttpGet]
        [Route("api/error")]
        public List<ngErrorModel> ErrorData()
        {

            List<ngErrorModel> res = ApiErrorManager.Inst.GetErrors();
            res = res.OrderByDescending(o => o.Date).ToList();
            return res;
        }

        [HttpDelete]
        [Route("api/error/{id}")]
        public ApiResponse<string> DeleteError(string id) {
            ApiErrorManager.Inst.DeleteError(id);
            ApiResponse<string> res = new ApiResponse<string>();
            res.Success = true;
            return res;
        }

        [HttpDelete]
        [Route("api/error")]
        public ApiResponse<string> DeleteAllError()
        {
            ApiErrorManager.Inst.DeleteAllError();
            ApiResponse<string> res = new ApiResponse<string>();
            res.Success = true;
            return res;
        }
    }
}