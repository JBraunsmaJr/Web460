// Jon Braunsma

using System;

namespace BraunsmaWeek1
{
    public partial class pgConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Only care about setting control values if this is a POST request
                if (!PreviousPage.IsCrossPagePostBack)
                    return;

                lblName.Text = $"{PreviousPage.FirstName} {PreviousPage.LastName}";
                lblAddress.Text = $"{PreviousPage.Street} {PreviousPage.City} {PreviousPage.State}";
                lblPhone.Text = PreviousPage.PhoneNumber;
                lblCCNumber.Text = PreviousPage.PhoneNumber;
                lblCCType.Text = PreviousPage.PaymentType;
            }
            catch(Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }
    }
}