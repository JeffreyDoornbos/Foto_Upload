using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.ComponentModel.DataAnnotations;

namespace Foto_Upload.Pages
{


    //public class RequireLoginAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        var session = context.HttpContext.Session;
    //        var username = session.GetString("Username");

    //        if (string.IsNullOrEmpty(username))
    //        {
    //            context.Result = new RedirectToPageResult("/Login");
    //        }
    //    }
    //}

    public class LoginModel : PageModel
    {
        private readonly string _dbPath;

        public LoginModel()
        {
            _dbPath = "users.db";
        }

        [BindProperty]
        public UserViewModel User { get; set; }

        public IActionResult OnPost()
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();

                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT * FROM Users WHERE username = @Username AND password = @Password";
                selectCommand.Parameters.AddWithValue("@Username", User.Username);
                selectCommand.Parameters.AddWithValue("@Password", User.Password);

                using (var reader = selectCommand.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password");
                        return Page();
                    }

                    // Perform login logic here (e.g., set authentication cookie)
                }
            }

            return RedirectToPage("/Index");
        }

        public class UserViewModel
        {
            public int ID { get; set; }

            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }
}
