using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceMovieApplication.Algorithm;
using FaceMovieApplication.Datatypes;

namespace FaceMovieApplication.Algorithm
{
    public class ItemBasedAlgorithm : IItemBasedAlgorithm
    {
        public double ComputeSimilarity(Movie movie1, Movie movie2)
        {
            //// Adjusted Cosine Similarity
            //// sum( Ranking(u,i) * Average(Ranking(u)) ) / ( square( sum ( Average(Ranking(u) ) ) * square ( ) )
            return 1;
        }

        public void GetUsersRatingsByMovies(Movie movie1, Movie movie2)
        {
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            IQueryable<UserMovie> umSet = from um in context.UserMovieSet
                                          where um.Movie.MovieId == movie1.MovieId || um.Movie.MovieId == movie2.MovieId
                                          orderby um.User.UserId, um.Movie.MovieId
                                          select um;
            Dictionary<long,DataUserMovieRating> dictDataUserMovieRating = new Dictionary<long,DataUserMovieRating>();
            DataUserMovieRating userMovieRating;
            foreach (UserMovie um in umSet)
            {
                userMovieRating = new DataUserMovieRating();
                userMovieRating.UserId = um.User.UserId;
                userMovieRating.MovieId = um.Movie.MovieId;
                userMovieRating.Rating = um.UserMovieRanking;
                userMovieRating.AverageRating = GetUserAverageByUserId(userMovieRating.UserId, umSet);
            }
        }

        private double GetUserAverageByUserId(int userId, IQueryable<UserMovie> userMovieSet)
        {
            var allRatings = from um in userMovieSet
                             where um.User.UserId == userId
                             select um.UserMovieRanking;
            return allRatings.Average();
        }
    }
}