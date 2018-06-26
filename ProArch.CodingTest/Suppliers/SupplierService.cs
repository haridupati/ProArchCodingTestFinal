using System.Collections.Generic;
using System.Linq;

namespace ProArch.CodingTest.Suppliers
{
    public class SupplierService : ISupplierService
	{
		private List<Supplier> _suppliers;

	    public SupplierService()
	    {
		   _suppliers = new List<Supplier>();
	    }
        public Supplier GetById(int id)
        {
	        return _suppliers.FirstOrDefault(x => x.Id == id);
        }
    }
}
