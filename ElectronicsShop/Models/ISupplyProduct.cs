using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public interface ISupplyProduct
    {
        IQueryable<SupplyProductParameter> SupplyProductParameters { get; }
        void SaveParameter (SupplyProductParameter supplyProductParameter);
        SupplyProductParameter GetParameterById(int parameterId);
    }
}
