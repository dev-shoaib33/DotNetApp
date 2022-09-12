using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Utils;
using System;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore
{
    private readonly IDbConnectionWrapper sqlConnectionWrapper = new SqlConnectionWrapper();

    public Product GetProduct(string identifier)
    {
        try
        {
            sqlConnectionWrapper.Open();

            string selectProductQuery = "SELECT Identifier, Price, Uom, SupportedIncentive FROM Product WHERE Identifier = @Identifier";

            using (SqlCommand selectCommand = sqlConnectionWrapper.CreateCommand())
            {
                selectCommand.CommandText = selectProductQuery;
                selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Product product = new Product
                        {
                            Identifier = reader.GetString(0),
                            Price = reader.GetDecimal(1),
                            Uom = reader.GetString(2),
                            SupportedIncentives = (SupportedIncentiveType)Enum.Parse(typeof(SupportedIncentiveType), reader.GetString(3))
                        };
                        return product;
                    }
                    else
                    {
                        Commons.Error("Product not found.");
                        return null;
                    }
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

    public string CreateProduct()
    {
        try
        {
            Product product = GetValues();

            if (product == null)
            {
                Commons.Error("Product object is null");
                // You may choose to log the exception or perform other error handling actions here
                return null;
            }

            sqlConnectionWrapper.Open();
            string insertProductQuery = "INSERT INTO Product (Identifier, Price, Uom, SupportedIncentive) VALUES (@Identifier, @Price, @Uom, @SupportedIncentive)";

            using (SqlCommand insertCommand = sqlConnectionWrapper.CreateCommand())
            {
                insertCommand.CommandText = insertProductQuery;
                insertCommand.Parameters.AddWithValue("@Identifier", product.Identifier);
                insertCommand.Parameters.AddWithValue("@Price", product.Price);
                insertCommand.Parameters.AddWithValue("@Uom", product.Uom);
                insertCommand.Parameters.AddWithValue("@SupportedIncentive", product.SupportedIncentives);
                insertCommand.ExecuteNonQuery();
            }

            Console.WriteLine("Product Data Inserted");
            Console.Write("Press anything to proceed...");
            Console.ReadLine();
            Console.Clear();
            sqlConnectionWrapper.Close();
            return product.Identifier;
        }
        catch (ArgumentException ex)
        {
            Commons.Error("Error: " + ex.Message);
            // You may choose to log the exception or perform other error handling actions here
            return null;
        }
        catch (Exception ex)
        {
            Commons.Error("An error occurred: " + ex.Message);
            // You may choose to log the exception or perform other error handling actions here
            return null;
        }
    }


    public Product GetValues()
    {
        try
        {
            Product product = new Product();
            Console.WriteLine("Please Enter Product Details:");
            product.Identifier = Guid.NewGuid().ToString();
            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
               Commons.Error("Invalid price. Please enter a valid decimal number.");
                return null;
            }
            product.Price = price;
            Console.Write("UOM: ");
            product.Uom = Console.ReadLine();
            Console.WriteLine("Press 1 for Fixed Rate Rebate");
            Console.WriteLine("Press 2 for Amount Per Uom");
            Console.WriteLine("Press 3 for Fixed Cash Amount");
            Console.Write("Press 1-3 for Selection of Incentive Types: ");
            if (!int.TryParse(Console.ReadLine(), out int select) || select < 1 || select > 3)
            {
                Commons.Error("Invalid selection. Please enter a number between 1 and 3.");
                return null;
            }

            switch (select)
            {
                case 1:
                    product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;
                    break;
                case 2:
                    product.SupportedIncentives = SupportedIncentiveType.AmountPerUom;
                    break;
                case 3:
                    product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;
                    break;
                default:
                    Commons.Error("Invalid selection. Please select a number between 1 and 3.");
                    return null;
            }

            return product;
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
