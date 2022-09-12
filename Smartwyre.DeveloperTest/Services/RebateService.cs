using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public class RebateService : IRebateService
    {
        private readonly RebateDataStore rebateDataStore;
        private readonly ProductDataStore productDataStore;

        public RebateService(RebateDataStore rebateDataStore, ProductDataStore productDataStore)
        {
            this.rebateDataStore = rebateDataStore;
            this.productDataStore = productDataStore;
        }

        public CalculateRebateResult Calculate(CalculateRebateRequest request)
        {
            var result = new CalculateRebateResult();

            var rebate = rebateDataStore.GetRebate(request.RebateIdentifier);
            var product = productDataStore.GetProduct(request.ProductIdentifier);

            if (rebate == null || product == null)
            {
                result.Success = false;
                return result;
            }

            switch (rebate.Incentive)
            {
                case IncentiveType.FixedCashAmount:
                    return CalculateFixedCashAmount(rebate, product, result);

                case IncentiveType.FixedRateRebate:
                    return CalculateFixedRateRebate(rebate, product, request, result);

                case IncentiveType.AmountPerUom:
                    return CalculateAmountPerUom(rebate, product, request, result);

                default:
                    result.Success = false;
                    return result;
            }
        }

        private CalculateRebateResult CalculateFixedCashAmount(Rebate rebate, Product product, CalculateRebateResult result)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) || rebate.Amount == 0)
            {
                result.Success = false;
                return result;
            }

            result.RebateAmount = rebate.Amount;
            result.Success = true;
            StoreRebateCalculation(rebate, result.RebateAmount);
            return result;
        }

        private CalculateRebateResult CalculateFixedRateRebate(Rebate rebate, Product product, CalculateRebateRequest request, CalculateRebateResult result)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) || rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0)
            {
                result.Success = false;
                return result;
            }

            result.RebateAmount = product.Price * rebate.Percentage * request.Volume;
            result.Success = true;
            StoreRebateCalculation(rebate, result.RebateAmount);
            return result;
        }

        private CalculateRebateResult CalculateAmountPerUom(Rebate rebate, Product product, CalculateRebateRequest request, CalculateRebateResult result)
        {
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) || rebate.Amount == 0 || request.Volume == 0)
            {
                result.Success = false;
                return result;
            }

            result.RebateAmount = rebate.Amount * request.Volume;
            result.Success = true;
            StoreRebateCalculation(rebate, result.RebateAmount);
            return result;
        }

        private void StoreRebateCalculation(Rebate rebate, decimal rebateAmount)
        {
            var storeRebateDataStore = new RebateDataStore();
            storeRebateDataStore.StoreCalculationResult(rebate, rebateAmount);
        }
    }
}