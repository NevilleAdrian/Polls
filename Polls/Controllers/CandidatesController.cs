using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Polls.Data;
using Polls.Models;
using Polls.Repository;

namespace Polls.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly CandidatesRepository _candidate;
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        public CandidatesController(CandidatesRepository candidate,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _candidate = candidate;
            _ctx = context;
            _userManager = userManager;
        }

        // GET: Voters
        public async Task<IActionResult> Index()
        {
            return View(await _candidate.GetAllCandidates());
        }

        // GET: Voters/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Voters/Create
        public IActionResult Create()
        {
            ViewData["ElectionName"] = new SelectList(_ctx.Elections, "ElectionId", "ElectionName");
            ViewData["Users"] = new SelectList(_userManager.Users, "Id", "Name");
            return View();
        }

        // POST: Voters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CandidateViewModel candidate)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }
            try
            {
                await _candidate.AddCandidate(candidate);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: Voters/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            Candidate candidate = await _candidate.GetCandidateById(id);
            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }

        // POST: Voters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CandidateViewModel candidate)
        {
            if (id != candidate.CandidateId)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            try
            {
                await _candidate.UpdateCandidateById(candidate);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: Voters/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            Candidate candidate = await _candidate.GetCandidateById(id);
            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }

        // POST: Voters/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _candidate.DeleteCandidate(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }
    }
}