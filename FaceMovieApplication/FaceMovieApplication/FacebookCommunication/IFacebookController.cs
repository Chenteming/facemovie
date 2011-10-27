//-----------------------------------------------------------------------
// <copyright file="IFacebookController.cs" company="Interpool">
//     Copyright Interpool. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace InterpoolCloudWebRole.FacebookCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using InterpoolCloudWebRole.Data;
    using InterpoolCloudWebRole.Datatypes;

    /// <summary>
    /// Interface Description IFacebookController
    /// </summary>
    public interface IFacebookController
    {
        /// <summary>
        /// Description for Method.</summary>
        /// <param name="auth"> Parameter description for auth goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        string GetUserId(OAuthFacebook auth);

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="userId"> Parameter description for userId goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        List<string> GetFriendsId(string userId);

        /// <summary>
        /// Returns all the user info by the token
        /// </summary>
        /// <param name="auth">Parameter description for auth goes here</param>
        /// <returns>Return results are described through the returns tag</returns>
        DataFacebookUser GetUserInfoByToken(OAuthFacebook auth);

        //// Saves all the friends information in the db.

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="auth"> Parameter description for auth goes here</param>
        /// <param name="game"> Parameter description for game goes here</param>
        /// <param name="limitSuspects"> Parameter description for limitSuspects goes here</param>
        /// <param name="context"> Parameter description for context goes here</param>
        void DownloadFacebookUserData(OAuthFacebook auth, Game game, int limitSuspects, InterpoolContainer context);
    }
}
