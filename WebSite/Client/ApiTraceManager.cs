using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using FoodApp.Common;
using FoodApp.Common.Model;
using FoodApp.Controllers;

namespace FoodApp.Client {
    public class ApiTraceManager {
        private static readonly object _lockObject = new object();
        public static ApiTraceManager Inst = new ApiTraceManager();

        private ApiTraceManager() {
        }

        private string c_sFileName {
            get { return "/trace_" + DateTime.Now.ToString("yyyy_MM") + ".log"; }
        }

        public long DbVersion { get; set; }

        public void LogTrace(string trace) {
            if (null != trace) {
                ngTraceModel lTrace = new ngTraceModel();
                lTrace.Date = DateTime.Now;
                lTrace.Message = trace;
                ngUserModel ngUserModel = ApiUtils.GetUser();
                lTrace.UserName = ngUserModel != null ? ngUserModel.Name : "";
                List<ngTraceModel> lTraces = GetTraces();
                lTraces.Add(lTrace);
                Save(lTraces);
            }
        }

        private void Save(List<ngTraceModel> traces) {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(traces);

            lock (_lockObject) {
                string file = HostingEnvironment.MapPath("~/logs") + c_sFileName;
                if (!File.Exists(file)) {
                    File.Create(file);
                }
                using (StreamWriter sw = new StreamWriter(file)) {
                    string url = "";
                    sw.Write(json);
                }
            }
        }

        public List<ngTraceModel> GetTraces() {
            string str = "";
            str = Load(str);
            List<ngTraceModel> traces = new List<ngTraceModel>();
            if (!string.IsNullOrEmpty(str)) {
                JavaScriptSerializer js = new JavaScriptSerializer();
                try {
                    traces = js.Deserialize<List<ngTraceModel>>(str);
                }
                catch (Exception ex) {
                }
            }
            return traces;
        }

        private string Load(string str) {
            string file = HostingEnvironment.MapPath("~/logs") + c_sFileName;
            lock (_lockObject) {
                if (File.Exists(file)) {
                    using (StreamReader sw = new StreamReader(file)) {
                        str = sw.ReadToEnd();
                    }
                }
                else {
                    using (File.Create(file)) {
                    }
                }
            }
            return str;
        }

        public void DeleteTrace(string id) {
            ngTraceModel res = null;
            List<ngTraceModel> traces = GetTraces();
            foreach (ngTraceModel trace in traces) {
                if (trace.ID.Equals(id)) {
                    res = trace;
                }
            }
            if (null != res) {
                traces.Remove(res);
                Save(traces);
            }
        }

        public void DeleteAllTraces() {
            List<ngTraceModel> traces = GetTraces();
            traces.Clear();
            Save(traces);
        }
    }
}