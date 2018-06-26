using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProArch.CodingTest.Summary;

namespace ProArch.CodingTest.UnitTests.TestDataBuilders
{
	public class SpendSummaryBuilder
	{
		public string Name = "SupplierName";

		public List<SpendDetail> Years { get; set; }

		public SpendSummary Build()
		{
			return new SpendSummary()
			{
				
				Name = Name,
				
			};
		}

		public static implicit operator SpendSummary(SpendSummaryBuilder instance)
		{
			return instance.Build();
		}
	}
}
