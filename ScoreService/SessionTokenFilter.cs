using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using ScoreService.Extenstion;
using ScoreService.Services;

namespace ScoreService
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class SessionTokenFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sessionTokenStorage = context.HttpContext.RequestServices.GetService(typeof(ISessionTokenStorageService)) as ISessionTokenStorageService;

            var token = context.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "SessionToken");
            if (token == null || token.Value != sessionTokenStorage.GetToken())
            {
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Result = new StatusCodeResult(401);
            
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}