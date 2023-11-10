using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PlanetApi.Controllers;

public class BaseController : Controller
{
    public static string GetErrorMessage(ModelStateDictionary modelState)
    {
        return string.Join(" ", modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
    }
}
