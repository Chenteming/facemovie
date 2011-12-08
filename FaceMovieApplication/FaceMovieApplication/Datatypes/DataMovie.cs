using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceMovieApplication.Datatypes
{
    public class DataMovie
    {
        /// <summary>
        /// Gets or sets for Method.</summary>
        public int MovieId { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string MovieName { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public double MovieRanking { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string MovieGenre { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string MovieImageUrl { get; set; }
        
        /// <summary>
        /// Gets or sets for Method.</summary>
        public string MovieUrl { get; set; }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string MoviePlot { get; set; }
    }
}