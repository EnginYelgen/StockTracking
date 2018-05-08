using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockTracking.Common;
using StockTracking.Models;

namespace StockTracking.Controllers
{
    public class EstimatesController : Controller
    {
        private readonly InvesmentContext _context;

        public EstimatesController(InvesmentContext context)
        {
            _context = context;
        }

        // GET: Estimates
        public async Task<IActionResult> Index()
        {
            var invesmentContext = _context.Estimate.Include(e => e.InvesmentCompany).Include(e => e.Period).Include(e => e.Stock);
            return View(await invesmentContext.ToListAsync());
        }

        // GET: Estimates/Details/5
        public async Task<IActionResult> Details(int? id, int pageIndex = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estimate = await _context.Estimate
                .Include(e => e.InvesmentCompany)
                .Include(e => e.Period)
                .Include(e => e.Stock)
                .SingleOrDefaultAsync(m => m.Id == id);

            //IQueryable<Estimate> iQueryable = _context.Estimate
            //    .Include(e => e.InvesmentCompany)
            //    .Include(e => e.Period)
            //    .Include(e => e.Stock)
            //    .Where(m => m.Id == id);

            //await PaginatedList<Estimate>.CreateAsync(iQueryable.AsNoTracking(), pageIndex);

            if (estimate == null)
            {
                return NotFound();
            }

            return View(estimate);
        }

        // GET: Estimates/Create
        public IActionResult Create()
        {
            ViewData["InvesmentCompanyId"] = new SelectList(_context.InvesmentCompany, "Id", "Name");
            ViewData["PeriodId"] = new SelectList(_context.Period, "Id", "Name");
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Code");
            return View();
        }

        // POST: Estimates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvesmentCompanyId,StockId,PeriodId,StartDate,EndDate,OpeningPrice,TargetPrice,ClosingPrice")] Estimate estimate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estimate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvesmentCompanyId"] = new SelectList(_context.InvesmentCompany, "Id", "Name", estimate.InvesmentCompanyId);
            ViewData["PeriodId"] = new SelectList(_context.Period, "Id", "Name", estimate.PeriodId);
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Code", estimate.StockId);
            return View(estimate);
        }

        // GET: Estimates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estimate = await _context.Estimate.SingleOrDefaultAsync(m => m.Id == id);
            if (estimate == null)
            {
                return NotFound();
            }
            ViewData["InvesmentCompanyId"] = new SelectList(_context.InvesmentCompany, "Id", "Name", estimate.InvesmentCompanyId);
            ViewData["PeriodId"] = new SelectList(_context.Period, "Id", "Name", estimate.PeriodId);
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Code", estimate.StockId);
            return View(estimate);
        }

        // POST: Estimates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvesmentCompanyId,StockId,PeriodId,StartDate,EndDate,OpeningPrice,TargetPrice,ClosingPrice")] Estimate estimate)
        {
            if (id != estimate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estimate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstimateExists(estimate.Id))
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
            ViewData["InvesmentCompanyId"] = new SelectList(_context.InvesmentCompany, "Id", "Name", estimate.InvesmentCompanyId);
            ViewData["PeriodId"] = new SelectList(_context.Period, "Id", "Name", estimate.PeriodId);
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Code", estimate.StockId);
            return View(estimate);
        }

        // GET: Estimates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estimate = await _context.Estimate
                .Include(e => e.InvesmentCompany)
                .Include(e => e.Period)
                .Include(e => e.Stock)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (estimate == null)
            {
                return NotFound();
            }

            return View(estimate);
        }

        // POST: Estimates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estimate = await _context.Estimate.SingleOrDefaultAsync(m => m.Id == id);
            _context.Estimate.Remove(estimate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstimateExists(int id)
        {
            return _context.Estimate.Any(e => e.Id == id);
        }
    }
}
