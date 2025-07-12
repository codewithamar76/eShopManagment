using AuthService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Services.Contract
{
    public interface IUserAppService
    {
        Task<UserDTO> LoginUserAsync(LoginDTO model);
        Task<bool> SignupUserAsync(SignUpDTO model, string role);

        Task<IEnumerable<UserDTO>> GetUsersAsync();
    }
}
