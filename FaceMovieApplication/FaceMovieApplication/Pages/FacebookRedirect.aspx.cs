//-----------------------------------------------------------------------
// <copyright file="FacebookRedirect.aspx.cs" company="Interpool">
//     Copyright Interpool. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InterpoolCloudWebRole.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using FaceMovieApplication.FacebookCommunication;
    
    /// <summary>
    /// Partial class declaration FacebookRedirect
    /// </summary>
    public partial class FacebookRedirect : System.Web.UI.Page
    {
        /// <summary>
        /// Description for Method.</summary>
        /// <param name="sender"> Parameter description for sender goes here</param>
        /// <param name="e"> Parameter description for e goes here</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            OAuthFacebook oauth = new OAuthFacebook();
            Response.Redirect(oauth.AuthorizationLinkGet());
        }
    }
}