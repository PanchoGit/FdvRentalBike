using FdvRentalBike.Common;
using FdvRentalBike.Domain;

namespace FdvRentalBike.Workflow.Interfaces
{
    public interface IRentalBikeWorkflow
    {
        Result Save(CustomerService customerService);
    }
}
