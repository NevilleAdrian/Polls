using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Polls.Models;
using Polls.Repository;

namespace Polls.Controllers
{
    public class GrieviancesController : Controller
    {
        private readonly GrieviancesRepository _grieviance;
        public GrieviancesController(GrieviancesRepository grieviance)
        {
            _grieviance = grieviance;
        }

        // GET: Voters
        public async Task<IActionResult> Index()
        {
            return View(await _grieviance.GetAllGrieviances());
        }

        // GET: Voters/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Grieviance grieviance = await _grieviance.GetGrievianceById(id);
            if (grieviance == null)
            {
                return NotFound();
            }
            return View(grieviance);
        }

        // GET: Voters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grieviance grieviance)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }
            try
            {
                await _grieviance.AddGrieviance(grieviance);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }

        // GET: Voters/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Grieviance grieviance = await _grieviance.GetGrievianceById(id);
            if (grieviance == null)
            {
                return NotFound();
            }
            return View(grieviance);
        }

        // POST: Voters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grieviance grieviance)
        {
            if (id != grieviance.GrievianceId)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            try
            {
                await _grieviance.UpdateGrevianceById(grieviance);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }

        // GET: Voters/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Grieviance grieviance = await _grieviance.GetGrievianceById(id);
            if (grieviance == null)
            {
                return NotFound();
            }
            return View(grieviance);
        }

        // POST: Voters/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _grieviance.DeleteGreviance(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }
    }
}