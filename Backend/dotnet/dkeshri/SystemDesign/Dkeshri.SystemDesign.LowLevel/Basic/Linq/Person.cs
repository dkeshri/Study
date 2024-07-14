using System;
using System.Collections.Generic;

namespace Dkeshri.SystemDesign.LowLevel.Basic.Linq
{
    public class Person{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearOfExpecience { get; set; }
        public DateTime Dob { get; set; }
        public double Salary { get; set; }
        public string FullName { get{
            return $"{FirstName} {LastName}";
        } }

    }
    public class ListManager{
        public List<Person> LoadSampleData(){
            return new List<Person>(){
                new Person(){FirstName = "Deepak",LastName="keshri",YearOfExpecience = 2
                ,Dob = Convert.ToDateTime("9/7/1995"),Salary = 100000},
                new Person(){FirstName = "Shubham",LastName="sharma",YearOfExpecience = 1
                ,Dob = Convert.ToDateTime("12/12/1995"),Salary = 50000},
                new Person(){FirstName = "ankit",LastName="sagger",YearOfExpecience = 7
                ,Dob = Convert.ToDateTime("9/8/1972"),Salary = 80000},
                new Person(){FirstName = "Monu",LastName="sharma",YearOfExpecience = 10
                ,Dob = Convert.ToDateTime("12/11/1993"),Salary = 90000},
                new Person(){FirstName = "PK",LastName="Suter",YearOfExpecience = 11
                ,Dob = Convert.ToDateTime("9/6/1996"),Salary = 100000}

            };
        }
    }
}