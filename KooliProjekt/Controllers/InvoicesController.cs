using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KooliProjekt.Data;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ICustomerService _customerService;
        private readonly InvoiceNumberService _invoiceNumberService;

        public InvoicesController(IInvoiceService invoiceService, ICustomerService customerService, InvoiceNumberService invoiceNumberService)
        {
            _invoiceService = invoiceService;
            _customerService = customerService;
            _invoiceNumberService = invoiceNumberService;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _invoiceService.List(page, 5));
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _invoiceService.Get(id.Value);
            if (invoice == null) return NotFound();

            return View(invoice);
        }

        // GET: Invoices/Create
        public async Task<IActionResult> Create()
        {
            var customers = await _customerService.List(1, 1000);
            ViewData["CustomerId"] = new SelectList(customers.Results, "Id", "Name");

            var invoice = new Invoice();
            invoice.InvoiceNo = _invoiceNumberService.GetNextInvoiceNumber();
            invoice.InvoiceDate = DateTime.Now;
            invoice.DueDate = DateTime.Now.AddDays(30);

            return View(invoice);
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvoiceNo,InvoiceDate,DueDate,Subtotal,Shipping,GrandTotal,CustomerId")] Invoice invoice)
        {
            ModelState.Remove("InvoiceNo");
            ModelState.Remove("Customer");
            ModelState.Remove("InvoiceLines");

            if (ModelState.IsValid)
            {
                await _invoiceService.Save(invoice);
                return RedirectToAction(nameof(Index));
            }

            var customers = await _customerService.List(1, 1000);
            ViewData["CustomerId"] = new SelectList(customers.Results, "Id", "Name", invoice.CustomerId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _invoiceService.Get(id.Value);
            if (invoice == null) return NotFound();

            var customers = await _customerService.List(1, 1000);
            ViewData["CustomerId"] = new SelectList(customers.Results, "Id", "Name", invoice.CustomerId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceNo,InvoiceDate,DueDate,Subtotal,Shipping,GrandTotal,CustomerId")] Invoice invoice)
        {
            if (id != invoice.Id) return NotFound();

            ModelState.Remove("Customer");
            ModelState.Remove("InvoiceLines");

            if (ModelState.IsValid)
            {
                await _invoiceService.Save(invoice);
                return RedirectToAction(nameof(Index));
            }

            var customers = await _customerService.List(1, 1000);
            ViewData["CustomerId"] = new SelectList(customers.Results, "Id", "Name", invoice.CustomerId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _invoiceService.Get(id.Value);
            if (invoice == null) return NotFound();

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}