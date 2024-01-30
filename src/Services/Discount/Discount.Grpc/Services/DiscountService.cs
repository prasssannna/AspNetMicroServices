using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        readonly IDiscountRepository discountRepository;
        readonly ILogger<DiscountService> logger;
        readonly IMapper mapper;
        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            this.discountRepository = discountRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var c = this.mapper.Map<Coupon>(request.Coupon);
            var created = await discountRepository.CreateDiscount(c);
            
            if (!created)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon not created"));
            }
            return this.mapper.Map<CouponModel>(c);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon not found for {request.ProductName}"));

            }

            var deleted = await discountRepository.DeleteDiscount(coupon.ProductName);

            if (!deleted)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon not deleted"));
            }
            return new DeleteDiscountResponse { Success = true};

        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"No discount available for {request.ProductName}"));

            }
            var couponModel = this.mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetDiscount(request.Coupon.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon not found for {request.Coupon.ProductName}"));

            }

            var c = this.mapper.Map<Coupon>(request.Coupon);
            var updated = await discountRepository.UpdateDiscount(c);

            if (!updated)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon not updated"));
            }
            return this.mapper.Map<CouponModel>(c);
        }
    }
}
