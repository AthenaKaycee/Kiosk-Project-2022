using System;
using System.ComponentModel.DataAnnotations;
//using System.Diagnostics;

//ProcessStartInfo startInfo = new ProcessStartInfo();
//startInfo.FileName = "Kiosk Log";
//startInfo.Arguments = "Transation number Transaction date Transaction time Payment cash amount Payment card vendor Payment card amount Change given";
//Process.Start(startInfo);
//Kiosk Project: Matt Harris && Kennedy Bowles
namespace KioskProject
{
    class Program
    {
        public struct CashNcoinTill        // keep up with the various tender that is in the kiosk
        {

            public int Hundreds;    // list all common currencies and change as well as less common $2 bill.
            public int Fifties;
            public int Twenties;
            public int Tens;
            public int Fives;
            public int Twos;
            public int Ones;
            public int Quarters;
            public int Dimes;
            public int Nickels;
            public int Pennies;


            public decimal HundredsValue;    // declaring in the struct decimal data type for money values
            public decimal FiftiesValue;
            public decimal TwentiesValue;
            public decimal TensValue;
            public decimal FivesValue;
            public decimal TwosValue;
            public decimal OnesValue;
            public decimal QuartersValue;
            public decimal DimesValue;
            public decimal NickelsValue;
            public decimal PenniesValue;
        }

        public static CashNcoinTill Bank = new CashNcoinTill();

        static void Main(string[] args)
        {
            Bank.Hundreds = 1;                      //Keep up with int amount of each currency in till. 
            Bank.Fifties = 1;
            Bank.Twenties = 10;
            Bank.Tens = 30;
            Bank.Fives = 10;
            Bank.Twos = 2;
            Bank.Ones = 15;
            Bank.Quarters = 20;
            Bank.Dimes = 20;
            Bank.Nickels = 20;
            Bank.Pennies = 50;

            Bank.HundredsValue = 100.00m;
            Bank.FiftiesValue = 50.00m;
            Bank.TwentiesValue = 20.00m;
            Bank.TensValue = 10.00m;
            Bank.FivesValue = 5.00m;
            Bank.TwosValue = 2.00m;
            Bank.OnesValue = 1.00m;
            Bank.QuartersValue = 0.25m;
            Bank.DimesValue = 0.10m;
            Bank.NickelsValue = 0.05m;
            Bank.PenniesValue = 0.01m;
            //Bank Contents is the total decimal amount in the kiosk.
            decimal BankContents = (Bank.Hundreds * Bank.HundredsValue) + (Bank.Fifties * Bank.FiftiesValue) + (Bank.Twenties * Bank.TwentiesValue) + (Bank.Tens * Bank.TensValue) + (Bank.Fives * Bank.FivesValue) + (Bank.Twos * Bank.TwosValue) + (Bank.Ones * Bank.OnesValue) + (Bank.Quarters * Bank.QuartersValue) + (Bank.Dimes * Bank.DimesValue) + (Bank.Nickels * Bank.NickelsValue) + (Bank.Pennies * Bank.PenniesValue);

            string UserCardNumber;
            bool validCard;
            char cashBack;
            decimal cashRequested = 0.00m;
            decimal cardPayamt;
            decimal balance = InputItemCost();                     //f(x) Get user's total item(s) cost / return balance
            Console.WriteLine();
            Console.WriteLine("Enter '1' to pay with cash or press any other key.");

            if (Console.ReadKey().Key == ConsoleKey.D1)
            {
                Console.WriteLine();
                decimal changeDue = InitializePayment(balance, BankContents);       //f(x) Get user to insert payment and add to the till and decrease balance
                ChangeBreaker(changeDue, cashRequested);          // display change due to the user.
            }
            Console.WriteLine();
            Console.WriteLine("Enter '2' if you would like to pay with card.");

            if (Console.ReadKey().Key == ConsoleKey.D2)
            {
                Console.WriteLine();
                Console.WriteLine("Please enter your 16 digit debit/credit card number.");
                UserCardNumber = Console.ReadLine();

                validCard = ValidateCard(UserCardNumber);

                if (validCard)
                {
                    static string[] MoneyRequest(string UserCardNumber, decimal balance)
                    {
                        Random rnd = new Random();//50% CHANCE TRANSACTION PASSES OR FAILS

                        bool pass = rnd.Next(100) < 50;
                        //50% CHANCE THAT A FAILED TRANSACTION IS DECLINED

                        bool declined = rnd.Next(100) < 50;

                        if (pass)
                        {
                            Console.WriteLine("Card Payment Accepted.");
                            return new string[] { UserCardNumber, balance.ToString() };

                        }

                        else
                        {

                            if (!declined)
                            {
                                Console.WriteLine("Card number: {0} Partial payment of ${1}", UserCardNumber, balance);
                                return new string[] { UserCardNumber, (balance / rnd.Next(2, 6)).ToString() };
                            }


                            else
                            {
                                Console.WriteLine("Card was declined.");
                                return new string[] { UserCardNumber, "declined" };

                            }
                        }
                    }


                    Console.WriteLine("Would you like cash back? Enter 'y' for yes or 'n' for no.");
                    cashBack = Convert.ToChar(Console.ReadLine().ToUpper());

                    //if (Console.ReadKey().Key == ConsoleKey.Y)
                    if (cashBack == 'Y')

                    {
                        Console.WriteLine("Please enter the amount of cash back: $20, $40, $60, $80 and $100.");

                        cashRequested = Convert.ToDecimal(Console.ReadLine());


                        if ((cashRequested == 20.00m) || (cashRequested == 40.00m) || (cashRequested == 60.00m) || (cashRequested == 80.00m) || (cashRequested == 100.00m))
                        {

                            balance += cashRequested;
                            Console.WriteLine("Your updated balance with cash back: {0}", balance);
                        }

                        else
                        {
                            Console.WriteLine("This is not a valid cash back amount. Enter an increment of $20 ($20 - $100)");
                            cashRequested = Convert.ToDecimal(Console.ReadLine());
                            balance += cashRequested;
                            Console.WriteLine("Your updated balance with cash back: {0}", balance);
                        }



                    }
                    MoneyRequest(UserCardNumber, balance);
                    Console.WriteLine("How much would you like to pay with your card?");
                    cardPayamt = Convert.ToDecimal(Console.ReadLine());
                    balance = balance - cardPayamt;

                    Console.WriteLine("Your remaining balance after card payment: {0}", balance);
                    Console.WriteLine();
                    Console.WriteLine("Now you will be prompted to pay the remaining balance with cash.");
                    decimal changeDue = InitializePayment(balance, BankContents);
                    ChangeBreaker(changeDue, cashRequested);
                }
            }



        }   //END MAIN

