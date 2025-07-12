using eShopFlix.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace eShopFlix.Controllers
{
    public class BaseController:Controller
    {
        public UserModel CurrentUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    string? userData = User.FindFirst(ClaimTypes.UserData)?.Value;
                    return JsonSerializer.Deserialize<UserModel>(userData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new UserModel();
                }
                return null;
            }
        }
    }
}
