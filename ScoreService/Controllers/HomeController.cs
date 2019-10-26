using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreService.Models;
using ScoreService.Services;
using ScoreService.ViewModel;

namespace ScoreService.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService, ISessionTokenStorageService tokenStorageService)
        {
            _userService = userService;
        }

        [Authorize]
        [SessionTokenFilter]
        public async Task<IActionResult> Index()
        {
            var user = User.Identity.Name;
            var teams = await _userService.GetTeamsAsync(user);
            var result = teams.Select(p => new TeamModel
            {
                Name = p.Name,
                Address = p.Address,
                Id = p.Id
            }).ToList();


            return View(new CollectionTeamModel
            {
                Teams = result
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
