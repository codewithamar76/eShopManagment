using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Mapper
{
    public class UserMapper:Profile
    {
        public UserMapper()
        {
            CreateMap<SignUpDTO, User>();
        }
    }
}
