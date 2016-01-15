using System;
using System.Collections.Generic;
using FoodApp.Client;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngHistoryGroupEntry : ngModelBase
    {
        public string DateStr { get; set; }
        public List<ngHistoryEntry> Entries { get; set; }

        public ngHistoryGroupEntry() {
            this.Entries = new List<ngHistoryEntry>();
        }
    }
}