using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Polls.Models;
using Polls.Repository;

namespace Polls.Controllers
{
    public class HomeController : Controller
    {
        private readonly ElectionsRepository _election;
        public HomeController(ElectionsRepository election)
        {
            _election = election;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _election.GetAllElections(includeCandidates: true));
        }

        public async Task<IActionResult> Vote(string id)
        {
            return View(await _election.GetElectionById(id, includeCandidates: true));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
