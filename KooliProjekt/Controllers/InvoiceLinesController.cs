using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class InvoiceLinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceLinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InvoiceLines
        public async Task<IActionResult> Index(int page = 1)
        {
            var invoiceLines = _context.InvoiceLines.Include(i => i.Invoice);
            return View(await invoiceLines.GetPagedAsync(page, 5));
        }

        // GET: InvoiceLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceLine = await _context.InvoiceLines
                .Include(i => i.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceLine == null)
            {
                return NotFound();
            }

            return View(invoiceLine);
        }

        // GET: InvoiceLines/Create
        public IActionResult Create()
        {
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "InvoiceNo");

            var invoiceLine = new InvoiceLine();
            invoiceLine.Quantity = 1;
            invoiceLine.VatRate = 0.2m;

            return View(invoiceLine);
        }

        // POST: InvoiceLines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LineItem,UnitPrice,Quantity,VatRate,Total,InvoiceId")] InvoiceLine invoiceLine)
        {
            ModelState.Remove("Invoice");

            if (ModelState.IsValid)
            {
                _context.Add(invoiceLine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "InvoiceNo", invoiceLine.InvoiceId);
            return View(invoiceLine);
        }

        // GET: InvoiceLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceLine = await _context.InvoiceLines.FindAsync(id);
            if (invoiceLine == null)
            {
                return NotFound();
            }

            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "InvoiceNo", invoiceLine.InvoiceId);
            return View(invoiceLine);
        }

        // POST: InvoiceLines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LineItem,UnitPrice,Quantity,VatRate,Total,InvoiceId")] InvoiceLine invoiceLine)
        {
            if (id != invoiceLine.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Invoice");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceLineExists(invoiceLine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "InvoiceNo", invoiceLine.InvoiceId);
            return View(invoiceLine);
        }

        // GET: InvoiceLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceLine = await _context.InvoiceLines
                .Include(i => i.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceLine == null)
            {
                return NotFound();
            }

            return View(invoiceLine);
        }

        // POST: InvoiceLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceLine = await _context.InvoiceLines.FindAsync(id);
            if (invoiceLine != null)
            {
                _context.InvoiceLines.Remove(invoiceLine);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceLineExists(int id)
        {
            return _context.InvoiceLines.Any(e => e.Id == id);
        }
    }
}