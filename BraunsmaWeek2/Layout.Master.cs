using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BraunsmaWeek2
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        public string UserFeedback
        {
            get => lblUserFeedback.Text;
            set => lblUserFeedback.Text = value;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                lblGUID.Text = System.Guid.NewGuid().ToString();
        }
    }
}