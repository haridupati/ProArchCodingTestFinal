using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProArch.CodingTest.Suppliers;

namespace ProArch.CodingTest.UnitTests.TestDataBuilders
{
	public class SupplierBuilder
	{
		public int Id = 1;
		public string Name = "Supplier1";
		public bool IsExternal = false;
		public Supplier Build()
		{
			return new Supplier()
			{
				Id = Id,
				Name = Name,
				IsExternal = IsExternal
			};
		}

		public SupplierBuilder WithInternalSupplier()
		{
			IsExternal = false;
			return this;
		}
		public SupplierBuilder WithExternalSupplier()
		{
			IsExternal = true;
			return this;
		}

		public SupplierBuilder WithName(string name)
		{
			Name = name;
			return this;
		}

		public static implicit operator Supplier(SupplierBuilder instance)
		{
			return instance.Build();
		}
	}
}
