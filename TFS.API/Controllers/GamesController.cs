using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFS.API.Data;

namespace TFS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly TFSDBContext _context;

        public GamesController(TFSDBContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            return await _context.Game.ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(Guid id)
        {
            var game = await _context.Game.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(Guid id, Game game)
        {
            if (id != game.Guid)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Game.Add(game);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GameExists(game.Guid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGame", new { id = game.Guid }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(Guid id)
        {
            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Game.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        private bool GameExists(Guid id)
        {
            return _context.Game.Any(e => e.Guid == id);
        }
    }
}
