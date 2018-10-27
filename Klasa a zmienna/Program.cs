using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasa_a_zmienna
{
    class Program
    {
        static void Main(string[] args)
        {
            Diary diary = new Diary();
            diary.AddRating(3);
            diary.AddRating(5);
            Diary diary2 = new Diary();
            diary.AddRating(8);


        }

    }
}
