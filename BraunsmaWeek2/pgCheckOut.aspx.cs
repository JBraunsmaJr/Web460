using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BraunsmaWeek2
{
    public partial class pgCheckOut : System.Web.UI.Page
    {
        /// <summary>
        /// Manage First Name value on form through <see cref="txtFirstName"/>
        /// </summary>
        public string FirstName
        {
            get => txtFirstName?.Text ?? string.Empty;
            private set => txtFirstName.Text = value;
        }

        /// <summary>
        /// Manage Last Name value on form through <see cref="txtLastName"/>
        /// </summary>
        public string LastName
        {
            get => txtLastName?.Text ?? string.Empty;
            private set => txtLastName.Text = value;
        }

        /// <summary>
        /// Manage Street value on form through <see cref="txtStreet"/>
        /// </summary>
        public string Street
        {
            get => txtStreet?.Text ?? string.Empty;
            private set => txtStreet.Text = value;
        }

        /// <summary>
        /// Manage State value on from through <see cref="txtState"/>
        /// </summary>
        public string State
        {
            get => txtState?.Text ?? string.Empty;
            private set => txtState.Text = value;
        }

        /// <summary>
        /// Manage City value on form through <see cref="txtCity"/>
        /// </summary>
        public string City
        {
            get => txtCity?.Text ?? string.Empty;
            private set => txtCity.Text = value;
        }

        /// <summary>
        /// Manage Credit Card Number on form through <see cref="txtCCNumber"/>
        /// </summary>
        public string CreditCardNumber
        {
            get => txtCCNumber?.Text ?? string.Empty;
            private set => txtCCNumber.Text = value;
        }

        /// <summary>
        /// Manage Payment Type on form through <see cref="rblCCType"/>
        /// </summary>
        /// <remarks>
        ///     <para>Valid values are "Visa", "Master Card", or "Discover".</para>
        ///     <para>Default value (if not provided, or invalid) is "Visa"</para>
        /// </remarks>
        public string PaymentType
        {
            get => rblCCType.SelectedValue ?? "Visa";
            private set
            {
                if(string.IsNullOrEmpty(value))
                {
                    rblCCType.SelectedIndex = 0;
                    return;
                }

                switch(value.ToLower().Trim())
                {
                    case "master card":
                        rblCCType.SelectedIndex = 1;
                        break;
                    case "discover":
                        rblCCType.SelectedIndex = 2;
                        break;
                    default:
                        rblCCType.SelectedIndex = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Manage Phone Number value on form through <see cref="txtPhone"/>
        /// </summary>
        public string PhoneNumber
        {
            get => txtPhone?.Text ?? string.Empty;
            set => txtPhone.Text = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.UserFeedback = "Please enter billing information.";
        }
    }
}