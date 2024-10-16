using DBM_ConsoleApp;
using Newtonsoft.Json;
using System;
using System.Net.Http.Json;

namespace DBM_ConsoleApp
{
    /// <summary>
    /// WARNING
    /// Code inside this class should be moved to separate classes
    /// For now it's here because it's just for testing purpose
    /// </summary>
    internal class Program
    {
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri("https://localhost:7233");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            await Run();
        }

        static async Task Run()
        {
            var run = true;
            while (run)
            {
                Console.WriteLine("-------------------------------------------------------------------------------------------\n");
                Console.WriteLine("1 - Get all courses\n2 - Get course by ID\n3 - Add new course\n4 - Delete course\n0 - Quit");
                var key = Console.ReadKey();
                string result = string.Empty;
                Console.WriteLine();
                switch (key.KeyChar.ToString())
                {
                    case "0":
                        run = false;
                        break;
                    case "1":
                        result = await GetAllCoursesAsync();
                        var courses = ConvertJsonToCourse(result);
                        PrintCourses(courses);
                        break;
                    case "3":
                        Console.Write("Course Name:");
                        var name = Console.ReadLine();
                        if(name is null || name.Equals(String.Empty) || name.Equals(""))
                        {
                            Console.WriteLine("Incorrect input."); break;
                        }
                        Course course = new() { CourseName = name };
                        result = await AddCourseAsync(course);
                        Console.WriteLine(result);
                        break;
                    default:
                        Console.WriteLine("\nIncorrect input.");
                        break;
                }
            }
        }

        static void PrintCourses(IEnumerable<Course> courses)
        {
            foreach (var item in courses)
            {
                Console.WriteLine($"{item.ID} {item.CourseName}");
            }
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

        static ICollection<Course> ConvertJsonToCourse(string json)
        {
            var result = JsonConvert.DeserializeObject<List<Course>>(json);
            return result ?? throw new NullReferenceException();
        }
    }
}