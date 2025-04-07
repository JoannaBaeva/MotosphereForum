// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotorcycleForum.Data.Entities;

namespace MotorcycleForum.Web.Areas.Identity.Pages.Account
{

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            [StringLength(100, ErrorMessage = "Name must be between {2} and {1} characters.", MinimumLength = 2)]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Check if FullName (display name) is already taken
            var existingName = await _userManager.Users.AnyAsync(u => u.FullName == Input.FullName);
            if (existingName)
            {
                ModelState.AddModelError("Input.FullName", "This username is already taken. Please choose another.");
                return Page();
            }

            var existingEmail = await _userManager.Users.AnyAsync(u => u.UserName == Input.Email);
            if (existingEmail)
            {
                ModelState.AddModelError("Input.Email", "This email is already taken. Please choose another.");
                return Page();
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.FullName = Input.FullName;

                user.ProfilePictureUrl = "https://motosphere-images.s3.eu-north-1.amazonaws.com/profiles/default-user-profile.jpg";


                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    string emailBody =
$@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #1c1c1c;
            margin: 0;
            padding: 0;
            color: #ffffff;
        }}
        .email-container {{
            max-width: 600px;
            margin: 40px auto;
            padding: 30px;
            background-color: #121212;
            border-radius: 10px;
            border: 1px solid #2e2e2e;
            box-shadow: 0 0 20px rgba(216, 19, 36, 0.3);
        }}
        h2 {{
            color: #d81324;
            margin-top: 0;
            font-weight: bold;
        }}
        p {{
            font-size: 16px;
            line-height: 1.6;
            color: #f1f1f1;
        }}
        .btn {{
            display: inline-block;
            padding: 12px 30px;
            background-color: #d81324;
            color: #fff;
            text-decoration: none;
            font-weight: bold;
            border-radius: 6px;
            margin-top: 20px;
        }}
        .btn:hover {{
            background-color: #b81220;
        }}
        .footer {{
            font-size: 13px;
            color: #999;
            margin-top: 40px;
            text-align: center;
        }}
        .link {{
            word-break: break-word;
            color: #bbb;
            font-size: 14px;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <h2>Welcome to Motosphere!</h2>
        <p>Hey {HtmlEncoder.Default.Encode(user.FullName ?? user.UserName)},</p>
        <p>Thanks for joining the ride! Before you can fully rev up, we need to verify your email address.</p>
        <p style='text-align: center;'>
            <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' class='btn'>Confirm Email</a>
        </p>
        <p>If that button doesn't work, copy and paste this link into your browser:</p>
        <p class='link'>{HtmlEncoder.Default.Encode(callbackUrl)}</p>
        <p>If you didn’t register, no worries — just ignore this message.</p>
        <p class='footer'>🏍️ Ride safe,<br />— The Motosphere Team</p>
    </div>
</body>
</html>";



                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email - Motosphere", emailBody);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. Ensure that it is not abstract and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
