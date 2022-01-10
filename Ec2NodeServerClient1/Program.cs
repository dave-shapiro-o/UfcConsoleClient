using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Console;

namespace Ec2NodeServerClient1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient();
            string searchString;
            do
            {
                WriteLine($"\n\nEnter \"All\" for all fighters\nEnter \"Name\" for name search");
                string searchType = ReadLine().Trim();
                searchString = string.Empty;

                switch (searchType)
                {
                    case "Name":
                        WriteLine("Enter the fighter's name");
                        string name = ReadLine();
                        searchString = "http://ec2-18-203-4-191.eu-west-1.compute.amazonaws.com:3000/name?name=" + name.ToString();
                        break;
                    case "All":
                    default:
                        searchString = "http://ec2-18-203-4-191.eu-west-1.compute.amazonaws.com:3000/";
                        break;
                }

                var result = await client.GetStringAsync(searchString);

                JObject parsed = JObject.Parse(result);

                foreach (var pair in parsed)
                {
                    Console.WriteLine("{0}:   {1}", pair.Key.Replace("\"", ""), pair.Value.ToString().Replace("\"", ""));
                }
            }
            while (!searchString.Equals("Q", StringComparison.OrdinalIgnoreCase));

            //WriteLine(result.ToString());
        }
    }
}
