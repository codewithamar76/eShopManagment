using AutoMapper;
using Catalog.Application.DTO;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Helper
{
    public class ProductMapper:Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductDTO, Product>()
                .ReverseMap();
        }
    }
}
