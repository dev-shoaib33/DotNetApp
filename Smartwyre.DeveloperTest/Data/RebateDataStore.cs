using Microsoft.VisualBasic;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Utils;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore
{

    private readonly IDbConnectionWrapper sqlConnectionWrapper = new SqlConnectionWrapper();



    public Rebate GetRebate(string rebateIdentifier)
    {
        try
        {
            sqlConnectionWrapper.Open();

            string getRebateQuery = "SELECT * FROM Rebate WHERE Identifier = @Identifier";
            using (SqlCommand getRebateCommand = sqlConnectionWrapper.CreateCommand())
            {
                getRebateCommand.CommandText = getRebateQuery;
                getRebateCommand.Parameters.AddWithValue("@Identifier", rebateIdentifier);

                using (SqlDataReader reader = getRebateCommand.ExecuteReader())
                {
                    Rebate rebate = null;

                    if (reader.Read())
                    {
                        rebate = new Rebate();
                        rebate.Identifier = reader.GetString(reader.GetOrdinal("Identifier"));
                        rebate.Amount = reader.GetDecimal(reader.GetOrdinal("Amount"));

                        string incentiveTypeString = reader.GetString(reader.GetOrdinal("IncentiveType"));
                        if (Enum.TryParse(incentiveTypeString, out IncentiveType incentiveType))
                        {
                            rebate.Incentive = incentiveType;
                        }
                        else
                        {
                            Console.WriteLine("Invalid IncentiveType value in the database. Using default value.");
                            rebate.Incentive = IncentiveType.FixedRateRebate;
                        }

                        rebate.Percentage = reader.GetDecimal(reader.GetOrdinal("Percentage"));
                    }

                    return rebate;
                }
            }
        }
        catch (SqlException ex)
        {
            Commons.Error("An error occurred while executing the SQL query: " + ex.Message);
            // Log the exception or perform other error handling tasks
            // ...
            return null;
        }
        catch (Exception ex)
        {
            Commons.Error("An unexpected error occurred: " + ex.Message);
            // Log the exception or perform other error handling tasks
            // ...
            return null;
        }
        finally
        {

                sqlConnectionWrapper.Close();            
        }
    }



    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        try
        {
            sqlConnectionWrapper.Open();

            string updateRebateQuery = "UPDATE Rebate SET Amount = @RebateAmount WHERE Identifier = @Identifier";
            using (SqlCommand updateRebate = sqlConnectionWrapper.CreateCommand())
            {
                updateRebate.CommandText = updateRebateQuery;
                updateRebate.Parameters.AddWithValue("@RebateAmount", rebateAmount);
                updateRebate.Parameters.AddWithValue("@Identifier", account.Identifier);

                int rowsAffected = updateRebate.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Commons.Error("No rows were affected. Rebate not updated.");
                }
                else
                {
                    Console.WriteLine("Rebate updated successfully.");
                }
            }

            // Additional code for updating account in the database if necessary

            sqlConnectionWrapper.Close();
        }
        catch (SqlException ex)
        {
            Commons.Error("An error occurred while executing the SQL query: " + ex.Message);
            // Log the exception or perform other error handling tasks
            // ...
        }
        catch (Exception ex)
        {
            Commons.Error("An unexpected error occurred: " + ex.Message);
            // Log the exception or perform other error handling tasks
            // ...
        }
        finally
        {
                sqlConnectionWrapper.Close();   
        }
    }


    public string CreateRebate()
    {
        try
        {
            // Rebate
            Rebate rebate = GetValues();

            if (rebate == null)
            {
                Commons.Error("Rebate object is null");
                // You may choose to log the exception or perform other error handling actions here
                return null;
            }

            sqlConnectionWrapper.Open();
            

            string insertRebateQuery = "INSERT INTO Rebate (Identifier, Amount, IncentiveType, Percentage) VALUES (@Identifier, @Amount, @IncentiveType, @Percentage)";
            using (SqlCommand insertRebateCommand = sqlConnectionWrapper.CreateCommand())
            {
                insertRebateCommand.CommandText = insertRebateQuery;
                insertRebateCommand.Parameters.AddWithValue("@Identifier", rebate.Identifier);
                insertRebateCommand.Parameters.AddWithValue("@Amount", rebate.Amount);
                insertRebateCommand.Parameters.AddWithValue("@IncentiveType", rebate.Incentive);
                insertRebateCommand.Parameters.AddWithValue("@Percentage", rebate.Percentage);
                insertRebateCommand.ExecuteNonQuery();
            }

            Console.WriteLine("Rebate Data Inserted");
            Console.Write("Press anything to proceed...");
            Console.ReadLine();
            Console.Clear();

            return rebate.Identifier;
        }
        catch (SqlException ex)
        {
            Commons.Error("An error occurred while executing the SQL query: " + ex.Message);
            // Log the exception or perform other error handling tasks
            // ...
            return null;
        }
        catch (Exception ex)
        {
            Commons.Error("An unexpected error occurred: " + ex.Message);
            // Log the exception or perform other error handling tasks
            // ...
            return null;
        }
        finally
        {
                sqlConnectionWrapper.Close();   
        }
    }


    public Rebate GetValues()
    {
        try
        {
            Rebate rebate = new();
            Console.WriteLine("Please Enter Rebate Details:");
            rebate.Identifier = Guid.NewGuid().ToString();
            Console.Write("Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Commons.Error("Invalid amount. Please enter a valid decimal number.");
                return null;
            }
            rebate.Amount = amount;

            Console.Write("Percentage: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal percentage))
            {
                Commons.Error("Invalid percentage. Please enter a valid decimal number.");
                return null;
            }
            rebate.Percentage = percentage;

            Console.WriteLine("Press 1 for Fixed Rate Rebate");
            Console.WriteLine("Press 2 for Amount Per Uom");
            Console.WriteLine("Press 3 for Fixed Cash Amount");
            Console.Write("Press 1-3 for Selection of Incentive Types: ");
            if (!int.TryParse(Console.ReadLine(), out int selectIncentive) || selectIncentive < 1 || selectIncentive > 3)
            {
                Commons.Error("Invalid selection. Please enter a number between 1 and 3.");
                return null;
            }

            switch (selectIncentive)
            {
                case 1:
                    rebate.Incentive = IncentiveType.FixedRateRebate;
                    break;
                case 2:
                    rebate.Incentive = IncentiveType.AmountPerUom;
                    break;
                case 3:
                    rebate.Incentive = IncentiveType.FixedCashAmount;
                    break;
                default:
                    Commons.Error("Invalid options. Please select a number between 1 and 3.");
                    return null;
            }

            return rebate;
        }
        catch (ArgumentException ex)
        {
            Commons.Error("An argument error occurred: " + ex.Message);
            // Log the exception or perform other error handling tasks
            // ...
            return null;
        }
        catch (Exception ex)
        {
            Commons.Error("An unexpected error occurred: " + ex.Message);
            // Log the exception or perform other error handling tasks
            // ...
            return null;
        }
    }

}
