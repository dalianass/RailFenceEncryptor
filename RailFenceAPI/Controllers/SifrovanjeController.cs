using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailFenceAPI.Data;
using RailFenceAPI.DTOs;
using RailFenceAPI.Entities;
using RailFenceAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailFenceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SifrovanjeController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SifrovanjeController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("sva-sifrovanja")]
        public async Task<ActionResult<IEnumerable<SvaSifrovanjaDto>>> GetSifrovanja()
        {
            var result = await _context.Sifrovanja
                .Include(a => a.AppUser)
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<SvaSifrovanjaDto>>(result));
        }

        [HttpGet("moja-sifrovanja")]
        public async Task<ActionResult<IEnumerable<SifrovanjeDto>>> GetMojaSifrovanja(int id)
        {
            var result = await _context.Sifrovanja
                //.Include(a => a.AppUser)
                .Where(a => a.AppUserId == id)
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<SifrovanjeDto>>(result));
        }

        //[Authorize]
        [HttpPost("add-sifrovanje")]
        public async Task<ActionResult<Sifrovanje>> AddSifrovanje(SifrovanjeDto sifrovanjeDto)
        {
            sifrovanjeDto.AppUserId = User.GetUserId();
            var sifrovanje = _mapper.Map<Sifrovanje>(sifrovanjeDto);
            await _context.AddAsync(sifrovanje);
            await _context.SaveChangesAsync();
            return Ok(sifrovanje);
        }
    }
}
