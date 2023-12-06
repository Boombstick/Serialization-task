using Faker;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SerializationTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PersonManager personManager = new PersonManager();
            var persons = personManager.PersonGenerate(10000);

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string fileName = "C:\\Users\\Boomb\\Desktop\\Persons.json";
            string jsonString = JsonSerializer.Serialize(persons, jsonOptions);
            File.WriteAllText(fileName, jsonString);

            persons.Clear();
            var stringFromJson = File.ReadAllText(fileName);
            var personFromJson = JsonSerializer.Deserialize<List<Person>>(stringFromJson, jsonOptions);

            var personWithChild = personFromJson.Where(x => x.Children.Length != 0).ToArray();

            var personCount = personFromJson.Count();
            Console.WriteLine($"Persons count: {personCount}");
            Console.WriteLine($"Persons credit card count: {personCount}");
            Console.WriteLine($"Average value of child age: {personManager.CalculateAverageChildAge(personWithChild)}");
            Console.WriteLine(  );

        }
    }
}