using BraunsmaWeek3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Validator = Database.Validator;

namespace BraunsmaWeek3
{
    public partial class pgCheckOut : System.Web.UI.Page
    {
        public Database.BadgerDbContext BadgerDbContext;
        Dictionary<string, Order> _cachedOrders = new Dictionary<string, Order>();

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

        public int? CustomerId
        {
            get
            {
                if (int.TryParse(lblCustomerId.Text, out var id))
                    return id;

                return null;
            }

            set => lblCustomerId.Text = value?.ToString();
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

            var orders = Master.Database.GetAll<Order>();
            _cachedOrders = orders.ToDictionary(x => x.LastName, x => x);

            var index = dropdownLastNames.SelectedIndex;
            dropdownLastNames.Items.Clear();
            dropdownLastNames.Items.Add(new ListItem());
            foreach (var order in orders)
                dropdownLastNames.Items.Add(new ListItem(order.LastName));

            if (index > 0)
            {
                dropdownLastNames.SelectedIndex = index;
                var value = dropdownLastNames.Items[index].Text;
                UpdateFields(_cachedOrders[value]);
            }
            else 
                ClearInputs();
        }

        private void ClearInputs()
        {
            PhoneNumber = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Street = string.Empty;
            PaymentType = "Visa";
            CreditCardNumber = string.Empty;
            CustomerId = null;
        }

        protected void ClearForm_OnClick(object sender, EventArgs e)
        {
            ClearInputs();
        }

        protected void btnAddCustomer_OnClick(object sender, EventArgs e)
        {
            try
            {
                var order = new Order()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    City = City,
                    State = State,
                    Street = Street,
                    PaymentType = PaymentType,
                    CreditCardNumber = CreditCardNumber,
                    PhoneNumber = PhoneNumber
                };

                Master.Database.Insert(order);

                Master.UserFeedback = $"Customer '{LastName}, {FirstName}' has been added";
                
                if(!_cachedOrders.ContainsKey(LastName))
                {
                    _cachedOrders.Add(LastName, order);
                    dropdownLastNames.Items.Add(new ListItem(LastName));
                }
            }
            catch(ValidationException ex)
            {
                Master.UserFeedback = ex.Message;
            }
        }

        void UpdateFields(Order order)
        {
            FirstName = order.FirstName;
            LastName = order.LastName;
            Street = order.Street;
            City = order.City;
            State = order.State;
            PhoneNumber = order.PhoneNumber;
            PaymentType = order.PaymentType;
            CreditCardNumber = order.CreditCardNumber;
            CustomerId = order.Id;
        }

        protected void btnFindLastName_OnClick(object sender, EventArgs e)
        {
            try
            {
                var result = Master.Database.FirstOrDefault<Order>(x => x.LastName == LastName);

                if(result == null)
                {
                    Master.UserFeedback = $"No records were found!";
                }
                else
                {
                    UpdateFields(result);
                    Master.UserFeedback = "Record Found";
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex);
                Master.UserFeedback = "Sorry, there was an error processing your request.";
            }
        }

        protected void btnUpdateCustomer_OnClick(object sender, EventArgs e)
        {
            try
            {
                if(!CustomerId.HasValue)
                {
                    Master.UserFeedback = "Invalid Customer Id";
                    return;
                }

                var order = new Order()
                {
                    Id = CustomerId.Value,
                    FirstName = FirstName,
                    LastName = LastName,
                    Street = Street,
                    City = City,
                    State = State,
                    PaymentType = PaymentType,
                    PhoneNumber = PhoneNumber,
                    CreditCardNumber = CreditCardNumber
                };

                var result = Master.Database.FindById<Order>(CustomerId.Value);

                if(result == null)
                {
                    Master.UserFeedback = "Error updating customer, please check form data.";
                }
                else
                {
                    Master.Database.Update(order);
                    Master.UserFeedback = "Customer Updated Successfully.";
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex);
                Master.UserFeedback = "Sorry, there was an error processing your request.";
            }
        }
    }
}