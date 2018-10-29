using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SingleTon
{
    class Program
    {
        static void Main(string[] args)
        {
            Singleton s1 = Singleton.Instance;

            s1.name = "Jan";
        }

        
    }

    Console.WriteLine(s1.name);

}
