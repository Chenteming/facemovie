using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FaceMovieApplication.FacebookCommunication;

namespace FaceMovieApplication.Management
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// test login
        /// </summary>
        /// <param name="sender">Parameter description for sender goes here</param>
        /// <param name="e">Parameter description for e goes here</param>
        protected void Login_Click(object sender, EventArgs e)
        {
            OAuthFacebook oauth = new OAuthFacebook();
            Response.Redirect(oauth.AuthorizationLinkGet());
        }
    }
}