using Microsoft.AspNetCore.Mvc;
namespace HamzaBreakfast.Controllers;
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        return Problem();
    }
}