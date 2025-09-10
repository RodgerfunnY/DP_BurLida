using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace DP_BurLida.Controllers
{
    public class BrigadeController : Controller
    {
        private readonly IBrigadeServices _brigadeService;

        public BrigadeController(IBrigadeServices brigadeService)
        {
            _brigadeService = brigadeService;
        }

        public async Task<ActionResult> Index()
        {
            var brigade = await _brigadeService.GetAllAsync();
            return View(brigade);
        }

        // GET: OrderController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var brigade = await _brigadeService.GetByIdAsync(id);
            return View(brigade);
        }

        // GET: OrderController/Create
        public async Task<ActionResult> Create()
        {
            var userService = HttpContext.RequestServices.GetRequiredService<IUserServices>();
            var users = await userService.GetAllAsync();
            var userItems = users.Select(u => new { u.Id, FullName = ($"{u.Name} {u.Surname}").Trim() });
            ViewBag.Users = new SelectList(userItems, "Id", "FullName");
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BrigadeModelData model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _brigadeService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
        // GET: OrderController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var brigade = await _brigadeService.GetByIdAsync(id);
            if (brigade == null)
                return NotFound();
            
            var userService = HttpContext.RequestServices.GetRequiredService<IUserServices>();
            var users = await userService.GetAllAsync();
            var userItems = users.Select(u => new { u.Id, FullName = ($"{u.Name} {u.Surname}").Trim() });
            ViewBag.Users = new SelectList(userItems, "Id", "FullName", brigade.ResponsibleUserId);
            
            return View(brigade);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BrigadeModelData brigade)
        {
            if (!ModelState.IsValid)
            {
                var userService = HttpContext.RequestServices.GetRequiredService<IUserServices>();
                var users = await userService.GetAllAsync();
                var userItems = users.Select(u => new { u.Id, FullName = ($"{u.Name} {u.Surname}").Trim() });
                ViewBag.Users = new SelectList(userItems, "Id", "FullName", brigade.ResponsibleUserId);
                
                return View(brigade);
            }

            await _brigadeService.UpdateAsync(brigade);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var brigade = await _brigadeService.GetByIdAsync(id);
            if (brigade == null)
                return NotFound();
            return View(brigade);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _brigadeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));

        }
    }
}
