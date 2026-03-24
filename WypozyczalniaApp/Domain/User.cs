using System;

namespace WypozyczalniaApp.Domain
{
    public abstract class User
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public abstract int MaxRentalsLimit { get; }

        protected User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public class Student : User
    {
        public override int MaxRentalsLimit => 2;

        public Student(string firstName, string lastName) : base(firstName, lastName) { }
    }

    public class Employee : User
    {
        public override int MaxRentalsLimit => 5;

        public Employee(string firstName, string lastName) : base(firstName, lastName) { }
    }
}