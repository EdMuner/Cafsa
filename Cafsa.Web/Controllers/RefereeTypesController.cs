using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Cafsa.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class RefereeTypesController : Controller
    {

        private readonly DataContext _context;

        public RefereeTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: RefereeTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.RefereeTypes.ToListAsync());
        }

        // GET: RefereeTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refereeType = await _context.RefereeTypes
                .FirstOrDefaultAsync(rt => rt.Id == id);
            if (refereeType == null)
            {
                return NotFound();
            }

            return View(refereeType);
        }

        // GET: RefereeTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RefereeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] RefereeType refereeType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refereeType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(refereeType);
        }

        // GET: RefereeTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refereeType = await _context.RefereeTypes.FindAsync(id);
            if (refereeType == null)
            {
                return NotFound();
            }
            return View(refereeType);
        }

        // POST: RefereeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] RefereeType refereeType)
        {
            if (id != refereeType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refereeType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefereeTypeExists(refereeType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(refereeType);
        }

        // GET: RefereeTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refereeType = await _context.RefereeTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (refereeType == null)
            {
                return NotFound();
            }

            return View(refereeType);
        }

        // POST: RefereeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refereeType = await _context.RefereeTypes.FindAsync(id);
            _context.RefereeTypes.Remove(refereeType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RefereeTypeExists(int id)
        {
            return _context.RefereeTypes.Any(e => e.Id == id);
        }
    }
}
