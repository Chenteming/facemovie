//-----------------------------------------------------------------------
// <copyright file="MovieController.cs" company="Interpool">
//     Copyright Interpool. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FaceMovieApplication.Update
{
    /// <summary>
    /// Class Description MovieController
    /// </summary>
    public class MovieController: IMovieController
    {
        /// <summary>
        /// Description for Method.</summary>
        /// <param name="movieTitle"> Parameter description for auth goes here</param>
        public Movie GetMovieInfoByTitle(string movieTitle)
        {
            string url = this.ConstructUrlByTitle(movieTitle);
            try
            {
                string json = this.WebRequest(url);
                Movie movie = this.GetMovieInfoByJson(json);
                return movie;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="json"> Parameter description for auth goes here</param>
        private Movie GetMovieInfoByJson(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            Movie movie = new Movie();
            movie.MovieName = (string)jsonObject["Title"];
            movie.MovieRanking = double.Parse(((string)jsonObject["Rating"]).Replace(".",","));
            movie.MovieImageUrl = (string)jsonObject["Poster"];
            string movieGenres = (string)jsonObject["Genre"];
            if (movieGenres.Contains(","))
            {
                movieGenres = (movieGenres.Split(','))[0];
            }
            movie.MovieGenre = movieGenres;
            return movie;
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="movieTitle"> Parameter description for auth goes here</param>
        private string ConstructUrlByTitle(string movieTitle)
        {
            string url = "http://www.imdbapi.com/?t=" + movieTitle;
            return url;
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="url"> Parameter description for auth goes here</param>
        private string WebRequest(string url)
        {
            HttpWebRequest webRequest = null;
            string responseData = string.Empty;

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "firefox";
            webRequest.Timeout = 20000;

            try
            {
                responseData = this.WebResponseGet(webRequest);
                webRequest = null;
            }
            catch (Exception e)
            {
                responseData = null;
                throw new Exception("Error al obtener la película de IMDB", e);
            }

            return responseData;
        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        private string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = string.Empty;

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }
    }
}