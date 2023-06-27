using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

public class GeoLocationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.Request.Query.ContainsKey("latitude") && context.HttpContext.Request.Query.ContainsKey("longitude"))
        {
            string latitude = context.HttpContext.Request.Query["latitude"];
            string longitude = context.HttpContext.Request.Query["longitude"];

            // Pass the latitude and longitude values to the action method
            context.ActionArguments["latitude"] = latitude;
            context.ActionArguments["longitude"] = longitude;
        }
        else
        {
            // Handle the case when latitude and longitude values are not present
            // You can redirect to an error page or perform other necessary actions
            context.Result = new BadRequestResult();
            return;
        }

        base.OnActionExecuting(context);
    }
}