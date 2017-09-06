using FdvRentalBike.Domain;
using FdvRentalBike.Workflow.Managers.Interfaces;

namespace FdvRentalBike.Workflow.Managers
{
    public class PromotionManager : IPromotionManager
    {
        const int FamilyPercentDiscount = 30;
        const int FamilyPromoServiceInterval1 = 3;
        const int FamilyPromoServiceInterval2 = 5;

        public void ApplyDiscount(CustomerService customerService, ref int totalCharge)
        {
            if (IsFamilyPromotion(customerService))
            {
                totalCharge = totalCharge - totalCharge * FamilyPercentDiscount / 100;
            }
        }

        private bool IsFamilyPromotion(CustomerService customerService)
        {
            if (customerService.Promotion != Promotion.family) return false;

            var serviceCount = customerService.RentServices.Count;

            return serviceCount >= FamilyPromoServiceInterval1
                && serviceCount <= FamilyPromoServiceInterval2;
        }
    }
}
