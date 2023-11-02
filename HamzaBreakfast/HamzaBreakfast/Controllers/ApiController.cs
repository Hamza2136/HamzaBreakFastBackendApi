using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HamzaBreakfast.Controllers;
[ApiController]
[Route("[Controller]")]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var modelstatedictionary = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelstatedictionary.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelstatedictionary);
        }

        if(errors.Any(e=> e.Type == ErrorType.Unexpected)){
            return Problem();
        }

        var FirstError = errors[0];
        var statusCode = FirstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError

        };
        return Problem(statusCode: statusCode, title: FirstError.Description);
    }
}