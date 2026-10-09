
namespace Bankomat_Internetbank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Store usernames and PIN codes for the five users
            string[] usernames = { "Måns", "Alvin", "Joel", "Oliver", "Adam" };
            string[] pincodes = { "2328", "1229", "5114", "8409", "0910" };

            // Store the account names and balances for each user
            string[][] accountNames =
            {
                new string[] {"Lönekonto", "Sparkonto"},
                new string[] {"Lönekonto"},
                new string[] {"Lönekonto", "Sparkonto", "Resekonto"},
                new string[] {"Lönekonto", "Sparkonto"},
                new string[] {"Lönekonto", "Sparkonto", "Fondkonto", "Framtidskonto"}
            };

            // Store the starting balance for each account
            decimal[][] balances =
            {
                new decimal[] { 12500.67m, 30157.78m },
                new decimal[] { 18750.29m },
                new decimal[] { 15000.06m, 25060.52m, 55780.75m },
                new decimal[] { 22360.58m, 45070.28m },
                new decimal[] { 17501.02m, 30970.56m, 8570.25m, 12460.75m }
            };

            Console.WriteLine("Välkommen till Bankomaten!");

            // Keep the program running until the login process is locked
            // or the program is stopped
            while (true)
            {
                int userIndex = -1;
                int attempts = 0;
                bool loggedIn = false;

                // Allow the user up to three login attempts
                while (attempts < 3 && !loggedIn)
                {
                    Console.Write("Ange ditt namn: ");
                    string username = Console.ReadLine();

                    Console.Write("Ange din PIN-kod: ");
                    string pin = Console.ReadLine();

                    // Check the entered username and PIN against all users
                    for (int i = 0; i < usernames.Length; i++)
                    {
                        if (username == usernames[i] && pin == pincodes[i])
                        {
                            loggedIn = true;
                            userIndex = i;
                        }
                    }

                    // Show a message depending on whether the login was successful
                    if (loggedIn)
                    {
                        Console.WriteLine("Inloggningen Lyckades!");
                    }
                    else
                    {
                        attempts++;
                        Console.WriteLine("Fel användarnamn eller PIN-kod");
                        Console.WriteLine("Försök kvar: " + (3 - attempts));
                    }
                }

                // Stop the program if the user fails to log in three times
                if (!loggedIn)
                {
                    Console.WriteLine("Du har misslyckats för många gånger. Var god starta om programmet");
                    break;
                }
                else
                {
                    int choice = 0;

                    // Display the main menu until the user logs out
                    while (choice != 4)
                    {
                        Console.WriteLine();
                        Console.WriteLine("----Välj ett alternativ nedan----");
                        Console.WriteLine("1. Se dina konton och saldo");
                        Console.WriteLine("2. Överföring mellan konton");
                        Console.WriteLine("3. Ta ut pengar");
                        Console.WriteLine("4. Logga ut");

                        Console.Write("Ange numret på alternativet du vill välja: ");
                        int.TryParse(Console.ReadLine(), out choice);

                        // Show the user's accounts and balances
                        if (choice == 1)
                        {
                            Console.WriteLine("Du valde alternativ 1. Se dina konton och saldo");
                            ShowAccounts(accountNames[userIndex], balances[userIndex]);

                            PressEnterToContinue();
                        }
                        // Transfer money between the user's accounts
                        else if (choice == 2)
                        {
                            Console.WriteLine("Du valde alternativ 2. Överföring mellan konton");
                            TransferBalance(accountNames[userIndex], balances[userIndex]);

                            PressEnterToContinue();
                        }
                        // Withdraw money from one of the user's accounts
                        else if (choice == 3)
                        {
                            Console.WriteLine("Du valde alternativ 3. Ta ut pengar");
                            WithdrawMoney(accountNames[userIndex], balances[userIndex], pincodes[userIndex]);

                            PressEnterToContinue();
                        }
                        // Log out and return to the login screen
                        else if (choice == 4)
                        {
                            Console.WriteLine("Du har loggats ut");
                            break;
                        }
                        // Handle an invalid menu choice
                        else
                        {
                            Console.WriteLine("Ogiltigt val");
                        }
                    }
                }
            }
        }

        // Display all accounts and their current balances
        static void ShowAccounts(string[] accounts, decimal[] balances)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                Console.WriteLine(accounts[i] + ": " + balances[i] + " kr");
            }
        }

        // Wait for the user to press Enter before returning to the main menu
        static void PressEnterToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Klicka 'Enter' för att komma till huvudmenyn");
            Console.ReadLine();
        }

        // Transfer money between two accounts belonging to the logged-in user
        static void TransferBalance(string[] accounts, decimal[] balances)
        {
            Console.WriteLine();
            Console.WriteLine("---- Överföring mellan konton ----");

            // Display all available accounts and their balances
            for (int i = 0; i < accounts.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + accounts[i] + " - " + balances[i] + " kr");
            }

            // Ask which account the money should be transferred from
            Console.Write("Välj kontot du vill överföra pengar från: ");
            int fromAccount = int.Parse(Console.ReadLine()) - 1;

            // Ask which account should receive the money
            Console.Write("Välj kontot du vill överföra pengarna till: ");
            int toAccount = int.Parse(Console.ReadLine()) - 1;

            // Ask how much money should be transferred
            Console.Write("Ange summan du vill flytta: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            // Check that both account numbers are valid
            if (fromAccount < 0 || fromAccount >= accounts.Length ||
                toAccount < 0 || toAccount >= accounts.Length)
            {
                Console.WriteLine("Ogiltigt konto.");
            }
            // Prevent transfers to the same account
            else if (fromAccount == toAccount)
            {
                Console.WriteLine("Du kan inte överföra pengar till samma konto.");
            }
            // Make sure the transfer amount is greater than zero
            else if (amount <= 0)
            {
                Console.WriteLine("Summan måste vara större än 0 kr.");
            }
            // Check that the source account has enough money
            else if (amount > balances[fromAccount])
            {
                Console.WriteLine("Det finns inte tillräckligt med pengar på kontot.");
            }
            else
            {
                // Subtract the amount from the source account
                balances[fromAccount] -= amount;

                // Add the amount to the destination account
                balances[toAccount] += amount;

                // Display the updated balances of both accounts
                Console.WriteLine();
                Console.Write("Överföringen lyckades! ");

                Console.WriteLine(accounts[fromAccount] + ": " + balances[fromAccount] + " kr");
                Console.WriteLine(accounts[toAccount] + ": " + balances[toAccount] + " kr");
            }
        }

        // Withdraw money from an account after checking the balance and PIN code
        static void WithdrawMoney(string[] accounts, decimal[] balances, string correctPin)
        {
            Console.WriteLine();
            Console.WriteLine("---- Ta ut pengar ----");

            // Display all available accounts and their balances
            for (int i = 0; i < accounts.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + accounts[i] + " - " + balances[i] + " kr");
            }

            // Ask which account the user wants to withdraw money from
            Console.Write("Välj kontot du vill ta ut pengar från: ");
            int account = int.Parse(Console.ReadLine()) - 1;

            // Check that the selected account number is valid
            if (account < 0 || account >= accounts.Length)
            {
                Console.WriteLine("Ogiltigt konto");
                return;
            }

            // Ask how much money the user wants to withdraw
            Console.Write("Ange summan du vill ta ut: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            // Check that the account has enough money for the withdrawal
            if (amount > balances[account])
            {
                Console.WriteLine("Det finns inte tillräckligt med pengar på kontot.");
                return;
            }

            // Ask the user to confirm the withdrawal with their PIN code
            Console.Write("Ange din PIN-kod för att bekräfta uttaget: ");
            string pin = Console.ReadLine();

            // Verify the PIN code before completing the withdrawal
            if (pin == correctPin)
            {
                // Subtract the withdrawn amount from the account balance
                balances[account] -= amount;

                Console.WriteLine("Uttaget lyckades!");
                Console.WriteLine("Nytt saldo på " + accounts[account] + ": " + balances[account] + " kr");
            }
            else
            {
                // Cancel the withdrawal if the PIN code is incorrect
                Console.WriteLine("Fel PIN-kod. Uttag avbrutet.");
            }
        }
    }
}
