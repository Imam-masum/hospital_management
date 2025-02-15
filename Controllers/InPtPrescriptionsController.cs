﻿using Hospital_Management.Models;
using Hospital_Management.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hospital_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InPtPrescriptionsController : ControllerBase
    {
        private readonly HospitalDbContext _context;

        public InPtPrescriptionsController(HospitalDbContext context)
        {
            _context = context;
        }

        // GET: api/InPtPrescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InPtPrescription>>> GetInPtPrescriptions()
        {
            return await _context.InPtPrescriptions.ToListAsync();
        }

        // GET: api/InPtPrescriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InPtPrescription>> GetInPtPrescription(int id)
        {
            if (id != null)
            {
                var inPtPrescription = await _context.InPtPrescriptions.Include(p => p.PntMedicines).ThenInclude(p => p.MedicineList).Include(p => p.PatientTests).ThenInclude(p => p.Testlist).SingleAsync();

                if (inPtPrescription == null)
                {
                    return NotFound();
                }

                return inPtPrescription;
            }
            else
            {
                var inPtPrescription = await _context.InPtPrescriptions.FindAsync(id);

                if (inPtPrescription == null)
                {
                    return NotFound();
                }

                return inPtPrescription;
            }
        }
    
        // PUT: api/InPtPrescriptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInPtPrescription(int id, InPtPrescription inPtPrescription)
        {
            if (id != inPtPrescription.InPtPrescriptionId)
            {
                return BadRequest();
            }

            _context.Entry(inPtPrescription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InPtPrescriptionExists(id))
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

        // POST: api/InPtPrescriptions
        [HttpPost]
        public async Task<ActionResult<InPtPrescription>> PostInPtPrescription(InPtPrescription inPtPrescription)
        {
            _context.InPtPrescriptions.Add(inPtPrescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInPtPrescription", new { id = inPtPrescription.InPtPrescriptionId }, inPtPrescription);
        }

        // DELETE: api/InPtPrescriptions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InPtPrescription>> DeleteInPtPrescription(int id)
        {
            var inPtPrescription = await _context.InPtPrescriptions.FindAsync(id);
            if (inPtPrescription == null)
            {
                return NotFound();
            }

            _context.InPtPrescriptions.Remove(inPtPrescription);
            await _context.SaveChangesAsync();

            return inPtPrescription;
        }

        private bool InPtPrescriptionExists(int id)
        {
            return _context.InPtPrescriptions.Any(e => e.InPtPrescriptionId == id);
        }
    }
}
