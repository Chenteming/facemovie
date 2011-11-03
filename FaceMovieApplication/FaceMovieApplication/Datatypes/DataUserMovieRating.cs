using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceMovieApplication.Datatypes
{
    /// <summary>
    /// Class statement DataUserMovieRating
    /// </summary>
    public class DataUserMovieRating
    {
        /// <summary>
        /// Gets or sets for Method.</summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public int MovieId { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public double AverageRating { get; set; }
    }
}