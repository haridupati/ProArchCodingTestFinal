using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.Summary;
using ProArch.CodingTest.Suppliers;
using ProArch.CodingTest.UnitTests.TestDataBuilders;
using Xunit;

namespace ProArch.CodingTest.UnitTests
{
	public class SpendServiceShould
	{
		
		[Fact]
		public void InvokeInvoiceRepository_For_InternalSupplier()
		{
			Mock<ISupplierService> supplierServiceMock = new Mock<ISupplierService>();
			Mock<IInvoiceRepository> invoiceRepositoryMock = new Mock<IInvoiceRepository>();
			Mock<IExternalInvoiceProcessor> externalInvoiceServiceProcessorMock = new Mock<IExternalInvoiceProcessor>();
			Mock<IExternalInvoiceServiceFacade> externalInvoiceSericeFacadeMock = new Mock<IExternalInvoiceServiceFacade>();

			var sut = new SpendService(supplierServiceMock.Object, invoiceRepositoryMock.Object,
				externalInvoiceServiceProcessorMock.Object);

			supplierServiceMock.SetupAllProperties();

			supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(new Supplier() {Id = 1, IsExternal = false, Name = "Internal Supplier"});
			sut.GetTotalSpend(1);

			invoiceRepositoryMock.Verify(x=>x.Get(), Times.Once);
			externalInvoiceServiceProcessorMock.Verify(x=>x.Process(It.IsAny<int>()), Times.Never);
		}

		[Fact]
		public void InvokeExternalInvoiceService_For_ExternalSupplier()
		{
			Mock<ISupplierService> supplierServiceMock = new Mock<ISupplierService>();
			Mock<IInvoiceRepository> invoiceRepositoryMock = new Mock<IInvoiceRepository>();
			Mock<IExternalInvoiceProcessor> externalInvoiceServiceProcessorMock = new Mock<IExternalInvoiceProcessor>();
			Mock<IExternalInvoiceServiceFacade> externalInvoiceSericeFacadeMock = new Mock<IExternalInvoiceServiceFacade>();

			var sut = new SpendService(supplierServiceMock.Object, invoiceRepositoryMock.Object,
				externalInvoiceServiceProcessorMock.Object);

			supplierServiceMock.SetupAllProperties();

			supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(new Supplier() { Id = 1, IsExternal = true, Name = "Internal Supplier" });
			sut.GetTotalSpend(1);

			invoiceRepositoryMock.Verify(x => x.Get(), Times.Never);
			externalInvoiceServiceProcessorMock.Verify(x => x.Process(It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public void Calculate_TotalSpend_GroupedByYears_ForInternalSupplier()
		{
			Mock<ISupplierService> supplierServiceMock = new Mock<ISupplierService>();
			Mock<IInvoiceRepository> invoiceRepositoryMock = new Mock<IInvoiceRepository>();
			Mock<IExternalInvoiceProcessor> externalInvoiceServiceProcessorMock = new Mock<IExternalInvoiceProcessor>();
			Mock<IExternalInvoiceServiceFacade> externalInvoiceSericeFacadeMock = new Mock<IExternalInvoiceServiceFacade>();

			var sut = new SpendService(supplierServiceMock.Object, invoiceRepositoryMock.Object,
				externalInvoiceServiceProcessorMock.Object);

			supplierServiceMock.SetupAllProperties();
			supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(new SupplierBuilder().WithInternalSupplier().WithName("InternalSupplier").Build());
			invoiceRepositoryMock.Setup(x => x.Get()).Returns(new InvoiceBuilder().SampleInvoices().AsQueryable);


			var spendSummary = sut.GetTotalSpend(1);
			Assert.NotNull(spendSummary);
			Assert.Equal("InternalSupplier", spendSummary.Name);
			Assert.Equal(2,spendSummary.Years.Count);
			Assert.Equal(2010, spendSummary.Years[0].Year);
			Assert.Equal(200, spendSummary.Years[0].TotalSpend);
			Assert.Equal(2011, spendSummary.Years[1].Year);
			Assert.Equal(400, spendSummary.Years[1].TotalSpend);

			invoiceRepositoryMock.Verify(x => x.Get(), Times.Once);
			externalInvoiceServiceProcessorMock.Verify(x => x.Process(It.IsAny<int>()), Times.Never);
		}

		[Fact]
		public void Calculate_TotalSpend_GroupedByYears_ForExternalSupplier()
		{
			Mock<ISupplierService> supplierServiceMock = new Mock<ISupplierService>();
			Mock<IInvoiceRepository> invoiceRepositoryMock = new Mock<IInvoiceRepository>();
			Mock<IExternalInvoiceProcessor> externalInvoiceServiceProcessorMock = new Mock<IExternalInvoiceProcessor>();
			Mock<IExternalInvoiceServiceFacade> externalInvoiceSericeFacadeMock = new Mock<IExternalInvoiceServiceFacade>();
			Mock<IFailoverInvoiceService> failOverInvoiceServiceMock = new Mock<IFailoverInvoiceService>();

			
			var sut = new SpendService(supplierServiceMock.Object, invoiceRepositoryMock.Object,
				new ExternalInvoiceProcessor(externalInvoiceSericeFacadeMock.Object,failOverInvoiceServiceMock.Object));

			
			supplierServiceMock.SetupAllProperties();
			supplierServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(new SupplierBuilder().WithExternalSupplier().WithName("ExternalSupplier").Build());
			externalInvoiceSericeFacadeMock.Setup(x => x.GetInvoices(It.IsAny<string>())).Returns(new ExternalInvoiceBuilder().SampleExternalInvoices());


			var spendSummary = sut.GetTotalSpend(1);

			invoiceRepositoryMock.Verify(x => x.Get(), Times.Never);
			externalInvoiceSericeFacadeMock.Verify(x => x.GetInvoices(It.IsAny<string>()), Times.AtLeastOnce);
		}
	}
}
