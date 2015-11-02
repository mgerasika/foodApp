using FoodApp.Common;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class eventManager
    {
        public static readonly string dayOfWeekChanged = "dayOfWeekChanged";
        public static readonly string userIdChanged = "userIdChanged";
        public static readonly string settingsLoaded = "settingsLoaded";
        public static readonly string orderCompleted = "orderCompleted";

        public static eventManager inst = new eventManager();
        private JsObject _dict = new JsObject();

        private eventManager()
        {
        }

        private JsArray GetHandlersByName(string name)
        {
            if (_dict[name] == null)
            {
                _dict[name] = new JsArray();
            }
            return _dict[name].As<JsArray>();
        }

        public void subscribe<T>(string eventName, JsAction<T> action)
        {
            JsArray array = GetHandlersByName(eventName);
            array.Add(action);
        }

        public void fire<T>(string name, T arg)
        {
            JsArray array = GetHandlersByName(name);
            foreach (object obj in array)
            {
                JsAction<T> action = obj.As<JsAction<T>>();
                action(arg);
            }
        }
    }
}