using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string[] Roles { get; set; }
        public string? Token { get; set; }
    }

    public static class UserDTOExtensions
    {
        public static UserDTO ToDTO(this User user)
        {
            return new UserDTO
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = user.Roles.Select(r => r.Name).ToArray(),
                Token = null // Token can be set later after authentication
            };
        }
    }

    public static class UserExtensions
    {
        public static User ToEntity(this UserDTO userDto)
        {
            return new User
            {
                Id = userDto.UserId,
                Name = userDto.Name,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Roles = userDto.Roles.Select(role => new Role { Name = role }).ToList()
            };
        }
    }
}
