using AutoMapper;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.Extensions.DependencyInjection;

namespace ASI.Basecode.WebApp
{
    // AutoMapper configuration
    internal partial class StartupConfigurer
    {
        /// <summary>
        /// Configure auto mapper
        /// </summary>
        private void ConfigureAutoMapper()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfileConfiguration());
            });

            this._services.AddSingleton<IMapper>(sp => mapperConfiguration.CreateMapper());
        }

        private class AutoMapperProfileConfiguration : Profile
        {
            public AutoMapperProfileConfiguration()
            {
                CreateMap<UserViewModel, User>();
                CreateMap<User, UserViewModel>()
                    .ForMember(dest => dest.TeamName, 
                        opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : null));

                CreateMap<Article, ArticleViewModel>();
                CreateMap<ArticleViewModel, Article>();

                CreateMap<Ticket, TicketViewModel>()
                    .ForMember(dest => dest.CreatedByName,
                        opt => opt.MapFrom(src => 
                            $"{src.CreatedByNavigation.Fname} {src.CreatedByNavigation.Lname}"))
                    .ForMember(dest => dest.ResolvedByName,
                        opt => opt.MapFrom(src => 
                            src.ResolvedByNavigation != null ? 
                            $"{src.ResolvedByNavigation.Fname} {src.ResolvedByNavigation.Lname}" : null));

                CreateMap<TicketViewModel, Ticket>();

                CreateMap<Comment, CommentViewModel>()
                    .ForMember(dest => dest.UserName,
                        opt => opt.MapFrom(src => $"{src.User.Fname} {src.User.Lname}"))
                    .ForMember(dest => dest.Comment,
                        opt => opt.MapFrom(src => src.Comment1));
                CreateMap<CommentViewModel, Comment>()
                    .ForMember(dest => dest.Comment1, 
                        opt => opt.MapFrom(src => src.Comment));
            }
        }
    }
}
