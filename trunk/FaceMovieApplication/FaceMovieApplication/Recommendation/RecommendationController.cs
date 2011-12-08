using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceMovieApplication.Datatypes;

namespace FaceMovieApplication
{
    public class RecommendationController
    {
        public List<DataUserMovieRating> UserGetRecommendation(int userId)
        {
            List<DataUserMovieRating> movies = new List<DataUserMovieRating>();
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            var candidateUserMovies =   from um in context.UserMovieSet
                                        where um.User.UserId == userId
                                        select um.Movie.MovieId;
            var candidateMovies = from movie in context.MovieSet
                                  where candidateUserMovies.Contains(movie.MovieId)
                                  select movie;
            var userMoviesIds = from um in context.UserMovieSet
                                where um.User.UserId == userId
                                select um.Movie.MovieId;
            foreach (Movie movie in candidateMovies)
            {
                var relatedMovies = from ms in context.MovieSimilaritySet
                                    where ms.Movie_1.MovieId == movie.MovieId && (!userMoviesIds.Contains(ms.Movie_2.MovieId))
                                    select ms;

                double upperSum = 0;

                if (relatedMovies != null && relatedMovies.Count() > 0)
                {
                    foreach (MovieSimilarity ms in relatedMovies)
                    {
                        var userRating = from um in context.UserMovieSet
                                         where um.Movie.MovieId == ms.Movie_2.MovieId && um.User.UserId == userId
                                         select um.UserMovieRanking;
                        //// If the user rated the movie
                        if (userRating != null && userRating.Count() > 0)
                        {
                            upperSum += userRating.First();
                        }
                    }

                    var relatedMoviesAbsSimilarity = from ms in relatedMovies
                                                     select Math.Abs(ms.Similarity);
                    double bottomSum = relatedMoviesAbsSimilarity.Sum();

                    double prediction;
                    if (bottomSum != 0)
                    {
                        prediction = upperSum / bottomSum;
                    }
                    else
                    {
                        prediction = 0;
                    }

                    DataUserMovieRating dumr = new DataUserMovieRating();
                    dumr.UserId = userId;
                    dumr.MovieId = movie.MovieId;
                    dumr.Rating = prediction;

                    movies.Add(dumr);
                }
            }
            movies.Sort(CompareUserMovieRating);
            movies.Reverse();
            return movies;
        }

        private static int CompareUserMovieRating(DataUserMovieRating dumr1, DataUserMovieRating dumr2)
        {
            if (dumr1 == null)
            {
                if (dumr2 == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (dumr2 == null)
                {
                    return 1;
                }
                else
                {
                    if (dumr1.Rating > dumr2.Rating)
                    {
                        return 1;
                    }
                    else if (dumr1.Rating < dumr2.Rating)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}