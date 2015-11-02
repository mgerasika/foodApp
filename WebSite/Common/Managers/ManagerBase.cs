using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using FoodApp.Client;

namespace FoodApp.Common
{
    public abstract class ManagerBase<T> where T : ngModelBase
    {
        protected static readonly object _lockObj = new object();
        private List<T> _items;

        protected abstract string FileName { get; }

        public List<T> GetItems() {
            if (null == _items) {
                lock (_lockObj) {
                    if (null == _items) {
                        var items = new List<T>();
                        var str = "";

                        str = LoadFromFile(str);
                        if (!string.IsNullOrEmpty(str)) {
                            var js = new JavaScriptSerializer();
                            try {
                                items = js.Deserialize<List<T>>(str);
                            }
                            catch (Exception ex) {
                            }
                        }
                        _items = items;
                    }
                }
            }
            return _items;
        }

        protected abstract string GetId(T obj);

        public T GetItem(string id) {
            T res = null;
            var items = GetItems();
            foreach (var item in items) {
                if (GetId(item).Equals(id)) {
                    res = item;
                }
            }
            return res;
        }

        public void AddItem(T model) {
            var items = GetItems();
            items.Add(model);
            SaveToFile();
        }

        public void EditItem(string id,T model) {
            var oldItem = GetItem(id);
            Debug.Assert(oldItem != null);
            GetItems().Remove(oldItem);
            GetItems().Add(model);
            SaveToFile();
        }

        public void Delete(string id) {
            T res = null;
            var items = GetItems();
            foreach (var error in items) {
                if (GetId(error).Equals(id)) {
                    res = error;
                }
            }
            if (null != res) {
                items.Remove(res);
                SaveToFile();
            }
        }

        protected void SaveToFile() {
            if (null != GetItems()) {
                var js = new JavaScriptSerializer();
                var json = js.Serialize(GetItems());

                lock (_lockObj) {
                    var file = HostingEnvironment.MapPath("~/data/") + FileName;
                    if (!File.Exists(file)) {
                        File.Create(file);
                    }
                    using (var sw = new StreamWriter(file)) {
                        var url = "";
                        sw.Write(json);
                    }
                }
            }
        }

        private string LoadFromFile(string str) {
            var file = HostingEnvironment.MapPath("~/data/") + FileName;
            lock (_lockObj) {
                if (File.Exists(file)) {
                    using (var sw = new StreamReader(file)) {
                        str = sw.ReadToEnd();
                    }
                }
                else {
                    using (File.Create(file)) {
                    }
                }
            }
            return str;
        }
    }
}