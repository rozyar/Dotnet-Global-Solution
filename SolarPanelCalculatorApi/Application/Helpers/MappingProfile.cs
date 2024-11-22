using AutoMapper;
using SolarPanelCalculatorApi.Application.DTO;
using SolarPanelCalculatorApi.Domain.Models;
using System.Text.Json;

namespace SolarPanelCalculatorApi.Application.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Appliance, ApplianceDto>().ReverseMap();
            CreateMap<Analysis, AnalysisDto>().ReverseMap();
            CreateMap<RegisterDto, User>();
            CreateMap<Analysis, AnalysisDto>()
           .ForMember(dest => dest.Appliances,
               opt => opt.MapFrom(src => JsonHelper.DeserializeAppliances(src.AppliancesJson)));

            CreateMap<AnalysisDto, Analysis>()
                .ForMember(dest => dest.AppliancesJson,
                    opt => opt.MapFrom(src => JsonHelper.SerializeAppliances(src.Appliances)));
        }
    }
}
