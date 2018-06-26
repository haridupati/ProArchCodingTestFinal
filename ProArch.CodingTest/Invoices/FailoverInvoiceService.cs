using System;
using ProArch.CodingTest.External;

namespace ProArch.CodingTest.Invoices
{
    public class FailoverInvoiceService : IFailoverInvoiceService
	{
        public FailoverInvoiceCollection GetInvoices(int supplierId)
        {
	       return new FailoverInvoiceCollection();
        }
    }
}
