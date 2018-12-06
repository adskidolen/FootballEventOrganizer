namespace Footeo.Web.Infrastructure.Mapping
{
    using AutoMapper;

    using Footeo.Models;
    using Footeo.Web.ViewModels.Fields.View;
    using Footeo.Web.ViewModels.Leagues.View;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Propery));

            this.CreateMap<Field, FieldViewModel>();

            this.CreateMap<League, LeagueViewModel>();
        }
    }
}