//-----------------------------------------------------------------------
// <copyright file="IFacebookController.cs" company="Interpool">
//     Copyright Interpool. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceMovieApplication.Update
{
    /// <summary>
    /// Interface Description IFacebookController
    /// </summary>
    interface IMovieController
    {
        /// <summary>
        /// Description for Method.</summary>
        /// <param name="movieTitle"> Parameter description for auth goes here</param>
        Movie GetMovieInfoByTitle(string movieTitle);
    }
}
