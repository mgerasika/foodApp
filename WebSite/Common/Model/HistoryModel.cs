using System.Collections.Generic;
using FoodApp.Client;

namespace FoodApp.Common
{
    public class HistoryModel : ngModelBase
    {
        public string UserId { get; set; }
        public List<ngHistoryEntry> Entries { get; set; }

        public HistoryModel() {
            this.Entries = new List<ngHistoryEntry>();
        }

    }
}