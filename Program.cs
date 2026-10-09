
namespace Bankomat_Internetbank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] usernames = { "Måns", "Alvin", "Joel", "Oliver", "Adam" };

            string[] pincodes = { "2328", "1229", "5114", "8409", "0910" };

            string[][] accountNames =
            {
                new string[] {"Lönekonto", "Sparkonto"},
                new string[] {"Lönekonto"},
                new string[] {"Lönekonto", "Sparkonto", "Resekonto"},
                new string[] {"Lönekonto", "Sparkonto"},
                new string[] {"Lönekonto", "Sparkonto", "Fondkonto", "Framtidskonto"}
            };

            decimal[][] balances =
            {
                new decimal[] { 12500.67m, 30157.78m },
                new decimal[] { 18750.29m },
                new decimal[] { 15000.06m, 25060.52m, 55780.75m },
                new decimal[] { 22360.58m, 45070.28m },
                new decimal[] { 17501.02m, 30970.56m, 8570.25m, 12460.75m }
            };

            Console.WriteLine("Välkommen till Bankomaten!");

            while (true)
            {
                int userIndex = -1;
                int attempts = 0;
                bool loggedIn = false;

                while (attempts < 3 && !loggedIn)
                {
                    Console.Write("Ange ditt namn: ");
                    string username = Console.ReadLine();

                    Console.Write("Ange din PIN-kod: ");
                    string pin = Console.ReadLine();

                    for (int i = 0; i < usernames.Length; i++)
                    {
                        if (username == usernames[i] && pin == pincodes[i])
                        {
                            loggedIn = true;
                            userIndex = i;
                        }
                    }

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

                if (!loggedIn)
                {
                    Console.WriteLine("Du har misslyckats för många gånger. Var god starta om programmet");
                    break;
                }
                else
                {
                    int choice = 0;

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

                        if (choice == 1)
                        {
                            Console.WriteLine("Du valde alternativ 1. Se dina konton och saldo");
                            ShowAccounts(accountNames[userIndex], balances[userIndex]);

                            PressEnterToContinue();
                        }
                        else if (choice == 2)
                        {
                            Console.WriteLine("Du valde alternativ 2. Överföring mellan konton");
                            TransferBalance(accountNames[userIndex], balances[userIndex]);

                            PressEnterToContinue();
                        }
                        else if (choice == 3)
                        {
                            Console.WriteLine("Du valde alternativ 3. Ta ut pengar");
                            WithdrawMoney(accountNames[userIndex], balances[userIndex], pincodes[userIndex]);

                            PressEnterToContinue();
                        }
                        else if (choice == 4)
                        {
                            Console.WriteLine("Du har loggats ut");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt val");
                        }
                    }
                }
            }
        }

        static void ShowAccounts(string[] accounts, decimal[] balances)
        {
            // Display all accounts and their balances
            for (int i = 0; i < accounts.Length; i++)
            {
                Console.WriteLine(accounts[i] + ": " + balances[i] + " kr");
            }
        }

        static void PressEnterToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Klicka 'Enter' för att komma till huvudmenyn");
            Console.ReadLine();
        }

        static void TransferBalance(string[] accounts, decimal[] balances)
        {
            Console.WriteLine();
            Console.WriteLine("---- Överföring mellan konton ----");

            for (int i = 0; i < accounts.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + accounts[i] + " - " + balances[i] + " kr");
            }

            Console.Write("Välj kontot du vill överföra pengar från: ");
            int fromAccount = int.Parse(Console.ReadLine()) - 1;

            Console.Write("Välj kontot du vill överföra pengarna till: ");
            int toAccount = int.Parse(Console.ReadLine()) - 1;

            Console.Write("Ange summan du vill flytta: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            if (fromAccount < 0 || fromAccount >= accounts.Length ||
                toAccount < 0 || toAccount >= accounts.Length)
            {
                Console.WriteLine("Ogiltigt konto.");
            }
            else if (fromAccount == toAccount)
            {
                Console.WriteLine("Du kan inte överföra pengar till samma konto.");
            }
            else if (amount <= 0)
            {
                Console.WriteLine("Summan måste vara större än 0 kr.");
            }
            else if (amount > balances[fromAccount])
            {
                Console.WriteLine("Det finns inte tillräckligt med pengar på kontot.");
            }
            else
            {
                balances[fromAccount] -= amount;
                balances[toAccount] += amount;

                Console.WriteLine();
                Console.Write("Överföringen lyckades! ");

                Console.WriteLine(accounts[fromAccount] + ": " + balances[fromAccount] + " kr");
                Console.WriteLine(accounts[toAccount] + ": " + balances[toAccount] + " kr");
            }
        }

        static void WithdrawMoney(string[] accounts, decimal[] balances, string correctPin)
        {
            Console.WriteLine();
            Console.WriteLine("---- Ta ut pengar ----");

            for (int i = 0; i < accounts.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + accounts[i] + " - " + balances[i] + " kr");
            }

            Console.Write("Välj kontot du vill ta ut pengar från: ");
            int account = int.Parse(Console.ReadLine()) - 1;

            if (account < 0 || account >= accounts.Length)
            {
                Console.WriteLine("Ogiltigt konto");
                return;
            }

            Console.Write("Ange summan du vill ta ut: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            if (amount > balances[account])
            {
                Console.WriteLine("Det finns inte tillräckligt med pengar på kontot.");
                return;
            }

            Console.Write("Ange din PIN-kod för att bekräfta uttaget: ");
            string pin = Console.ReadLine();

            if (pin == correctPin)
            {
                balances[account] -= amount;

                Console.WriteLine("Uttaget lyckades!");
                Console.WriteLine("Nytt saldo på " + accounts[account] + ": " + balances[account] + " kr");
            }
            else
            {
                Console.WriteLine("Fel PIN-kod. Uttag avbrutet.");
            }
        }
    }
}
