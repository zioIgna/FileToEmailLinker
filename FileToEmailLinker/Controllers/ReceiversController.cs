using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileToEmailLinker.Data;
using FileToEmailLinker.Models;

namespace FileToEmailLinker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiversController : ControllerBase
    {
        private readonly FileToEmailLinkerContext _context;

        public ReceiversController(FileToEmailLinkerContext context)
        {
            _context = context;
        }

        // GET: api/Receivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receiver>>> GetReceiver()
        {
          if (_context.Receiver == null)
          {
              return NotFound();
          }
            return await _context.Receiver.ToListAsync();
        }

        // GET: api/Receivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Receiver>> GetReceiver(int id)
        {
          if (_context.Receiver == null)
          {
              return NotFound();
          }
            var receiver = await _context.Receiver.FindAsync(id);

            if (receiver == null)
            {
                return NotFound();
            }

            return receiver;
        }

        // PUT: api/Receivers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReceiver(int id, Receiver receiver)
        {
            if (id != receiver.Id)
            {
                return BadRequest();
            }

            _context.Entry(receiver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceiverExists(id))
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

        // POST: api/Receivers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Receiver>> PostReceiver(Receiver receiver)
        {
          if (_context.Receiver == null)
          {
              return Problem("Entity set 'FileToEmailLinkerContext.Receiver'  is null.");
          }
            _context.Receiver.Add(receiver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReceiver", new { id = receiver.Id }, receiver);
        }

        // DELETE: api/Receivers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceiver(int id)
        {
            if (_context.Receiver == null)
            {
                return NotFound();
            }
            var receiver = await _context.Receiver.FindAsync(id);
            if (receiver == null)
            {
                return NotFound();
            }

            _context.Receiver.Remove(receiver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReceiverExists(int id)
        {
            return (_context.Receiver?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
