using Faker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SerializationTask
{
    internal class PersonManager
    {
        private const int maxNumberOfChildren = 3;
        private const int numberCreditCardSymbols = 16;
        private const int numberPhoneNumberSymbols = 16;
        private int currentYear = DateTime.Now.Year;
        private int id = 1;

        static Random rand = new Random();
        public int CalculateAverageChildAge(Person[] personWithChild)
        {
            if (personWithChild.Length != 0)
            {
                int numberOfChild = 0;
                int maxChildAge = 0;
                for (int i = 0; i < personWithChild.Length; i++)
                {
                    for (int j = 0; j < personWithChild[i].Children.Length; j++)
                    {
                        int age = DateTimeOffset.FromUnixTimeSeconds(personWithChild[i].Children[j].BirthDate).Year;
                        maxChildAge += currentYear - age;
                        numberOfChild++;
                    }
                }
                return maxChildAge / numberOfChild;
            }
            return 0;
        }

        public List<Person> PersonGenerate(int count)
        {
            List<Person> persons = new List<Person>(count);
            while (id <= count)
            {
                Person person = new Person
                {
                    Id = id,
                    TransportId = Guid.NewGuid(),
                    Gender = Faker.EnumFaker.SelectFrom<Gender>(),
                    LastName = NameFaker.LastName(),
                    SequenceId = id,
                    CreditCardNumbers = GetCreditCardNumber(),
                    Phones = GetPhoneNumbers(),
                    BirthDate = ((DateTimeOffset)Faker.DateTimeFaker.DateTime(DateTime.Now.AddYears(-100), DateTime.Now.AddYears(-18))).ToUnixTimeSeconds(),
                    Salary = rand.Next(20000, 100000),
                    IsMarred = Faker.BooleanFaker.Boolean()
                };
                person.FirstName = person.Gender == Gender.Male ? Faker.NameFaker.MaleFirstName() : Faker.NameFaker.FemaleFirstName();
                id++;
                if (id <= count - maxNumberOfChildren) person.Children = ChildrenGenerator(person.LastName, person.BirthDate);
                else person.Children = Array.Empty<Child>();

                person.Age = currentYear - DateTimeOffset.FromUnixTimeSeconds(person.BirthDate).Year;
                persons.Add(person);
            }

            return persons;
        }
        public Child[] ChildrenGenerator(string parentLastName, long parentBirthDay)
        {
            var result = new Child[rand.Next(0, maxNumberOfChildren)];
            if (result.Length != 0)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new Child
                    {
                        Id = id,
                        Gender = Faker.EnumFaker.SelectFrom<Gender>(),
                        LastName = parentLastName,
                        BirthDate = ((DateTimeOffset)Faker.DateTimeFaker.DateTime(DateTimeOffset.FromUnixTimeSeconds(parentBirthDay).DateTime.AddYears(18), DateTime.Now)).ToUnixTimeSeconds(),

                    };
                    result[i].FirstName = result[i].Gender == Gender.Male ? Faker.NameFaker.MaleFirstName() : Faker.NameFaker.FemaleFirstName();
                    id++;
                }
            }

            return result;
        }

        public string[] GetCreditCardNumber()
        {
            var result = new string[numberCreditCardSymbols];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = rand.Next(0, 9).ToString();
            }
            return result;
        }
        public string[] GetPhoneNumbers()
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
