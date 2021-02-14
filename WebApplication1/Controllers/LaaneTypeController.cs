using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class LaaneTypeController : Controller
    {
        private readonly BankContext _context;

        public LaaneTypeController(BankContext context)
        {
            _context = context;
        }


        //GET api/LaaneType
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<IEnumerable<LaaneType>>> GetLåneTyper()
        {
            return await _context.LaaneTyper.ToListAsync();
        }


        //GET api/LaaneType/{id}
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public async Task<IActionResult> GetLåneType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var låneType = await _context.LaaneTyper
                .FirstOrDefaultAsync(m => m.Id == id);
            if (låneType == null)
            {
                return NotFound();
            }

            return Ok(låneType);
        }


        // POST: api/LaaneType
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult<LaaneType>> PostLåneType(LaaneType laaneType)
        {
            if (LåneTypeExists(laaneType.Id))
            {
                return BadRequest();
            }
            _context.LaaneTyper.Add(laaneType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLåneType), new { Id = laaneType.Id }, laaneType);
        }


        //DELETE api/LaaneType/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLåneType(int? id)
        {
            if (!id.HasValue || !LåneTypeExists(id.Value))
            {
                return BadRequest();
            }
            var type = await _context.LaaneTyper.FirstOrDefaultAsync(p => p.Id == id);
            if (type == null)
            {
                return BadRequest();
            }
            _context.LaaneTyper.Remove(type);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool LåneTypeExists(int id)
        {
            return _context.LaaneTyper.Any(e => e.Id == id);
        }
    }
}
