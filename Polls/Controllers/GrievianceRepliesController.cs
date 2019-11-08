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
    public class GrievianceRepliesController : Controller
    {
        private readonly GrievianceRepliesRepository _reply;
        private readonly ApplicationDbContext _ctx;
        public GrievianceRepliesController(GrievianceRepliesRepository reply,
            ApplicationDbContext context)
        {
            _reply = reply;
            _ctx = context;
        }

        // GET: Voters
        public async Task<IActionResult> Index()
        {
            return View(await _reply.GetAllGrievianceReplies());
        }

        // GET: Voters/Details/5
        public async Task<IActionResult> Details(int id)
        {
            GrievianceReply reply = await _reply.GetGrievianceReplyById(id);
            if(reply == null)
            {
                return NotFound();
            }
            return View(reply);
        }

        // GET: Voters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GrievianceReply reply)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }
            try
            {
                await _reply.ReplyAGrieviance(reply);

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
            GrievianceReply reply = await _reply.GetGrievianceReplyById(id);
            if (reply == null)
            {
                return NotFound();
            }
            return View(reply);
        }

        // POST: Voters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GrievianceReply reply)
        {
            if (id != reply.GrievianceReplyId)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            try
            {
                await _reply.UpdateGrevianceReplyById(reply);

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
            GrievianceReply reply = await _reply.GetGrievianceReplyById(id);
            if (reply == null)
            {
                return NotFound();
            }
            return View(reply);
        }

        // POST: Voters/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _reply.DeleteGreviance(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }
    }
}