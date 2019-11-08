using Microsoft.AspNetCore.Mvc;
using Polls.Models;
using Polls.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polls.Controllers
{
    public class ElectionWinnersController : Controller
    {
        private readonly ElectionWinnersRepository _winner;
        public ElectionWinnersController(ElectionWinnersRepository winner)
        {
            _winner = winner;
        }

        // GET: Voters
        public async Task<IActionResult> Index()
        {
            return View(await _winner.GetAllElectionWinners());
        }

        // GET: Voters/Details/5
        public async Task<IActionResult> Details(string id)
        {
            ElectionWinner winner = await _winner.GetElectionWinnerById(id);
            if (winner == null)
            {
                return NotFound();
            }
            return View(winner);
        }

        // GET: Voters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ElectionWinner winner)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }
            try
            {
                await _winner.AddElectionWinner(winner);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }

        // GET: Voters/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ElectionWinner winner = await _winner.GetElectionWinnerById(id);
            if (winner == null)
            {
                return NotFound();
            }
            return View(winner);
        }

        // POST: Voters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ElectionWinner winner)
        {
            if (id != winner.ElectionWinnerId)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            try
            {
                await _winner.UpdateElectionWinnerById(winner);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }

        // GET: Voters/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            ElectionWinner winner = await _winner.GetElectionWinnerById(id);
            if (winner == null)
            {
                return NotFound();
            }
            return View(winner);
        }

        // POST: Voters/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _winner.DeleteElectionWinner(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }
    }
}
