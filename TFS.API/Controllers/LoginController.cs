using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFS.API.Convertors;
using TFS.API.Data;
using TFS.API.Dtos;

namespace TFS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly TFSDBContext _context;

        public LoginController(TFSDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<LeagueMemberDto>>> PostLogin(LoginDto dto)
        {
            if(string.IsNullOrEmpty(dto.Email))
            {
                return BadRequest("You must specify an email in order to login");
            }

            if(string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest("You must specify a password in order to login");
            }

            var userEntity = await _context.User.FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == dto.Email.Trim().ToLower() && u.Password == dto.Password);

            if(userEntity == null)
            {
                return NotFound("You have entered an invalid email or password. Please try again.");
            }

            var leagueEntitiess = await _context.LeagueMember.Include(lm => lm.LeagueGu).Where(lm => lm.UserGuid == userEntity.Guid).ToListAsync();
            var leagues = leagueEntitiess.ConvertAll(le => le.Convert());

            return leagues;
        }
    }
}
