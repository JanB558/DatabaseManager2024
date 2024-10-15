using DBM_ConsoleApp;
using System;
using System.Net.Http.Json;

namespace DBM_ConsoleApp
{
    internal class Program
    {
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri("https://localhost:7233");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = await GetAllCoursesAsync();
            Console.WriteLine(response);
            Console.ReadKey();
        }

        static async Task<string> AddCourseAsync(Course course)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/course", course);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        static async Task<string> GetAllCoursesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("/course");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}