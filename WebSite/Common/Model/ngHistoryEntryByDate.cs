using System;
using System.Collections.Generic;
using FoodApp.Client;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngHistoryEntryByDate : ngModelBase
    {
        public DateTime Date { get; set; }
        public List<ngHistoryEntry> Entries { get; set; }

        public ngHistoryEntryByDate() {
            this.Entries = new List<ngHistoryEntry>();
        }
    }
}