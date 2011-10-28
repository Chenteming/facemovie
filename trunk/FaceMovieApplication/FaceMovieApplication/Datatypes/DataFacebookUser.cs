//-----------------------------------------------------------------------
// <copyright file="DataFacebookUser.cs" company="Interpool">
//     Copyright Interpool. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace FaceMovieApplication.Datatypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using FaceMovieApplication.FacebookCommunication;

    /// <summary>
    /// Class statement DataFacebookUser
    /// </summary>
    public class DataFacebookUser
    {
        /// <summary>
        /// Gets or sets for Method.</summary>
        public string FacebookUserId { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public OAuthFacebook OAuth { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string Gender { get; set; }
        
        /// <summary>
        /// Gets or sets for Method.</summary>
        public string PictureLink { get; set; }

    }
}