using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FaceMovieApplication.FacebookCommunication;
using FaceMovieApplication.Update;
using FaceMovieApplication.Algorithm;

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
            uc.UpdateSimilarities();
        }

        protected void SaveSimilarity_Click(object sender, EventArgs e)
        {
            Movie movie1;
            Movie movie2;
            MovieSimilarity movieSimilarity;
            IItemBasedAlgorithm iba = new ItemBasedAlgorithm();
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            movieSimilarity = new MovieSimilarity();
            try {
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
}