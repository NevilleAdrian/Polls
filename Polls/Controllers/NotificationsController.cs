using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Polls.Models;
using Polls.Repository;

namespace Polls.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly NotificationsRepository _notify;
        public NotificationsController(NotificationsRepository notify)
        {
            _notify = notify;
        }

        // GET: Voters
        public async Task<IActionResult> Index()
        {
            return View(await _notify.GetAllNotifications());
        }

        // GET: Voters/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Notification notification = await _notify.GetNotificationById(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        public async Task<IActionResult> OpenNotification(int id)
        {
            Notification notification = await _notify.GetNotificationById(id, isSeeing: true);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // GET: Voters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }
            try
            {
                await _notify.AddNotification(notification.Message, notification.UserId);

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
            Notification notification = await _notify.GetNotificationById(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Voters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Notification notification)
        {
            if (id != notification.NotificationId)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit), new { id });
            }
            try
            {
                await _notify.UpdateNotificationById(notification);
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
            Notification notification = await _notify.GetNotificationById(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Voters/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _notify.DeleteNotification(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(nameof(HomeController.Error));
            }
        }
    }
}