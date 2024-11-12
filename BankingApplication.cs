using System;
using System.Collections.Generic;

class User
{
    public string Username { get; set; }
    public string Password { get; set; }

    public User(string username, string password)
    {
        this.Username = username;
        this.Password = password;
    }
}

class Account
{
    public string AccountNumber { get; set; }
    public string AccountHolderName { get; set; }
    public string AccountType { get; set; }
    public double Balance { get; set; }
    public List<Transaction> Transactions { get; set; }

    public Account(string holderName, string accountType, double initialDeposit)
    {
        this.AccountNumber = GenerateAccountNumber();
        this.AccountHolderName = holderName;
        this.AccountType = accountType;
        this.Balance = initialDeposit;
        this.Transactions = new List<Transaction>();
    }

    private string GenerateAccountNumber()
    {
        return "ACCT" + new Random().Next(10000, 99999).ToString();
    }

    public void Deposit(double amount)
    {
        this.Balance += amount;
        Transactions.Add(new Transaction("Deposit", amount));
        Console.WriteLine("Deposit successful. New balance: " + this.Balance);
    }

    public void Withdraw(double amount)
    {
        if (amount > this.Balance)
        {
            Console.WriteLine("Insufficient funds.");
        }
        else
        {
            this.Balance -= amount;
            Transactions.Add(new Transaction("Withdrawal", amount));
            Console.WriteLine("Withdrawal successful. New balance: " + this.Balance);
        }
    }

    public void GenerateStatement()
    {
        Console.WriteLine("Transaction history for account: " + this.AccountNumber);
        foreach (var t in Transactions)
        {
            Console.WriteLine($"{t.Date.ToShortDateString()} - {t.Type} - {t.Amount}");
        }
    }

    public void AddMonthlyInterest(double interestRate)
    {
        if (this.AccountType == "Savings")
        {
            double interest = this.Balance * interestRate;
            this.Balance += interest;
            Transactions.Add(new Transaction("Interest", interest));
            Console.WriteLine($"Interest added: {interest}. New balance: {this.Balance}");
        }
        else
        {
            Console.WriteLine("Interest calculation is only applicable to Savings accounts.");
        }
    }

    public void CheckBalance()
    {
        Console.WriteLine($"Your current balance is: {this.Balance}");
    }
}

class Transaction
{
    public string TransactionID { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; }
    public double Amount { get; set; }

    public Transaction(string type, double amount)
    {
        this.TransactionID = GenerateTransactionID();
        this.Date = DateTime.Now;
        this.Type = type;
        this.Amount = amount;
    }

    private string GenerateTransactionID()
    {
        return "TXN" + new Random().Next(10000, 99999).ToString();
    }
}

class BankingApplication
{
    private List<User> Users = new List<User>();
    private Dictionary<string, Account> Accounts = new Dictionary<string, Account>();
    private User LoggedInUser;

    public void Register(string username, string password)
    {
        Users.Add(new User(username, password));
        Console.WriteLine("Registration successful.");
    }

    public bool Login(string username, string password)
    {
        foreach (var user in Users)
        {
            if (user.Username == username && user.Password == password)
            {
                LoggedInUser = user;
                Console.WriteLine("Login successful.");
                return true;
            }
        }
        Console.WriteLine("Invalid credentials.");
        return false;
    }

    public void Logout()
    {
        LoggedInUser = null;
        Console.WriteLine("You have been logged out.");
    }

    public bool IsUserLoggedIn()
    {
        if (LoggedInUser == null)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            return false;
        }
        return true;
    }

    public void OpenAccount(string holderName, string accountType, double initialDeposit)
    {
        if (IsUserLoggedIn())
        {
            Account newAccount = new Account(holderName, accountType, initialDeposit);
            Accounts[newAccount.AccountNumber] = newAccount;
            Console.WriteLine($"Account created. Account number: {newAccount.AccountNumber}");
        }
    }

    public Account GetAccount(string accountNumber)
    {
        if (Accounts.ContainsKey(accountNumber))
        {
            return Accounts[accountNumber];
        }
        else
        {
            Console.WriteLine("Account not found.");
            return null;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        BankingApplication app = new BankingApplication();
        bool running = true;

        while (running)
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Open Account");
            Console.WriteLine("4. Deposit");
            Console.WriteLine("5. Withdraw");
            Console.WriteLine("6. Check Balance");
            Console.WriteLine("7. Generate Statement");
            Console.WriteLine("8. Add Monthly Interest");
            Console.WriteLine("9. Logout");
            Console.WriteLine("10. Exit");
            Console.Write("Select an option: ");
            int choice = -1;
            try
            {
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                choice = -1; // Assigning an invalid value to repeat the loop
            }

            switch (choice)
            {
                case 1:
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    app.Register(username, password);
                    break;
                case 2:
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    password = Console.ReadLine();
                    app.Login(username, password);
                    break;
                case 3:
                    if (app.IsUserLoggedIn())
                    {
                        Console.Write("Enter account holder name: ");
                        string holderName = Console.ReadLine();
                        Console.Write("Enter account type (Savings/Checking): ");
                        string accountType = Console.ReadLine();
                        Console.Write("Enter initial deposit: ");
                        double deposit = Convert.ToDouble(Console.ReadLine());
                        app.OpenAccount(holderName, accountType, deposit);
                    }
                    break;
                case 4:
                    if (app.IsUserLoggedIn())
                    {
                        Console.Write("Enter account number: ");
                        string acctNumber = Console.ReadLine();
                        Account account = app.GetAccount(acctNumber);
                        if (account != null)
                        {
                            Console.Write("Enter deposit amount: ");
                            double depositAmount = Convert.ToDouble(Console.ReadLine());
                            account.Deposit(depositAmount);
                        }
                    }
                    break;
                case 5:
                    if (app.IsUserLoggedIn())
                    {
                        Console.Write("Enter account number: ");
                        string acctNumber = Console.ReadLine();
                        Account account = app.GetAccount(acctNumber);
                        if (account != null)
                        {
                            Console.Write("Enter withdrawal amount: ");
                            double withdrawal = Convert.ToDouble(Console.ReadLine());
                            account.Withdraw(withdrawal);
                        }
                    }
                    break;
                case 6:
                    if (app.IsUserLoggedIn())
                    {
                        Console.Write("Enter account number: ");
                        string acctNumber = Console.ReadLine();
                        Account account = app.GetAccount(acctNumber);
                        if (account != null)
                        {
                            account.CheckBalance();
                        }
                    }
                    break;
                case 7:
                    if (app.IsUserLoggedIn())
                    {
                        Console.Write("Enter account number: ");
                        string acctNumber = Console.ReadLine();
                        Account account = app.GetAccount(acctNumber);
                        if (account != null)
                        {
                            account.GenerateStatement();
                        }
                    }
                    break;
                case 8:
                    if (app.IsUserLoggedIn())
                    {
                        Console.Write("Enter account number: ");
                        string acctNumber = Console.ReadLine();
                        Account account = app.GetAccount(acctNumber);
                        if (account != null && account.AccountType == "Savings")
                        {
                            Console.Write("Enter interest rate: ");
                            double interestRate = Convert.ToDouble(Console.ReadLine());
                            account.AddMonthlyInterest(interestRate);
                        }
                    }
                    break;
                case 9:
                    app.Logout();
                    break;
                case 10:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}
