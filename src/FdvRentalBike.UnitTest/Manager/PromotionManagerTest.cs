using FdvRentalBike.Domain;
using FdvRentalBike.Workflow.Managers;
using NUnit.Framework;
using System.Collections.Generic;

namespace FdvRentalBike.UnitTest.Manager
{
    [TestFixture]
    public class PromotionManagerTest
    {
        private PromotionManager sut;

        [SetUp]
        public void Setup()
        {
            sut = new PromotionManager();
        }

        [Test]
        public void ShouldPassApplyDiscountNonePromo()
        {
            var customer = GetCustomerService(RentalType.Day, 2);
            customer.Promotion = Promotion.none;

            var total = 10;

            sut.ApplyDiscount(customer, ref total);

            Assert.AreEqual(10, total);
        }

        [TestCase(2, 100, 100)]
        [TestCase(3, 100, 70)]
        [TestCase(4, 100, 70)]
        [TestCase(5, 100, 70)]
        [TestCase(6, 100, 100)]
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

            Assert.AreEqual(expectedTotal, total);
        }

        [TestCase(2, 100, 100)]
        [TestCase(3, 100, 100)]
        [TestCase(4, 100, 100)]
        [TestCase(5, 100, 100)]
        [TestCase(6, 100, 100)]
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

            Assert.AreEqual(expectedTotal, total);
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
