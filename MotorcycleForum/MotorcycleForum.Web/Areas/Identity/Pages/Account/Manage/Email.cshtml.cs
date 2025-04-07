// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using MotorcycleForum.Data.Entities;

namespace MotorcycleForum.Web.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public EmailModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;
            Input = new InputModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);

            if (Input.NewEmail == email)
            {
                StatusMessage = "Your email is unchanged.";
                return RedirectToPage();
            }

            var existingUser = await _userManager.FindByEmailAsync(Input.NewEmail);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError(string.Empty, "This email is already taken.");
                await LoadAsync(user);
                return Page();
            }

            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmailChange",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, email = Input.NewEmail, code = code },
                protocol: Request.Scheme);

            var emailBody = $@"
                <html>
                    <body style='font-family:Arial,sans-serif;background-color:#f4f4f4;padding:20px;'>
                        <div style='max-width:600px;margin:auto;background:#fff;padding:30px;border-radius:8px;'>
                            <h2 style='color:#d81324;'>Motosphere - Confirm Email Change</h2>
                            <p>Hi {HtmlEncoder.Default.Encode(user.FullName ?? user.Email)},</p>
                            <p>Click the button below to confirm your new email address:</p>
                            <p style='text-align:center;'>
                                <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='display:inline-block;padding:10px 20px;background:#d81324;color:#fff;border-radius:5px;text-decoration:none;'>Confirm Email</a>
                            </p>
                            <p>If you did not request this change, please ignore this message.</p>
                        </div>
                    </body>
                </html>";

            await _emailSender.SendEmailAsync(Input.NewEmail, "Confirm your new email - Motosphere", emailBody);

            StatusMessage = "Confirmation link to change email sent. Please check your inbox.";
            return RedirectToPage();
        }
    }
}