using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceMovieApplication.Utilities
{
    public class Constants
    {
        /// <summary>
        /// Gets for the property
        /// </summary>
        public static string ConsumerKey
        {
            get
            {
                return "159559157472530";
            }
        }

        /// <summary>
        /// Gets for the property
        /// </summary>
        public static string ConsumerSecret
        {
            get
            {
                return "9c803fb159f4a030ac2d87e4e8ed7823";
            }
        }

        /// <summary>
        /// Gets for the property
        /// </summary>
        public static string FacebookCallbackUrl
        {
            get
            {
                return "http://localhost:1222/FacebookCommunication/FacebookCallback.aspx/";
            }
        }

        /// <summary>
        /// Gets for the property
        /// </summary>
        public static string AppId
        {
            get
            {
                return "A00C4105122186E4F9F0DFD82CDF594DD866BC1F";
            }
        }

        /// <summary>
        /// Gets for the property
        /// </summary>
        public static double DEFAULT_FACEBOOK_RANKING
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// Gets for the property
        /// </summary>
        public static double FACEBOOK_MAX_ELEMENTS
        {
            get
            {
                return 200;
            }
        }

        /// <summary>
        /// Gets for the property
        /// </summary>
        public static string REDIRECT_URL_AFTER_FACEBOOK_LOGIN
        {
            get
            {
                return "http://localhost:1222/Pages/FaceMovie.aspx";
            }
        }

    }
}