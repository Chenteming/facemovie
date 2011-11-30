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
                }
                else
                {
                    context.AddToUserSet(user);
                }
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Description for Method.</summary>
        /// <param name="name"> Parameter description for name goes here</param>
        /// <returns>
        /// Return results are described through the returns tag.</returns>
        public string GetParameter(string name)
        {
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            IQueryable<string> res = from p in context.Parameters
                                     where p.ParameterName == name
                                     select p.ParameterValue;
            if (res != null && res.Count() != 0)
            {
                return res.First();
            }
            else
            {
                throw new Exception("No se encontró el parámetro con nombre " + name);
            }
        }

        public void DeleteAllMovieSimilarities(FaceMovieModelContainer context)
        {
            context.ExecuteStoreCommand("TRUNCATE TABLE MovieSimilaritySet");
        }
    }
}