using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KooliProjekt.Data;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class InvoiceLinesController : Controller
    {
        private readonly IInvoiceLineService _invoiceLineService;
        private readonly IInvoiceService _invoiceService;

        public InvoiceLinesController(IInvoiceLineService invoiceLineService, IInvoiceService invoiceService)
        {
            _invoiceLineService = invoiceLineService;
            _invoiceService = invoiceService;
        }

        // GET: InvoiceLines
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _invoiceLineService.List(page, 5));
        }

        // GET: InvoiceLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var invoiceLine = await _invoiceLineService.Get(id.Value);
            if (invoiceLine == null) return NotFound();

            return View(invoiceLine);
        }

        // GET: InvoiceLines/Create
        public async Task<IActionResult> Create()
        {
            var invoices = await _invoiceService.List(1, 1000);
            ViewData["InvoiceId"] = new SelectList(invoices.Results, "Id", "InvoiceNo");

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
                await _invoiceLineService.Save(invoiceLine);
                return RedirectToAction(nameof(Index));
            }

            var invoices = await _invoiceService.List(1, 1000);
            ViewData["InvoiceId"] = new SelectList(invoices.Results, "Id", "InvoiceNo", invoiceLine.InvoiceId);
            return View(invoiceLine);
        }

        // GET: InvoiceLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var invoiceLine = await _invoiceLineService.Get(id.Value);
            if (invoiceLine == null) return NotFound();

            var invoices = await _invoiceService.List(1, 1000);
            ViewData["InvoiceId"] = new SelectList(invoices.Results, "Id", "InvoiceNo", invoiceLine.InvoiceId);
            return View(invoiceLine);
        }

        // POST: InvoiceLines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LineItem,UnitPrice,Quantity,VatRate,Total,InvoiceId")] InvoiceLine invoiceLine)
        {
            if (id != invoiceLine.Id) return NotFound();

            ModelState.Remove("Invoice");

            if (ModelState.IsValid)
            {
                await _invoiceLineService.Save(invoiceLine);
                return RedirectToAction(nameof(Index));
            }

            var invoices = await _invoiceService.List(1, 1000);
            ViewData["InvoiceId"] = new SelectList(invoices.Results, "Id", "InvoiceNo", invoiceLine.InvoiceId);
            return View(invoiceLine);
        }

        // GET: InvoiceLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var invoiceLine = await _invoiceLineService.Get(id.Value);
            if (invoiceLine == null) return NotFound();

            return View(invoiceLine);
        }

        // POST: InvoiceLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceLineService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}