using System.Collections.Generic;

namespace FdvRentalBike.Domain
{
    public class CustomerService
    {
        public Customer Customer { get; set; }

        public Promotion Promotion { get; set; }

        public List<RentService> RentServices { get; set; }

        public int TotalCharge { get; set; }
    }
}
