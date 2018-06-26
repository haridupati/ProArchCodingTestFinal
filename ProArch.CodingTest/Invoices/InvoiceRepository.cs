using System;
using System.Collections.Generic;
using System.Linq;

namespace ProArch.CodingTest.Invoices
{
    public class InvoiceRepository : IInvoiceRepository
	{
	    readonly List<Invoice> _invoices;

	    public InvoiceRepository()
	    {
			_invoices = new List<Invoice>();
	    }
		public IQueryable<Invoice> Get()
        {
            return _invoices.AsQueryable();
        }
    }
}