        static decimal InputItemCost()
        {
            decimal UserItemCost;
            decimal TotalBalance = 0.00m;

            bool KeepGoing = true;

            do
            {
                Console.WriteLine(DateTime.Now);
                Console.WriteLine();
                Console.WriteLine("Please enter the cost of the item. ");
                Console.WriteLine();
                UserItemCost = Convert.ToDecimal(Console.ReadLine());

                // possible add an itemized list that keeps up with each item purchased (Item 1  $5.00, Item 2 ...)


                while (UserItemCost <= 0)   //input validation for price
                {
                    Console.WriteLine("You entered an invalid price. Please enter a cost that is more than $0.00 ");
                    Console.WriteLine();
                    UserItemCost = Convert.ToDecimal(Console.ReadLine());
                }

                TotalBalance += UserItemCost;     //running total of costs

                Console.WriteLine("You entered an item cost of ${0} with a total cost of ${1}.", UserItemCost, TotalBalance);
                Console.WriteLine();
                Console.WriteLine("Do you have another item's cost to enter? Enter 'y' for yes or 'n' for no. ");
                Console.WriteLine();
                KeepGoing = Console.ReadKey(true).KeyChar.ToString().ToLower() == "y";
                Console.WriteLine();


            } while (KeepGoing);

            Console.WriteLine();
            Console.WriteLine("Total price is:\t${0}", TotalBalance);

            return TotalBalance;
        }

