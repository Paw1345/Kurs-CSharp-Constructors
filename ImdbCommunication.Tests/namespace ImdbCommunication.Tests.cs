using System;
using Xunit;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ImdbCommunication.Tests
{
    public class imdb_api_exploration
    {
        string execute(string query)
        {
            string url = "http://www.omdbapi.com/?=" + query;

            return new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        }
        [Fact]
        public void returns_response()
        {
            string results = execute("Batman");

            Console.Write(results);

            Assert.NotEmpty(results);
          
        }

        
    }

    
}
