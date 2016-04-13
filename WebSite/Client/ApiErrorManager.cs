using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using FoodApp.Common;
using FoodApp.Controllers;

namespace FoodApp.Client {
    public class ApiErrorManager {
        private static readonly object _lockObj = new object();
        public static ApiErrorManager Inst = new ApiErrorManager();

        private ApiErrorManager() {
        }

        private string c_sFileName {
            get { return "/error_" + DateTime.Now.ToString("yyyy_MM") + ".log"; }
        }

        public void LogError(Exception exception) {
            if (null != exception) {
                try {
                    ngErrorModel error = new ngErrorModel();
                    error.Date = DateTime.Now;
                    error.Error = exception.Message;
                    if (!string.IsNullOrEmpty(exception.StackTrace)) {
                        error.StackTrace = exception.StackTrace;
                    }
                    error.UserName = ApiUtils.GetLoggedInUser().Name;
                    try {
                        if (HttpContext.Current != null && HttpContext.Current.Request != null) {
                            error.Url = HttpContext.Current.Request.Url.ToString();
                        }
                    }
                    catch (Exception ex) {
                    }
                    List<ngErrorModel> errors = GetErrors();
                    errors.Add(error);

                    Save(errors);
                }
                catch (Exception e) {
                    Debug.Assert(false);
                }
            }
        }

        private void Save(List<ngErrorModel> errors) {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(errors);

            lock (_lockObj) {
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

        public List<ngErrorModel> GetErrors() {
            List<ngErrorModel> errors = new List<ngErrorModel>();
            string str = "";

            str = Load(str);

            if (!string.IsNullOrEmpty(str)) {
                JavaScriptSerializer js = new JavaScriptSerializer();
                try {
                    errors = js.Deserialize<List<ngErrorModel>>(str);
                }
                catch (Exception ex) {
                }
            }
            return errors;
        }

        private string Load(string str) {
            string file = HostingEnvironment.MapPath("~/logs") + c_sFileName;
            lock (_lockObj) {
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

        public void DeleteError(string id) {
            ngErrorModel res = null;
            List<ngErrorModel> errors = GetErrors();
            foreach (ngErrorModel error in errors) {
                if (error.ID.Equals(id)) {
                    res = error;
                }
            }
            if (null != res) {
                errors.Remove(res);
                Save(errors);
            }
        }

        public void DeleteAllError() {
            List<ngErrorModel> errors = GetErrors();
            errors.Clear();
            Save(errors);
        }
    }
}