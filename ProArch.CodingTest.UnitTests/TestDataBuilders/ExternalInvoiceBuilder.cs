using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProArch.CodingTest.External;
using ProArch.CodingTest.Invoices;

namespace ProArch.CodingTest.UnitTests.TestDataBuilders
{
	public class ExternalInvoiceBuilder
	{
		public decimal TotalAmount { get; set; }

		public int Year { get; set; }

		public ExternalInvoice Build()
		{
			return new ExternalInvoice()
			{
				Year = Year,
				TotalAmount = TotalAmount
			};
		}
		public ExternalInvoice[] SampleExternalInvoices()
		{
			var externalInvoices = new ExternalInvoice[]
			{
				new ExternalInvoice()
				{
					Year = 1999,
					TotalAmount = 10
				},
				new ExternalInvoice()
				{
					Year = 1999,
					TotalAmount = 15
				}
			};
			return externalInvoices;
		}

		public static implicit operator ExternalInvoice(ExternalInvoiceBuilder instance)
		{
			return instance.Build();
		}
	}
}
