using eShopFlix.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;

namespace eShopFlix.Helper
{
    public abstract class BaseViewPage<TModel>:RazorPage<TModel>
    {
        public UserModel CurrentUser {
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
