using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProArch.CodingTest.External;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.Suppliers;

namespace ProArch.CodingTest.Summary
{
	public class SpendService : ISpendService
	{
		private readonly ISupplierService _supplierService;
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly IExternalInvoiceProcessor _externalInvoiceProcessor;

		public SpendService(ISupplierService supplierService,
			IInvoiceRepository invoiceRepository,
			IExternalInvoiceProcessor externalInvoiceProcessor)
		{
			_supplierService = supplierService;
			_invoiceRepository = invoiceRepository;
			_externalInvoiceProcessor = externalInvoiceProcessor;
		}
		public SpendSummary GetTotalSpend(int supplierId)
		{
			var spendSummary = new SpendSummary();

			var supplier = _supplierService.GetById(supplierId);
			if (!supplier.IsExternal)
			{
				var invoices = _invoiceRepository.Get();
				var spendDetails = from invoice in invoices
								   group invoice by invoice.InvoiceDate.Year into g
								   select new SpendDetail()
								   {
									   TotalSpend = g.Sum(x => x.Amount),
									   Year = g.Key
								   };
				spendSummary.Name = supplier.Name;
				spendSummary.Years = spendDetails.ToList();
			}
			else
			{
				var externalInvoices = _externalInvoiceProcessor.Process(supplierId);

				var spendDetails = from invoice in externalInvoices
								   group invoice by invoice.Year into g
								   select new SpendDetail()
								   {
									   TotalSpend = g.Sum(x => x.TotalAmount),
									   Year = g.Key
								   };
				spendSummary.Name = supplier.Name;
				spendSummary.Years = spendDetails.ToList();
			}
			return spendSummary;
		}

		
	}
}
