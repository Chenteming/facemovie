//-----------------------------------------------------------------------
// <copyright file="FacebookCallback.aspx.cs" company="Interpool">
//     Copyright Interpool. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace FaceMovieApplication.FacebookCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using FaceMovieApplication;
    using FaceMovieApplication.Datatypes;
    using FaceMovieApplication.FacebookCommunication;
    using FaceMovieApplication.Data;
    
    /// <summary>
    /// Partial class declaration Face
    /// </summary>
    public partial class FacebookCallback : System.Web.UI.Page
    {
        /// <summary>
        /// Description for Method.</summary>
        /// <param name="sender"> Parameter description for sender goes here</param>
        /// <param name="e"> Parameter description for e goes here</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            OAuthFacebook auth = new OAuthFacebook();

            if (Request["code"] == null)
            {
                //// Redirect the user back to Facebook for authorization.
                Response.Redirect(auth.AuthorizationLinkGet());
            }
            else
            {
                //// Gets the access token and secret.
                auth.AccessTokenGet(Request["code"]);

                if (auth.Token.Length > 0)
                {
                    DataManager dm = new DataManager();
                    IFacebookController facebookController = new FacebookController();
                    //IDataManager dataManager = new DataManager();
                    FaceMovieModelContainer container = new FaceMovieModelContainer();
                    User user = new User();
                    Dictionary<long,User> usersData = facebookController.GetUsersFacebookData(auth);
                    dm.StoreUsersInformation(usersData, container);
                    /*
                    Response.Redirect(Constants.RedirectUrlAfterLoginFacebook);
                    */
                }
            }
        }
    }
}