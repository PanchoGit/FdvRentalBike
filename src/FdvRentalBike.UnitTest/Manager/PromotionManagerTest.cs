using FdvRentalBike.Domain;
using FdvRentalBike.Workflow.Managers;
using System.Collections.Generic;
using Xunit;

namespace FdvRentalBike.UnitTest.Manager
{
    public class PromotionManagerTest
    {
        private PromotionManager sut;

        public PromotionManagerTest()
        {
            sut = new PromotionManager();
        }

        [Fact]
        public void ShouldPassApplyDiscountNonePromo()
        {
            var customer = GetCustomerService(RentalType.Day, 2);
            customer.Promotion = Promotion.none;

            var total = 10;

            sut.ApplyDiscount(customer, ref total);

            Assert.Equal(10, total);
        }

        [Theory]
        [InlineData(2, 100, 100)]
        [InlineData(3, 100, 70)]
        [InlineData(4, 100, 70)]
        [InlineData(5, 100, 70)]
        [InlineData(6, 100, 100)]
        public void ShouldPassApplyDiscountFamilyPromo(int countService, int totalCharge, int expectedTotal)
        {
            var customer = new CustomerService { Promotion = Promotion.family };
            customer.RentServices = new List<RentService>();
            for (int i = 0; i < countService; i++)
            {
                customer.RentServices.Add(new RentService
                {
                    RentalType = RentalType.Hour,
                    Total = 1
                });
            }

            var total = totalCharge;

            sut.ApplyDiscount(customer, ref total);

            Assert.Equal(expectedTotal, total);
        }

        [Theory]
        [InlineData(2, 100, 100)]
        [InlineData(3, 100, 100)]
        [InlineData(4, 100, 100)]
        [InlineData(5, 100, 100)]
        [InlineData(6, 100, 100)]
        public void ShouldFailApplyDiscountFamilyPromo(int countService, int totalCharge, int expectedTotal)
        {
            var customer = new CustomerService { Promotion = Promotion.none };
            customer.RentServices = new List<RentService>();
            for (int i = 0; i < countService; i++)
            {
                customer.RentServices.Add(new RentService
                {
                    RentalType = RentalType.Hour,
                    Total = 1
                });
            }

            var total = totalCharge;

            sut.ApplyDiscount(customer, ref total);

            Assert.Equal(expectedTotal, total);
        }

        private CustomerService GetCustomerService(RentalType rentalType, int total)
        {
            return new CustomerService
            {
                RentServices = new List<RentService>
                {
                    new RentService
                    {
                        RentalType = rentalType,
                        Total = total
                    }
                }
            };
        }
    }
}
