using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ICustomerService _customerService;
        private readonly IInvoiceNumberService _invoiceNumberService;

        public InvoicesController(IInvoiceService invoiceService, ICustomerService customerService, IInvoiceNumberService invoiceNumberService)
        {
            _invoiceService = invoiceService;
            _customerService = customerService;
            _invoiceNumberService = invoiceNumberService;
        }

        public async Task<IActionResult> Index(int page = 1, InvoicesIndexModel model = null)
        {
            model = model ?? new InvoicesIndexModel();
            model.Data = await _invoiceService.List(page, 5, model.Search);
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var invoice = await _invoiceService.Get(id.Value);
            if (invoice == null) return NotFound();
            return View(invoice);
        }

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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var invoice = await _invoiceService.Get(id.Value);
            if (invoice == null) return NotFound();

            var customers = await _customerService.List(1, 1000);
            ViewData["CustomerId"] = new SelectList(customers.Results, "Id", "Name", invoice.CustomerId);
            return View(invoice);
        }

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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var invoice = await _invoiceService.Get(id.Value);
            if (invoice == null) return NotFound();
            return View(invoice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoiceService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}