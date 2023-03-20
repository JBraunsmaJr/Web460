using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProgramaholicsAnonymous
{
    public partial class AccountConfirmation : System.Web.UI.Page
    {
        public string Password
        {
            get => lblPassword.Text;
            set => lblPassword.Text = value;
        }

        public string Username
        {
            get => lblUsername.Text;
            set => lblUsername.Text = value;
        }

        public string City
        {
            get => txtCity.Text;
            set => txtCity.Text = value;
        }

        public string State
        {
            get => txtState.Text; 
            set => txtState.Text = value;
        }

        public string LeastFavoriteLanguage
        {
            get => txtLeastFavorite.Text;
            set => txtLeastFavorite.Text = value;
        }

        public string FavoriteLanguage
        {
            get => txtFavorite.Text;
            set => txtFavorite.Text = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {           
            Initialize();
        }

        void Initialize()
        {
            LoadData(Constants.USERNAME, nameof(lblUsername), x => Username = x);
            LoadData(Constants.CITY, nameof(txtCity), x => City = x);
            LoadData(Constants.STATE, nameof(txtState), x => State = x);
            LoadData(Constants.LEAST_FAVORITE_LANGUAGE, nameof(txtLeastFavorite), x => LeastFavoriteLanguage = x);
            LoadData(Constants.FAVORITE_LANGUAGE, nameof(txtFavorite), x => FavoriteLanguage = x);
            LoadData(Constants.PASSWORD, nameof(lblPassword), x => Password = x);
        }

        void LoadPreviousPageData()
        {
            Master.SetGlobalMessage("Please fill in your account information.", MessageType.Warning);
            Username = PreviousPage.Username;
            Password = PreviousPage.Password;
        }

        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            Master.SetGlobalMessage("Your account has been created");
            
            Session[Constants.USERNAME] = Username;
            Session[Constants.CITY] = City;
            Session[Constants.STATE] = State;
            Session[Constants.LEAST_FAVORITE_LANGUAGE] = LeastFavoriteLanguage;
            Session[Constants.FAVORITE_LANGUAGE] = FavoriteLanguage;

            Response.Redirect("AccountDetails.aspx");
        }

        /// <summary>
        /// Was having issues getting session data / postback information to carry over.
        /// This method helps grab data from session, then form if session does not have value
        /// </summary>
        /// <param name="sessionName">Name of field in Session to get value from (Prioritized)</param>
        /// <param name="formName">Name of field in the Form to get value from</param>
        /// <param name="setter">Action to perform with retrieved value</param>
        public void LoadData(string sessionName, string formName, Action<string> setter)
        {
            var value = string.Empty;

            if (Session[sessionName] != null)
            {
                value = Session[sessionName].ToString();
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                value = SearchForFormValue(formName);
            }

            setter(value);
        }

        /// <summary>
        /// Grab Form Control from FORM, by <paramref name="controlName"/>
        /// </summary>
        /// <param name="controlName"></param>
        /// <returns></returns>
        string SearchForFormValue(string controlName)
        {
            foreach (var control in Form.Controls)
            {
                if (control.GetType() == typeof(TextBox))
                {
                    var box = (TextBox)control;
                    if (box.ID != controlName)
                        continue;
                    return box.Text;
                }

                if (control.GetType() == typeof(Label))
                {
                    var label = (Label)control;
                    if (label.ID == controlName)
                        return label.Text;
                }
            }

            return null;
        }
    }
}