using System;
using System.Collections.Generic;

// Enum for Houses
public enum House
{
    Fangs,
    Claws,
    Shadows,
    Spirits
}

// Base Student Class
public abstract class Student
{
    public string Name { get; }
    public int Age { get; }
    public House House { get; }

    protected Student(string name, int age, House house)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be blank.");
        if (age < 10)
            throw new ArgumentException("Age must be 10 or older.");

        Name = name;
        Age = age;
        House = house;
    }

    public abstract void UseAbility();
}

// Derived Classes for Each Type of Student
public class Vampire : Student
{
    public Vampire(string name, int age, House house) : base(name, age, house) { }

    public override void UseAbility()
    {
        Console.WriteLine($"{Name} bites!");
    }
}

public class Werewolf : Student
{
    public Werewolf(string name, int age, House house) : base(name, age, house) { }

    public override void UseAbility()
    {
        Console.WriteLine($"{Name} howls at the moon!");
    }
}

public class Psychic : Student
{
    public Psychic(string name, int age, House house) : base(name, age, house) { }

    public override void UseAbility()
    {
        Console.WriteLine($"{Name} has a vision!");
    }
}

// Student Manager Class
public class StudentManager
{
    private List<Student> students = new List<Student>();
    public event Action<string>? StudentAdmitted;
    public event Action<string>? AbilityUsed;

    public void AdmitStudent(string name, int age, House house)
    {
        if (students.Exists(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Duplicate student name.");

        Student student;
        switch (house)
        {
            case House.Fangs:
                student = new Vampire(name, age, house);
                break;
            case House.Claws:
                student = new Werewolf(name, age, house);
                break;
            case House.Shadows:
                student = new Psychic(name, age, house);
                break;
            case House.Spirits:
                throw new ArgumentException("Invalid house.");
            default:
                throw new ArgumentException("Invalid house.");
        }

        students.Add(student);
        StudentAdmitted?.Invoke($"{name} has been admitted to {house}.");
    }

    public void UseStudentAbility(string name)
    {
        var student = students.Find(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (student == null)
            throw new InvalidOperationException("Student not found.");

        student.UseAbility();
        AbilityUsed?.Invoke($"{name} used their ability.");
    }

    public void ListStudentsByHouse()
    {
        foreach (var house in Enum.GetValues(typeof(House)))
        {
            Console.WriteLine($"{house}:");
            foreach (var student in students.FindAll(s => s.House == (House)house))
            {
                Console.WriteLine($" - {student.Name}");
            }
        }
    }
}

// Main Program
class Program
{
    static void Main()
    {
        var manager = new StudentManager();
        manager.StudentAdmitted += (message) => Console.WriteLine(message);
        manager.AbilityUsed += (message) => Console.WriteLine(message);

        while (true)
        {
            Console.WriteLine("1. Admit Student");
            Console.WriteLine("2. Use Student Ability");
            Console.WriteLine("3. List Students by House");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter student name: ");
                        var name = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(name))
                        {
                            Console.WriteLine("Name cannot be empty!");
                            continue;
                        }
                            Console.Write("Enter student age: ");
                        var ageInput = Console.ReadLine();
                        if (ageInput == null || !int.TryParse(ageInput, out var age))
                        {
                            Console.WriteLine("Invalid age input.");
                            continue;
                        }
                        Console.Write("Enter house (Fangs, Claws, Shadows, Spirits): ");
                        var houseInput = Console.ReadLine();
                        if (houseInput == null || !Enum.TryParse(houseInput, true, out House house))
                        {
                            Console.WriteLine("Invalid house input.");
                            continue;
                        }
                        manager.AdmitStudent(name, age, house);
                        break;

                    case "2":
                          Console.Write("Enter student name to use ability: ");
                          var studentName = Console.ReadLine();
                         if (string.IsNullOrWhiteSpace(studentName))  // ADD THIS NULL CHECK
                     {
                             Console.WriteLine("Name cannot be empty!");
                                  continue;
                                 }
                             manager.UseStudentAbility(studentName);
                      break;

                    case "3":
                        manager.ListStudentsByHouse();
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
