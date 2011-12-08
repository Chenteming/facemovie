using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FaceMovieApplication.FacebookCommunication;
using FaceMovieApplication.Update;
using FaceMovieApplication.Algorithm;
using System.Diagnostics;
using FaceMovieApplication.Datatypes;
using FaceMovieApplication.Data;
using FaceMovieApplication.Utilities;

namespace FaceMovieApplication.Management
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// test login
        /// </summary>
        /// <param name="sender">Parameter description for sender goes here</param>
        /// <param name="e">Parameter description for e goes here</param>
        protected void Login_Click(object sender, EventArgs e)
        {
            OAuthFacebook oauth = new OAuthFacebook();
            Response.Redirect(oauth.AuthorizationLinkGet());
        }

        protected void Similarity_Click(object sender, EventArgs e)
        {
            IItemBasedAlgorithm iba = new ItemBasedAlgorithm();
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            try
            {
                Movie movie1 = context.MovieSet.Where(m => m.MovieName == TextBoxMovie1.Text).First();
                Movie movie2 = context.MovieSet.Where(m => m.MovieName == TextBoxMovie2.Text).First();
                double similarity = iba.ComputeSimilarity(movie1, movie2, context);
                LabelOut.Text = "The similarity between the movies is " + similarity.ToString();
            }
            catch(Exception ex)
            {
                LabelOut.Text = "At least one of the movies doesn't exist";
            }
        }

        protected void UpdateSimilarities_Click(object sender, EventArgs e)
        {
            UpdateController uc = new UpdateController();
            try
            {
                uc.UpdateSimilarities();
                LabelOut.Text = "Se actualizaron las similaridades entre las películas correctamente";
            }
            catch (Exception ex)
            {
                LabelOut.Text = ex.Message;
            }
        }

        protected void SaveSimilarity_Click(object sender, EventArgs e)
        {
            Movie movie1;
            Movie movie2;
            MovieSimilarity movieSimilarity;
            List<MovieSimilarity> movieSimilaritySet;
            HashSet<Movie> relatedMovies;
            DataManager dm = new DataManager();
            IItemBasedAlgorithm iba = new ItemBasedAlgorithm();
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            movieSimilarity = new MovieSimilarity();
            if (String.IsNullOrEmpty(TextBoxMovie2.Text))
            {
                context = new FaceMovieModelContainer();
                movie1 = context.MovieSet.Where(m => m.MovieName == TextBoxMovie1.Text).First();
                context.ExecuteStoreCommand("DELETE FROM MovieSimilaritySet WHERE Movie_1_MovieId = " + movie1.MovieId + ";");
                context.SaveChanges();
                context = new FaceMovieModelContainer();
                movie1 = context.MovieSet.Where(m => m.MovieName == TextBoxMovie1.Text).First();
                int amountMovieSimilarities = int.Parse(dm.GetParameter(Parameters.AmountSimilarMovies));
                //// Do the same thing as in the foreach of the UpdateController
                relatedMovies = new HashSet<Movie>();
                var users = from us in context.UserMovieSet
                            where us.Movie.MovieId == movie1.MovieId
                            select us.User;
                foreach (User user in users.AsEnumerable())
                {
                    var movies = from us in context.UserMovieSet
                                 where us.User.UserId == user.UserId && us.Movie.MovieId != movie1.MovieId
                                 select us.Movie;
                    foreach (Movie m2 in movies.AsEnumerable())
                    {
                        relatedMovies.Add(m2);
                    }
                }

                movieSimilaritySet = new List<MovieSimilarity>();
                foreach (Movie relatedMovie in relatedMovies)
                {
                    movieSimilarity = new MovieSimilarity();
                    movieSimilarity.Movie_1Reference.EntityKey = movie1.EntityKey;
                    movieSimilarity.Movie_2Reference.EntityKey = relatedMovie.EntityKey;
                    movieSimilarity.Similarity = iba.ComputeSimilarity(movie1, relatedMovie, context);
                    movieSimilaritySet.Add(movieSimilarity);
                }
                Debug.WriteLine(movie1.MovieId + " " + movie1.MovieName);
                movieSimilaritySet.Sort(CompareMoviesBySimilarity);
                movieSimilaritySet.Reverse();
                movieSimilaritySet = movieSimilaritySet.Take(amountMovieSimilarities).ToList();
                foreach (MovieSimilarity ms in movieSimilaritySet)
                {
                    context.AddToMovieSimilaritySet(ms);
                }
                context.SaveChanges();
            }
            else
            {
                try
                {
                    movie1 = context.MovieSet.Where(m => m.MovieName == TextBoxMovie1.Text).First();
                    movie2 = context.MovieSet.Where(m => m.MovieName == TextBoxMovie2.Text).First();
                    movieSimilarity.Movie_1 = movie1;
                    movieSimilarity.Movie_2 = movie2;
                    movieSimilarity.Similarity = iba.ComputeSimilarity(movie1, movie2, context);
                    context.AddToMovieSimilaritySet(movieSimilarity);
                    context.SaveChanges();

                    LabelOut.Text = "Se guardó la similaridad entre las películas con el valor " + movieSimilarity.Similarity.ToString();
                }
                catch (Exception ex)
                {
                    LabelOut.Text = ex.Message;
                }
            }
        }

        protected void ButtonInformation_Click(object sender, EventArgs e)
        {
            IMovieController mc = new MovieController();
            try
            {
                Movie movie = mc.GetMovieInfoByTitle(TextBoxMovieName.Text);
                LabelOut.Text = "Title:" + movie.MovieName + " Genre: " + movie.MovieGenre + " Rating: " + movie.MovieRanking;
                ImageMovie.ImageUrl = movie.MovieImageUrl;
            }
            catch (Exception ex)
            {
                LabelOut.Text = ex.Message;
            }
        }

        protected void ButtonCompleteInformation_Click(object sender, EventArgs e)
        {
            IMovieController mc = new MovieController();
            try
            {
                DataMovie movie = mc.GetMovieCompleteInfoByTitle(TextBoxMovieName.Text);
                LabelOut.Text = "Title:" + movie.MovieName + " Genre: " + movie.MovieGenre + " Rating: " + movie.MovieRanking + " Plot: " + movie.MoviePlot;
                this.HyperLinkIMDB.NavigateUrl = movie.MovieUrl;
                ImageMovie.ImageUrl = movie.MovieImageUrl;
            }
            catch (Exception ex)
            {
                LabelOut.Text = ex.Message;
            }
        }

        protected void ButtonUpdateMoviesInformation_Click(object sender, EventArgs e)
        {
            UpdateController uc = new UpdateController();
            try
            {
                uc.UpdateMoviesInformation();
                LabelOut.Text = "Se actualizó la información de todas las películas correctamente";
            }
            catch (Exception ex)
            {
                LabelOut.Text = ex.Message;
            }
        }

        protected void ButtonRandomize_Click(object sender, EventArgs e)
        {
            try
            {
                this.RandomizeUsersRatings();
                LabelOut.Text = "Se generaron ratings aleatorios para todas las películas";
            }
            catch (Exception ex)
            {
                LabelOut.Text = ex.Message;
            }
        }

        private void RandomizeUsersRatings()
        {
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            Random random = new Random(DateTime.Now.Millisecond);
            double rating;
            foreach (UserMovie us in context.UserMovieSet)
            {
                rating = random.Next(1, 6);
                rating = Math.Truncate(rating);
                if (rating == 6)
                {
                    rating = 5;
                }
                us.UserMovieRanking = rating;
            }
            context.SaveChanges();
        }

        protected void ButtonGetRecommendation_Click(object sender, EventArgs e)
        {
            RecommendationController rc = new RecommendationController();
            try
            {
                List<DataUserMovieRating> movies = rc.UserGetRecommendation(int.Parse(TextBoxUser.Text));
                LabelOut.Text = "";
                foreach (DataUserMovieRating movie in movies)
                {
                    LabelOut.Text += movie.MovieId + " " + movie.Rating + " ";
                }
            }
            catch (Exception ex)
            {
                LabelOut.Text = ex.Message;
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

        protected void ButtonMovieInformation_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(HyperLinkIMDB.NavigateUrl);
        }
    }
}