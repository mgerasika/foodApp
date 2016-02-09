using System;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs)]
    public class ngErrorModel
    {
        public DateTime Date { get; set; }
        public string StackTrace { get; set; }
        public string Url { get; set; }
        public string ID { get;set; }
        public string UserName { get; set; }

        public ngErrorModel() {
            this.ID = Guid.NewGuid().ToString();
        }

        public string Error { get; set; }
    }
}