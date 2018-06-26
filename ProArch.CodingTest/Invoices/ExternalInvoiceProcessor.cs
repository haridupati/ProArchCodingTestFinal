using System;
using System.Threading;
using ProArch.CodingTest.External;

namespace ProArch.CodingTest.Invoices
{
	public class ExternalInvoiceProcessor : IExternalInvoiceProcessor
	{
		private readonly IExternalInvoiceServiceFacade _externalInvoiceServiceFacade;
		private readonly IFailoverInvoiceService _failoverInvoiceService;

		public ExternalInvoiceProcessor(
			IExternalInvoiceServiceFacade externalInvoiceServiceFacade,
			IFailoverInvoiceService failoverInvoiceService)
		{
			_externalInvoiceServiceFacade = externalInvoiceServiceFacade;
			_failoverInvoiceService = failoverInvoiceService;
		}
		public ExternalInvoice[] Process(int supplierId)
		{
			ExternalInvoice[] externalInvoices = new ExternalInvoice[] { };

			int attempted = 0;
			while (attempted < 3)
			{
				if (attempted > 0)
				{
					Thread.Sleep(1000);
				}

				try
				{
					externalInvoices = _externalInvoiceServiceFacade.GetInvoices(supplierId.ToString());
					return externalInvoices;
				}
				catch (TimeoutException)
				{
					attempted++;
				}
				catch (Exception)
				{
					externalInvoices = getFailoverInvoiceCollection(supplierId);
					return externalInvoices;
				}

				if (attempted == 3)
				{
					externalInvoices = getFailoverInvoiceCollection(supplierId);
				}
			}

			return externalInvoices;
		}

		private ExternalInvoice[] getFailoverInvoiceCollection(int supplierId)
		{
			ExternalInvoice[] externalInvoices;
			var failoverInvoiceCollection = _failoverInvoiceService.GetInvoices(supplierId);
			externalInvoices = failoverInvoiceCollection.Invoices;
			if (failoverInvoiceCollection.IsOldData)
			{
				throw new Exception("Failover data is quite old");
			}

			return externalInvoices;
		}
	}
}