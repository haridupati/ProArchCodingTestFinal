using System.Collections.Generic;
using System.Text;
using ProArch.CodingTest.External;

namespace ProArch.CodingTest.Invoices
{

    public interface IExternalInvoiceProcessor
    {
	   ExternalInvoice[] Process(int supplied);
    }
}
