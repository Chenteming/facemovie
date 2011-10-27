//-----------------------------------------------------------------------
// <copyright file="FacebookController.cs" company="Interpool">
//     Copyright Interpool. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace InterpoolCloudWebRole.FacebookCommunication
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using InterpoolCloudWebRole.Data;
    using InterpoolCloudWebRole.Datatypes;
    using InterpoolCloudWebRole.Utilities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    
    /// <summary>
    /// Class statement FacebookController
    /// </summary>
    public class FacebookController : IFacebookController
    {
        /// <summary>
        /// Store for the property
        /// </summary>
        private IDataManager dataManager = new DataManager();

        /// <summary>
        /// Downloads from Facebook all the information from user and user's friends and stores it on the data base.
        /// </summary>
        /// <param name="auth"> Parameter description for auth goes here</param>
        /// <param name="game"> Parameter description for game goes here</param>
        /// <param name="limitSuspects"> Parameter description for limitSuspects goes here</param>
        /// <param name="context"> Parameter description for context goes here</param>
        public void DownloadFacebookUserData(OAuthFacebook auth, Game game, int limitSuspects, InterpoolContainer context)
        {
            IDataManager dm = new DataManager();
            string userId = dm.GetUserByToken(auth.Token, dm.GetContainer()).UserIdFacebook;
            if (!userId.Equals(string.Empty))
            {
                DataFacebookUser fbud = new DataFacebookUser();
                fbud.UserId = userId;
                fbud.OAuth = auth;

                List<string> friendsIds = this.GetFriendsId(userId);
                List<string> shuffleFriendsIds = Functions.ShuffleList<string>(friendsIds);
                Suspect suspect;
                List<Suspect> suspects = new List<Suspect>();
                DataFacebookUser fbudOfSuspect; 

                //// Creates and stores the suspects for the current user
                int i = 0;
                
                foreach (string friendId in shuffleFriendsIds)
                {
                    fbudOfSuspect = this.GetFriendInfo(userId, auth, friendId);
                    suspect = this.NewSuspectFromFacebookUserData(fbudOfSuspect);
                    if (this.HaveEnoughFields(suspect, Constants.DataRequired))
                    {
                        this.dataManager.StoreSuspect(suspect, context);
                        game.PossibleSuspect.Add(suspect);
                        i++;
                    }

                    if (i >= limitSuspects)
                    {
                        break;
                    }
                }

                // Stores the changes made to the game
                this.dataManager.SaveChanges(context);
            }
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="auth"> Parameter description for auth goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        public string GetUserId(OAuthFacebook auth)
        {
            string url = String.Format("https://graph.facebook.com/me?access_token={0}", auth.Token);
            string jsonUser = auth.WebRequest(OAuthFacebook.Method.GET, url, String.Empty);
            bool error = false;
            JObject jsonUserObject = JObject.Parse(jsonUser);
            jsonUserObject.SelectToken("id", error);
            string userIdFacebook = (string)jsonUserObject.SelectToken("id", error);
            return userIdFacebook;
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="userId"> Parameter description for userId goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        public List<string> GetFriendsId(string userId)
        {
            IDataManager dm = new DataManager();
            IQueryable<User> query = dm.GetUserByIdFacebook(dm.GetContainer(), userId);
            if (query.Count() == 0)
            {
                throw new GameException("error_notExistsUser", null);
            }

            OAuthFacebook auth = new OAuthFacebook { Token = query.First().UserTokenFacebook };

            if (auth != null && auth.Token.Length > 0)
            {
                // We now have the credentials, so we can start making API calls
                string url = String.Format("https://graph.facebook.com/{0}/friends?access_token={1}", userId, auth.Token);
                string jsonFriends = auth.WebRequest(OAuthFacebook.Method.GET, url, String.Empty);
                List<string> friendsId = this.GetFriendsIdByJson(jsonFriends);
                return friendsId;
            }

            return null;
        }

        /// <summary>
        /// Description for Method.</summary>
        public void UploadUserFriendsInformation()
        {
        }

        /// <summary>
        /// Returns the user's info
        /// </summary>
        /// <param name="auth">Contains the token</param>
        /// <returns>Returns the data of the user with token auth.Token</returns>
        public DataFacebookUser GetUserInfoByToken(OAuthFacebook auth)
        {
            DataFacebookUser userData = new DataFacebookUser();
            
            string url = String.Format("https://graph.facebook.com/me?access_token={0}", auth.Token);
            string jsonUserInfo = auth.WebRequest(OAuthFacebook.Method.GET, url, String.Empty);
            userData = this.GetUserStandardInfoByJson(jsonUserInfo);
            
            url = String.Format("https://graph.facebook.com/{0}/likes?access_token={1}", userData.UserId, auth.Token);
            jsonUserInfo = auth.WebRequest(OAuthFacebook.Method.GET, url, String.Empty);

            //// The likes will be discriminated as Television, Cinema and Music
            userData = this.GetLikesInfoByJson(jsonUserInfo, userData);

            userData.PictureLink = String.Format("https://graph.facebook.com/{0}/picture", userData.UserId);
            
            return userData;
        }

        /// <summary>
        /// Returns the standard information by a json object
        /// </summary>
        /// <param name="userId">Parameter description for userId goes here</param>
        /// <param name="auth">Parameter description for auth goes here</param>
        /// <param name="userFriendId">Parameter description for userFriendId goes here</param>
        /// <returns>Return results are described through the returns tag</returns>
        public DataFacebookUser GetFriendInfo(string userId, OAuthFacebook auth, string userFriendId)
        {
            ////OAuthFacebook auth = this.GetOAuthFacebook(userId);
            DataFacebookUser friendData = new DataFacebookUser();

            string url = String.Format("https://graph.facebook.com/{0}?access_token={1}", userFriendId, auth.Token);
            string jsonFriendInfo = auth.WebRequest(OAuthFacebook.Method.GET, url, String.Empty);
            friendData = this.GetFriendStandardInfoByJson(jsonFriendInfo);

            url = String.Format("https://graph.facebook.com/{0}/likes?access_token={1}", userFriendId, auth.Token);
            jsonFriendInfo = auth.WebRequest(OAuthFacebook.Method.GET, url, String.Empty);

            // The likes will be discriminated as Television, Cinema and Music
            friendData = this.GetFriendLikesInfoByJson(jsonFriendInfo, friendData);

            friendData.PictureLink = String.Format("https://graph.facebook.com/{0}/picture", friendData.IdFriend, string.Empty);

            return friendData;
        }

        /// <summary>
        /// Returns the likes of an user
        /// </summary>
        /// <param name="jsonUserInfo"> Parameter description for jsonUserInfo goes here</param>
        /// <param name="userData"> Parameter description for userData goes here</param>
        /// <returns>Return results are described through the returns tag</returns>
        private DataFacebookUser GetLikesInfoByJson(string jsonUserInfo, DataFacebookUser userData)
        {
            JObject jsonUserObject = JObject.Parse(jsonUserInfo);

            ////================GETTING LIKES FRIENDS DATA=====================//
            string like_category = (string)jsonUserObject.SelectToken("data[0].category");

            int i = 0;
            bool exit = false;
            userData.Music = string.Empty;
            userData.Television = string.Empty;
            userData.Cinema = string.Empty;

            while (like_category != null && !exit)
            {
                switch (like_category)
                {
                    case "Music":
                    case "Musicians":
                        userData.Music = (string)jsonUserObject.SelectToken("data[" + i + "].name");
                        break;
                    case "Television":
                        userData.Television = (string)jsonUserObject.SelectToken("data[" + i + "].name");
                        break;
                    case "Movie":
                    case "Film":
                        userData.Cinema = (string)jsonUserObject.SelectToken("data[" + i + "].name");
                        break;
                }

                i++;
                like_category = (string)jsonUserObject.SelectToken("data[" + i + "].category");
                if (userData.Music != string.Empty && userData.Television != string.Empty && userData.Cinema != string.Empty)
                {
                    exit = true;
                }
            }

            return userData;
        }

        /// <summary>
        /// Returns the standard information by a json object
        /// </summary>
        /// <param name="jsonUserInfo">Parameter description for jsonUserInfo goes here</param>
        /// <returns>Return results are described through the returns tag</returns>
        private DataFacebookUser GetUserStandardInfoByJson(string jsonUserInfo)
        {
            //// It's the same code as GetFriendStandardInfo, and the caller must
            //// set what's needed in the UserId and IdFriend
            JObject jsonFriendObject = JObject.Parse(jsonUserInfo);
            List<string> friendsId = new List<string>();
            DataFacebookUser fbud = new DataFacebookUser();
            fbud.Birthday = string.Empty;
            fbud.Cinema = string.Empty;
            fbud.FirstName = string.Empty;
            fbud.Hometown = string.Empty;
            fbud.LastName = string.Empty;
            fbud.Email = string.Empty;
            fbud.Music = string.Empty;
            fbud.Television = string.Empty;
            fbud.UserId = string.Empty;
            fbud.IdFriend = string.Empty;
            fbud.Gender = string.Empty;
            fbud.PictureLink = string.Empty;

            //// Error = if true rise exception when does not match token.
            bool error = false;
            fbud.UserId = (string)jsonFriendObject.SelectToken("id", error);
            fbud.FirstName = (string)jsonFriendObject.SelectToken("first_name", error);
            fbud.LastName = (string)jsonFriendObject.SelectToken("last_name", error);
            fbud.Birthday = (string)jsonFriendObject.SelectToken("birthday", error);
            fbud.Email = (string)jsonFriendObject.SelectToken("email", error);
            //// TODO: check the output format
            if (fbud.Birthday != null)
            {
                string[] fecha = fbud.Birthday.Split('/');
                switch (fecha[0])
                {
                    case "01":
                        fbud.Birthday = "Enero";
                        break;
                    case "02":
                        fbud.Birthday = "Febrero";
                        break;
                    case "03":
                        fbud.Birthday = "Marzo";
                        break;
                    case "04":
                        fbud.Birthday = "Abril";
                        break;
                    case "05":
                        fbud.Birthday = "Mayo";
                        break;
                    case "06":
                        fbud.Birthday = "Junio";
                        break;
                    case "07":
                        fbud.Birthday = "Julio";
                        break;
                    case "08":
                        fbud.Birthday = "Agosto";
                        break;
                    case "09":
                        fbud.Birthday = "Setiembre";
                        break;
                    case "10":
                        fbud.Birthday = "Octubre";
                        break;
                    case "11":
                        fbud.Birthday = "Noviembre";
                        break;
                    case "12":
                        fbud.Birthday = "Diciembre";
                        break;
                }
            }

            fbud.Gender = (string)jsonFriendObject.SelectToken("gender", error);
            JObject jsonFriendObjectAnid = (JObject)jsonFriendObject.SelectToken("hometown", error);
            if (jsonFriendObjectAnid != null)
            {
                fbud.Hometown = (string)jsonFriendObjectAnid.SelectToken("name", error);
            }

            return fbud;
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="fbudOfSuspect"> Parameter description for fbudOfSuspect goes here</param>
        /// <param name="cantDataRequired"> Parameter description for cantDataRequired goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        private bool HaveEnoughFields(Suspect fbudOfSuspect, int cantDataRequired)
        {
            int cant = 0;
            if (fbudOfSuspect.SuspectBirthday != string.Empty)
            {
                cant++;
            }

            if (fbudOfSuspect.SuspectCinema != string.Empty)
            {
                cant++;
            }

            if (fbudOfSuspect.SuspectGender != string.Empty)
            {
                cant++;
            }

            if (fbudOfSuspect.SuspectHometown != string.Empty)
            {
                cant++;
            }

            if (fbudOfSuspect.SuspectMusic != string.Empty)
            {
                cant++;
            }

            if (fbudOfSuspect.SuspectTelevision != string.Empty)
            {
                cant++;
            }

            if (cant < cantDataRequired)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="fbudOfSuspect"> Parameter description for fbudOfSuspect goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        private Suspect NewSuspectFromFacebookUserData(DataFacebookUser fbudOfSuspect)
        {
            Suspect suspect = new Suspect();
            suspect.SuspectFacebookId = (fbudOfSuspect.IdFriend == null) ? string.Empty : fbudOfSuspect.IdFriend;
            suspect.SuspectFirstName = fbudOfSuspect.FirstName;
            suspect.SuspectLastName = fbudOfSuspect.LastName;
            suspect.SuspectBirthday = (fbudOfSuspect.Birthday == null) ? string.Empty : fbudOfSuspect.Birthday;
            suspect.SuspectHometown = (fbudOfSuspect.Hometown == null) ? string.Empty : fbudOfSuspect.Hometown;
            suspect.SuspectMusic = (fbudOfSuspect.Music == null) ? string.Empty : fbudOfSuspect.Music;
            suspect.SuspectTelevision = (fbudOfSuspect.Television == null) ? string.Empty : fbudOfSuspect.Television;
            suspect.SuspectCinema = (fbudOfSuspect.Cinema == null) ? string.Empty : fbudOfSuspect.Cinema;
            suspect.SuspectGender = (fbudOfSuspect.Gender == null) ? string.Empty : fbudOfSuspect.Gender;
            suspect.SuspectPicLInk = (fbudOfSuspect.PictureLink == null) ? string.Empty : fbudOfSuspect.PictureLink;

            return suspect;
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="jsonFriendInfo"> Parameter description for jsonFriendInfo goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        private DataFacebookUser GetFriendStandardInfoByJson(string jsonFriendInfo)
        {
            JObject jsonFriendObject = JObject.Parse(jsonFriendInfo);
            List<string> friendsId = new List<string>();
            DataFacebookUser fbud = new DataFacebookUser();
            fbud.Birthday = string.Empty;
            fbud.Cinema = string.Empty;
            fbud.FirstName = string.Empty;
            fbud.Hometown = string.Empty;
            fbud.LastName = string.Empty;
            fbud.Music = string.Empty;
            fbud.Television = string.Empty;
            fbud.UserId = string.Empty;
            fbud.IdFriend = string.Empty;
            fbud.Gender = string.Empty;
            fbud.PictureLink = string.Empty;

            ////================GETTING STANDARD FRIENDS DATA=====================//
            // Error = if true rise exception when does not match token.
            bool error = false;
            fbud.IdFriend = (string)jsonFriendObject.SelectToken("id", error);
            fbud.FirstName = (string)jsonFriendObject.SelectToken("first_name", error);
            fbud.LastName = (string)jsonFriendObject.SelectToken("last_name", error);
            fbud.Birthday = (string)jsonFriendObject.SelectToken("birthday", error);
            ////TODO: check the output format
            if (fbud.Birthday != null)
            {
                string[] fecha = fbud.Birthday.Split('/');
                switch (fecha[0])
                {
                    case "01":
                        fbud.Birthday = "Enero";
                        break;
                    case "02":
                        fbud.Birthday = "Febrero";
                        break;
                    case "03":
                        fbud.Birthday = "Marzo";
                        break;
                    case "04":
                        fbud.Birthday = "Abril";
                        break;
                    case "05":
                        fbud.Birthday = "Mayo";
                        break;
                    case "06":
                        fbud.Birthday = "Junio";
                        break;
                    case "07":
                        fbud.Birthday = "Julio";
                        break;
                    case "08":
                        fbud.Birthday = "Agosto";
                        break;
                    case "09":
                        fbud.Birthday = "Setiembre";
                        break;
                    case "10":
                        fbud.Birthday = "Octubre";
                        break;
                    case "11":
                        fbud.Birthday = "Noviembre";
                        break;
                    case "12":
                        fbud.Birthday = "Diciembre";
                        break;
                }
            }

            fbud.Gender = (string)jsonFriendObject.SelectToken("gender", error);
            JObject jsonFriendObjectAnid = (JObject)jsonFriendObject.SelectToken("hometown", error);
            if (jsonFriendObjectAnid != null)
            {
                fbud.Hometown = (string)jsonFriendObjectAnid.SelectToken("name", error);
            }

            return fbud;                     
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="jsonFriendInfo"> Parameter description for jsonFriendInfo goes here</param>
        /// <param name="friendData"> Parameter description for friendData goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        private DataFacebookUser GetFriendLikesInfoByJson(string jsonFriendInfo, DataFacebookUser friendData)
        {
            JObject jsonFriendObject = JObject.Parse(jsonFriendInfo);

            ////================GETTING LIKES FRIENDS DATA=====================//
            string like_category = (string)jsonFriendObject.SelectToken("data[0].category");

            int i = 0;
            friendData.Music = string.Empty;
            friendData.Television = string.Empty;
            friendData.Cinema = string.Empty;

            List<string> music = new List<string>();
            List<string> television = new List<string>();
            List<string> movies = new List<string>();

            while (like_category != null)
            {
                switch (like_category)
                {
                    case "Music":
                    case "Musicians":
                        ////friendData.Music = (string)jsonFriendObject.SelectToken("data[" + i + "].name");
                        music.Add((string)jsonFriendObject.SelectToken("data[" + i + "].name"));
                        break;
                    case "Television":
                        ////friendData.Television = (string)jsonFriendObject.SelectToken("data[" + i + "].name");
                        television.Add((string)jsonFriendObject.SelectToken("data[" + i + "].name"));
                        break;
                    case "Movie":
                    case "Film":
                        ////friendData.Cinema = (string)jsonFriendObject.SelectToken("data[" + i + "].name");
                        movies.Add((string)jsonFriendObject.SelectToken("data[" + i + "].name"));
                        break;
                }

                i++;
                like_category = (string)jsonFriendObject.SelectToken("data[" + i + "].category");                         
            }

            List<string> music_shuffle = Functions.ShuffleList<string>(music);
            List<string> television_shuffle = Functions.ShuffleList<string>(television);
            List<string> movies_shuffle = Functions.ShuffleList<string>(movies);

            if (music_shuffle.Count > 0)
            {
                friendData.Music = music_shuffle[0];
            }

            if (television_shuffle.Count > 0)
            {
                friendData.Television = television_shuffle[0];
            }

            if (movies_shuffle.Count > 0)
            {
                friendData.Cinema = movies_shuffle[0];
            }

            return friendData;          
        }
            
        /// <summary>
        /// Description for Method.</summary>
        /// <param name="jsonFriends"> Parameter description for jsonFriends goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        private List<string> GetFriendsIdByJson(string jsonFriends)
        {
            JObject jsonFriendObject = JObject.Parse(jsonFriends);
            List<string> friendsId = new List<string>();

            string id = (string)jsonFriendObject.SelectToken("data[0].id");

            int i = 1;
            while (id != null)
            {
                friendsId.Add(id);
                id = (string)jsonFriendObject.SelectToken("data[" + i + "].id");
                i++;
            }

            return friendsId;
        }

        //// Only for the Prototype

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="jsonFriends"> Parameter description for jsonFriends goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        private List<string> GetFriendsNamesByJson(string jsonFriends)
        {
            JObject jsonFriendObject = JObject.Parse(jsonFriends);
            List<string> friendsNames = new List<string>();

            string name = (string)jsonFriendObject.SelectToken("data[0].name");

            int i = 1;
            while (name != null)
            {
                friendsNames.Add(name);
                name = (string)jsonFriendObject.SelectToken("data[" + i + "].name");
                i++;
            }

            return friendsNames;
        }
    }
}
