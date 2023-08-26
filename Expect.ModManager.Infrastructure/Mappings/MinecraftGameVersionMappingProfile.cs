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
	public class MinecraftGameVersionMappingProfile : Profile
	{
        public MinecraftGameVersionMappingProfile()
        {
            CreateMap<MinecraftGameVersion, MinecraftGameVersionViewModel>()
                .ForMember(vm => vm.Name, 
                    opt => opt.MapFrom(version => version.VersionString));
        }
    }
}
