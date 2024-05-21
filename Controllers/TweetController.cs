using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Models;

namespace project.Controllers
{
    public class TweetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TweetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tweet
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(await _context.Tweet.OrderByDescending(d => d.Id).ToListAsync());
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
        }

        // GET: Tweet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tweet = await _context.Tweet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tweet == null)
            {
                return NotFound();
            }

            return View(tweet);
        }

        // GET: Tweet/Create
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction(actionName: "Index");
            }
        }

        // POST: Tweet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,ImageUrl,TwUserName")] Tweet tweet)
        {
            if (ModelState.IsValid)
            {
                tweet.TwUserName = User.Identity.Name; // Atan kullanıcı adını kaydet
                _context.Add(tweet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tweet);
        }

        // GET: Tweet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tweet = await _context.Tweet.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }

            // Eğer atan kullanıcı adı ve mevcut kullanıcı adı eşleşiyorsa Edit sayfasını göster
            if (tweet.TwUserName == User.Identity.Name)
            {
                return View(tweet);
            }
            else
            {
                // Eşleşmiyorsa yetki hatası göster
                TempData["ErrorMessage"] = "Bu tweeti düzenlemek için yetkiniz bulunmamaktadır.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Tweet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,ImageUrl,TwUserName")] Tweet tweet)
        {
            if (id != tweet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Eğer atan kullanıcı adı ve mevcut kullanıcı adı eşleşiyorsa güncelleme işlemini yap
                    if (tweet.TwUserName == User.Identity.Name)
                    {
                        _context.Update(tweet);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Eşleşmiyorsa yetki hatası göster
                        TempData["ErrorMessage"] = "Bu tweeti düzenlemek için yetkiniz bulunmamaktadır.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TweetExists(tweet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(tweet);
        }

        // GET: Tweet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tweet = await _context.Tweet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tweet == null)
            {
                return NotFound();
            }

            return View(tweet);
        }

        // POST: Tweet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tweet = await _context.Tweet.FindAsync(id);

            // Eğer atan kullanıcı adı ve mevcut kullanıcı adı eşleşiyorsa silme işlemini yap
            if (tweet.TwUserName == User.Identity.Name)
            {
                _context.Tweet.Remove(tweet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Eşleşmiyorsa yetki hatası göster
                TempData["ErrorMessage"] = "Bu tweeti silmek için yetkiniz bulunmamaktadır.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool TweetExists(int id)
        {
            return _context.Tweet.Any(e => e.Id == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
