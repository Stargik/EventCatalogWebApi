using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using PL.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IParticipantService participantService;
        private readonly IMapper mapper;

        public AccountController(IAccountService accountService, IParticipantService participantService, IMapper mapper)
        {
            this.accountService = accountService;
            this.participantService = participantService;
            this.mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await accountService.RegisterUser(userModel);
                if (userModel.Roles.FirstOrDefault(roles => roles.ToUpper() == "USER") is not null)
                {
                    var participantModel = mapper.Map<ParticipantModel>(userModel);
                    await participantService.AddAsync(participantModel);
                }
                
                return Accepted();
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                int participantId = (await participantService.GetByEmailAsync(userLoginModel.Email)).Id;
                var resultToken = await accountService.LoginUser(userLoginModel);

                return Accepted(new {
                    Token = resultToken,
                    Id = participantId
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(userLoginModel);
            }
        }
    }
}
