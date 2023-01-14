using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RailFenceAPI.DTOs;
using RailFenceAPI.Entities;
using RailFenceAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailFenceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper, RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleManager = roleManager;
            _config = config;
        }

        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _roleManager.Roles.AnyAsync() == false)
            {
                var roles = new List<AppRole>
                {
                    new AppRole{Name="Korisnik"},
                    new AppRole{Name="Admin"},
                    //new AppRole{Name="Moderator"}
                };

                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }


            if (await UserExists(registerDto.Email))
            {
                return BadRequest("Korisicko ime je zauzeto");
            }

            var user = _mapper.Map<AppUser>(registerDto);
            //to, from

            user.Email = registerDto.Email.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            //kreira usera i cuva ga u bazi

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Korisnik");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email,
                UserSurname = user.UserSurname,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(s => s.Sifrovanja)
                .Include(d => d.Desifrovanja)
            .SingleOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

            if (user == null)
            {
                return Unauthorized("Ne postoji korisnik sa tom kombinacijom emaila i lozinke. Pokusajte ponovo");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                UserSurname = user.UserSurname,
                Email = user.Email,
                //Porudzbine = _mapper.Map<ICollection<PorudzbinaDto>>(user.Porudzbine)
            };
        }

        private async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
