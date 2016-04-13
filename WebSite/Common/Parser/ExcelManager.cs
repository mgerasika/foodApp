using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Timers;
using Google.GData.Spreadsheets;
using Timer = System.Timers.Timer;

namespace FoodApp.Common.Parser {
    public class ExcelManager {
        private static readonly object _lockObj = new object();
        public static ExcelManager Inst = new ExcelManager();
        private readonly List<ExcelParser> _parsers = new List<ExcelParser>();
        private readonly Timer _timer = new Timer();

        public SpreadsheetsService SpreadsheetsService {
            get {
                return GetParser().SpreadsheetsService;
            }
        }

        public ExcelDoc Doc {
            get { return GetParser().Doc; }
        }

        public void RefreshAccessToken() {
            lock (_parsers) {
                foreach (ExcelParser parser in _parsers) {
                    if (parser.IsInit) {
                        parser.RefreshAccessToken();
                    }
                }
            }
        }

        internal void Init() {
            if (GetParser() == null) {
                ExcelParser parser = new ExcelParser();
                parser.Init();
                lock (_parsers) {
                    _parsers.Add(parser);
                }
            }
        }

        private ExcelManager() {
            _timer = new Timer();
            _timer.Interval = 5*60*1000; //every 30 minutes
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e) {
            //start sunday
            CultureInfo info = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = info;
            if (DateTime.Now.Hour >= 7 || DateTime.Now.Hour <= 12) {
                ExcelParser newParser = new ExcelParser();
                newParser.Init();
                lock (_parsers) {
                    _parsers.Add(newParser);

                    if (_parsers.Count >= 3) {
                        _parsers.RemoveAt(0);
                    }
                }
            }
        }

        private ExcelParser GetParser() {
            ExcelParser res = null;
            lock (_parsers) {
                if (_parsers.Count > 0) {
                    res = _parsers[_parsers.Count - 1];
                }
            }
           
            return res;
        }
    }
}