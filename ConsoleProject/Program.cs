using ConsoleProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleProject
{
    public class Program
    {
        static List<User> users = new List<User>();
        const decimal MaxCreditAmount = 1000m;
        const int MaxCreditMonths = 12;
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Choose an option: 1.Register\t 2.Login\t 3.Exit");
                var choice = Console.ReadLine().Trim();
                switch (choice)
                {
                    case "1":
                        string name;
                        string surname;
                        int age;
                        string mail;
                        string password;
                        decimal balance;
                        bool isEmployed;

                        while (true)
                        {
                            Console.WriteLine("Enter Name: ");
                            name = Console.ReadLine();
                            if (name.Length >= 2) break;
                            Console.WriteLine("Name must contain at least 2 characters.");
                        }
                        while (true)
                        {
                            Console.WriteLine("Enter Surname: ");
                            surname = Console.ReadLine();
                            if (surname.Length >= 2) break;
                            Console.WriteLine("Surname must contain at least 2 characters.");
                        }
                        while (true)
                        {
                            Console.WriteLine("Enter Age: ");
                            if (int.TryParse(Console.ReadLine(), out age))
                            {
                                if (age < 18)
                                {
                                    Console.WriteLine("You must be at least 18 years old to register.");
                                    continue;
                                }
                                break;
                            }
                            Console.WriteLine("Invalid age. Please enter a numeric value.");
                        }

                        while (true)
                        {
                            Console.WriteLine("Enter Mail: ");
                            mail = Console.ReadLine();
                            if (mail.Contains("@")) break;
                            Console.WriteLine("Mail must contain '@'.");
                        }
                        while (true)
                        {
                            Console.WriteLine("Enter password: ");
                            var password1 = Console.ReadLine();
                            Console.WriteLine("Re-enter password: ");
                            var password2 = Console.ReadLine();
                            if (password1 == password2)
                            {
                                password = password1;
                                break;
                            }
                            Console.WriteLine("Passwords do not match. Please try again.");
                        }
                        while (true)
                        {
                            Console.WriteLine("Enter Balance: ");
                            if (decimal.TryParse(Console.ReadLine(), out balance))
                            {
                                break;
                            }
                            Console.WriteLine("Invalid balance. Please enter a numeric value.");
                        }
                        while (true)
                        {
                            Console.WriteLine("Are you employed? (yes/no): ");
                            var employedInput = Console.ReadLine().ToLower();
                            if (employedInput == "yes")
                            {
                                isEmployed = true;
                                break;
                            }
                            else if (employedInput == "no")
                            {
                                isEmployed = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                            }
                        }
                        var user = new User
                        {
                            Name = name,
                            Surname = surname,
                            Password = password,
                            Balance = balance,
                            Mail = mail,
                            IsEmployed = isEmployed
                        };
                        users.Add(user);
                        Console.WriteLine("Registration successful");
                        break;

                    case "2":
                        string logmail;
                        while (true)
                        {
                            Console.WriteLine("Enter Mail: ");
                            logmail = Console.ReadLine();
                            if (logmail.Contains("@")) break;
                            Console.WriteLine("Mail must contain '@'.");
                        }
                        var exUser = users.FirstOrDefault(u => u.Mail == logmail);
                        if (exUser == null)
                        {
                            Console.WriteLine("Invalid email. If you don't have an account, please register.");
                            return;
                        }
                        else
                            Console.WriteLine("Enter password: ");
                        var logpassword = Console.ReadLine();

                        var usr = users.SingleOrDefault(u => u.Mail == logmail && u.Password == logpassword);
                        if (usr != null)
                        {
                            usr.FailedLoginAttempts = 0;
                            Console.WriteLine($"Logged in as {usr.Name} {usr.Surname}");
                            var a = true;
                            while (a)
                            {
                                Console.WriteLine("\nChoose an option: 1.Deposit\t 2.Withdraw\t 3.Credit\t 4.Show Balance\t 5.Logout");
                                var logchoice = Console.ReadLine().Trim();
                                switch (logchoice)
                                {
                                    case "1":
                                        Console.WriteLine("Enter amount to deposit: ");
                                        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                                        {
                                            usr.Balance += amount;
                                            Console.WriteLine($"Deposited ${amount}. New balance: ${usr.Balance}.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid amount. Please enter a numeric value.");
                                        }
                                        break;
                                    case "2":
                                        while (true)
                                        {
                                            Console.WriteLine("Enter amount to withdraw: ");
                                            if (decimal.TryParse(Console.ReadLine(), out decimal amn))
                                            {
                                                if (amn <= usr.Balance)
                                                {
                                                    usr.Balance -= amn;
                                                    Console.WriteLine($"Withdrew ${amn}. New balance: ${usr.Balance}.");
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Insufficient funds. Please enter a smaller amount.");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid amount. Please enter a numeric value.");
                                            }
                                        }
                                        break;
                                    case "3":
                                        if (!usr.IsEmployed)
                                        {
                                            Console.WriteLine("Credit is not available for unemployed users.");
                                            break;
                                        }
                                        while (true)
                                        {
                                            Console.WriteLine($"Enter credit amount (Max ${MaxCreditAmount - usr.CreditBalance}): ");
                                            if (decimal.TryParse(Console.ReadLine(), out decimal ant) && ant <= (MaxCreditAmount - usr.CreditBalance))
                                            {
                                                Console.WriteLine($"Enter credit period in months (Max {MaxCreditMonths}): ");
                                                if (int.TryParse(Console.ReadLine(), out int months) && months <= MaxCreditMonths)
                                                {
                                                    decimal monthlyPayment = ant / months;
                                                    usr.Balance += ant;
                                                    usr.CreditBalance += ant;

                                                    Console.WriteLine($"Credit approved. Monthly payment: ${monthlyPayment} for {months} months.");
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Max credit period exceeded. Max: {MaxCreditMonths} months.");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Credit limit exceeded. Max available: ${MaxCreditAmount - usr.CreditBalance}.");
                                            }
                                        }
                                        break;
                                    case "4":
                                        Console.WriteLine($"Current balance: ${usr.Balance}.");
                                        break;
                                    case "5":
                                        a = false;
                                        break;
                                    default:
                                        Console.WriteLine("Invalid option. Please try again.");
                                        break;
                                }
                            }
                        }
                        else
                        {
                            var existingUser = users.FirstOrDefault(u => u.Mail == logmail);
                            if (existingUser != null)
                            {
                                Console.WriteLine("Invalid password.");
                                existingUser.FailedLoginAttempts++;
                            }
                            if (existingUser != null && existingUser.FailedLoginAttempts >= 3)
                            {
                                Console.WriteLine("Too many failed login attempts. Returning to main menu.");
                                return;
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                Console.WriteLine("Enter password again: ");
                                logpassword = Console.ReadLine();
                                usr = users.SingleOrDefault(u => u.Mail == logmail && u.Password == logpassword);
                                if (usr != null)
                                {
                                    usr.FailedLoginAttempts = 0;
                                    Console.WriteLine($"Logged in as {usr.Name} {usr.Surname}");
                                    return;
                                }
                            }
                            Console.WriteLine("Too many failed login attempts. Returning to main menu.");
                        }

                        Console.ReadLine();
                        break;

                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
