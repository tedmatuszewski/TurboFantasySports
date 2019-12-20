using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFS.API.Convertors;
using TFS.API.Data;

namespace TFS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly TFSDBContext _context;

        public UsersController(TFSDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUser()
        {
            var task = await _context.User.ToListAsync();

            return task.ConvertAll(u => u.Convert());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var entity = await _context.User.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity.Convert();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UserDto user)
        {
            if (id != user.Guid)
            {
                return BadRequest();
            }

            var entity = user.Convert();
            
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto user)
        {
            var entity = user.Convert();
            var exists = _context.User.Where(u => u.Email.ToLower().Trim() == user.Email.ToLower().Trim()).Count() > 0;

            if(exists)
            {
                return Conflict();
            }

            _context.User.Add(entity);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Guid }, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser(Guid id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user.Convert();
        }
    }
}
