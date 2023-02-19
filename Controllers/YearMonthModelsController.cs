using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using PogodaArchive.Data;
using PogodaArchive.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Diagnostics.Contracts;
using System.Text;
using EFCore.BulkExtensions;
using Microsoft.VisualBasic;
using NPOI.HPSF;

namespace PogodaArchive.Controllers
{
    public class YearMonthModelsController : Controller
    {
        private readonly PogodaArchiveContext _context;

        public YearMonthModelsController(PogodaArchiveContext context)
        {
            _context = context;
        }

        // GET: YearMonthModels
        public IActionResult Index()
        {
            return View();
        }

        // GET: YearMonthModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.YearMonthModel == null)
            {
                return NotFound();
            }

            var yearMonthModel = await _context.YearMonthModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yearMonthModel == null)
            {
                return NotFound();
            }

            return View(yearMonthModel);
        }

        // GET: YearMonthModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: YearMonthModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year,Month,Discription")] YearMonthModel yearMonthModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yearMonthModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(yearMonthModel);
        }

        // GET: YearMonthModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.YearMonthModel == null)
            {
                return NotFound();
            }

            var yearMonthModel = await _context.YearMonthModel.FindAsync(id);
            if (yearMonthModel == null)
            {
                return NotFound();
            }
            return View(yearMonthModel);
        }

        // POST: YearMonthModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Year,Month,Discription")] YearMonthModel yearMonthModel)
        {
            if (id != yearMonthModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yearMonthModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearMonthModelExists(yearMonthModel.Id))
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
            return View(yearMonthModel);
        }

        // GET: YearMonthModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.YearMonthModel == null)
            {
                return NotFound();
            }

            var yearMonthModel = await _context.YearMonthModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yearMonthModel == null)
            {
                return NotFound();
            }

            return View(yearMonthModel);
        }

        // POST: YearMonthModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.YearMonthModel == null)
            {
                return Problem("Entity set 'PogodaArchiveContext.YearMonthModel'  is null.");
            }
            var yearMonthModel = await _context.YearMonthModel.FindAsync(id);
            if (yearMonthModel != null)
            {
                _context.YearMonthModel.Remove(yearMonthModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YearMonthModelExists(int id)
        {
            return _context.YearMonthModel.Any(e => e.Id == id);
        }
    }
}
