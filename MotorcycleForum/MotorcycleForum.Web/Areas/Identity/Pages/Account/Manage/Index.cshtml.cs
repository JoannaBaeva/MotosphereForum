using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MotorcycleForum.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MotorcycleForum.Data.Entities;

namespace MotorcycleForum.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string FullName { get; set; } = null!;
        public string ProfilePictureUrl { get; set; } = null!;

        [BindProperty]
        public IFormFile ProfilePicture { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            FullName = user.FullName;
            ProfilePictureUrl = user.ProfilePictureUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            bool updated = false;

            // Update full name if changed
            if (!string.IsNullOrWhiteSpace(FullName) && FullName != user.FullName)
            {
                user.FullName = FullName;
                updated = true;
            }

            // Only update the image if a new file was provided
            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                var s3 = new S3Service();
                var fileExt = Path.GetExtension(ProfilePicture.FileName);
                var fileKey = $"profiles/{user.Id}/profile{fileExt}";

                using var stream = ProfilePicture.OpenReadStream();
                var uploadedUrl = await s3.UploadFileAsync(stream, fileKey);

                user.ProfilePictureUrl = uploadedUrl;
                updated = true;
            }

            if (updated)
            {
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Profile updated successfully.";
            }
            else
            {
                StatusMessage = "No changes were made.";
            }

            return RedirectToPage();
        }
    }
}