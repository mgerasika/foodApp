using System;
using System.Collections.Generic;
using System.Timers;
using FoodApp.Common.Managers;
using FoodApp.Common.Model;
using Google.GData.Spreadsheets;

namespace FoodApp.Common.Parser {
    public class ExcelManager {
        private static readonly object _lockObj = new object();
        public static ExcelManager Inst = new ExcelManager();
        private readonly ExcelParser _parser = new ExcelParser();
        private ExcelParser _parserBackground = null;
        private readonly Timer _timer = new Timer();

        private ExcelManager()
        {
            _timer = new Timer();
            _timer.Interval = 30 * 60 * 1000;//every 30 minutes
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //start sunday
            if (DateTime.Now.Hour >= 9 || DateTime.Now.Hour <= 16)
            {
               _parserBackground = new ExcelParser();
                _parserBackground.Init();
            }
        }

        public SpreadsheetsService SpreadsheetsService {
            get { return _parser.SpreadsheetsService; }
        }

        public ExcelDoc Doc {
            get { return _parser.Doc; }
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