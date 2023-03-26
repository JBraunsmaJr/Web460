using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using BraunsmaWeek4.Models;
using Database;
using Faker;

namespace BraunsmaWeek4
{
    public class WebLabsDbContext : IDisposable
    {
        private static BadgerDbContext _instance;
        public static BadgerDbContext Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
        
                var dbPath = HttpContext.Current.Server.MapPath("~/App_Data/orders.db");

                if (!File.Exists(dbPath))
                {
                    var part = dbPath.LastIndexOf(Path.DirectorySeparatorChar);
                    var dir = dbPath.Substring(0, part);
                    Directory.CreateDirectory(dir);
                    File.Create(dbPath);

                    /*
                        Not entirely sure why this is required.

                        By creating the file path above at runtime (when it does not exist)
                        The database connection fails to establish until app-restart. Almost
                        as if the resource isn't released?

                        Even with retry attempts, for N amount of seconds fails.
                        This behavior is not experienced with Net5/6/7 so perhaps it's a weird
                        nuance of this framework.
                     */

                    Console.WriteLine("Created database file. Must restart application");
                    Environment.Exit(0);
                }

                
                _instance = new BadgerDbContext($"Data Source={dbPath}");
                _instance.InitializeDatabase<WebLabsDbContext>();
                
                SeedDatabase();

                return _instance;
            }
        }

        ~WebLabsDbContext()
        {
            Dispose();
        }

        public void Dispose()
        {
            Instance?.Dispose();
        }
        
        public static void SeedDatabase()
        {
            try
            {
                var checkOrders = _instance.GetAll<Order>();

                if (checkOrders.Any())
                    return;

                string[] paymentTypes = { "Visa", "Master Card", "Discover"};
                
                var usedLastNames = new HashSet<string>();
                for (var i = 0; i < RandomNumber.Next(10, 50); i++)
                {
                    var lastName = Faker.Name.Last();
                    var attempts = 10;
                    while (attempts > 0 && usedLastNames.Contains(lastName))
                    {
                        lastName = Name.Last();
                        attempts--;
                    }

                    if (usedLastNames.Contains(lastName))
                        continue;

                    var order = new Order
                    {
                        LastName = lastName,
                        FirstName = Name.First(),
                        City = Address.City(),
                        State = Address.UsState(),
                        Street = Address.StreetAddress(),
                        PhoneNumber = Phone.Number(),
                        CreditCardNumber = $"{RandomNumber.Next(1000, 9999)}-{RandomNumber.Next(1000, 9999)}-{RandomNumber.Next(1000, 9999)}-{RandomNumber.Next(1000, 9999)}",
                        PaymentType = paymentTypes[RandomNumber.Next(0,2)]
                    };

                    _instance.Insert(order);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                Environment.Exit(1);
            }
        }
    }
}