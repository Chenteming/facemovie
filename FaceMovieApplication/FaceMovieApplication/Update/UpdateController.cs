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
            /*
             *  For Each movie1 in Movies
             *      For Each movie2 <> movie1 in Movies
             *          sim = ComputeSimilarity(movie1, movie2)
             *          save(movie1, movie2, sim)
             *      EndFor
             * EndFor
            */
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            DataManager dm = new DataManager();
            MovieSimilarity movieSimilarity;
            IItemBasedAlgorithm iba = new ItemBasedAlgorithm();
            List<MovieSimilarity> movieSimilaritySet;

            int amountMovieSimilarities = int.Parse(dm.GetParameter(Parameters.AmountSimilarMovies));
            //try
            //{
            //    dm.DeleteAllMovieSimilarities(context);
            //    foreach (Movie movie1 in context.MovieSet)
            //    {
            //        movieSimilaritySet = new List<MovieSimilarity>();
            //        foreach (Movie movie2 in context.MovieSet)
            //        {
            //            if (movie1.MovieId != movie2.MovieId)
            //            {
            //                movieSimilarity = new MovieSimilarity();
            //                movieSimilarity.Movie_1 = movie1;
            //                movieSimilarity.Movie_2 = movie2;
            //                movieSimilarity.Similarity = ibm.ComputeSimilarity(movie1, movie2);
            //                movieSimilaritySet.Add(movieSimilarity);
            //            }
            //        }
            //        movieSimilaritySet.Sort(CompareMoviesBySimilarity);
            //        movieSimilaritySet.Take(amountMovieSimilarities);
            //        foreach (MovieSimilarity ms in movieSimilaritySet)
            //        {
            //            context.AddToMovieSimilaritySet(ms);
            //        }
            //    }
            //    context.SaveChanges();
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}
            List<DataUserMovieRating> listDataUserMovieRating = new List<DataUserMovieRating>();
            DataUserMovieRating userMovieRating;
            HashSet<Movie> relatedMovies;
            Debug.WriteLine(" Hola");
            movieSimilaritySet = null;
            try
            {
                dm.DeleteAllMovieSimilarities(context);
                foreach (Movie movie1 in context.MovieSet)
                {
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
                        if (movieSimilarity.Movie_1 == null || movieSimilarity.Movie_1Reference == null)
                        {
                            Debug.WriteLine("Movie nula!");
                        }
                        if (movieSimilarity.Movie_2 == null || movieSimilarity.Movie_2Reference == null)
                        {
                            Debug.WriteLine("Movie nula!");
                        }
                        movieSimilarity.Similarity = iba.ComputeSimilarity(movie1, relatedMovie, context);
                        movieSimilaritySet.Add(movieSimilarity);
                    }
                    Debug.WriteLine(movie1.MovieId + " " + movie1.MovieName);
                    movieSimilaritySet.Sort(CompareMoviesBySimilarity);
                    movieSimilaritySet.Take(amountMovieSimilarities);
                    foreach (MovieSimilarity ms in movieSimilaritySet)
                    {
                        context.AddToMovieSimilaritySet(ms);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(movieSimilaritySet.ToString());
                throw ex;
            }
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