using System.Linq;
using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public class InvoiceNumberService : IInvoiceNumberService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceNumberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetNextInvoiceNumber()
        {
            var lastInvoice = _context.Invoices
                .OrderByDescending(i => i.Id)
                .FirstOrDefault();

            if (lastInvoice == null)
            {
                return "INV-0001";
            }

            // Parse the number from format "INV-XXXX"
            var parts = lastInvoice.InvoiceNo.Split('-');
            if (parts.Length == 2 && int.TryParse(parts[1], out int lastNumber))
            {
                var nextNumber = lastNumber + 1;
                return $"INV-{nextNumber:D4}";
            }

            return "INV-0001";
        }
    }
}