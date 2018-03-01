using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public class EFSupplyingsRepository : ISupplyProduct
    {
        private ApplicationDbContext context;
        public EFSupplyingsRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<SupplyProductParameter> SupplyProductParameters => context.SupplyProductParameters.AsQueryable();

        public SupplyProductParameter GetParameterById(int parameterId)
        {
            return SupplyProductParameters.FirstOrDefault(s => s.Id == parameterId);
        }

        public void SaveParameter(SupplyProductParameter supplyProductParameter)
        {
            if (supplyProductParameter.Id == 0) //add new
            {
                context.SupplyProductParameters.Add(supplyProductParameter);
            } else //edit existing
            {
                var currentParameter = GetParameterById(supplyProductParameter.Id);
                if (currentParameter != null)
                {
                    currentParameter.SafetyRatio = supplyProductParameter.SafetyRatio;
                    currentParameter.SupplyFrequency = supplyProductParameter.SupplyFrequency;
                    currentParameter.TimeToFormSupply = supplyProductParameter.TimeToFormSupply;
                }
            }
            context.SaveChanges();
        }
    }
}
