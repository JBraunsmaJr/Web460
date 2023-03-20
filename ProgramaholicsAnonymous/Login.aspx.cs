using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProgramaholicsAnonymous
{
    public partial class Login : System.Web.UI.Page
    {
        public string Username
        {
            get => txtUsername.Text;
            set => txtUsername.Text = value;
        }

        public string Password
        {
            get => txtPassword.Text;
            set => txtPassword.Text = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Session[Constants.USERNAME] = Username;
            Response.Redirect("AccountDetails.aspx");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Session[Constants.USERNAME] = Username;
            Response.Redirect("AccountConfirmation.aspx");
        }
    }
}