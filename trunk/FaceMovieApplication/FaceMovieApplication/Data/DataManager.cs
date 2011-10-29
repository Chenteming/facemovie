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
            foreach(KeyValuePair<long,User> dictUser in dictUsers)
            {
                user = dictUser.Value;
                bool userExists = context.UserSet.Where(u => u.UserFacebookId == user.UserFacebookId).Count() > 0;
                if (userExists)
                {
                    User userDB = context.UserSet.Where(u => u.UserFacebookId == user.UserFacebookId).First();
                    //// Updates the data of the user
                    userDB.UserFirstName = user.UserFirstName;
                    userDB.UserLastName = user.UserLastName;
                    //// context.Detach(user);
                }
                else
                {
                    context.AddToUserSet(user);
                }
            }
            context.SaveChanges();
        }
    }
}