        static bool ValidateCard(string UserCardNumber)
        {
            bool valid = false;



            char[] UserCardNumberChars = new char[16];                          //create a char array with same amount of chars as UserCardNumber

            for (int i = 0; i < UserCardNumberChars.Length; i++)            //copy each char from string to a char array 
            { UserCardNumberChars[i] = UserCardNumber[i]; }


            if (UserCardNumberChars[0] == '3')                          //validate card vendor by testing the 1st number
            { Console.WriteLine("American Express."); }
            else if (UserCardNumberChars[0] == '4')
            { Console.WriteLine("Visa."); }
            else if (UserCardNumberChars[0] == '5')
            { Console.WriteLine("MasterCard."); }
            else if (UserCardNumberChars[0] == '6')
            { Console.WriteLine("Discover Card."); }
            else { Console.WriteLine("Invalid card vendor. We accept American Express, Visa, Mastercard, and Discover Card."); }





            //used youtube video for understanding and made it our own (in-progress)
            ///*https://www.youtube.com/watch?v=g-5fXthdZ9U*/


            int[] ints = new int[16];
            for (int i = 0; i < UserCardNumberChars.Length; i++)
            {
                //Convert.ToInt32(Convert.ToString(UserCardNumberChars[i]));
                ints[i] = Convert.ToInt32(Convert.ToString(UserCardNumberChars[i]));

            }

            for (int i = ints.Length - 2; i >= 0; i = i - 2)
            {
                int tempValue = ints[i];
                tempValue = tempValue * 2;

                if (tempValue > 9) { tempValue = tempValue % 10 + 1; }
                //{ tempValue = tempValue - 9; }
                ints[i] = tempValue;

            }

            int total = 0;
            for (int i = 0; i < ints.Length; i++)
            {

                total += ints[i];
            }

            if (total % 10 == 0)
            {
                Console.WriteLine("Card number is valid.");
                valid = true;


            }
            else
            {
                Console.WriteLine("Card invalid.");
                valid = false;
            }

            return valid;



        }

        static decimal InitializePayment(decimal balance, decimal BankContents)
        {                                               //user should pay until balance is zero or less than zero

            decimal UserPayment;
            decimal UserPaymentUpdate = 0.00m;
            decimal changeDue = 0.00m;

            while (balance > 0)
            {

                Console.WriteLine("Please insert payment one bill at a time. Cash/Coin only. ");
                UserPayment = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine();

                //input validation for correct bill or change
                if (UserPayment == 100.00m || UserPayment == 50.00m || UserPayment == 20.00m || UserPayment == 10.00m || UserPayment == 5.00m || UserPayment == 2.00m || UserPayment == 1.00m || UserPayment == 0.25m || UserPayment == 0.10m || UserPayment == 0.05m || UserPayment == 0.01m)
                {

                    if (UserPayment == 100.00m) { Bank.Hundreds += 1; }
                    else if (UserPayment == 50.00m) { Bank.Fifties += 1; }
                    else if (UserPayment == 20.00m) { Bank.Twenties += 1; }
                    else if (UserPayment == 10.00m) { Bank.Tens += 1; }
                    else if (UserPayment == 5.00m) { Bank.Fives += 1; }
                    else if (UserPayment == 2.00m) { Bank.Twos += 1; }
                    else if (UserPayment == 1.00m) { Bank.Ones += 1; }
                    else if (UserPayment == 0.25m) { Bank.Quarters += 1; }
                    else if (UserPayment == 0.10m) { Bank.Dimes += 1; }
                    else if (UserPayment == 0.05m) { Bank.Nickels += 1; }
                    else if (UserPayment == 0.01m) { Bank.Pennies += 1; }
                }
                else          //currency validation
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid currency. This machine accepts $100, $50, $20, $10, $5, $2, $1.");
                    Console.WriteLine("Quarters, Dimes, Nickels, and Pennies.");
                    UserPayment = Convert.ToDecimal(Console.ReadLine());
                }


                if (UserPayment > balance)
                {
                    changeDue = (UserPayment - balance);

                }

                UserPaymentUpdate += UserPayment;    // running total of payment
                balance -= UserPayment;

                Console.WriteLine();
                Console.WriteLine("Balance:\t${0}", balance);

            }


            if (changeDue > BankContents)          //is there enough change in till?
            {

                Console.WriteLine();
                Console.WriteLine("Sorry, there is insufficient funds in the kiosk to complete this transaction. Please use another kiosk or ask an employee for help.");
            }
            //display message to user thanking them and showing how much they paid and what they are owed
            else
            {

                Console.WriteLine(DateTime.Now);
                Console.WriteLine();
                Console.WriteLine("Thank you for your purchase! Your change is: " + changeDue);
            }

