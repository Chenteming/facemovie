using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Objects;

namespace FaceMovieApplication
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        public void BindData()
        {
            FaceMovieModelContainer context = new FaceMovieModelContainer();


            Grid.DataSource = context.MovieSet.Where(m => m.MovieName == "El Padrino");
            Grid.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ClickHandler(object sender, EventArgs e)
        {
            BindData();
        }

    }
}