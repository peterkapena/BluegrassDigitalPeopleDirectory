using BluegrassDigitalPeopleDirectory.Models;
using Microsoft.EntityFrameworkCore;

namespace BluegrassDigitalPeopleDirectory.Services
{
    public interface IPeopleService
    {
        Task GenerateAndSavePersonsAsync();
        Task<IEnumerable<Person>> GetPeople();
        Task<Person> GetPerson(long id);
        Task<Person> UpdatePerson(long id, Person person);
        Task<Person> AddPerson(Person person);
        Task<Person> DeletePerson(long id);
        bool PersonExists(long id);
    }
    public class PeopleService(DBContext dBContext) : IPeopleService
    {
        public DBContext DBContext { get; } = dBContext;

        public async Task<IEnumerable<Person>> GetPeople()
        {
            return await DBContext.People.ToListAsync();
        }

        public async Task GenerateAndSavePersonsAsync()
        {
            var persons = new List<Person>();

            for (int i = 0; i < 100; i++)
            {
                var generatedName = NameGenerator.GenerateName();
                var person = new Person
                {
                    Name = generatedName.Split(' ')[0],
                    Surname = generatedName.Split(' ')[1],
                    Country = i < 50 ? "Congo, Democratic Republic of the" : null,
                    Gender = i % 2 == 0 ? "M" : "F"
                };
                persons.Add(person);
            }

            await DBContext.People.AddRangeAsync(persons);
            await DBContext.SaveChangesAsync();
        }

        public async Task<Person> GetPerson(long id)
        {
            var person = await DBContext.People.FindAsync(id);
            return person;
        }
        public bool PersonExists(long id)
        {
            return DBContext.People.Any(e => e.Id == id);
        }

        public async Task<Person> UpdatePerson(long id, Person person)
        {
            DBContext.Entry(person).State = EntityState.Modified;

            try
            {
                await DBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    throw new InvalidDataException();
                }
                else
                {
                    throw;
                }
            }

            return person;
        }

        public async Task<Person> AddPerson(Person person)
        {
            await DBContext.People.AddAsync(person);
            await DBContext.SaveChangesAsync();

            return person;
        }
        public async Task<Person> DeletePerson(long id)
        {
            var person = await DBContext.People.FindAsync(id);
            DBContext.People.Remove(person);
            await DBContext.SaveChangesAsync();

            return person;
        }
    }
}
