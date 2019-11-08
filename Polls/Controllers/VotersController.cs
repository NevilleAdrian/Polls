using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Polls.Data;
using Polls.Models;
using Polls.Repository;

namespace Polls.Controllers
{
    public class VotersController : Controller
    {
        private readonly VotersRepository _voters;
        private readonly ApplicationDbContext _ctx;
        public VotersController(VotersRepository voters,
            ApplicationDbContext context)
        {
            _voters = voters;
            _ctx = context;
        }

        // GET: Voters
        public async Task<IActionResult> Index()
        {
            return View(await _voters.GetAllVoters());
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
            return View();
        }

        // POST: Voters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Voter voter)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }
            try
            {
                await _voters.AddVoter(voter);

                return RedirectToAction(nameof(Index), "Home");
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }

        //// GET: Voters/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    Voter voter = await _voters.GetVoterById(id);
        //    if(voter == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(voter);
        //}

        // POST: Voters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id)
        {
            
            try
            {
                await _voters.UpdateVoterById(id);

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
            Voter voter = await _voters.GetVoterById(id);
            if (voter == null)
            {
                return NotFound();
            }
            return View(voter);
        }

        // POST: Voters/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _voters.DeleteVoter(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }
    }
}