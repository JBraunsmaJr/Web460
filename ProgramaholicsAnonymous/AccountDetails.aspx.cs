using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProgramaholicsAnonymous
{
    public partial class AccountDetails : System.Web.UI.Page
    {
        public string Username
        {
            get => lblUsername.Text;
            set => lblUsername.Text = value;
        }

        public string City
        {
            get => lblCity.Text;
            set => lblCity.Text = value;
        }

        public string State
        {
            get => lblState.Text;
            set => lblState.Text = value;
        }

        public string LeastFavoriteLanguage
        {
            get => lblLeastFavorite.Text;
            set => lblLeastFavorite.Text = value;
        }

        public string FavoriteLanguage
        {
            get => lblFavorite.Text;
            set => lblFavorite.Text = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeThroughSession();
        }

        private void InitializeThroughSession()
        {
            if (Session[Constants.USERNAME] != null)
                Username = Session[Constants.USERNAME].ToString();
            if (Session[Constants.CITY] != null)
                City = Session[Constants.CITY].ToString();
            if (Session[Constants.STATE] != null)
                State = Session[Constants.STATE].ToString();
            if (Session[Constants.LEAST_FAVORITE_LANGUAGE] != null)
                LeastFavoriteLanguage = Session[Constants.LEAST_FAVORITE_LANGUAGE].ToString();
            if (Session[Constants.FAVORITE_LANGUAGE] != null)
                FavoriteLanguage = Session[Constants.FAVORITE_LANGUAGE].ToString();
        }

        protected void btnExportStats_Click(object sender, EventArgs e)
        {
            Master.SetGlobalMessage("Placeholder: Stats exported");
        }

        protected void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            Master.SetGlobalMessage("Placeholder: User Deleted");
        }

        protected void btnEditDetails_Click(object sender, EventArgs e)
        {
            Session[Constants.USERNAME] = Username;
            Session[Constants.CITY] = City;
            Session[Constants.STATE] = State;
            Session[Constants.FAVORITE_LANGUAGE] = FavoriteLanguage;
            Session[Constants.LEAST_FAVORITE_LANGUAGE] = LeastFavoriteLanguage;
            Response.Redirect("EditAccountDetails.aspx");
        }
    }
}