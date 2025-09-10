using DP_BurLida.Data.ModelsData;
using DP_BurLida.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DP_BurLida.Controllers
{
    public class SkladController : Controller
    {
        private readonly ISkladServices _skladService;

        public SkladController(ISkladServices skladService)
        {
            _skladService = skladService;
        }

        public async Task<ActionResult> Index()
        {
            var items = await _skladService.GetAllAsync();
            return View(items);
        }

        // GET: SkladController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var item = await _skladService.GetByIdAsync(id);
            return View(item);
        }

        // GET: SkladController/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: SkladController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SkladModelData model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _skladService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: SkladController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var item = await _skladService.GetByIdAsync(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        // POST: SkladController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SkladModelData model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _skladService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _skladService.GetByIdAsync(id);
            if (item == null)
                return NotFound();
            return View(item);
        }

        // POST: SkladController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _skladService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
