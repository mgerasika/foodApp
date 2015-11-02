using System;
using System.Collections.Generic;
using FoodApp.Common;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngOrderModel : ngModelBase
    {
        public decimal Count { get; set; }
        public string FoodId;
    }
}