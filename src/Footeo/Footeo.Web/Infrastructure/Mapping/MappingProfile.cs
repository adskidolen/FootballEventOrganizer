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
    using Footeo.Web.ViewModels.Referees.Output;
    using Footeo.Web.ViewModels.Matches.Output;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Propery));

            this.CreateMap<Field, Field>();
            this.CreateMap<Field, FieldViewModel>();
            this.CreateMap<Field, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.Name));

            this.CreateMap<League, League>();
            this.CreateMap<League, PendingLeagueViewModel>();
            this.CreateMap<League, InProgressLeagueViewModel>();
            this.CreateMap<League, CompletedLeagueViewModel>();
            this.CreateMap<League, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.Name));

            this.CreateMap<Team, Team>();
            this.CreateMap<Team, TeamViewModel>();
            this.CreateMap<Team, TeamMatchViewModel>();

            this.CreateMap<Player, Player>();
            this.CreateMap<Player, PlayerTeamViewModel>();

            this.CreateMap<Town, Town>();

            this.CreateMap<TeamLeague, TeamLeague>();
            this.CreateMap<TeamLeague, TeamLeagueViewModel>();
            this.CreateMap<TeamLeagueViewModel, TeamLeagueViewModel>();
            this.CreateMap<TeamLeague, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.TeamId))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.Team.Name));

            this.CreateMap<Fixture, Fixture>();
            this.CreateMap<Fixture, FixtureViewModel>();
            this.CreateMap<Fixture, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => $"{src.Name} {src.Date}"));

            this.CreateMap<Referee, RefereeViewModel>()
                .ForMember(dest => dest.MatchAttendances,
                           opt => opt.MapFrom(src => src.Matches.Count));
            this.CreateMap<Referee, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.FullName));

            this.CreateMap<Match, Match>();

            this.CreateMap<SelectListItem, SelectListItem>();

        }
    }
}