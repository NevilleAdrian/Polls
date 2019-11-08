using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Polls.Data;
using Polls.Models;
using Polls.Repository;

namespace Polls.Controllers
{
    public class ElectionsController : Controller
    {
        private readonly ElectionsRepository _election;
        public ElectionsController(ElectionsRepository election)
        {
            _election = election;
        }

        // GET: Voters
        public async Task<IActionResult> Index()
        {
            return View(await _election.GetAllElections());
        }

        // GET: Voters/Details/5
        public async Task<IActionResult> Details(string id)
        {
            Election election = await _election.GetElectionById(id);
            if(election == null)
            {
                return NotFound();
            }
            return View(election);
        }

        public async Task<IActionResult> Publish(string id)
        {
            Election election = await _election.GetElectionById(id, includeCandidates: true);
            if (election == null)
            {
                return NotFound();
            }
            return View(election);
        }

        public async Task<IActionResult> PublishResult(string id)
        {
            try
            {
                bool result = await _election.PublishElectionResult(id);
                if(result)
                {
                    return RedirectToAction(nameof(Publish), new { id });
                }
                return Json(new { value = false });
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: Voters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Election election)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }
            try
            {
                await _election.AddElection(election);

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
            Election election = await _election.GetElectionById(id);
            if (election == null)
            {
                return NotFound();
            }
            return View(election);
        }

        // POST: Voters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Election election)
        {
            if (id != election.ElectionId)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            try
            {
                await _election.UpdateElectionById(election);

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
            Election election = await _election.GetElectionById(id);
            if (election == null)
            {
                return NotFound();
            }
            return View(election);
        }

        // POST: Voters/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _election.DeleteElection(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }
    }
}