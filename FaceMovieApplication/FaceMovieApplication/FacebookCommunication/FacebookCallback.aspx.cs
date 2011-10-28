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
                    IFacebookController facebookController = new FacebookController();
                    //IDataManager dataManager = new DataManager();
                    FaceMovieModelContainer container = new FaceMovieModelContainer();
                    User user = new User();
                    Dictionary<String,DataFacebookUser> usersData = facebookController.GetUsersFacebookData(auth);
                    /*
                    user.UserIdFacebook = userData.UserId;
                    user.UserTokenFacebook = auth.Token;
                    user.UserLoginId = userData.Email;
                    user.UserBirthday = (userData.Birthday == null) ? string.Empty : userData.Birthday;
                    user.UserCinema = userData.Cinema;
                    user.UserFirstName = userData.FirstName;
                    user.UserGender = (userData.Gender == null) ? string.Empty : userData.Gender;
                    user.UserHometown = userData.Hometown;
                    user.UserLastName = userData.LastName;
                    user.UserMusic = userData.Music;
                    user.UserPictureLink = userData.PictureLink;
                    user.UserTelevision = userData.Television;
                    user.SubLevel = 0;
                    string codLevel = dataManager.GetParameter(Parameters.LevelRookie, container);
                    user.Level = container.Levels.Where(l => l.LevelName == codLevel).First();
                    
                    dataManager.StoreUser(user, container);

                    Response.Redirect(Constants.RedirectUrlAfterLoginFacebook);
                    */
                }
            }
        }
    }
}