using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceMovieApplication.Data;
using FaceMovieApplication.Algorithm;
using FaceMovieApplication.Utilities;
using FaceMovieApplication.Datatypes;
using System.Diagnostics;

namespace FaceMovieApplication.Update
{
    public class UpdateController
    {
        public void UpdateSimilarities()
        {
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            DataManager dm = new DataManager();
            MovieSimilarity movieSimilarity;
            IItemBasedAlgorithm iba = new ItemBasedAlgorithm();
            List<MovieSimilarity> movieSimilaritySet, movieSimilarityTotalSet;

            int amountMovieSimilarities = int.Parse(dm.GetParameter(Parameters.AmountSimilarMovies));
            List<DataUserMovieRating> listDataUserMovieRating = new List<DataUserMovieRating>();
            DataUserMovieRating userMovieRating;
            HashSet<Movie> relatedMovies;
            movieSimilaritySet = null;
            movieSimilarityTotalSet = new List<MovieSimilarity>();
            int i = 0;
            try
            {
                //dm.DeleteAllMovieSimilarities(context);
                foreach (Movie movie1 in context.MovieSet)
                {
                    i++;
                    relatedMovies = new HashSet<Movie>();
                    var users = from us in context.UserMovieSet
                                where us.Movie.MovieId == movie1.MovieId
                                select us.User;
                    foreach (User user in users.AsEnumerable())
                    {
                        var movies = from us in context.UserMovieSet
                                     where us.User.UserId == user.UserId && us.Movie.MovieId != movie1.MovieId
                                     select us.Movie;
                        foreach (Movie movie2 in movies.AsEnumerable())
                        {
                            relatedMovies.Add(movie2);
                        }
                    }

                    movieSimilaritySet = new List<MovieSimilarity>();
                    foreach (Movie relatedMovie in relatedMovies)
                    {
                        movieSimilarity = new MovieSimilarity();
                        movieSimilarity.Movie_1 = movie1;
                        movieSimilarity.Movie_2 = relatedMovie;
                        movieSimilarity.Similarity = iba.ComputeSimilarity(movie1, relatedMovie, context);
                        movieSimilaritySet.Add(movieSimilarity);
                    }
                    Debug.WriteLine(movie1.MovieId + " " + movie1.MovieName);
                    movieSimilaritySet.Sort(CompareMoviesBySimilarity);
                    movieSimilaritySet = movieSimilaritySet.Take(1).ToList();
                    foreach (MovieSimilarity ms in movieSimilaritySet)
                    {
                        context.AddToMovieSimilaritySet(ms);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateMoviesInformation()
        {
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            IMovieController mc = new MovieController();
            Movie movie;
            foreach (Movie m in context.MovieSet)
            {
                try
                {
                    //// Find with the IMDB api the information about the movie
                    movie = mc.GetMovieInfoByTitle(m.MovieName);

                    //// Store it
                    m.MovieRanking = movie.MovieRanking;
                    m.MovieImageUrl = movie.MovieImageUrl;
                    m.MovieGenre = movie.MovieGenre;

                    Debug.WriteLine(m.MovieId + " " + m.MovieName);
                }
                catch
                {
                    //// It some kind of error occurs during obtaining the information, the function
                    //// continues bringing data from other movies.
                }
            }
            context.SaveChanges();
        }

        private static int CompareMoviesBySimilarity(MovieSimilarity ms1, MovieSimilarity ms2)
        {
            if (ms1 == null)
            {
                if (ms2 == null)
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
                if (ms2 == null)
                {
                    return 1;
                }
                else
                {
                    if (ms1.Similarity > ms2.Similarity)
                    {
                        return 1;
                    }
                    else if (ms1.Similarity < ms2.Similarity)
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