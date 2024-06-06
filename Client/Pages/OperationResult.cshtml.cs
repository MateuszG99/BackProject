using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages
{
    public class OperationResultModel : PageModel
    {
        [BindProperty]
        public string Header { get; set; } = "";

        [BindProperty]
        public string Message { get; set; } = "";

        public void OnGet(string header, string message)
        {
            Header = header;
            Message = message;
        }
    }
}
