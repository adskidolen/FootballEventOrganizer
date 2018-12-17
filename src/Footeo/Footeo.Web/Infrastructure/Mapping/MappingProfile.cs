namespace Footeo.Web.Infrastructure.Mapping
{
    using AutoMapper;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using Footeo.Models;

    using Footeo.Web.ViewModels.Fields.Output;
    using Footeo.Web.ViewModels.Leagues.Output;
    using Footeo.Web.ViewModels.Players.Output;
    using Footeo.Web.ViewModels.TeamLeagues.Output;
    using Footeo.Web.ViewModels.Teams.Output;
    using Footeo.Web.ViewModels.Fixtures.Output;
    using System.Linq;
    using Footeo.Web.ViewModels.Matches.Output;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Propery));

            this.CreateMap<Field, FieldViewModel>();
            this.CreateMap<Field, Field>();
            this.CreateMap<Field, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.Name));

            this.CreateMap<League, LeagueViewModel>();
            this.CreateMap<League, League>();
            this.CreateMap<League, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.Name));

            this.CreateMap<Team, TeamViewModel>();
            this.CreateMap<Team, Team>();

            this.CreateMap<Player, PlayerViewModel>();
            this.CreateMap<Player, Player>();

            this.CreateMap<Town, Town>();

            this.CreateMap<TeamLeague, TeamLeagueViewModel>();
                //.ForMember(dest => dest.GoalDifference,
                //           opt => opt.MapFrom(src => src.GoalsFor - src.GoalsAgainst));
            this.CreateMap<TeamLeague, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.TeamId))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.Team.Name));

            this.CreateMap<Fixture, FixturesViewModel>();
            this.CreateMap<Fixture, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.Name)); ;

            this.CreateMap<Referee, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.FullName));

            this.CreateMap<Match, Match>();
        }
    }
}