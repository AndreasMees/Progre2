using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static async Task Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            // Kui andmed on olemas, välju
            if (context.Customers.Any())
            {
                return;
            }

            // Lisa kasutajad
            var user1 = new IdentityUser { UserName = "employee1@test.com", Email = "employee1@test.com", EmailConfirmed = true };
            var user2 = new IdentityUser { UserName = "employee2@test.com", Email = "employee2@test.com", EmailConfirmed = true };
            var user3 = new IdentityUser { UserName = "employee3@test.com", Email = "employee3@test.com", EmailConfirmed = true };

            await userManager.CreateAsync(user1, "Test1234!");
            await userManager.CreateAsync(user2, "Test1234!");
            await userManager.CreateAsync(user3, "Test1234!");

            // Lisa kliendid
            var customers = new List<Customer>
            {
                new Customer { Name = "AS Tallinna Bussid", Address = "Pärnu mnt 1, Tallinn", Email = "info@tallinnabussid.ee", Phone = "+37251234567", Discount = 0.1m },
                new Customer { Name = "OÜ Kiirveos", Address = "Tartu mnt 5, Tallinn", Email = "info@kiirveos.ee", Phone = "+37251234568", Discount = 0.05m },
                new Customer { Name = "AS Logistika Pro", Address = "Narva mnt 10, Tallinn", Email = "info@logistikapro.ee", Phone = "+37251234569", Discount = 0.15m },
                new Customer { Name = "OÜ Transpordi Teenused", Address = "Viru 2, Tallinn", Email = "info@transpordi.ee", Phone = "+37251234570", Discount = 0m },
                new Customer { Name = "AS Eesti Autod", Address = "Liivalaia 3, Tallinn", Email = "info@eestiauto.ee", Phone = "+37251234571", Discount = 0.2m },
                new Customer { Name = "OÜ Sõidukid", Address = "Gonsiori 4, Tallinn", Email = "info@soidukid.ee", Phone = "+37251234572", Discount = 0.05m },
                new Customer { Name = "AS Veokid Nord", Address = "Peterburi tee 6, Tallinn", Email = "info@veokid.ee", Phone = "+37251234573", Discount = 0.1m },
                new Customer { Name = "OÜ Autopark", Address = "Mustamäe tee 7, Tallinn", Email = "info@autopark.ee", Phone = "+37251234574", Discount = 0m },
                new Customer { Name = "AS Kiirtransport", Address = "Paldiski mnt 8, Tallinn", Email = "info@kiirtransport.ee", Phone = "+37251234575", Discount = 0.1m },
                new Customer { Name = "OÜ Fleetmaster", Address = "Järvevana tee 9, Tallinn", Email = "info@fleetmaster.ee", Phone = "+37251234576", Discount = 0.05m }
            };
            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();

            // Lisa sõidukid
            var vehicles = new List<Vehicle>
            {
                new Vehicle { Manufacturer = "Volvo", Model = "FH16", LicensePlate = "ABC123" },
                new Vehicle { Manufacturer = "Mercedes", Model = "Sprinter", LicensePlate = "DEF456" },
                new Vehicle { Manufacturer = "BMW", Model = "X5", LicensePlate = "GHI789" },
                new Vehicle { Manufacturer = "Toyota", Model = "Land Cruiser", LicensePlate = "JKL012" },
                new Vehicle { Manufacturer = "Ford", Model = "Transit", LicensePlate = "MNO345" },
                new Vehicle { Manufacturer = "Scania", Model = "R500", LicensePlate = "PQR678" },
                new Vehicle { Manufacturer = "MAN", Model = "TGX", LicensePlate = "STU901" },
                new Vehicle { Manufacturer = "DAF", Model = "XF105", LicensePlate = "VWX234" },
                new Vehicle { Manufacturer = "Renault", Model = "Master", LicensePlate = "YZA567" },
                new Vehicle { Manufacturer = "Iveco", Model = "Daily", LicensePlate = "BCD890" }
            };
            context.Vehicles.AddRange(vehicles);
            await context.SaveChangesAsync();

            // Lisa operatsioonitüübid (kui pole olemas)
            if (!context.OperationTypes.Any())
            {
                var operationTypes = new List<OperationType>
                {
                    new OperationType { Name = "Maintenance" },
                    new OperationType { Name = "Repair" },
                    new OperationType { Name = "Cleaning" },
                    new OperationType { Name = "Relocation" },
                    new OperationType { Name = "Inspection" }
                };
                context.OperationTypes.AddRange(operationTypes);
                await context.SaveChangesAsync();
            }

            // Lisa operatsioonid
            var opTypes = context.OperationTypes.ToList();
            var savedVehicles = context.Vehicles.ToList();
            var employees = userManager.Users.ToList();

            var operations = new List<Operation>
            {
                new Operation { VehicleId = savedVehicles[0].Id, OperationTypeId = opTypes[0].Id, AssignedEmployeeId = user1.Id, Date = DateTime.Now.AddDays(-10), Status = OperationStatus.Completed, Cost = 150m },
                new Operation { VehicleId = savedVehicles[1].Id, OperationTypeId = opTypes[1].Id, AssignedEmployeeId = user2.Id, Date = DateTime.Now.AddDays(-8), Status = OperationStatus.Completed, Cost = 320m },
                new Operation { VehicleId = savedVehicles[2].Id, OperationTypeId = opTypes[2].Id, AssignedEmployeeId = user3.Id, Date = DateTime.Now.AddDays(-6), Status = OperationStatus.InProgress, Cost = 80m },
                new Operation { VehicleId = savedVehicles[3].Id, OperationTypeId = opTypes[3].Id, AssignedEmployeeId = user1.Id, Date = DateTime.Now.AddDays(-4), Status = OperationStatus.Pending, Cost = null },
                new Operation { VehicleId = savedVehicles[4].Id, OperationTypeId = opTypes[4].Id, AssignedEmployeeId = user2.Id, Date = DateTime.Now.AddDays(-3), Status = OperationStatus.Completed, Cost = 200m },
                new Operation { VehicleId = savedVehicles[5].Id, OperationTypeId = opTypes[0].Id, AssignedEmployeeId = user3.Id, Date = DateTime.Now.AddDays(-2), Status = OperationStatus.InProgress, Cost = 175m },
                new Operation { VehicleId = savedVehicles[6].Id, OperationTypeId = opTypes[1].Id, AssignedEmployeeId = user1.Id, Date = DateTime.Now.AddDays(-1), Status = OperationStatus.Pending, Cost = null },
                new Operation { VehicleId = savedVehicles[7].Id, OperationTypeId = opTypes[2].Id, AssignedEmployeeId = user2.Id, Date = DateTime.Now, Status = OperationStatus.Pending, Cost = null },
                new Operation { VehicleId = savedVehicles[8].Id, OperationTypeId = opTypes[3].Id, AssignedEmployeeId = user3.Id, Date = DateTime.Now.AddDays(1), Status = OperationStatus.Pending, Cost = null },
                new Operation { VehicleId = savedVehicles[9].Id, OperationTypeId = opTypes[4].Id, AssignedEmployeeId = user1.Id, Date = DateTime.Now.AddDays(2), Status = OperationStatus.Pending, Cost = null }
            };
            context.Operations.AddRange(operations);
            await context.SaveChangesAsync();

            // Lisa arved
            var savedCustomers = context.Customers.ToList();
            var invoices = new List<Invoice>
            {
                new Invoice { InvoiceNo = "INV-0001", InvoiceDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(0), Subtotal = 500m, Shipping = 20m, GrandTotal = 520m, CustomerId = savedCustomers[0].Id },
                new Invoice { InvoiceNo = "INV-0002", InvoiceDate = DateTime.Now.AddDays(-28), DueDate = DateTime.Now.AddDays(2), Subtotal = 300m, Shipping = 15m, GrandTotal = 315m, CustomerId = savedCustomers[1].Id },
                new Invoice { InvoiceNo = "INV-0003", InvoiceDate = DateTime.Now.AddDays(-25), DueDate = DateTime.Now.AddDays(5), Subtotal = 750m, Shipping = 30m, GrandTotal = 780m, CustomerId = savedCustomers[2].Id },
                new Invoice { InvoiceNo = "INV-0004", InvoiceDate = DateTime.Now.AddDays(-20), DueDate = DateTime.Now.AddDays(10), Subtotal = 1200m, Shipping = 50m, GrandTotal = 1250m, CustomerId = savedCustomers[3].Id },
                new Invoice { InvoiceNo = "INV-0005", InvoiceDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(15), Subtotal = 450m, Shipping = 25m, GrandTotal = 475m, CustomerId = savedCustomers[4].Id },
                new Invoice { InvoiceNo = "INV-0006", InvoiceDate = DateTime.Now.AddDays(-12), DueDate = DateTime.Now.AddDays(18), Subtotal = 900m, Shipping = 40m, GrandTotal = 940m, CustomerId = savedCustomers[5].Id },
                new Invoice { InvoiceNo = "INV-0007", InvoiceDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(20), Subtotal = 600m, Shipping = 35m, GrandTotal = 635m, CustomerId = savedCustomers[6].Id },
                new Invoice { InvoiceNo = "INV-0008", InvoiceDate = DateTime.Now.AddDays(-8), DueDate = DateTime.Now.AddDays(22), Subtotal = 350m, Shipping = 20m, GrandTotal = 370m, CustomerId = savedCustomers[7].Id },
                new Invoice { InvoiceNo = "INV-0009", InvoiceDate = DateTime.Now.AddDays(-5), DueDate = DateTime.Now.AddDays(25), Subtotal = 800m, Shipping = 45m, GrandTotal = 845m, CustomerId = savedCustomers[8].Id },
                new Invoice { InvoiceNo = "INV-0010", InvoiceDate = DateTime.Now.AddDays(-3), DueDate = DateTime.Now.AddDays(27), Subtotal = 1000m, Shipping = 55m, GrandTotal = 1055m, CustomerId = savedCustomers[9].Id }
            };
            context.Invoices.AddRange(invoices);
            await context.SaveChangesAsync();

            // Lisa arveread
            var savedInvoices = context.Invoices.ToList();
            var invoiceLines = new List<InvoiceLine>
            {
                new InvoiceLine { LineItem = "Oil Change", UnitPrice = 50m, Quantity = 1, VatRate = 0.2m, Total = 60m, InvoiceId = savedInvoices[0].Id },
                new InvoiceLine { LineItem = "Tire Replacement", UnitPrice = 120m, Quantity = 2, VatRate = 0.2m, Total = 288m, InvoiceId = savedInvoices[0].Id },
                new InvoiceLine { LineItem = "Brake Service", UnitPrice = 200m, Quantity = 1, VatRate = 0.2m, Total = 240m, InvoiceId = savedInvoices[1].Id },
                new InvoiceLine { LineItem = "Engine Tune-up", UnitPrice = 300m, Quantity = 1, VatRate = 0.2m, Total = 360m, InvoiceId = savedInvoices[2].Id },
                new InvoiceLine { LineItem = "Windshield Repair", UnitPrice = 150m, Quantity = 1, VatRate = 0.2m, Total = 180m, InvoiceId = savedInvoices[3].Id },
                new InvoiceLine { LineItem = "AC Service", UnitPrice = 180m, Quantity = 1, VatRate = 0.2m, Total = 216m, InvoiceId = savedInvoices[4].Id },
                new InvoiceLine { LineItem = "Battery Replacement", UnitPrice = 90m, Quantity = 1, VatRate = 0.2m, Total = 108m, InvoiceId = savedInvoices[5].Id },
                new InvoiceLine { LineItem = "Transmission Service", UnitPrice = 400m, Quantity = 1, VatRate = 0.2m, Total = 480m, InvoiceId = savedInvoices[6].Id },
                new InvoiceLine { LineItem = "Wheel Alignment", UnitPrice = 80m, Quantity = 1, VatRate = 0.2m, Total = 96m, InvoiceId = savedInvoices[7].Id },
                new InvoiceLine { LineItem = "Full Service", UnitPrice = 500m, Quantity = 1, VatRate = 0.2m, Total = 600m, InvoiceId = savedInvoices[8].Id }
            };
            context.InvoiceLines.AddRange(invoiceLines);
            await context.SaveChangesAsync();
        }
    }
}