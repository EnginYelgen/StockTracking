using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockTracking.Models;
using StockTracking.Extensions;

namespace StockTracking.Controllers
{
    public class EstimatesController : Controller
    {
        private readonly InvesmentContext _context;

        public EstimatesController(InvesmentContext context)
        {
            _context = context;
        }

        public List<ViewModels.CreateBulkyModel> listEstimateOnFly
        {
            get
            {
                object obj = HttpContext.Session.Get<List<ViewModels.CreateBulkyModel>>("Estimate");
                if (obj == null)
                {
                    obj = listEstimateOnFly = new List<ViewModels.CreateBulkyModel>();
                }

                return (List<ViewModels.CreateBulkyModel>)obj;
            }
            set { HttpContext.Session.Set<List<ViewModels.CreateBulkyModel>>("Estimate", value); }
        }

        #region Get

        // GET: Estimates
        public async Task<IActionResult> Index(int? pageIndex)
        {
            //var invesmentContext = _context.Estimate.Include(e => e.InvesmentCompany).Include(e => e.Period).Include(e => e.Stock);

            //Estimate = await PaginatedList<Estimate>.CreateAsync(invesmentContext.AsNoTracking(), pageIndex ?? 1);


            //return View(Estimate);

            var invesmentContext = _context.Estimate.Include(e => e.InvesmentCompany).Include(e => e.Period).Include(e => e.Stock);

            DateTime maxDateDaily = invesmentContext.Where(x => x.PeriodId == 1).GroupBy(x => x.StartDate).OrderByDescending(x => x.Key).Select(x => x.Key).FirstOrDefault();
            DateTime maxDateWeekly = invesmentContext.Where(x => x.PeriodId == 2).GroupBy(x => x.StartDate).OrderByDescending(x => x.Key).Select(x => x.Key).FirstOrDefault();

            return View(await invesmentContext.Where(x => (x.PeriodId == 1 && x.StartDate == maxDateDaily) || (x.PeriodId == 2 && x.StartDate == maxDateWeekly)).OrderByDescending(x => x.StartDate).ToListAsync());
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

        // GET: Estimates/CreateBulky
        public IActionResult CreateBulky()
        {
            HttpContext.Session.Clear();

            ViewData["InvesmentCompanyId"] = new SelectList(_context.InvesmentCompany, "Id", "Name");
            ViewData["PeriodId"] = new SelectList(_context.Period, "Id", "Name");
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Code");

            ViewModels.CreateBulkyModel createBulkyModel = new ViewModels.CreateBulkyModel
            {
                InvesmentCompanyId = 0,
                StockId = 0,
                PeriodId = 0,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                OpeningPrice = 0,
                TargetPrice = 0,
                ClosingPrice = null,
                InvesmentCompanyName = string.Empty,
                StockCode = string.Empty,
                PeriodName = string.Empty,

                Estimates = listEstimateOnFly
            };

            return View(createBulkyModel);

            //return View();
        }

        // GET: Estimates/MultipleEdit
        public async Task<IActionResult> MultipleEdit()
        {
            var invesmentContext = _context.Estimate.Include(e => e.InvesmentCompany).Include(e => e.Period).Include(e => e.Stock);

            return View(await invesmentContext.Where(x => !x.ClosingPrice.HasValue).ToListAsync());
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

        // GET: Estimates/DeleteItem/5
        public ActionResult DeleteItem(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<ViewModels.CreateBulkyModel> estimates = listEstimateOnFly;
            estimates.Remove(estimates.Where(x => x.Key == id).First());
            listEstimateOnFly = estimates;

            ViewModels.CreateBulkyModel createBulkyModel = new ViewModels.CreateBulkyModel
            {
                InvesmentCompanyId = 0,
                StockId = 0,
                PeriodId = 0,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                OpeningPrice = 0,
                TargetPrice = 0,
                ClosingPrice = null,
                InvesmentCompanyName = string.Empty,
                StockCode = string.Empty,
                PeriodName = string.Empty,

                Estimates = estimates
            };

            ViewData["InvesmentCompanyId"] = new SelectList(_context.InvesmentCompany, "Id", "Name");
            ViewData["PeriodId"] = new SelectList(_context.Period, "Id", "Name");
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Code");

            return View("CreateBulky", createBulkyModel);
        }

        // GET: History
        public async Task<IActionResult> History(bool isfilter, DateTime? startDate, DateTime? endDate)
        {
            bool isValid = isfilter;
            string validationMess = string.Empty;


            if (isValid && (!startDate.HasValue || !endDate.HasValue))
            {
                isValid = false;
                validationMess = "Lütfen tarih aralığı giriniz.";
            }

            if (isValid && startDate > endDate)
            {
                isValid = false;
                validationMess = "Başlangıç tarihi bitiş tarihinden büyük olmalı.";
            }

            if (isValid && endDate.Value.Subtract(startDate.Value).Days > 30)
            {
                isValid = false;
                validationMess = "Tarih aralığı en fazla 30 gün olabilir.";
            }


            //ViewData["StartDate"] = startDate;
            //ViewData["EndDate"] = endDate;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;


            if (isValid)
            {
                var invesmentContext = _context.Estimate.Include(e => e.InvesmentCompany).Include(e => e.Period).Include(e => e.Stock);

                return View(await invesmentContext.Where(x => x.StartDate >= startDate && x.StartDate <= endDate).OrderByDescending(x => x.StartDate).ToListAsync());
            }
            else
            {
                if (isfilter)
                    ModelState.AddModelError("Error", validationMess);

                var invesmentContext = _context.Estimate.Include(e => e.InvesmentCompany).Include(e => e.Period).Include(e => e.Stock);

                DateTime maxDateDaily = invesmentContext.Where(x => x.PeriodId == 1).GroupBy(x => x.StartDate).OrderByDescending(x => x.Key).Select(x => x.Key).FirstOrDefault();
                DateTime maxDateWeekly = invesmentContext.Where(x => x.PeriodId == 2).GroupBy(x => x.StartDate).OrderByDescending(x => x.Key).Select(x => x.Key).FirstOrDefault();

                return View(await invesmentContext.Where(x => (x.PeriodId == 1 && x.StartDate == maxDateDaily) || (x.PeriodId == 2 && x.StartDate == maxDateWeekly)).OrderByDescending(x => x.StartDate).ToListAsync());
            }
        }

        #endregion

        #region Post

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

        // POST: Estimates/CreateBulky
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBulky([Bind("Id,InvesmentCompanyId,StockId,PeriodId,StartDate,EndDate,OpeningPrice,TargetPrice,ClosingPrice")] ViewModels.CreateBulkyModel estimate)
        {
            List<ViewModels.CreateBulkyModel> estimates = listEstimateOnFly;

            if (estimates.Where(x => x.InvesmentCompanyId == estimate.InvesmentCompanyId && x.StockId == estimate.StockId && x.PeriodId == estimate.PeriodId && x.StartDate == estimate.StartDate).Any())
            {
                ModelState.AddModelError("Error", "Lütfen farklı bir tahmin giriniz.");
            }
            else if (ModelState.IsValid)
            {
                estimates.Add(new ViewModels.CreateBulkyModel
                {
                    Key = Guid.NewGuid(),
                    InvesmentCompanyId = estimate.InvesmentCompanyId,
                    StockId = estimate.StockId,
                    PeriodId = estimate.PeriodId,
                    StartDate = estimate.StartDate,
                    EndDate = estimate.EndDate,
                    OpeningPrice = estimate.OpeningPrice,
                    TargetPrice = estimate.TargetPrice,
                    ClosingPrice = estimate.ClosingPrice,
                    InvesmentCompanyName = _context.InvesmentCompany.Where(x => x.Id == estimate.InvesmentCompanyId).Select(x => x.Name).FirstOrDefault(),
                    StockCode = _context.Stock.Where(x => x.Id == estimate.StockId).Select(x => x.Code).FirstOrDefault(),
                    PeriodName = _context.Period.Where(x => x.Id == estimate.PeriodId).Select(x => x.Name).FirstOrDefault()
                });
            }

            estimate.Estimates = listEstimateOnFly = estimates;

            ViewData["InvesmentCompanyId"] = new SelectList(_context.InvesmentCompany, "Id", "Name", estimate.InvesmentCompanyId);
            ViewData["PeriodId"] = new SelectList(_context.Period, "Id", "Name", estimate.PeriodId);
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Code", estimate.StockId);

            return View(estimate);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEstimates()
        {
            Estimate estimate;
            foreach (ViewModels.CreateBulkyModel model in listEstimateOnFly)
            {
                estimate = new Estimate();
                estimate.ClosingPrice = model.ClosingPrice;
                estimate.EndDate = model.EndDate;
                estimate.InvesmentCompanyId = model.InvesmentCompanyId;
                estimate.OpeningPrice = model.OpeningPrice;
                estimate.PeriodId = model.PeriodId;
                estimate.StartDate = model.StartDate;
                estimate.StockId = model.StockId;
                estimate.TargetPrice = model.TargetPrice;

                _context.Add(estimate);
            }

            if (listEstimateOnFly.Count > 0)
                await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

        // POST: Estimates/MultipleEdit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> MultipleEdit(List<Estimate> estimate)
        {
            List<Estimate> updatedEstimates = estimate.Where(x => x.ClosingPrice > 0).ToList();

            if (ModelState.IsValid && updatedEstimates.Any())
            {
                try
                {
                    Estimate _estimate;
                    foreach (Estimate item in updatedEstimates)
                    {
                        _estimate = _context.Estimate.Where(x => x.Id == item.Id).First();
                        _estimate.ClosingPrice = item.ClosingPrice;
                        _context.Update(_estimate);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
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

        #endregion

        private bool EstimateExists(int id)
        {
            return _context.Estimate.Any(e => e.Id == id);
        }
    }
}
