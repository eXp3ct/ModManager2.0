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
	public class ModMappingProfile : Profile
	{
        public ModMappingProfile()
        {
            CreateMap<Mod, ModViewModel>()
                .ForMember(vm => vm.Author, 
                    opt => opt.MapFrom(mod => mod.Authors.FirstOrDefault()!.Name))
                .ForMember(vm => vm.Link, 
                    opt => opt.MapFrom(mod => new Uri(mod.Links.WebSiteUrl)));
        }
    }
}
