// Jon Braunsma

using System;
using System.Linq;
using System.Web.UI;
using BraunsmaWeek3.Models;

namespace BraunsmaWeek3
{
    public partial class pgConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                /*
                 *  When the user first navigates to this page
                 *  there "previous page" will be set.
                 *  After the user has confirmed their order, the previous
                 *  page will no longer be set. Throwing a null reference exception
                 */
                if(PreviousPage != null && PreviousPage.IsCrossPagePostBack)
                    LoadPreviousPageData();
                
                // IsPostBack means we confirmed our order.
                Master.UserFeedback = !IsPostBack ? "Please confirm your billing information." : "Your order has been submitted for processing.";
            }
            catch
            {
                Master.UserFeedback = "Sorry, there was an error processing your request.";
            }
        }
        
        Order BuildOrder() => new Order
        {
            FirstName = lblFirstName.Text,
            LastName = lblLastName.Text,
            Street = lblStreet.Text,
            City = lblCity.Text,
            State = lblState.Text,
            PhoneNumber = lblPhone.Text,
            CreditCardNumber = lblCCNumber.Text,
            PaymentType = lblCCType.Text
        };

        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            var results = Database.Validator.Validate(BuildOrder());

            if (results.Any())
            {
                Master.UserFeedback = string.Join("<br/>", results.Select(x => x.ErrorMessage));
                return;
            }
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        private void LoadPreviousPageData()
        {
            lblName.Text = $"{PreviousPage.FirstName} {PreviousPage.LastName}";
            lblAddress.Text = $"{PreviousPage.Street} {PreviousPage.City} {PreviousPage.State}";
            lblPhone.Text = PreviousPage.PhoneNumber;
            lblCCNumber.Text = PreviousPage.CreditCardNumber;
            lblCCType.Text = PreviousPage.PaymentType;
        }
    }
}