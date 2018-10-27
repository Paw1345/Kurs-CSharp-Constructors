using System;
using System.Collections.Generic;
using System.Linq;


namespace Klasa_a_zmienna
{
    class Diary
    {
        
        List<float> ratings = new List<float>();


        public void AddRating(float rating)
        {
            ratings.Add(rating);
        }

        public float CalculateAverage()
        {
            float sum = 0, avg = 0;

            foreach (var rating in ratings)
            {
                sum = sum + rating;
            }

            avg = sum / ratings.Count();

            return avg;
            
        }

        public float GiveMaxRating()
        {
            return ratings.Max();

           
        }

        public float GiveMinRating()
        {
            return ratings.Min();


        }

    }

}
