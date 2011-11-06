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
        public double ComputeSimilarity(Movie movie1, Movie movie2, FaceMovieModelContainer context)
        {
            //// Adjusted Cosine Similarity
            DataUserMovieRating[] arrayDataUserMovieRating = GetUsersRatingsByMovies(movie1, movie2, context);
            double upperSum = 0;
            double bottomLeftSum = 0;
            double bottomRightSum = 0;
            int i = 0;
            int count = arrayDataUserMovieRating.Length;
            double similarity = 0;
            if (count > 0)
            {
                while (i < count)
                {
                    upperSum += (arrayDataUserMovieRating[i].Rating - arrayDataUserMovieRating[i].AverageRating) *
                        (arrayDataUserMovieRating[i + 1].Rating - arrayDataUserMovieRating[i].AverageRating);
                    bottomLeftSum += Math.Pow((arrayDataUserMovieRating[i].Rating - arrayDataUserMovieRating[i].AverageRating), 2);
                    bottomRightSum += Math.Pow((arrayDataUserMovieRating[i + 1].Rating - arrayDataUserMovieRating[i + 1].AverageRating), 2);
                    i += 2;
                }
                if (bottomLeftSum != 0 && bottomRightSum != 0)
                {
                    similarity = upperSum / (Math.Sqrt(bottomLeftSum) * Math.Sqrt(bottomRightSum));
                }
            }
            return similarity;
        }

        private DataUserMovieRating[] GetUsersRatingsByMovies(Movie movie1, Movie movie2, FaceMovieModelContainer context)
        {
            //// First, the users who have seen both films are isolated
            var usersIds =  from um in context.UserMovieSet
                            where um.Movie.MovieId == movie1.MovieId
                            select um.User.UserId;
            var usersIds2 = from um in context.UserMovieSet
                            where um.Movie.MovieId == movie2.MovieId && usersIds.Contains(um.User.UserId)
                            select um.User.UserId;
            //// Then, all the UserMoviesRatings are obtained
            IQueryable<UserMovie> umSet =   from um in context.UserMovieSet
                                            where (um.Movie.MovieId == movie1.MovieId || um.Movie.MovieId == movie2.MovieId) && usersIds2.Contains(um.User.UserId)
                                            orderby um.User.UserId, um.Movie.MovieId
                                            select um;
            List<DataUserMovieRating> listDataUserMovieRating = new List<DataUserMovieRating>();
            DataUserMovieRating userMovieRating, userMovieRatingTemp;
            userMovieRatingTemp = null;
            foreach (UserMovie um in umSet)
            {
                userMovieRating = new DataUserMovieRating();
                userMovieRating.UserId = um.User.UserId;
                userMovieRating.MovieId = um.Movie.MovieId;
                userMovieRating.Rating = um.UserMovieRanking;
                //// If the user is already in the collection, then it's no necessary to get the average again
                if (userMovieRatingTemp != null)
                {
                    userMovieRating.AverageRating = userMovieRatingTemp.AverageRating;
                    userMovieRatingTemp = null;
                }
                else
                {
                    userMovieRating.AverageRating = GetUserAverageByUserId(userMovieRating.UserId, umSet);
                    userMovieRatingTemp = userMovieRating;
                }
                listDataUserMovieRating.Add(userMovieRating);
            }
            return listDataUserMovieRating.ToArray();
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