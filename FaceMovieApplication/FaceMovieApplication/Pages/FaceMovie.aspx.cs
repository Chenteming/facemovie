using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FaceMovieApplication.Update;
using FaceMovieApplication.Datatypes;
using FaceMovieApplication.Data;

namespace FaceMovieApplication.Pages
{
    public partial class FaceMovie : System.Web.UI.Page
    {
        DataMovie dataMovie = null;
        int userId = 0;
        int movieId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            userId = int.Parse(Request.QueryString["userid"]);
            movieId = int.Parse(HiddenFieldMovieId.Value);
            if (movieId == 0)
            {
                TableMovie.Visible = false;
            }
            TableRanking.Visible = false;
        }

        protected void ButtonMovieRecommendation_Click(object sender, EventArgs e)
        {
            this.RecommendMovie();
        }

        protected void ButtonMovieIMDB_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(LabelLinkIMDB.Text);
        }

        protected void ButtonRank_Click(object sender, EventArgs e)
        {
            DataManager dm = new DataManager();
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            int movieId = int.Parse(HiddenFieldMovieId.Value);
            bool userMovieExists = context.UserMovieSet.Where(um => um.User.UserId == userId && um.Movie.MovieId == movieId).Count() > 0;
            if (!userMovieExists)
            {
                UserMovie newUserMovie = new UserMovie();
                Movie movie = context.MovieSet.Where(m => m.MovieId == movieId).First();
                User user = context.UserSet.Where(u => u.UserId == userId).First();
                newUserMovie.User = user;
                newUserMovie.Movie = movie;
                newUserMovie.UserMovieRanking = double.Parse(RadioButtonListRanking.SelectedValue);

                context.AddToUserMovieSet(newUserMovie);
                context.SaveChanges();
            }

            TableMovie.Visible = false;
            TableRanking.Visible = false;
            this.RecommendMovie();
        }

        private void RecommendMovie()
        {
            List<DataUserMovieRating> movies = null;
            RecommendationController rc = new RecommendationController();
            IMovieController mc = new MovieController();
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            Movie movie;
            if (userId != 0)
                movies = rc.UserGetRecommendation(userId);
            if (movies != null && movies.Count > 0)
            {
                int movieId = movies.ElementAt(0).MovieId;
                HiddenFieldMovieId.Value = movieId.ToString();
                try
                {
                    movie = context.MovieSet.Where(m => m.MovieId == movieId).First();
                    TableMovie.Visible = true;
                    TableRanking.Visible = true;
                    LabelMovieName.Text = movie.MovieName;
                    try
                    {
                        dataMovie = mc.GetMovieCompleteInfoByTitle(movie.MovieName);
                        LabelPlot.Text = dataMovie.MoviePlot;
                        LabelGenre.Text = dataMovie.MovieGenre;
                        ImageMovie.ImageUrl = dataMovie.MovieImageUrl;
                        LabelLinkIMDB.Text = dataMovie.MovieUrl;
                    }
                    catch
                    {
                        //// If no internet connection is available, data from the database is used
                        LabelGenre.Text = movie.MovieGenre;
                        LabelPlot.Text = "A true story about Frank Abagnale Jr. who, before his 19th birthday, successfully conned millions of dollars worth of checks as a Pan Am pilot, doctor, and legal prosecutor.";
                        ImageMovie.ImageUrl = movie.MovieImageUrl;
                    }
                }
                catch
                {
                    throw new Exception("Error al obtener la Película");
                }
            }
        }

        protected void ButtonOK_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("www.cuevana.tv");
            movieId = 0;
            HiddenFieldMovieId.Value = movieId.ToString();
        }
    }
}