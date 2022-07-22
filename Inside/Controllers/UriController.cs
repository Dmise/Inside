using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inside.Controllers
{
    public class UriController : Controller
    {
        // GET: UriController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UriController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UriController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UriController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UriController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UriController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UriController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UriController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
