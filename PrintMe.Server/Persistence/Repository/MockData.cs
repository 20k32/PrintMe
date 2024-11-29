using System.Runtime.InteropServices;
using Bogus;
using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Logic.Helpers;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository
{
    internal static class MockData
    {
        private static int _printersCount;
        private static int _printMaterialCount;
        private static int _modelCount;
        private static readonly Faker _faker = new();
        private static (string passwordHash, string passwordSalt) GenerateSequrityInfo(string password)
        {
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(password, salt);

            return (hash, salt);
        }

        private static UserRole[] roles =
        [
            new() { UserRoleId = 1, UserRoleName = "User" },
            new () { UserRoleId = 2, UserRoleName = "PrinterOwner"},
            // admin is set by system
            //new () { UserRoleId = 3, UserRoleName = "Admin"}
        ];

        private static UserStatus[] statuses =
        [
            new UserStatus(){ UserStatusId = 1, Status = "Active"},
            new UserStatus(){ UserStatusId = 2, Status = "Inactive"}
        ];
        
        private static User GenerateUser(int id, string password)
        {
            var securityInfo = GenerateSequrityInfo(password);

            var userRole = _faker.Random.ArrayElement(roles);
            var userStatus = _faker.Random.ArrayElement(statuses);
            
            return new User()
            {
                UserId = id,
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Description = _faker.Company.CompanyName(),
                Password = securityInfo.passwordHash,
                PasswordSalt = securityInfo.passwordSalt,
                PhoneNumber = _faker.Phone.PhoneNumber("##########"),
                UserStatusId = userStatus.UserStatusId,
                Email = _faker.Internet.Email(),
                ShouldHidePhoneNumber = false,
                UserRoleId = userRole.UserRoleId
            };
        }
        
        public static void GenerateForUsers(this ModelBuilder builder, int count)
        {
            var userList = new List<User>(count);

            foreach (var index in Enumerable.Range(1, count))
            {
                var user = GenerateUser(index, $"user{index}");
                userList.Add(user);
            }
            
            builder.Entity<User>().HasData(userList);
        }

        public static void GenerateForUserRoles(this ModelBuilder builder) =>
            builder.Entity<UserRole>().HasData(roles);
        
        public static void GenerateForUserStatuses(this ModelBuilder builder) =>
            builder.Entity<UserStatus>().HasData(statuses);


        private static PrinterModel GenerateModel(int index)
        {
            return new()
            {
                PrinterModelId = index,
                Name = _faker.Vehicle.Model()
            };
        }

        public static void GenerateForPrinterModels(this ModelBuilder builder, int count)
        {
            _modelCount = count;
            
            var models = new List<PrinterModel>(count);

            foreach (var index in Enumerable.Range(1, count))
            {
                var model = new PrinterModel()
                {
                    Name = _faker.Vehicle.Model(),
                    PrinterModelId = index
                };
                
                models.Add(model);
            }

            builder.Entity<PrinterModel>().HasData(models);
        }
        
        private static Printer GeneratePrinter(int index)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(_modelCount, 0);
            
            return new()
            {
                PrinterId = index,
                PrinterModelId = _faker.Random.Int(1, _modelCount),
                Description = _faker.Vehicle.Manufacturer(),
                LocationX = _faker.Random.Double(0, 100),
                LocationY = _faker.Random.Double(0, 100),
                MinModelWidth = _faker.Random.Double(10, 100),
                MinModelHeight = _faker.Random.Double(0, 100),
                MaxModelHeight = _faker.Random.Double(0, 100),
                MaxModelWidth = _faker.Random.Double(0, 100),
            };
        }
        
        public static void GenerateForPrinters(this ModelBuilder builder, int count)
        {
            _printersCount = count;
            var printers = new List<Printer>(count);
            
            foreach (var index in Enumerable.Range(1, count))
            {
                var printer = GeneratePrinter(index);
                printers.Add(printer);
            }

            builder.Entity<Printer>().HasData(printers);
        }

        public static void GenerateForPrintMaterial(this ModelBuilder builder, int count)
        {
            _printMaterialCount = count;
            
            var materials = new List<PrintMaterial>(count);
            
            foreach (var index in Enumerable.Range(1, count))
            {
                var material = new PrintMaterial()
                {
                    Name = _faker.Music.Genre(),
                    PrintMaterialId = index
                };
                
                materials.Add(material);
            }

            builder.Entity<PrintMaterial>().HasData(materials);
        }
    }
}