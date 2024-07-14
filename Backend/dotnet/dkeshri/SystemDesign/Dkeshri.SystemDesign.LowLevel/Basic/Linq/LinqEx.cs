using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace Dkeshri.SystemDesign.LowLevel.Basic.Linq
{
    public class LinqEx : IExecute
    {
        public void run()
        {
            ListManager listManager = new ListManager();
            List<Person> persons = listManager.LoadSampleData();
            //persons = persons.OrderBy(x => x.LastName).ToList();
            //persons = persons.OrderByDescending(x => x.LastName).ToList();
            //persons = persons.OrderBy(x => x.LastName).ThenBy(x => x.YearOfExpecience).ToList();
            //persons = persons.OrderBy(x => x.LastName).ThenByDescending(x => x.YearOfExpecience).ToList();
            persons = persons.Where(x => x.YearOfExpecience>2).OrderBy(x => x.LastName).ToList();
            foreach (Person person in persons)
            {
                Console.WriteLine($"{person.FullName} Dob: {person.Dob.ToShortDateString()} Experience: {person.YearOfExpecience} Salary: {person.Salary}");
            }

            int TotalExperience = persons.Sum(x => x.YearOfExpecience);
            Console.WriteLine(TotalExperience);


        }
    }
}