using System;
using System.Timers;
using Google.GData.Spreadsheets;

namespace FoodApp.Common.Parser {
    public class ExcelManager {
        private static readonly object _lockObj = new object();
        public static ExcelManager Inst = new ExcelManager();
        private readonly ExcelParser _parser = new ExcelParser();
        private readonly Timer _timer = new Timer();
        private ExcelParser _parserBackground;

        private ExcelManager() {
            _timer = new Timer();
            _timer.Interval = 20*60*1000; //every 30 minutes
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        public SpreadsheetsService SpreadsheetsService {
            get { return _parser.SpreadsheetsService; }
        }

        public ExcelDoc Doc {
            get { return _parser.Doc; }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e) {
            //start sunday
            if (DateTime.Now.Hour >= 7 || DateTime.Now.Hour <= 8) {
                _parserBackground = new ExcelParser();
                _parserBackground.Init();
            }
        }

        public void RefreshAccessToken() {
            if (_parser.IsInit) {
                _parser.RefreshAccessToken();
            }
        }

        internal void Init() {
            _parser.Init();
        }
    }
}