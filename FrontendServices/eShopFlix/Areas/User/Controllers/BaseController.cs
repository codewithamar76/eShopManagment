using eShopFlix.Helper;
using eShopFlix.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace eShopFlix.Areas.User.Controllers
{
    [CustomAuthorization(Role = "User")]
    [Area("User")]
    public class BaseController : Controller
    {
        public UserModel CurrentUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    string strData = User.FindFirst(ClaimTypes.UserData)?.Value ?? string.Empty;
                    return JsonSerializer.Deserialize<UserModel>(strData) ?? new UserModel();
                }
                return null;
            }
        }
    }
}
