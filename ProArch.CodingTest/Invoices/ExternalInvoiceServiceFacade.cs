using System;
using ProArch.CodingTest.External;

namespace ProArch.CodingTest.Invoices
{
	public class ExternalInvoiceServiceFacade : IExternalInvoiceServiceFacade
	{
		public ExternalInvoice[] GetInvoices(string supplierId)
		{
			var externalInvoicesOriginal = ExternalInvoiceService.GetInvoices(supplierId);

			return externalInvoicesOriginal;
		}
	}
}