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
        private readonly S3Service _s3Service;

        public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager, S3Service s3Service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _s3Service = s3Service;
        }

        [BindProperty]
        public string FullName { get; set; } = null!;
        public string ProfilePictureUrl { get; set; } = null!;

        [BindProperty]
        public IFormFile ProfilePicture { get; set; }
        
        [BindProperty]
        public string Bio { get; set; } = string.Empty;

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            FullName = user.FullName;
            ProfilePictureUrl = user.ProfilePictureUrl;
            Bio = user.Bio ?? "";
            
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

            // Update bio if changed
            if (!string.IsNullOrWhiteSpace(Bio) && Bio != user.Bio)
            {
                user.Bio = Bio;
                updated = true;
            }

            // Update profile picture
            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                var fileExt = Path.GetExtension(ProfilePicture.FileName);
                var fileKey = $"profiles/{user.Id}/profile{fileExt}";

                using var stream = ProfilePicture.OpenReadStream();
                var uploadedUrl = await _s3Service.UploadFileAsync(stream, fileKey);

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