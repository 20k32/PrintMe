using System.Runtime.InteropServices;
using Bogus;
using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Logic.Helpers;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository
{
    internal static class MockData
    {
        private static readonly Faker _faker = new();

        private static (string passwordHash, string passwordSalt) GenerateSequrityInfo(
            string password)
        {
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(password, salt);

            return (hash, salt);
        }

        private static UserRole[] _roles =
        [
            new()
            {
                //UserRoleId = 1,
                UserRoleName = "User"
            },
            new()
            {
                //UserRoleId = 2, 
                UserRoleName = "PrinterOwner"
            },
            // admin is set by system
            //new () { UserRoleId = 3, UserRoleName = "Admin"}
        ];

        private static UserStatus[] _statuses =
        [
            new UserStatus()
            {
                //UserStatusId = 1,
                Status = "Active"
            },
            new UserStatus()
            {
                //UserStatusId = 2, 
                Status = "Inactive"
            }
        ];

        private static User GenerateUser(string password)
        {
            var securityInfo = GenerateSequrityInfo(password);

            var userRole = _faker.Random.ArrayElement(_roles);
            var userStatus = _faker.Random.ArrayElement(_statuses);

            return new User()
            {
                //UserId = id,
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Description = _faker.Company.CompanyName(),
                Password = securityInfo.passwordHash,
                PasswordSalt = securityInfo.passwordSalt,
                PhoneNumber = _faker.Phone.PhoneNumber("##########"),
                UserStatus = userStatus,
                Email = _faker.Internet.Email(),
                ShouldHidePhoneNumber = false,
                UserRole = userRole
            };
        }

        public static Task GenerateForUsersAsync(this DbSet<User> users, int count)
        {
            var userList = new List<User>(count);

            foreach (var index in Enumerable.Range(1, count))
            {
                var user = GenerateUser($"user{index}");
                userList.Add(user);
            }

            return users.AddRangeAsync(userList);
        }

        public static Task GenerateForUserRolesAsync(this DbSet<UserRole> roles) =>
            roles.AddRangeAsync(_roles);

        public static Task GenerateForUserStatusesAsync(this DbSet<UserStatus> statuses) =>
            statuses.AddRangeAsync(_statuses);


        private static PrinterModel GeneratePrinterModel(int index, List<Printer> printers)
        {
            return new()
            {
                //PrinterModelId = index,
                Name = _faker.Vehicle.Model(),
                Printers = printers
            };
        }

        private static Task GenerateForPrinterModels(DbSet<PrinterModel> printerModels, int count,
            Printer[] allPrinters)
        {
            var models = new List<PrinterModel>(count);

            foreach (var index in Enumerable.Range(1, count))
            {
                var randomPrinters = _faker.Random.ArrayElements(allPrinters);

                if (randomPrinters.Length > 0)
                {
                    var model = GeneratePrinterModel(index, randomPrinters.ToList());

                    foreach (var printer in randomPrinters)
                    {
                        printer.PrinterModel = model;
                    }

                    models.Add(model);
                }
            }

            return printerModels.AddRangeAsync(models);
        }

        private static Printer GeneratePrinter(int index, int modelsCount)
        {
            return new()
            {
                //PrinterId = index,
                Description = _faker.Vehicle.Manufacturer(),
                LocationX = _faker.Random.Double(0, 100),
                LocationY = _faker.Random.Double(0, 100),
                MinModelWidth = _faker.Random.Double(10, 100),
                MinModelHeight = _faker.Random.Double(0, 100),
                MaxModelHeight = _faker.Random.Double(0, 100),
                MaxModelWidth = _faker.Random.Double(0, 100),
            };
        }

        public static async Task GenerateForPrintersAsync(this DbSet<Printer> printers,
            DbSet<PrintMaterial> materials, DbSet<PrinterModel> models, int count,
            int printMaterialCount, int modelsCount)
        {
            var printerList = new List<Printer>(count);

            foreach (var index in Enumerable.Range(1, count))
            {
                var printer = GeneratePrinter(index, modelsCount);
                printerList.Add(printer);
            }

            await printers.AddRangeAsync(printerList);
            await GenerateForPrinterModels(models, modelsCount, printerList.ToArray());
            await GenerateForPrintMaterials(materials, printMaterialCount, printerList.ToArray());
        }

        private static PrintMaterial GeneratePrintMaterial(int index, Printer[] printers)
        {
            return new()
            {
                Name = _faker.Company.CompanyName(),
                //PrintMaterialId = index,
                Printers = printers
            };
        }

        private static Task GenerateForPrintMaterials(DbSet<PrintMaterial> materials, int count,
            Printer[] printers)
        {
            var materialList = new List<PrintMaterial>(count);

            foreach (var index in Enumerable.Range(1, count))
            {
                var randomPrinters = _faker.Random.ArrayElements(printers);
                var material = GeneratePrintMaterial(index, randomPrinters);
                materialList.Add(material);
            }

            return materials.AddRangeAsync(materialList);
        }
    }
}