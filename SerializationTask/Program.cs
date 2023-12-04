using Faker;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SerializationTask
{
    internal class Program
    {
        const int numberCreditCardSymbols = 16;
        const int numberPhoneNumberSymbols = 16;
        static int CurrentYear = DateTime.Now.Year;

        static Random rand = new Random();
        static void Main(string[] args)
        {
            var persons = PersonGenerate(10000);

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
            Console.WriteLine($"Average value of child age: {CalculateAverageChildAge(personWithChild)}");

        }

        private static int CalculateAverageChildAge(Person[] personWithChild)
        {
            if (personWithChild.Length != 0) {
                int numberOfChild = 0;
                int maxChildAge = 0;
                for (int i = 0; i < personWithChild.Length; i++)
                {
                    for (int j = 0; j < personWithChild[i].Children.Length; j++)
                    {
                        int age = DateTimeOffset.FromUnixTimeSeconds(personWithChild[i].Children[j].BirthDate).Year;
                        maxChildAge += CurrentYear - age;
                        numberOfChild++;
                    }
                }
                return maxChildAge / numberOfChild;
            }
            return 0;
        }

        static List<Person> PersonGenerate(int count)
        {
            int id = 1;
            List<Person> persons = new List<Person>(count);
            for (int i = 0; i < count; i++)
            {
                Person person = new Person
                {
                    Id = id,
                    TransportId = Guid.NewGuid(),
                    FirstName = NameFaker.FirstName(),
                    LastName = NameFaker.LastName(),
                    SequenceId = id,
                    CreditCardNumbers = GetCreditCardNumber(),
                    Age = rand.Next(18, 100),
                    Phones = GetPhoneNumbers(),
                    BirthDate = ((DateTimeOffset)Faker.DateTimeFaker.DateTime(DateTime.Now.AddYears(-100), DateTime.Now.AddYears(-18))).ToUnixTimeSeconds(),
                    Salary = rand.Next(20000, 100000),
                    IsMarred = Faker.BooleanFaker.Boolean(),
                    Gender = Faker.EnumFaker.SelectFrom<Gender>(),
                };
                person.Children = ChildrenGenerator(person.LastName, person.BirthDate);
                person.Age = DateTimeOffset.FromUnixTimeSeconds(person.BirthDate).Year;
                id++;
                persons.Add(person);
            }

            return persons;
        }
        static Child[] ChildrenGenerator(string parentLastName, long parentBirthDay)
        {
            var result = new Child[rand.Next(0, 3)];
            if (result.Length != 0)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new Child
                    {
                        Id = i + 1,
                        Gender = Faker.EnumFaker.SelectFrom<Gender>(),
                        LastName = parentLastName,
                        BirthDate = ((DateTimeOffset)Faker.DateTimeFaker.DateTime(DateTimeOffset.FromUnixTimeSeconds(parentBirthDay).DateTime.AddYears(17), DateTime.Now)).ToUnixTimeSeconds(),

                    };
                    result[i].FirstName = result[i].Gender == Gender.Male ? Faker.NameFaker.MaleFirstName() : Faker.NameFaker.FemaleFirstName();
                }
            }

            return result;
        }

        static string[] GetCreditCardNumber()
        {
            var result = new string[numberCreditCardSymbols];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = rand.Next(0, 9).ToString();
            }
            return result;
        }
        static string[] GetPhoneNumbers()
        {
            var result = new string[numberPhoneNumberSymbols];
            result[0] = "7";
            for (int i = 1; i < result.Length; i++)
            {
                result[i] = rand.Next(0, 9).ToString();
            }
            return result;
        }




    }
}