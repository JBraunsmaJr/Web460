using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProgramaholicsAnonymous
{
    public enum MessageType
    {
        Success, Warning, Error
    }

    public partial class Layout : System.Web.UI.MasterPage
    {
        const string ErrorCss = "text-danger bg-dark";
        const string WarningCss = "text-warning bg-dark";
        const string SuccessCss = "text-success bg-dark";

        public string Username
        {
            get => lblUsername.Text; 
            set => lblUsername.Text = value; 
        }

        private string GlobalMessage
        {
            get => lblGlobalMessage.Text;
            set => lblGlobalMessage.Text = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constants.MESSAGE] != null && !string.IsNullOrWhiteSpace(Session[Constants.MESSAGE].ToString()))
            {
                var source = Session[Constants.MESSAGE].ToString();
                var split = source.Split('|');

                switch (split[1].ToLower())
                {
                    case "error":
                        SetGlobalMessage(split[0], MessageType.Error);
                        break;
                    case "warning":
                        SetGlobalMessage(split[0], MessageType.Warning);
                        break;
                    default:
                        SetGlobalMessage(split[0]);
                        break;
                }
            }
            else
                SetGlobalMessage(null);
        }

        

        public void SetGlobalMessage(string message, MessageType type = MessageType.Success)
        {
            Session[Constants.MESSAGE] = $"{message}|{type}";

            if(string.IsNullOrWhiteSpace(message))
            {
                GlobalMessage = null;
                lblGlobalMessage.CssClass = string.Empty;
                return;
            }

            switch(type)
            {
                case MessageType.Success:
                    lblGlobalMessage.CssClass = SuccessCss;
                    break;
                case MessageType.Warning:
                    lblGlobalMessage.CssClass = WarningCss;
                    break;
                case MessageType.Error:
                    lblGlobalMessage.CssClass = ErrorCss;
                    break;
            }

            GlobalMessage = message;
        }
    }
}