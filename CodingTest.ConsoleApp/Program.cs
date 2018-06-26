using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.Summary;
using ProArch.CodingTest.Suppliers;

namespace CodingTest.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			SpendService spendService = new SpendService(
				new SupplierService(), 
				new InvoiceRepository(),
				new ExternalInvoiceProcessor(new ExternalInvoiceServiceFacade(), new FailoverInvoiceService()));

			var spendSummary = spendService.GetTotalSpend(1);
			var spendSummary2 = spendService.GetTotalSpend(2);
			Console.ReadLine();
		}
	}
}
