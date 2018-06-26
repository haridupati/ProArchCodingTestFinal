using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProArch.CodingTest.External;
using ProArch.CodingTest.Invoices;

namespace ProArch.CodingTest.UnitTests.TestDataBuilders
{
	

	public class FailoverInvoiceCollectionBuilder
	{
		public DateTime Timestamp = DateTime.Now.AddDays(-10);
		public ExternalInvoice[] Invoices { get; set; }
		public bool IsOldData = false;
		public FailoverInvoiceCollection Build()
		{
			return new FailoverInvoiceCollection()
			{
				Timestamp = Timestamp,
				IsOldData = IsOldData,
				Invoices = new ExternalInvoice[]
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
				}
			};
		}
		public FailoverInvoiceCollectionBuilder WithOlderThan30DaysTimestamp()
		{
			Timestamp = DateTime.Now.AddDays(-45);
			return this;
		}

		public FailoverInvoiceCollectionBuilder WithLessThan30DaysTimestamp()
		{
			Timestamp = DateTime.Now.AddDays(-15);
			return this;
		}

		public FailoverInvoiceCollectionBuilder WithOldData()
		{
			IsOldData = true;
			return this;
		}

		public static implicit operator FailoverInvoiceCollection(FailoverInvoiceCollectionBuilder instance)
		{
			return instance.Build();
		}
	}
}
