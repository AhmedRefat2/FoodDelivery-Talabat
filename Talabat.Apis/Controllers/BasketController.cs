using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.Dtos;
using Talabat.Apis.Errors;
using Talabat.APIs.Controllers;
using Talabat.Core.Models.Basket;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Apis.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepo, IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }


        // 1. Get Customer Basket By basketId
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepo.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {

            var mappedBasket = _mapper.Map<CustomerBasket>(basket);

            var createdOrUpdatedBasket = await _basketRepo.UpdateBasketAsync(mappedBasket);
            if (createdOrUpdatedBasket == null)
                return BadRequest(new ApiResponse(400));
            return Ok(createdOrUpdatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepo.DeleteBasketAsync(id);
        }

    }
}
