using DBM_ConsoleApp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                Console.WriteLine("-------------------------------------------------------------------------------------------");
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
                    case "2":
                        Console.Write("ID:");
                        var id = Console.ReadLine();
                        int idInt;
                        if (id is null || id.Equals(String.Empty) || id.Equals("") || !int.TryParse(id, out idInt))
                        {
                            Console.WriteLine("Incorrect input."); break;
                        }
                        result = await GetCourseByIdAsync(idInt);
                        Console.WriteLine(result);
                        var course = ConvertJsonToCourse(result);
                        PrintCourses(course);
                        break;
                    case "3":
                        Console.Write("Course Name:");
                        var name = Console.ReadLine();
                        if(name is null || name.Equals(String.Empty) || name.Equals(""))
                        {
                            Console.WriteLine("Incorrect input."); break;
                        }
                        Course courseToAdd = new() { CourseName = name };
                        result = await AddCourseAsync(courseToAdd);
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

        static async Task<string> GetCourseByIdAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"/course/{id}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        static ICollection<Course> ConvertJsonToCourse(string json)
        {
            var jsonObject = JObject.Parse(json);

            if (jsonObject["statusCode"] != null)
            {
                var result = jsonObject.ToObject<CourseWrapper>();
                if (result is null) throw new NullReferenceException();
                List<Course> courses = new List<Course>();
                courses.Add(result.Value);
                return courses;
            }
            else
            {
                var result = jsonObject.ToObject<ICollection<Course>>();
                if (result is null) throw new NullReferenceException();
                return result;
            }
            //var result = JsonConvert.DeserializeObject<CourseWrapper>(json);
            //if (result is null) throw new NullReferenceException();
            //if (result.Value != null) return result.Value;
            
            ////fallback
            //var resultAlt = JsonConvert.DeserializeObject<ICollection<Course>>(json);
            //return resultAlt ?? throw new NullReferenceException();
        }
    }
}