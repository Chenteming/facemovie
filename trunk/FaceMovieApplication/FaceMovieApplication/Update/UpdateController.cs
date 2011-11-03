using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaceMovieApplication.Data;
using FaceMovieApplication.Algorithm;

namespace FaceMovieApplication.Update
{
    public class UpdateController
    {
        public void UpdateSimilarities()
        {
            /*
             *  For Each movie1 in Movies
             *      For Each movie2 <> movie1 in Movies
             *          sim = ComputeSimilarity(movie1, movie2)
             *          save(movie1, movie2, sim)
             *      EndFor
             * EndFor
            */
            FaceMovieModelContainer context = new FaceMovieModelContainer();
            MovieSimilarity movieSimilarity;
            IItemBasedAlgorithm ibm = new ItemBasedAlgorithm();
            foreach (Movie movie1 in context.MovieSet)
            {
                foreach (Movie movie2 in context.MovieSet)
                {
                    if (movie1.MovieId != movie2.MovieId)
                    {
                        movieSimilarity = new MovieSimilarity();
                        movieSimilarity.Movie_1 = movie1;
                        movieSimilarity.Movie_2 = movie2;
                        movieSimilarity.Similarity = ibm.ComputeSimilarity(movie1, movie2);
                        context.MovieSimilaritySet.AddObject(movieSimilarity);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}