using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using FoodApp.Model;

namespace FoodApp.Common.Managers {
    public class MoneyLogger : ManagerBase<ngMoneyLoggerModel> {
        public static MoneyLogger Inst = new MoneyLogger();

        private MoneyLogger() {
        }

        protected override string FileName {
            get { return "moneyLogs.json"; }
        }

        protected override string GetId(ngMoneyLoggerModel obj) {
            return obj.OrderId;
        }

        private string GetOperatoinStr(EMoneyOperation operation) {
            string res = "";
            if (EMoneyOperation.Buy == operation) {
                res = "Знятя готівки";
            }
            else if (EMoneyOperation.AddMoneyToBank == operation) {
                res = "Поповнення готівки";
            }
            else if (EMoneyOperation.Refund == operation) {
                res = "Повернення готівки";
            }
            return res;
        }

        public bool CanRefund() {
            return false;
        }

        public void CreateBuyLog(ngUserModel user,decimal val, string orderId) {
            ngMoneyLoggerModel loggerModel = new ngMoneyLoggerModel();
            loggerModel.OrderId = orderId;
            loggerModel.Operation = EMoneyOperation.Buy;
            loggerModel.UserId = user.Id;
            loggerModel.Value = val;
            loggerModel.DateTime = DateTime.Now;

            WriteLogAndSendEmail(user,loggerModel);
        }

        public void CreateRefundLog(ngUserModel user,decimal val, string orderId) {
            ngMoneyLoggerModel loggerModel = new ngMoneyLoggerModel();
            loggerModel.OrderId = orderId;
            loggerModel.Operation = EMoneyOperation.Refund;
            loggerModel.UserId = user.Id;
            loggerModel.Value = val;
            loggerModel.DateTime = DateTime.Now;

            WriteLogAndSendEmail(user,loggerModel);
        }

        public void CreateAddMoneyLog(ngUserModel user, decimal total) {
            ngMoneyLoggerModel loggerModel = new ngMoneyLoggerModel();
            loggerModel.OrderId = "id -> add money";
            loggerModel.Operation = EMoneyOperation.AddMoneyToBank;
            loggerModel.UserId = user.Id;
            loggerModel.Value = total;
            loggerModel.DateTime = DateTime.Now;

            WriteLogAndSendEmail(user,loggerModel);
        }

        public void CreateRemoveMoneyLog(ngUserModel user, decimal total)
        {
            ngMoneyLoggerModel loggerModel = new ngMoneyLoggerModel();
            loggerModel.OrderId = "id -> remove money";
            loggerModel.Operation = EMoneyOperation.RemoveMoneyFromBank;
            loggerModel.UserId = user.Id;
            loggerModel.Value = total;
            loggerModel.DateTime = DateTime.Now;

            WriteLogAndSendEmail(user, loggerModel);
        }

        private void WriteLogAndSendEmail(ngUserModel user,ngMoneyLoggerModel loggerModel) {
            Debug.Assert(!string.IsNullOrEmpty(loggerModel.OrderId));
            AddItemAndSave(loggerModel);

            SendEmail(user, GetOperatoinStr(loggerModel.Operation), loggerModel.Value, "");
        }


        private void SendEmail(ngUserModel user, string operationName, decimal val, string msg) {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587) {Credentials = new NetworkCredential("gamgamlviv@gmail.com", "nikita1984"), EnableSsl = true};
            string subject = string.Format("{0} {1}", user.Email,DateTime.Now);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Операція: {0}\n", operationName);
            sb.AppendFormat("Сума: {0} грн\n", val);

            ngMoneyModel model = MoneyManager.Inst.EnsureMoneyModel(user);
            sb.AppendFormat("Залишок: {0} грн\n", model.Total);
            sb.AppendFormat("\ngamgamlviv@gmail.com");

            client.Send("gamgamlviv@gmail.com", "gamgamlviv@gmail.com", subject, sb.ToString());
            client.Send("gamgamlviv@gmail.com", user.Email, subject, sb.ToString());
        }
    }
}