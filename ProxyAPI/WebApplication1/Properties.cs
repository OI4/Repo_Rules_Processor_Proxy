using Microsoft.AspNetCore.Mvc;

namespace RestServerExample;

public class Properties : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}