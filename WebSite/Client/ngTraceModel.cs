using System;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs)]
    public class ngTraceModel {
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string ID { get; set; }
        public string UserName { get; set; }

        public ngTraceModel() {
            this.ID = Guid.NewGuid().ToString();
        }
    }
}