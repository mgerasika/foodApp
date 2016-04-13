using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace FoodApp.Common.Managers
{
    public abstract class ManagerBase<T> where T : ngModelBase
    {
        protected static readonly object _lockObj = new object();
        private List<T> _items;

        protected abstract string FileName { get; }

        protected List<T> GetItems() {
            if (null == _items) {
                lock (_lockObj) {
                    if (null == _items) {
                        List<T> items = new List<T>();
                        string str = "";

                        str = LoadFromFile(str);
                        if (!string.IsNullOrEmpty(str)) {
                            try {
                                items = JsonConvert.DeserializeObject<List<T>>(str);
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

        protected T GetItem(string id) {
            T res = null;
            List<T> items = GetItems();
            foreach (T item in items) {
                if (GetId(item).Equals(id)) {
                    res = item;
                }
            }
            return res;
        }

        public virtual void Add(T model)
        {
            List<T> items = GetItems();
            items.Add(model);
        }

        public void AddItemAndSave(T model) {
           Add(model);
            Save();
        }

        public void EditItem(string id,T model) {
            T oldItem = GetItem(id);
            Debug.Assert(oldItem != null);
            GetItems().Remove(oldItem);
            GetItems().Add(model);
            Save();
        }

        public void Delete(string id) {
            T res = null;
            List<T> items = GetItems();
            foreach (T error in items) {
                if (GetId(error).Equals(id)) {
                    res = error;
                }
            }
            if (null != res) {
                items.Remove(res);
                Save();
            }
        }

        public void Save() {
            if (null != GetItems()) {
                string json = JsonConvert.SerializeObject(GetItems(),Formatting.Indented);

                lock (_lockObj) {
                    string file = HostingEnvironment.MapPath("~/data/") + FileName;
                    if (!File.Exists(file)) {
                        File.Create(file);
                    }
                    using (StreamWriter sw = new StreamWriter(file)) {
                        string url = "";
                        sw.Write(json);
                    }
                }
            }
        }


        private string LoadFromFile(string str) {
            string file = HostingEnvironment.MapPath("~/data/") + FileName;
            lock (_lockObj) {
                if (File.Exists(file)) {
                    using (StreamReader sw = new StreamReader(file)) {
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