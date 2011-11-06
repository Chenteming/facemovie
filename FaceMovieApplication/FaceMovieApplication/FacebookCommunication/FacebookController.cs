//-----------------------------------------------------------------------
// <copyright file="FacebookController.cs" company="Interpool">
//     Copyright Interpool. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace FaceMovieApplication.FacebookCommunication
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using FaceMovieApplication.Datatypes;
    /*
    using InterpoolCloudWebRole.Data;
    using InterpoolCloudWebRole.Datatypes;
    using InterpoolCloudWebRole.Utilities;
     */
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using FaceMovieApplication.Utilities;
    

    /// <summary>
    /// Class statement FacebookController
    /// </summary>
    public class FacebookController : IFacebookController
    {
        /// <summary>
        /// FQL query
        /// </summary>
        string fqlQuery;
        string url;
        
        /*
        /// <summary>
        /// Store for the property
        /// </summary>
        private IDataManager dataManager = new DataManager();
        */
        /// <summary>
        /// Downloads from Facebook all the information from user and user's friends and stores it on the data base.
        /// </summary>
        /// <param name="auth"> Parameter description for auth goes here</param>
        /// <param name="game"> Parameter description for game goes here</param>
        /// <param name="limitSuspects"> Parameter description for limitSuspects goes here</param>
        /// <param name="context"> Parameter description for context goes here</param>
        public Dictionary<long, User> GetUsersFacebookData(OAuthFacebook auth)
        {
            Dictionary<long, User> dictUser = new Dictionary<long, User>();

            // Get friends standard information
            fqlQuery = "SELECT+uid,first_name,last_name+FROM+user+WHERE+uid+IN+(SELECT+uid2+FROM+friend+WHERE+uid1=me())";
            url = String.Format("https://graph.facebook.com/fql?q={0}&access_token={1}", fqlQuery, auth.Token);
            string json = auth.WebRequest(OAuthFacebook.Method.GET, url, String.Empty);
            dictUser = this.GetFriendsStandardInfoByJson(json);
            // Parse the information returned by Facebook

            // Get friends favourite films (only the ids)
            fqlQuery = "SELECT+uid,page_id+FROM+page_fan+WHERE+type='MOVIE'+AND+uid+IN+(SELECT+uid2+FROM+friend+WHERE+uid1=me())";
            url = String.Format("https://graph.facebook.com/fql?q={0}&access_token={1}", fqlQuery, auth.Token);
            json = auth.WebRequest(OAuthFacebook.Method.GET, url, String.Empty);

            Dictionary<long, Movie> dictMovies = this.GetMoviesInformationByJson(json, auth);
            //friendData = this.GetFriendStandardInfoByJson(jsonFriendInfo);
            this.AddMoviesInformationToFriends(dictUser, dictMovies, json);

            return dictUser;
        }

        private Dictionary<long, Movie> GetMoviesInformationByJson(string json, OAuthFacebook auth)
        {
            Dictionary<long,Movie> dictMovies = new Dictionary<long,Movie>();
            Movie movie;
            JObject jsonObject = JObject.Parse(json);
            string concatenatedPageIds = string.Empty;
            long pageId;
            int i = 0;
            JArray jsonArray = (JArray)jsonObject["data"];
            int count = jsonArray.Count;
            while (i < count)
            {
                pageId = getPageId(jsonObject, i);
                if (String.IsNullOrEmpty(concatenatedPageIds))
                {
                    concatenatedPageIds = pageId.ToString();
                }
                else
                {
                    concatenatedPageIds += "," + pageId.ToString();
                }
                i++;
            }
            fqlQuery = "SELECT+name,page_id+FROM+page+WHERE+page_id+IN+(" + concatenatedPageIds + ")";
            url = String.Format("https://graph.facebook.com/fql?q={0}&access_token={1}", fqlQuery, auth.Token);
            string jsonMovies = auth.WebRequest(OAuthFacebook.Method.GET, url, String.Empty);
            jsonObject = JObject.Parse(jsonMovies);
            jsonArray = (JArray)jsonObject["data"];
            count = jsonArray.Count;
            i = 0;
            while (i < count)
            {
                movie = new Movie();
                movie.MovieFacebookPageId = getPageId(jsonObject, i);
                movie.MovieName = (string)jsonObject["data"][i]["name"];
                dictMovies.Add(movie.MovieFacebookPageId, movie);
                i++;
            }
            return dictMovies;
        }

        private void AddMoviesInformationToFriends(Dictionary<long, User> dictUser, Dictionary<long, Movie> dictMovies, string json)
        {
            JObject jsonObject = JObject.Parse(json);
            JArray jsonArray = (JArray)jsonObject["data"];
            int i = 0;
            long pageId, facebookId;
            User user;
            Movie movie;
            UserMovie userMovie;
            int count = jsonArray.Count;
            while (i < count)
            {
                pageId = getPageId(jsonObject, i);
                facebookId = (long)jsonObject["data"][i]["uid"];
                try
                {
                    dictUser.TryGetValue(facebookId, out user);
                    dictMovies.TryGetValue(pageId, out movie);
                    userMovie = new UserMovie();
                    userMovie.UserMovieRanking = Constants.DEFAULT_FACEBOOK_RANKING;
                    user.UserMovie.Add(userMovie);
                    userMovie.Movie = movie;
                }
                catch (Exception ex)
                {
                }
                
                i++;
            }
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="jsonFriendInfo"> Parameter description for jsonFriendInfo goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        private Dictionary<long,User> GetFriendsStandardInfoByJson(string json)
        {
            Dictionary<long, User> dictUser = new Dictionary<long, User>();
            JObject jsonObject = JObject.Parse(json);
            int i = 0;
            JArray jsonArray = (JArray)jsonObject["data"];
            int friendsCount = jsonArray.Count;
            User fbud;
            while (i < friendsCount)
            {
                fbud = new User();
                fbud.UserFacebookId = (long)jsonObject["data"][i]["uid"];
                fbud.UserFirstName = (string)jsonObject["data"][i]["first_name"];
                fbud.UserLastName = (string)jsonObject["data"][i]["last_name"];
                dictUser.Add(fbud.UserFacebookId, fbud);
                i++;
            }

            return dictUser;
        }

        private long getPageId(JObject jsonObject, int i)
        {
            String pageIdString = jsonObject["data"][i]["page_id"].ToString();
            if (pageIdString.StartsWith("\"")) pageIdString = pageIdString.Substring(1, pageIdString.Length - 2);
            return long.Parse(pageIdString);
        }
    }
         
}
