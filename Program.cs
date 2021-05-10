using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net.Http;
using Newtonsoft.Json;



namespace ConnexDemo
{
    
    public class User
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            //create db context reference
            var userContext = new UserDbContext();


            //Get user data from JSON
            string jsonUrl = "https://jsonplaceholder.typicode.com/todos/1";
            HttpClient client = new HttpClient();
            client.GetAsync(jsonUrl);
            HttpResponseMessage response = client.GetAsync(jsonUrl).Result;
            Console.WriteLine("JSON received through HttpClient: ");
            Console.Write(response.Content.ReadAsStringAsync().Result.ToString());
            Console.WriteLine("Press ENTER to continue");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            //Deserialize JSON to an object
            User user = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result.ToString());


            //Create a record for the new User object
            userContext.Users.Add(user);
            userContext.SaveChanges();


            //Read the record from the database
            User readUser = userContext.Users.First();
            string readJson = JsonConvert.SerializeObject(readUser, Formatting.Indented);
            Console.WriteLine("Record read from SQL and reserialized to JSON:");
            Console.Write(readJson);
            Console.WriteLine("Press ENTER to continue");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            //Modify the record
            readUser.completed = true;
            userContext.SaveChanges();


            //Read the record again
            readUser = userContext.Users.First();
            readJson = JsonConvert.SerializeObject(readUser, Formatting.Indented);
            Console.WriteLine("Modified record read from SQL and reserialized to JSON:");
            Console.Write(readJson);
            Console.WriteLine("Press ENTER to continue");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            //Delete the record
            userContext.Users.Remove(readUser);
            userContext.SaveChanges();
            Console.WriteLine("Record deleted");
            Console.WriteLine("Press ENTER to finish");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

        }


    }
}
