using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProArch.CodingTest.Invoices;

namespace ProArch.CodingTest.UnitTests.TestDataBuilders
{
	public class InvoiceBuilder
	{
		public int SupplierId { get; set; }

		public DateTime InvoiceDate { get; set; }

		public decimal Amount { get; set; }

		public Invoice Build()
		{
			return new Invoice()
			{
				SupplierId = SupplierId,
				InvoiceDate = InvoiceDate,
				Amount = Amount
			};
		}

		public InvoiceBuilder WithSupplierId(int supplierId)
		{
			SupplierId = supplierId;
			return this;
		}
		public InvoiceBuilder WithInvoiceDate(DateTime invoiceDate)
		{
			InvoiceDate = invoiceDate;
			return this;
		}

		public List<Invoice> SampleInvoices()
		{
			var invoices = new List<Invoice>()
			{
				new Invoice
				{
					SupplierId = 1,
					Amount = 100,
					InvoiceDate = new DateTime(2010, 01, 01)
				},
				new Invoice
				{
					SupplierId = 1,
					Amount = 200,
					InvoiceDate = new DateTime(2011, 01, 01)
				},
				new Invoice
				{
					SupplierId = 1,
					Amount = 100,
					InvoiceDate = new DateTime(2010, 05, 01)
				},
				new Invoice
				{
					SupplierId = 1,
					Amount = 200,
					InvoiceDate = new DateTime(2011, 07, 01)
				}
			};
			return invoices;
		}
		public InvoiceBuilder WithAmount(decimal amount)
		{
			Amount = amount;
			return this;
		}

		public static implicit operator Invoice(InvoiceBuilder instance)
		{
			return instance.Build();
		}
	}
}
