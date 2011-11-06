using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceMovieApplication.Algorithm
{
    interface IItemBasedAlgorithm
    {
        double ComputeSimilarity(Movie movie1, Movie movie2, FaceMovieModelContainer context);
    }
}
