//-----------------------------------------------------------------------
// <copyright file="OAuthFacebook.cs" company="Interpool">
//     Copyright Interpool. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace FaceMovieApplication.FacebookCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using FaceMovieApplication.Utilities;

    /// <summary>
    /// Class statement OAuthFacebook
    /// </summary>
    public class OAuthFacebook
    {
        /// <summary>
        /// Store for the property
        /// </summary>
        public const string Authorize = "https://graph.facebook.com/oauth/authorize";

        /// <summary>
        /// Store for the property
        /// </summary>
        public const string AccessToken = "https://graph.facebook.com/oauth/access_token";

        /// <summary>
        /// Store for the property
        /// </summary>
        private string consumerKey = string.Empty;

        /// <summary>
        /// Store for the property
        /// </summary>
        private string consumerSecret = string.Empty;

        /// <summary>
        /// Store for the property
        /// </summary>
        private string token = string.Empty;

        /// <summary>
        /// Store for the property
        /// </summary>
        public enum Method
        {
            /// <summary>
            /// listed field
            /// </summary>
            GET,

            /// <summary>
            /// listed field
            /// </summary>
            POST
        }

        /// <summary>
        /// Gets for the property
        /// </summary>
        public string CallBackUrl
        {
            get
            {
                return Constants.FacebookCallbackUrl;
            }
        }

        #region Properties

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string ConsumerKey
        {
            get
            {
                if (this.consumerKey.Length == 0)
                {
                    this.consumerKey = Constants.ConsumerKey; ////Your application ID
                }

                return this.consumerKey;
            }

            set 
            {
                this.consumerKey = value;
            }
        }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string ConsumerSecret
        {
            get
            {
                if (this.consumerSecret.Length == 0)
                {
                    this.consumerSecret = Constants.ConsumerSecret; ////Your application secret
                }

                return this.consumerSecret;
            }

            set 
            {
                this.consumerSecret = value;
            }
        }

        /// <summary>
        /// Gets or sets for Method.</summary>
        public string Token 
        { 
            get 
            {
                return this.token; 
            } 
            
            set 
            {
                this.token = value; 
            } 
        }

        #endregion

        /// <summary>
        /// Get the link to Facebook's authorization page for this application.
        /// </summary>
        /// <returns>The url with a valid request token, or a null string.</returns>
        public string AuthorizationLinkGet()
        {
            return string.Format("{0}?client_id={1}&redirect_uri={2}&scope=offline_access, user_about_me, friends_about_me, user_likes, friends_likes, publish_stream", Authorize, this.ConsumerKey, this.CallBackUrl);
        }

        /// <summary>
        /// Exchange the Facebook "code" for an access token.
        /// </summary>
        /// <param name="authToken">The oauth_token or "code" is supplied by Facebook's authorization page following the callback.</param>
        public void AccessTokenGet(string authToken)
        {
            this.Token = authToken;
            string accessTokenUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&client_secret={3}&code={4}", AccessToken, this.ConsumerKey, this.CallBackUrl, this.ConsumerSecret, authToken);

            string response = this.WebRequest(Method.GET, accessTokenUrl, String.Empty);
            
            if (response.Length > 0)
            {
                ////Store the returned access_token
                NameValueCollection qs = HttpUtility.ParseQueryString(response);

                if (qs["access_token"] != null)
                {
                    this.Token = qs["access_token"];
                }
            }
        }

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequest(Method method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = string.Empty;

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "firefox";
            webRequest.Timeout = 20000;

            if (method == Method.POST)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";

                ////POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());

                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            try
            {
                responseData = this.WebResponseGet(webRequest);
                webRequest = null;
            }
            catch (Exception e)
            {
                responseData = null;
                //throw new GameException("error_tokenNotValid", e);
                throw new Exception("error_tokenNotValid", e);
            }

            return responseData;
        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        public string WebResponseGet(HttpWebRequest webRequest)
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