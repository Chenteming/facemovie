using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceMovieApplication.Data
{
    public class DataManager
    {
        /// <summary>
        /// Description for Method.</summary>
        /// <param name="user"> Parameter description for user goes here</param>
        /// <param name="context"> Parameter description for context goes here</param>
        public void StoreUsersInformation(Dictionary<long,User> dictUsers, FaceMovieModelContainer context)
        {
            User user;
            Movie movie, movieTemp;
            Dictionary<string, Movie> dictMovies = new Dictionary<string, Movie>();
                
            foreach (KeyValuePair<long, User> dictUser in dictUsers)
            {
                user = dictUser.Value;

                foreach (UserMovie um in user.UserMovie)
                {
                    dictMovies.TryGetValue(um.Movie.MovieName.ToUpper(), out movie);
                    if (movie == null)
                    {
                        movie = new Movie();
                        movie.MovieName = um.Movie.MovieName;
                        movie.MovieFacebookPageId = um.Movie.MovieFacebookPageId;
                        dictMovies.Add(movie.MovieName.ToUpper(), movie);
                    }
                    else
                    {
                        movie.MovieFacebookPageId = um.Movie.MovieFacebookPageId;
                    }
                }

                this.StoreUser(user, context);
            }

            foreach (KeyValuePair<string, Movie> dictMovie in dictMovies)
            {
                movie = dictMovie.Value;
                this.StoreMovie(movie, context);
            }

            //// Stores the movies
            context.SaveChanges();
            context = new FaceMovieModelContainer();

            foreach(KeyValuePair<long, User> dictUser in dictUsers)
            {
                user = dictUser.Value;
                dictMovies = new Dictionary<string, Movie>();
                
                foreach (UserMovie um in user.UserMovie)
                {
                    //// It only stores the "like" once
                    dictMovies.TryGetValue(um.Movie.MovieName.ToUpper(), out movieTemp);
                    if (movieTemp == null)
                    {
                        dictMovies.Add(um.Movie.MovieName.ToUpper(), um.Movie);
                        this.StoreUserMovie(um, context);
                    }
                }
            }
            context.SaveChanges();
        }

        private void StoreUser(User user, FaceMovieModelContainer context)
        {
            bool userExists = context.UserSet.Where(u => u.UserFacebookId == user.UserFacebookId).Count() > 0;
            if (userExists)
            {
                User userDB = context.UserSet.Where(u => u.UserFacebookId == user.UserFacebookId).First();
                userDB.UserFirstName = user.UserFirstName;
                userDB.UserLastName = user.UserLastName;
                userDB.UserFacebookToken = user.UserFacebookToken;
            }
            else
            {
                User newUser = new User();
                newUser.UserFirstName = user.UserFirstName;
                newUser.UserLastName = user.UserLastName;
                newUser.UserFacebookId = user.UserFacebookId;
                newUser.UserFacebookToken = user.UserFacebookToken;

                context.AddToUserSet(newUser);
            }
        }

        private void StoreUserMovie(UserMovie userMovie, FaceMovieModelContainer context)
        {
            bool userMovieExists = context.UserMovieSet.Where(um => um.User.UserFacebookId == userMovie.User.UserFacebookId && um.Movie.MovieName.ToUpper() == userMovie.Movie.MovieName.ToUpper()).Count() > 0;
            if (!userMovieExists)
            {
                UserMovie newUserMovie = new UserMovie();
                Movie movie = context.MovieSet.Where(m => m.MovieName.ToUpper() == userMovie.Movie.MovieName.ToUpper()).First();
                User user = context.UserSet.Where(u => u.UserFacebookId == userMovie.User.UserFacebookId).First();
                newUserMovie.User = user;
                newUserMovie.Movie = movie;
                newUserMovie.UserMovieRanking = userMovie.UserMovieRanking;

                context.AddToUserMovieSet(newUserMovie);
            }
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="dictMovies"> Parameter description for user goes here</param>
        /// <param name="context"> Parameter description for context goes here</param>
        private void StoreMovie(Movie movie, FaceMovieModelContainer context)
        {
            bool movieExists = context.MovieSet.Where(m => m.MovieName.ToUpper() == movie.MovieName.ToUpper()).Count() > 0;
            if (movieExists)
            {
                Movie movieDB = context.MovieSet.Where(m => m.MovieName.ToUpper() == movie.MovieName.ToUpper()).First();
                //// Updates the data of the movie
                movieDB.MovieFacebookPageId = movie.MovieFacebookPageId;
            }
            else
            {
                context.AddToMovieSet(movie);
            }
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="name"> Parameter description for name goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        public string GetParameter(string name)
        {
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            IQueryable<string> res = from p in context.Parameters
                                     where p.ParameterName == name
                                     select p.ParameterValue;
            if (res != null && res.Count() != 0)
            {
                return res.First();
            }
            else
            {
                throw new Exception("No se encontró el parámetro con nombre " + name);
            }
        }

        public void DeleteAllMovieSimilarities(FaceMovieModelContainer context)
        {
            context.ExecuteStoreCommand("TRUNCATE TABLE MovieSimilaritySet");
        }

        public long GetUserIdByFacebookToken(string token)
        {
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            try
            {
                User user = context.UserSet.Where(u => u.UserFacebookToken == token).First();
                return user.UserId;
            }
            catch
            {
                throw new Exception("No existe usuario");
            }
        }
    }
}