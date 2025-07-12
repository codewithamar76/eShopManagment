using AuthService.Application.DTOs;
using AuthService.Application.RepositoryContract;
using AuthService.Application.Services.Contract;
using AuthService.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Services.Implementation
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _co;
        public UserAppService(IUserRepository userRepository, 
            IMapper mapper,
            IConfiguration co)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _co = co;
        }

        private string GenerateJWTToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_co["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            int tokenExpirationMinutes = int.Parse(_co["Jwt:AccessTokenExpiration"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Role", string.Join(",", user.Roles))
            };

            var token = new JwtSecurityToken(
                issuer: _co["Jwt:Issuer"],
                audience: _co["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenExpirationMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users =  await _userRepository.GetUsersAsync();
            return users.Select(user => user.ToDTO()).ToList();
        }

        public async Task<UserDTO> LoginUserAsync(LoginDTO model)
        {
            User user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user!=null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                UserDTO userDTO = user.ToDTO();
                userDTO.Token = GenerateJWTToken(userDTO);
                return userDTO;
            }

            return null;
        }

        public async Task<bool> SignupUserAsync(SignUpDTO model, string role)
        {
            User user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                User newUser = _mapper.Map<User>(model);
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                return await _userRepository.RegisterUser(newUser, role);
            }

            return false;
        }
    }
}
