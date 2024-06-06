using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;

namespace Client.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string RoleName { get; set; }

        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        public string Password { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            var client = new RestClient($"https://localhost:7254/api/Users/Register");
            var request = new RestRequest($"?roleName={RoleName}", Method.Post);
            request.AddJsonBody(new { Login, Password });
            var requestResult = await client.ExecuteAsync(request);

            if (requestResult.IsSuccessful)
                return RedirectToPage("OperationResult", new { header = "Sukces!", message = "Mo¿esz siê teraz zalogowaæ!"});
            else
                return RedirectToPage("OperationResult", new { header = "B³¹d!", message = $"Wyst¹pi³ B³¹d!: {requestResult.Content}" });
        }
    }
}
