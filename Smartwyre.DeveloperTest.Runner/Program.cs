using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Utils;
using System;
using System.Runtime.CompilerServices;

namespace Smartwyre.DeveloperTest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CalculateRebateRequest crr = new();
                ProductDataStore pds = new();
                RebateDataStore rds = new();

                bool isError = true;


                while (isError)
                {
                    Console.WriteLine("Welcome to Rebate Calculation Console Application");
                    Console.WriteLine("1. Calulate Rebate");
                    Console.WriteLine("2. Quit");
                    Console.Write("Enter your choice: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            var productIdentifier = pds.CreateProduct();
                            if (productIdentifier == null)
                            {
                                Commons.Error("Failed to post the product. Exiting...");
                                break;
                            }
                            crr.ProductIdentifier = productIdentifier;

                            // Handle errors when posting a rebate
                            var rebateIdentifier = rds.CreateRebate();
                            if (rebateIdentifier == null)
                            {
                                Commons.Error("Failed to post the rebate. Exiting...");
                                break;
                            }
                            crr.RebateIdentifier = rebateIdentifier;

                            Console.Write("Please Enter the Volume: ");

                            // Handle errors when parsing the volume
                            if (!int.TryParse(Console.ReadLine(), out int volume))
                            {
                                Commons.Error("Invalid volume entered. Exiting...");
                                break;
                            }
                            crr.Volume = volume;

                            IRebateService rebateService = new RebateService(rds, pds);
                            CalculateRebateResult result = rebateService.Calculate(crr);

                            if (result.Success)
                            {
                                Console.WriteLine("Rebate calculation is updated");
                                isError = false;
                            }
                            else
                            {
                                Console.WriteLine("Rebate calculation is not updated due to an issue");
                                break;
                            }
                            break;
                        case "2":
                            isError = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice! Please try again.");
                            break;
                    }

                    Console.WriteLine();
                }

                // Handle errors when posting a product
              
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
