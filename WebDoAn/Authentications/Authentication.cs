using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebDoAn.Authentications
{
	public class Authentication : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if(context.HttpContext.Session.GetString("PhoneNumber") == null)
			{
				context.Result = new RedirectToRouteResult(
					new RouteValueDictionary
					{
                        {"Area", "" },
                        {"Controller","Access" },
						{"Action", "Login" }
					});
			}
		}
	}
}
