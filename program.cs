using System;
using System.Collections.Generic;
using System.Linq;

namespace FestivalBoothTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            BoothManager boothManager = new BoothManager();
            int choice;

            do
            {
                Console.Clear();
                Console.WriteLine("Festival Booth Tracker");
                Console.WriteLine("1. Register a new booth");
                Console.WriteLine("2. Record attendee at a booth");
                Console.WriteLine("3. Display all booths with attendee count");
                Console.WriteLine("4. View feedback for a booth");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                
                // Input validation for menu choice
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
                {
                    Console.Write("Invalid choice. Please enter a number between 1 and 5: ");
                }

                switch (choice)
                {
                    case 1:
                        boothManager.RegisterBooth();
                        break;
                    case 2:
                        boothManager.RecordAttendee();
                        break;
                    case 3:
                        boothManager.DisplayBooths();
                        break;
                    case 4:
                        boothManager.ViewFeedback();
                        break;
                    case 5:
                        Console.WriteLine("Exiting the program. Thank you!");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            } while (choice != 5);
        }
    }

    // Class to represent a booth
    public class Booth
    {
        public int BoothID { get; set; }
        public string BoothName { get; set; }
        public string BoothType { get; set; }
        public List<Attendee> Attendees { get; set; } = new List<Attendee>();
    }

    // Class to represent an attendee
    public class Attendee
    {
        public string Name { get; set; }
        public int? Rating { get; set; }
    }

    // Class to manage booths and attendees
    public class BoothManager
    {
        private List<Booth> booths = new List<Booth>();
        private int nextBoothID = 1;

        // Method to register a new booth
        public void RegisterBooth()
        {
            Console.Write("Enter Booth Name: ");
            string boothName = Console.ReadLine();

            Console.Write("Enter Booth Type (Game, Food, Craft, Info): ");
            string boothType = Console.ReadLine();

            Booth booth = new Booth
            {
                BoothID = nextBoothID++,
                BoothName = boothName,
                BoothType = boothType
            };

            booths.Add(booth);
            Console.WriteLine("Booth registered successfully!");
        }

        // Method to record an attendee at a booth
        public void RecordAttendee()
        {
            Console.Write("Enter Booth ID: ");
            int boothID;
            while (!int.TryParse(Console.ReadLine(), out boothID) || !booths.Any(b => b.BoothID == boothID))
            {
                Console.Write("Invalid Booth ID. Please enter a valid Booth ID: ");
            }

            Console.Write("Enter Attendee Name: ");
            string attendeeName = Console.ReadLine();

            Console.Write("Enter Rating (1-5, or leave blank for no rating): ");
            int? rating = null;
            string ratingInput = Console.ReadLine();
            if (int.TryParse(ratingInput, out int r) && r >= 1 && r <= 5)
            {
                rating = r;
            }

            Attendee attendee = new Attendee
            {
                Name = attendeeName,
                Rating = rating
            };

            booths.First(b => b.BoothID == boothID).Attendees.Add(attendee);
            Console.WriteLine("Attendee recorded successfully!");
        }

        // Method to display all booths with attendee count
        public void DisplayBooths()
        {
            Console.WriteLine("Booths and Attendee Count:");
            foreach (var booth in booths)
            {
                Console.WriteLine($"Booth ID: {booth.BoothID}, Name: {booth.BoothName}, Type: {booth.BoothType}, Attendees: {booth.Attendees.Count}");
            }
        }

        // Method to view feedback for a specific booth
        public void ViewFeedback()
        {
            Console.Write("Enter Booth ID: ");
            int boothID;
            while (!int.TryParse(Console.ReadLine(), out boothID) || !booths.Any(b => b.BoothID == boothID))
            {
                Console.Write("Invalid Booth ID. Please enter a valid Booth ID: ");
            }

            var booth = booths.First(b => b.BoothID == boothID);
            Console.WriteLine($"Feedback for Booth: {booth.BoothName}");
            foreach (var attendee in booth.Attendees)
            {
                Console.WriteLine($"Attendee: {attendee.Name}, Rating: {attendee.Rating}");
            }

            var ratings = booth.Attendees.Where(a => a.Rating.HasValue).Select(a => a.Rating.Value).ToList();
            if (ratings.Count > 0)
            {
                double averageRating = ratings.Average();
                Console.WriteLine($"Average Rating: {averageRating:F2}");
            }
            else
            {
                Console.WriteLine("No ratings available for this booth.");
            }
        }
    }
}
