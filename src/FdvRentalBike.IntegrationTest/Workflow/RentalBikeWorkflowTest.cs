using System.Collections.Generic;
using FdvRentalBike.Domain;
using FdvRentalBike.Workflow;
using FdvRentalBike.Workflow.Managers;
using Xunit;

namespace FdvRentalBike.IntegrationTest.Workflow
{
    public class RentalBikeWorkflowTest
    {
        private RentalBikeWorkflow sut;

        public RentalBikeWorkflowTest()
        {
            sut = new RentalBikeWorkflow(new PromotionManager());
        }

        [Theory]
        [InlineData(RentalType.Hour, 1, 5)]
        [InlineData(RentalType.Hour, 2, 10)]
        [InlineData(RentalType.Hour, 3, 15)]
        [InlineData(RentalType.Day, 1, 20)]
        [InlineData(RentalType.Day, 2, 40)]
        [InlineData(RentalType.Day, 3, 60)]
        [InlineData(RentalType.Week, 1, 60)]
        [InlineData(RentalType.Week, 2, 120)]
        [InlineData(RentalType.Week, 3, 180)]
        public void ShouldPassRentByHour(RentalType rentalType, int total, int expectedCharge)
        {
            var customerService = GetCustomerService(rentalType, total);

            var actual = sut.Save(customerService);

            Assert.False(actual.HasErrors);

            var actualResult = ((CustomerService)actual.ResultData).TotalCharge;

            Assert.Equal(expectedCharge, actualResult);
        }

        [Fact]
        public void ShouldPassFamilyRent()
        {
            var customerService = new CustomerService
            {
                Promotion = Promotion.family,
                RentServices = new List<RentService>
                {
                    new RentService
                    {
                        RentalType = RentalType.Day, Total = 1
                    },
                    new RentService
                    {
                        RentalType = RentalType.Day, Total = 1
                    },
                    new RentService
                    {
                        RentalType = RentalType.Day, Total = 1
                    }
                }
            };

            int actualResult = 0;

            var actual = sut.Save(customerService);

            Assert.False(actual.HasErrors);

            actualResult = ((CustomerService)actual.ResultData).TotalCharge;

            Assert.Equal(42, actualResult);
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
