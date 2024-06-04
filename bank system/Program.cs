using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static Dictionary<string, Dictionary<string, string>> users = new Dictionary<string, Dictionary<string, string>>();
    static Dictionary<string, decimal> balances = new Dictionary<string, decimal>();
    const int MaxPasswordAttempts = 3;

    static void Main(string[] args)
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Register();
                    break;
                case "2":
                    if (!Login())
                    {
                        Console.WriteLine("You have reached the maximum number of password attempts. Closing the program...");
                        isRunning = false;
                    }
                    break;
                case "3":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void Register()
    {
        Console.WriteLine("Please enter the following details:");

        string name, surname;
        do
        {
            Console.Write("Name: ");
            name = Console.ReadLine();
            if (name.Length <= 2)
            {
                Console.WriteLine("Name must be longer than 2 characters.");
            }
        } while (name.Length <= 2);

        do
        {
            Console.Write("Surname: ");
            surname = Console.ReadLine();
            if (surname.Length <= 2)
            {
                Console.WriteLine("Surname must be longer than 2 characters.");
            }
        } while (surname.Length <= 2);

        DateTime birthDate;
        do
        {
            Console.Write("Birth Date (DD-MM-YYYY): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out birthDate))
            {
                Console.WriteLine("Invalid date format. Please enter the date in DD-MM-YYYY format.");
            }
            else if (DateTime.Today.Year - birthDate.Year < 18)
            {
                Console.WriteLine("You must be 18 years or older to register.");
            }
        } while (DateTime.Today.Year - birthDate.Year < 18);

        string email;
        do
        {
            Console.Write("Email: ");
            email = Console.ReadLine();
            if (!IsValidEmail(email))
            {
                Console.WriteLine("Invalid email format. Please enter a valid email address.");
            }
        } while (!IsValidEmail(email));

        Console.Write("Password: ");
        string password = Console.ReadLine();

        Console.Write("Work: ");
        string work = Console.ReadLine();

        decimal initialBalance;
        do
        {
            Console.Write("Initial Balance: ");
            if (!decimal.TryParse(Console.ReadLine(), out initialBalance))
            {
                Console.WriteLine("Invalid amount. Please enter a valid number.");
            }
            else if (initialBalance < 0)
            {
                Console.WriteLine("Initial balance cannot be negative.");
            }
        } while (initialBalance < 0);

        string username = email;

        if (users.Any(u => u.Value["Email"] == email))
        {
            Console.WriteLine("Email already exists. Please choose another.");
            return;
        }

        Dictionary<string, string> userInfo = new Dictionary<string, string>()
        {
            { "Name", name },
            { "Surname", surname },
            { "BirthDate", birthDate.ToString("dd-MM-yyyy") },
            { "Email", email },
            { "Password", password },
            { "Work", work }
        };

        users.Add(username, userInfo);
        balances.Add(username, initialBalance);
        Console.WriteLine("Registration successful!");

        Console.WriteLine("Returning to login screen...");
    }

    static bool Login()
    {
        int passwordAttempts = 0;
        while (passwordAttempts < MaxPasswordAttempts)
        {
            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            KeyValuePair<string, Dictionary<string, string>> user = users.FirstOrDefault(u => u.Value["Email"] == email && u.Value["Password"] == password);

            if (!user.Equals(default(KeyValuePair<string, Dictionary<string, string>>)))
            {
                Console.WriteLine("Login successful!");
                Console.WriteLine($"Welcome, {user.Value["Name"]} {user.Value["Surname"]}!");
                Console.WriteLine($"Work: {user.Value["Work"]}");

                ShowOperations(user.Key);

                return true;
            }
            else
            {
                passwordAttempts++;
                Console.WriteLine("Invalid email or password.");
                Console.WriteLine($"Remaining attempts: {MaxPasswordAttempts - passwordAttempts}");
            }
        }

        return false;
    }

    static void ShowOperations(string username)
    {
        bool loggedIn = true;
        while (loggedIn)
        {
            Console.WriteLine();
            Console.WriteLine("Operations:");
            Console.WriteLine("1. Add balance");
            Console.WriteLine("2. Take cash out");
            Console.WriteLine("3. Borrow money");
            Console.WriteLine("4. Show balance");
            Console.WriteLine("5. Log out");
            Console.Write("Choose an operation: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBalance(username);
                    break;
                case "2":
                    TakeCashOut(username);
                    break;
                case "3":
                    BorrowMoney(username);
                    break;
                case "4":
                    ShowBalance(username);
                    break;
                case "5":
                    Console.WriteLine("Logging out...");
                    loggedIn = false;
                    break;
                default:
                    Console.WriteLine("Invalid operation. Please choose again.");
                    break;
            }
        }
    }

    static void AddBalance(string username)
    {
        Console.WriteLine("Add balance operation.");

        Console.Write("Enter the amount of money to add: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine("Invalid amount. Please enter a valid number.");
            return;
        }

        if (amount <= 0)
        {
            Console.WriteLine("Amount must be greater than zero.");
            return;
        }

        balances[username] += amount;

        Console.WriteLine($"Added {amount} to your balance. Current balance: {balances[username]}");
    }

    static void TakeCashOut(string username)
    {
        Console.WriteLine("Take cash out operation.");

        Console.Write("Enter the amount of money to withdraw: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine("Invalid amount. Please enter a valid number.");
            return;
        }

        if (amount <= 0)
        {
            Console.WriteLine("Amount must be greater than zero.");
            return;
        }

        if (balances[username] < amount)
        {
            Console.WriteLine("Insufficient balance to withdraw.");
            return;
        }

        if (balances[username] - amount < 0)
        {
            Console.WriteLine("You cannot withdraw more than your current balance.");
            return;
        }

        balances[username] -= amount;

        Console.WriteLine($"Withdrawal of {amount} successfully completed.");
    }

    static void BorrowMoney(string username)
    {
        Console.WriteLine("Borrow money operation.");

        Console.Write("Enter the amount of money to borrow: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            Console.WriteLine("Invalid amount. Please enter a valid number.");
            return;
        }

        if (amount <= 0)
        {
            Console.WriteLine("Amount must be greater than zero.");
            return;
        }

        Console.Write("Enter the number of months for repayment: ");
        if (!int.TryParse(Console.ReadLine(), out int months))
        {
            Console.WriteLine("Invalid number of months. Please enter a valid number.");
            return;
        }

        if (months <= 0)
        {
            Console.WriteLine("Number of months must be greater than zero.");
            return;
        }

        decimal monthlyPayment = amount / months;
        balances[username] += amount;

        Console.WriteLine($"Borrowed {amount} successfully. Monthly payment: {monthlyPayment} for {months} months.");
    }

    static void ShowBalance(string username)
    {
        Console.WriteLine($"Your balance: {balances[username]}");
    }

    static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

