using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceMovieApplication
{
    public class RecommendationController
    {
        List<Movie> UserGetRecommendation(int userId)
        {
            List<Movie> movies = new List<Movie>();
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            var candidateUserMovies =   from um in context.UserMovieSet
                                        where um.User.UserId == userId
                                        select um.Movie.MovieId;
            var candidateMovies = from movie in context.MovieSet
                                  where candidateUserMovies.Contains(movie.MovieId)
                                  select movie;
            foreach (Movie movie in candidateMovies)
            {
                //// TODO: Compute wheighted sum and store in order (must create a new datatype)
                movies.Add(movie);
            }
            return movies;
        }
    }
}