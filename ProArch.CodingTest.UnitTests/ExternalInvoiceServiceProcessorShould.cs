using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ProArch.CodingTest.External;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.UnitTests.TestDataBuilders;
using Xunit;

namespace ProArch.CodingTest.UnitTests
{
	public class ExternalInvoiceServiceProcessorShould
	{
		[Fact]
		public void InvokeFailOverInvoiceService_After_3Consecutives_ExternalInvoiceService_TimeoutException()
		{
			Mock<IExternalInvoiceServiceFacade> externalInvoiceFacadeMock = new Mock<IExternalInvoiceServiceFacade>();
			Mock<IFailoverInvoiceService> failoverInvoiceServiceMock = new Mock<IFailoverInvoiceService>();

			externalInvoiceFacadeMock.SetupSequence(x => x.GetInvoices(It.IsAny<string>()))
				.Returns(() => throw new TimeoutException())
				.Returns(() => throw new TimeoutException())
				.Returns(() => throw new TimeoutException());


			failoverInvoiceServiceMock.Setup(x => x.GetInvoices(It.IsAny<int>()))
				.Returns(new FailoverInvoiceCollectionBuilder().WithLessThan30DaysTimestamp().Build());


			var sut = new ExternalInvoiceProcessor(externalInvoiceFacadeMock.Object, failoverInvoiceServiceMock.Object);

			sut.Process(1);
			failoverInvoiceServiceMock.Verify(x => x.GetInvoices(It.IsAny<int>()));

		}

		[Fact]
		public void InvokeFailOverInvoiceService_On_ExternalInvoiceServiceException()
		{
			Mock<IExternalInvoiceServiceFacade> externalInvoiceFacadeMock = new Mock<IExternalInvoiceServiceFacade>();
			Mock<IFailoverInvoiceService> failoverInvoiceServiceMock = new Mock<IFailoverInvoiceService>();

			externalInvoiceFacadeMock.SetupSequence(x => x.GetInvoices(It.IsAny<string>()))
				.Returns(() => throw new Exception());

			failoverInvoiceServiceMock.Setup(x => x.GetInvoices(It.IsAny<int>()))
				.Returns(new FailoverInvoiceCollectionBuilder().WithLessThan30DaysTimestamp().Build());


			var sut = new ExternalInvoiceProcessor(externalInvoiceFacadeMock.Object, failoverInvoiceServiceMock.Object);

			sut.Process(1);
			failoverInvoiceServiceMock.Verify(x => x.GetInvoices(It.IsAny<int>()));

		}

		[Fact]
		public void ThrowExceptionFor_FailOverInvoiceData_OlderThan30Days()
		{
			Mock<IExternalInvoiceServiceFacade> externalInvoiceFacadeMock = new Mock<IExternalInvoiceServiceFacade>();
			Mock<IFailoverInvoiceService> failoverInvoiceServiceMock = new Mock<IFailoverInvoiceService>();

			externalInvoiceFacadeMock.SetupSequence(x => x.GetInvoices(It.IsAny<string>()))
				.Returns(() => throw new Exception());

			failoverInvoiceServiceMock.Setup(x => x.GetInvoices(It.IsAny<int>()))
				.Returns(new FailoverInvoiceCollectionBuilder().WithOlderThan30DaysTimestamp().Build());


			var sut = new ExternalInvoiceProcessor(externalInvoiceFacadeMock.Object, failoverInvoiceServiceMock.Object);

			Exception ex = Assert.Throws<Exception>(() => sut.Process(1));
			Assert.Equal("Failover data is quite old", ex.Message);
			failoverInvoiceServiceMock.Verify(x => x.GetInvoices(It.IsAny<int>()));
		}
	}
}
