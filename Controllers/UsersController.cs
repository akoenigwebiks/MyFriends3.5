using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFriends3._5.Data;
using MyFriends3._5.Models;
using MyFriends3._5.Services;
using MyFriends3._5.ViewModels;

namespace MyFriends3._5.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyFriends3_5Context _context;

        public UsersController(MyFriends3_5Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var usersWithProfilePictures = await _context.User
                .Include(u => u.ProfilePicture)
                    .ThenInclude(pp => pp.Picture)
                .ToListAsync();
            return View(usersWithProfilePictures);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.ProfilePicture)
                    .ThenInclude(pp => pp.Picture)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Convert the profile picture data to a base64 string
            if (user.ProfilePicture != null && user.ProfilePicture.Picture.PictureFile != null)
            {
                string base64Image = Convert.ToBase64String(user.ProfilePicture.Picture.PictureFile);
                ViewData["ProfilePicture"] = $"data:image/jpeg;base64,{base64Image}";
            }
            else
            {
                ViewData["ProfilePicture"] = "https://via.placeholder.com/150";
            }

            return View(user);
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            var viewModel = new UserCreateViewModel();
            viewModel.User = new User
            {
                FirstName = "dudu",
                LastName = "Shuku"
            };
            return View(viewModel);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel userCreateViewModel)
        {

            if (
                ModelState.IsValid == false ||
                FileService.IsValidImageFile(userCreateViewModel.ProfileImage) == false
                )
            {
                return View(userCreateViewModel);
            }
            // 1.add  a user
            _context.User.Add(userCreateViewModel.User);
            await _context.SaveChangesAsync();

            // 2.add a picture
            var picture = new Picture
            {
                UserId = userCreateViewModel.User.Id,
                PictureFile = FileService.ConvertToByteArray(userCreateViewModel.ProfileImage),
                PictureName = userCreateViewModel.ProfileImage.Name
            };
            _context.Pictures.Add(picture);
            await _context.SaveChangesAsync();

            // 3. add a profile picture relation
            var profilePicture = new ProfilePicture
            {
                PictureId = picture.Id,
                UserId = userCreateViewModel.User.Id
            };
            _context.ProfilePictures.Add(profilePicture);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
