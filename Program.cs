using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Strava
{
    internal class Program
    {
        static User currentUser = null;
        static List<User> users = new List<User>();

        static void Main(string[] args)
        {
            while (true)
            {
                LoginPrompt();
                MainMenu();
            }
        }

        static void Logout()
        {
            currentUser = null;
            Console.WriteLine("Logging out...");
            Console.WriteLine();
        }
        static void LoginPrompt()
        {
            while (currentUser == null)
            {
                Console.Write("Please enter user name: ");
                string userInput = Console.ReadLine();
                User user = users.FirstOrDefault(p => p.userName == userInput);
                if (user != null)
                {
                    Console.Write("Please type in password: ");
                    string userPassword = Console.ReadLine();
                    if (user.passWord.Equals(userPassword))
                    {
                        Console.WriteLine("Logging in...\n");
                        currentUser = user;
                    }
                    else
                    {
                        bool incorrectpass = true;
                        while (incorrectpass)
                        {
                            Console.WriteLine("Incorrect password.");
                            Console.Write("Please re-type password, or type \"change\" to change users: ");
                            userPassword = Console.ReadLine();
                            if (userPassword.Trim().ToLower() == "change")
                            {
                                incorrectpass = false;
                            }
                            else if (user.passWord.Equals(userPassword))
                            {
                                Console.WriteLine("Logging in...");
                                incorrectpass = false;
                                currentUser = user;
                            }
                        }
                    }
                }
                else
                {
                    Console.Write($"Username: \"{userInput}\" not found. \nWould you like to create a new user named \"{userInput}\"? ");
                    string answer = Console.ReadLine().Trim().ToLower();
                    if (answer == "yes" || answer == "yeah" || answer == "yea" || answer == "ye" || answer == "yup")
                    {
                        Console.Write("Please create a password: ");
                        string passInput = Console.ReadLine();
                        currentUser = new User(userInput, passInput);
                        users.Add(currentUser);
                        Console.WriteLine($"User \"{userInput}\" created. \n");
                        Console.WriteLine("Logging in...\n");
                    }
                }

                if (currentUser != null) 
                    Console.WriteLine($"\n\nWelcome {currentUser.userName}!");

            }

        }
        static void MainMenu()
        {
            bool programRunning = true;
            while (programRunning)
            {
                string option = MenuOptions();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        while (true)
                        {
                            string type = TrimmedInput("Choose: Bike activity or run activity? (Type \"bike\" or \"run\"): ");
                            if (type.Equals("run") || type.Equals("bike"))
                            {
                                currentUser.activities.Add(AddPrompts(type));
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please choose a valid option.");
                            }
                        }
                        Continue();
                        break;

                    case "2":
                        bool looping = true;
                        while (looping)
                        {
                            iActivity found = Search();
                            if (found == null) looping = false;

                            string prompt = $"Type \"name\", \"distance\", \"time\" to change, or \"exit\" to return to main menu. ";
                            while (found != null && looping)
                            {
                                string editOption = TrimmedInput(prompt);
                                switch (editOption)
                                {
                                    case "name":
                                        Console.Write("Type in updated activity name: ");
                                        found.name = Console.ReadLine();
                                        looping = false;
                                        break;

                                    case "distance":
                                        found.distance = DistanceCheck("Type in updated distance: ");
                                        looping = false;
                                        break;

                                    case "time":
                                        found.timeMinutes = TimeCheck("Type in updated time: ");
                                        looping = false;
                                        break;

                                    case "exit":
                                        looping = false;
                                        break;

                                    default:
                                        prompt = "Please choose a valid option, or type \"exit\" to return to main menu: ";
                                        break;
                                }
                            }
                            if (found != null) Continue();
                        }
                        
                        break;

                    case "3":
                        iActivity activity = Search();
                        if (activity != null) Continue();
                        break;

                    case "4":
                        bool doOnce = true;
                        foreach (var item in currentUser.activities)
                        {
                            while (doOnce)
                            {
                                Console.WriteLine("Runs: ");
                                doOnce = false;
                            }
                            if (item is RunActivity)
                            {
                                Console.WriteLine($"Title - {item.name}: {item.distance} total miles in {item.timeMinutes} minutes.");
                            }
                        }

                        foreach (var item in currentUser.activities)
                        {
                            while (doOnce)
                            {
                                Console.WriteLine("Bike rides: ");
                                doOnce = false;
                            }
                            if (item is BikeActivity)
                            {
                                Console.WriteLine($"Title - {item.name}: {item.distance} total miles in {item.timeMinutes} minutes.");
                            }
                        }

                        ActivityMessage();
                        Continue();
                        break;

                    case "5":
                        foreach (var item in currentUser.activities)
                        {
                            if (item is RunActivity)
                            {
                                Console.WriteLine($"Title - {item.name}: {item.distance} total miles in {item.timeMinutes} minutes.");
                            }
                        }
                        ActivityMessage();
                        Continue();
                        break;

                    case "6":
                        foreach (var item in currentUser.activities)
                        {
                            if (item is BikeActivity)
                            {
                                Console.WriteLine($"Title - {item.name}: {item.distance} total miles in {item.timeMinutes} minutes.");
                            }
                        }
                        ActivityMessage();
                        Continue();
                        break;

                    case "7":
                        while (true)
                        {
                            iActivity iFound = Search();
                            if (iFound != null)
                            {
                                currentUser.activities.Remove(iFound);
                                Console.WriteLine($"{iFound.name}: deleted. ");
                                Continue();
                                break;
                            }
                            if (iFound == null)
                            {
                                break;
                            }
                        }
                        break;

                    case "8":
                        Logout();
                        programRunning = false;
                        break;

                    default:
                        Console.Write("Please choose a valid option: ");
                        break;
                }
            }
        }

        static string MenuOptions()
        {
            Console.WriteLine();
            Console.WriteLine("1: Add activity.");
            Console.WriteLine("2: Edit activity.");
            Console.WriteLine("3: Search activity by name.");
            Console.WriteLine("4: View all activities.");
            Console.WriteLine("5: View all runs.");
            Console.WriteLine("6: View all bike rides.");
            Console.WriteLine("7: Remove an activity.");
            Console.WriteLine("8: Log out.");
            Console.Write("Please type in your option: ");
            return Console.ReadLine();
        }
        static string TrimmedInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine().Trim().ToLower();
        }
        static iActivity AddPrompts(string type)
        {
            Console.Write("Enter your activity name: ");
            string activityName = Console.ReadLine();

            double distance = DistanceCheck("Enter your distance in miles: ");

            int timeMinutes = TimeCheck("Enter your total time in minutes: ");

            iActivity activity = null;
            if (type.Equals("run"))
            {
                activity = new RunActivity(activityName, distance, timeMinutes);
                Console.WriteLine($"\nRun created: Title - {activityName}: {distance} total miles in {timeMinutes} minutes. Great work!");
            }
            else if (type.Equals("bike"))
            {
                activity = new BikeActivity(activityName, distance, timeMinutes);
                Console.Write($"\nBike ride created: Title - {activityName}: {distance} total miles in {timeMinutes} minutes. Great work!");
            }

            return activity;
        }
        static iActivity Search()
        {
            iActivity iFound = null;
            string prompt = "Search activity name: ";
            while (true)
            {
                Console.Write(prompt);
                string search = Console.ReadLine();
                if (search.Trim().ToLower().Equals("exit")) 
                {
                    Console.WriteLine("Returning to main menu.");
                    break;
                }
                iFound = currentUser.activities.FirstOrDefault(p => p.name == search);
                if (iFound != null)
                {
                    Console.WriteLine($"Title - {iFound.name}: {iFound.distance} total miles in {iFound.timeMinutes} minutes.");
                    break;
                }
                prompt = "Activity not found. Search for another activity or type \"exit\" to go back to main menu: ";
            }
            return iFound;
        }
        static void ActivityMessage()
        {
            if (!currentUser.activities.Any())
            {
                Console.WriteLine("No activities!");
            }
            else
            {
                Console.WriteLine("Great work!");
            }
        }
        static double DistanceCheck(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                bool success = double.TryParse(input, out double distance);
                if (success && distance > 0)
                {
                    return distance;
                }
                else if (success && distance <= 0) { message = ("Please type in a valid distance: "); }
                else { message = ("Please only type in numbers: "); }
            }
        }
        static int TimeCheck(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int timeMinutes) && timeMinutes > 0)
                {
                    return timeMinutes;
                }
                else { message = ("Please type in a valid number: "); }
            }
        }
        static void Continue()
        {
            Console.Write("Press enter to return to main menu. ");
            Console.ReadLine();
        }
    }
}
