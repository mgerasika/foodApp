using System;
using System.Collections.Generic;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Json, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class ngHistoryGroupEntry : ngModelBase
    {
        public string DateStr { get; set; }
        public DateTime Date { get; set; }
        public List<ngHistoryEntry> Entries { get; set; }

        public ngHistoryGroupEntry() {
            this.Entries = new List<ngHistoryEntry>();
        }

        public override string ToString() {
            return this.DateStr;
        }
    }
}