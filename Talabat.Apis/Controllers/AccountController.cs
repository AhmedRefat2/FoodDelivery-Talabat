using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Apis.Errors;
using Talabat.APIs.Dtos;
using Talabat.APIs.Extensions;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManger, 
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuthService authService, IMapper mapper)
        {
            _userManger = userManger;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _authService = authService;
            _mapper = mapper;
        }


        // 1. Login Endpoint
        [HttpPost("login")]
        [ProducesResponseType(typeof (ApiResponse),401)]
        [ProducesResponseType(typeof (UserDto),200)]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "Invalid Login"));
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!signInResult.Succeeded)
                return Unauthorized(new ApiResponse(401, "Invalid Login"));


            var token = await _authService.CreateTokenAsync(user, _userManger);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = token
            });
        }


        // Sign Up Endpoint
        [HttpPost("register")] // POST : api/account/register
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.Phone
            };

            var result = await _userManger.CreateAsync(user, model.Password);

            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = errors
                });
            }

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManger)
            });
        }

        // 3. Get Current User Endpoint 
        [Authorize]
        [HttpGet] // GET : /api/Account
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var user = await _userManger.FindByEmailAsync(email);

            var mappedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManger)
            };

            return Ok(mappedUser);
        }

        // 4. Get User Address
        [Authorize]
        [HttpGet("address")] // GET : /api/Account/address
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManger.FindUserWithAddressAsync(User);
            var mappedAddress = _mapper.Map<AddressDto>(user?.Address);
            return Ok(mappedAddress);
        }

        // 5. Update User Address
        [Authorize]
        [HttpPut("address")] // PUT : /api/Account/address
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var updatedAddress = _mapper.Map<Address>(address);
            var user = await _userManger.FindUserWithAddressAsync(User);

            updatedAddress.Id = user.Address.Id;
            user.Address = updatedAddress;

            var result = await _userManger.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = result.Errors.Select(E => E.Description)
                });


            return Ok(address);
        }
    }
}
