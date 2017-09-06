using System;
using System.Collections.Generic;
using FdvRentalBike.Domain;
using FdvRentalBike.Workflow.Interfaces;
using FdvRentalBike.Common;
using FdvRentalBike.Workflow.Managers.Interfaces;

namespace FdvRentalBike.Workflow
{
    public class RentalBikeWorkflow : IRentalBikeWorkflow
    {
        private Dictionary<RentalType, Func<int, int>> rentalTypeMap;
        private IPromotionManager promotionManager;

        public RentalBikeWorkflow(IPromotionManager promotionManager)
        {
            this.promotionManager = promotionManager;
            InitRentalTypeMap();
        }

        private void InitRentalTypeMap()
        {
            rentalTypeMap = new Dictionary<RentalType, Func<int, int>>
            {
                { RentalType.Hour, CalculateByHour },
                { RentalType.Day, CalculateByDay },
                { RentalType.Week, CalculateByWeek }
            };
        }

        public Result Save(CustomerService customerService)
        {
            var result = ValidateSave(customerService);

            if (result.HasErrors) return result;

            var totalCharge = 0;

            foreach(var item in customerService.RentServices)
            {
                totalCharge += rentalTypeMap[item.RentalType](item.Total);
            }

            promotionManager.ApplyDiscount(customerService, ref totalCharge);

            customerService.TotalCharge = totalCharge;

            return new Result(customerService);
        }

        private Result ValidateSave(CustomerService customerService)
        {
            var result = new Result();

            foreach(var item in customerService.RentServices)
            {
                var resultItem = ValidateSaveItem(item.RentalType, item.Total);

                result.AddErrorRange(resultItem.Errors);
            }

            return result;
        }

        private Result ValidateSaveItem(RentalType rentalType, int total)
        {
            var result = new Result();

            if (!rentalTypeMap.ContainsKey(rentalType))
            {
                result.AddError(RentalBikeResource.InvalidRentalTypeError, nameof(RentalBikeResource.InvalidRentalTypeError));
            }

            if (total == 0)
            {
                result.AddError(RentalBikeResource.InvalidTotalItemError, nameof(RentalBikeResource.InvalidTotalItemError));
            }

            return result;
        }

        private int CalculateByHour(int total)
        {
            return total * (int)RentalType.Hour;
        }

        private int CalculateByDay(int total)
        {
            return total * (int)RentalType.Day;
        }

        private int CalculateByWeek(int total)
        {
            return total * (int)RentalType.Week;
        }
    }
}
