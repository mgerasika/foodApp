using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FoodApp.Client
{
    public class TraceApiController: ApiController {
        [HttpGet]
        [Route("api/trace")]
        public List<ngTraceModel> TraceData() {
            List<ngTraceModel> res = ApiTraceManager.Inst.GetTraces();
            res = res.OrderByDescending(o => o.Date).ToList();
            return res;
        }

        [HttpDelete]
        [Route("api/trace/{id}")]
        public ApiResponse<string> DeleteTrace(string id) {
            ApiTraceManager.Inst.DeleteTrace(id);
            ApiResponse<string> res = new ApiResponse<string>();
            res.Success = true;
            return res;
        }

        [HttpDelete]
        [Route("api/trace")]
        public ApiResponse<string> DeleteAllTraces() {
            ApiTraceManager.Inst.DeleteAllTraces();
            ApiResponse<string> res = new ApiResponse<string>();
            res.Success = true;
            return res;
        }
    }
}