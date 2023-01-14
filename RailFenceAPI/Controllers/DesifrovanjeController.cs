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

    public class DesifrovanjeController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DesifrovanjeController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("sva-desifrovanja")]
        public async Task<ActionResult<IEnumerable<SvaDesifrovanjaDto>>> GetDesifrovanja()
        {
            var result = await _context.Desifrovanja
                .Include(a => a.AppUser)
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<SvaDesifrovanjaDto>>(result));
        }

        [HttpGet("moja-desifrovanja")]
        public async Task<ActionResult<IEnumerable<DesifrovanjeDto>>> GetMojaDesifrovanja(int id)
        {
            var result = await _context.Desifrovanja
                //.Include(a => a.AppUser)
                .Where(a => a.AppUserId == id)
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<DesifrovanjeDto>>(result));
        }

        //[Authorize]
        [HttpPost("add-desifrovanje")]
        public async Task<ActionResult<Desifrovanje>> AddDesifrovanje(DesifrovanjeDto desifrovanjeDto)
        {
            desifrovanjeDto.AppUserId = User.GetUserId();
            var desifrovanje = _mapper.Map<Desifrovanje>(desifrovanjeDto);
            await _context.AddAsync(desifrovanje);
            await _context.SaveChangesAsync();
            return Ok(desifrovanje);
        }
    }
}
