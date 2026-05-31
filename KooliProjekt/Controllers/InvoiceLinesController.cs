using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KooliProjekt.Data;
using KooliProjekt.Models;
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

        public async Task<IActionResult> Index(int page = 1, InvoiceLinesIndexModel model = null)
        {
            model = model ?? new InvoiceLinesIndexModel();
            model.Data = await _invoiceLineService.List(page, 5, model.Search);
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var invoiceLine = await _invoiceLineService.Get(id.Value);
            if (invoiceLine == null) return NotFound();
            return View(invoiceLine);
        }

        public async Task<IActionResult> Create()
        {
            var invoices = await _invoiceService.List(1, 1000);
            ViewData["InvoiceId"] = new SelectList(invoices.Results, "Id", "InvoiceNo");

            var invoiceLine = new InvoiceLine();
            invoiceLine.Quantity = 1;
            invoiceLine.VatRate = 0.2m;

            return View(invoiceLine);
        }

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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var invoiceLine = await _invoiceLineService.Get(id.Value);
            if (invoiceLine == null) return NotFound();

            var invoices = await _invoiceService.List(1, 1000);
            ViewData["InvoiceId"] = new SelectList(invoices.Results, "Id", "InvoiceNo", invoiceLine.InvoiceId);
            return View(invoiceLine);
        }

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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var invoiceLine = await _invoiceLineService.Get(id.Value);
            if (invoiceLine == null) return NotFound();
            return View(invoiceLine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceLineService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}