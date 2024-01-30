using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _grpcClient;
        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient _grpcClient)
        {
            this._grpcClient = _grpcClient;
        }
        public async Task<CouponModel> GetDiscount(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };
            return await _grpcClient.GetDiscountAsync(request);

        }
    }
}
