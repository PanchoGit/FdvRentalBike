using System.Collections.Generic;
using FdvRentalBike.Domain;
using FdvRentalBike.Workflow;
using FdvRentalBike.Workflow.Managers.Interfaces;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace FdvRentalBike.UnitTest.Workflow
{
    [TestFixture]
    public class RentalBikeWorkflowTest
    {
        private Mock<IPromotionManager> promotionManagerMock;

        private RentalBikeWorkflow sut;

        [SetUp]
        public void Setup()
        {
            promotionManagerMock = new Mock<IPromotionManager>();

            sut = new RentalBikeWorkflow(promotionManagerMock.Object);
        }

        [TestCase(RentalType.Hour, 1, 5)]
        [TestCase(RentalType.Hour, 2, 10)]
        [TestCase(RentalType.Hour, 3, 15)]
        [TestCase(RentalType.Day, 1, 20)]
        [TestCase(RentalType.Day, 2, 40)]
        [TestCase(RentalType.Day, 3, 60)]
        [TestCase(RentalType.Week, 1, 60)]
        [TestCase(RentalType.Week, 2, 120)]
        [TestCase(RentalType.Week, 3, 180)]
        public void ShouldPassRentByHour(RentalType rentalType, int total, int expectedCharge)
        {
            var customerService = GetCustomerService(rentalType, total);

            var actual = sut.Save(customerService);

            Assert.False(actual.HasErrors);

            var actualResult = ((CustomerService)actual.ResultData).TotalCharge;

            Assert.AreEqual(expectedCharge, actualResult);
        }

        [Test]
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

            promotionManagerMock.Setup(s => s.ApplyDiscount(customerService, ref actualResult));

            var actual = sut.Save(customerService);

            Assert.False(actual.HasErrors);

            actualResult = ((CustomerService)actual.ResultData).TotalCharge;

            Assert.AreEqual(60, actualResult);

            promotionManagerMock.Verify(s => s.ApplyDiscount(customerService, ref actualResult));
        }

        [Test]
        public void ShouldFailValidateSave()
        {
            var customerService = GetCustomerService((RentalType)1, 0);

            var actual = sut.Save(customerService);

            Assert.True(actual.HasErrors);

            var rentalTypeError = actual.Errors.FirstOrDefault(s => s.ErrorCode == nameof(RentalBikeResource.InvalidRentalTypeError));

            Assert.AreEqual(RentalBikeResource.InvalidRentalTypeError, rentalTypeError.ErrorMessage);

            var totalError = actual.Errors.FirstOrDefault(s => s.ErrorCode == nameof(RentalBikeResource.InvalidTotalItemError));

            Assert.AreEqual(RentalBikeResource.InvalidTotalItemError, totalError.ErrorMessage);
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
