using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Console;

namespace UfcConsoleClient
{
    class Search
    {
        static string input, category, url;
        static async Task Main()
        {
            WriteLine("UFC FIGHTER DATABASE");
            using var client = new HttpClient();
            while (true)
            {
                input = GetInput($"\n\nEnter number for search category:\n1: All Fighters\n2: for Name\n3: "
                    + $"Nickname\n4: Height\n5: Height Range\n6: Weight\n7:Weight Range\n8:Country\n");

                if (input.Equals("Q", StringComparison.OrdinalIgnoreCase)) { Environment.Exit(0); };

                category = input switch
                {
                    "1" => "All",
                    "2" => "Name",
                    "3" => "Nickname",
                    "4" => "Height",
                    "5" => "Height Range",
                    "6" => "Weight",
                    "7" => "Weight Range",
                    "8" => "Country",
                    _ => ""
                };

                url = GetSearchString();
                var response = await client.GetStringAsync(url);
                JObject parsedData = JObject.Parse(response);

                foreach (var pair in parsedData)
                {
                    WriteLine($"{pair.Key.Replace("\"", "")} {pair.Value.ToString().Replace("\"", "")}");
                }
            }
        }

        static string GetSearchString()
        {
            if (category.Equals("All")) { return "http://ec2-18-203-4-191.eu-west-1.compute.amazonaws.com:3000/"; }

            if (category.Equals("Height Range") || category.Equals("Weight Range"))
            {
                string minimum = GetInput("Min");
                string maximum = GetInput("Max");
                category = input.Equals("5") ? "heightrange" : "weightrange";
                return "http://ec2-18-203-4-191.eu-west-1.compute.amazonaws.com:3000/"
                    + $"{category}?min={minimum}&max={maximum}";
            }

            input = GetInput(category);
            return "http://ec2-18-203-4-191.eu-west-1.compute.amazonaws.com:3000/"
                + $"{category.ToLower()}?{category.ToLower()}={input}";
        }

        static string GetInput(string text)
        {
            WriteLine("Enter " + text);
            return ReadLine().Trim();
        }
    }
}
