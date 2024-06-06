using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;

namespace Client.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = new RestClient($"https://localhost:7254/api/Users");
            var request = new RestRequest("Login", Method.Post);
            request.AddJsonBody(new { Login, Password });
            var requestResult = await client.ExecuteAsync(request);

            return RedirectToPage("OperationResult", new { header = requestResult.IsSuccessful ? "Sukces!" : "B³¹d!", message = requestResult.Content.Substring(1, requestResult.Content.Length - 2) });
        }
    }
}
