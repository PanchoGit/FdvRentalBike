using FdvRentalBike.Domain;

namespace FdvRentalBike.Workflow.Managers.Interfaces
{
    public interface IPromotionManager
    {
        void ApplyDiscount(CustomerService customerService, ref int totalCharge);
    }
}