            Console.WriteLine();
            Console.WriteLine("You entered ${0} cash and change due is ${1}", UserPaymentUpdate, changeDue);
            return changeDue;

        }
        //changebreaker uses greedy algorithm to give out bigger bills if in the till as change bfore smaller bills
        static void ChangeBreaker(decimal changeDue, decimal cashRequested)
        {
            decimal changeDueRemaining = 0;
            changeDue = changeDue + cashRequested;

            for (int i = 0; changeDue > 0; i++)
            {                                            //is $100 less than change owed? Is there at least one $100 bill in till?
                                                         //if true then dispense $100 bill. If not, then dispense $50 and so on...
                if ((changeDue % 100.00m) < changeDue)
                {
                    Bank.HundredsValue = (int)(changeDue / 100.00m);
                    changeDueRemaining = changeDue % 100.00m;
                    changeDue = changeDueRemaining;
                    Bank.Hundreds--;
                    Console.WriteLine("$100.00 Dispensed");

                }


                else if ((changeDue % 50.00m) < changeDue)
                {
                    Bank.FiftiesValue = (int)(changeDue / 50.00m);
                    changeDueRemaining = changeDue % 50.00m;
                    changeDue = changeDueRemaining;
                    Bank.Fifties--;
                    Console.WriteLine("$50.00 Dispensed");

                }

                else if ((changeDue % 20.00m) < changeDue)
                {
                    Bank.TwentiesValue = (int)(changeDue / 20.00m);
                    changeDueRemaining = changeDue % 20.00m;
                    changeDue = changeDueRemaining;
                    Bank.Twenties--;
                    Console.WriteLine("$20.00 Dispensed");

                }

                else if ((changeDue % 10.00m) < changeDue)
                {
                    Bank.TensValue = (int)(changeDue / 10.00m);
                    changeDueRemaining = changeDue % 10.00m;
                    changeDue = changeDueRemaining;
                    Bank.Tens--;
                    Console.WriteLine("$10.00 Dispensed");

                }

                else if ((changeDue % 5.00m) < changeDue)
                {
                    Bank.FivesValue = (int)(changeDue / 5.00m);
                    changeDueRemaining = changeDue % 5.00m;
                    changeDue = changeDueRemaining;
                    Bank.Fives--;
                    Console.WriteLine("$5.00 Dispensed");

                }

                else if ((changeDue % 2.00m) < changeDue)
                {
                    Bank.TwosValue = (int)(changeDue / 2.00m);
                    changeDueRemaining = changeDue % 2.00m;
                    changeDue = changeDueRemaining;
                    Bank.Twos--;
                    Console.WriteLine("$2.00 Dispensed");

                }

                else if ((changeDue % 1.00m) < changeDue)
                {
                    Bank.OnesValue = (int)(changeDue / 1.00m);
                    changeDueRemaining = changeDue % 1.00m;
                    changeDue = changeDueRemaining;
                    Bank.Ones--;
                    Console.WriteLine("$1.00 Dispensed");

                }

                else if ((changeDue % 0.25m) < changeDue)
                {
                    Bank.QuartersValue = (int)(changeDue / 0.25m);
                    changeDueRemaining = changeDue % 0.25m;
                    changeDue = changeDueRemaining;
                    Bank.Quarters--;
                    Console.WriteLine("$0.25 Dispensed");

                }

                else if ((changeDue % 0.10m) < changeDue)
                {
                    Bank.DimesValue = (int)(changeDue / 0.10m);
                    changeDueRemaining = changeDue % 0.10m;
                    changeDue = changeDueRemaining;
                    Bank.Dimes--;
                    Console.WriteLine("$0.10 Dispensed");

                }

                else if ((changeDue % 0.05m) < changeDue)
                {
                    Bank.NickelsValue = (int)(changeDue / 0.05m);
                    changeDueRemaining = changeDue % 0.05m;
                    changeDue = changeDueRemaining;
                    Bank.Nickels--;
                    Console.WriteLine("$0.05 Dispensed");

                }

                else if ((changeDue % 0.01m) < changeDue)
                {
                    Bank.PenniesValue = (int)(changeDue / 0.01m);
                    changeDueRemaining = changeDue % 0.01m;
                    changeDue = changeDueRemaining;
                    Bank.Pennies--;
                    Console.WriteLine("$0.01 Dispensed");

                }

            }

            Console.ReadKey();      //hide the ending code. (the wizard behind the curtain)


        }


    }


}