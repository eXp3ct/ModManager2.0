using AutoMapper;
using Expect.ModManager.Domain.Models;
using Expect.ModManager.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Infrastructure.Mappings
{
	public class CategoryMappingProfile : Profile
	{
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryViewModel>();
        }
    }
}
