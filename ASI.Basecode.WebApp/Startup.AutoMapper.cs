using AutoMapper;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

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

                CreateMap<Article, ArticleViewModel>()
                    .ForMember(dest => dest.CreatedByName,
                        opt => opt.MapFrom(src => $"{src.CreatedByNavigation.Fname} {src.CreatedByNavigation.Lname}"))
                    .ForMember(dest => dest.CreatedByProfilePicUrl,
                        opt => opt.MapFrom(src =>
                            string.IsNullOrEmpty(src.CreatedByNavigation.ProfilePicUrl)
                                ? AvatarHelper.GetInitialAvatar(
                                    src.CreatedByNavigation.Fname,
                                    src.CreatedByNavigation.Lname)
                                : src.CreatedByNavigation.ProfilePicUrl));

                CreateMap<ArticleViewModel, Article>();

                CreateMap<Ticket, TicketViewModel>()
                    .ForMember(dest => dest.CreatedByName,
                        opt => opt.MapFrom(src =>
                            $"{src.CreatedByNavigation.Fname} {src.CreatedByNavigation.Lname}"))
                    .ForMember(dest => dest.CreatedByProfilePicture,
                        opt => opt.MapFrom(src =>
                            string.IsNullOrEmpty(src.CreatedByNavigation.ProfilePicUrl)
                                ? AvatarHelper.GetInitialAvatar(
                                    src.CreatedByNavigation.Fname,
                                    src.CreatedByNavigation.Lname)
                                : src.CreatedByNavigation.ProfilePicUrl))
                    .ForMember(dest => dest.ResolvedByName,
                        opt => opt.MapFrom(src =>
                            src.ResolvedByNavigation != null ?
                            $"{src.ResolvedByNavigation.Fname} {src.ResolvedByNavigation.Lname}" : null))
                    .ForMember(dest => dest.AssignedToId,
                        opt => opt.MapFrom(src =>
                            src.Assignments.OrderByDescending(a => a.AssignedOn).FirstOrDefault().AssignedTo))
                    .ForMember(dest => dest.AssignedToName,
                        opt => opt.MapFrom(src =>
                            src.Assignments.OrderByDescending(a => a.AssignedOn)
                                .Select(a => $"{a.AssignedToNavigation.Fname} {a.AssignedToNavigation.Lname}")
                                .FirstOrDefault()));

                CreateMap<TicketViewModel, Ticket>();

                CreateMap<ArticleVersion, ArticleVersionViewModel>()
                .ForMember(dest => dest.VersionedByName,
                           opt => opt.MapFrom(src => $"{src.VersionedByNavigation.Fname} {src.VersionedByNavigation.Lname}"));
                           
                CreateMap<Comment, CommentViewModel>()
                    .ForMember(dest => dest.UserName,
                        opt => opt.MapFrom(src => $"{src.User.Fname} {src.User.Lname}"))
                    .ForMember(dest => dest.Comment,
                        opt => opt.MapFrom(src => src.Comment1));
                CreateMap<CommentViewModel, Comment>()
                    .ForMember(dest => dest.Comment1,
                        opt => opt.MapFrom(src => src.Comment));

                CreateMap<Assignment, AssignmentViewModel>()
                    .ForMember(dest => dest.AssignedToName,
                        opt => opt.MapFrom(src =>
                            $"{src.AssignedToNavigation.Fname} {src.AssignedToNavigation.Lname}"))
                    .ForMember(dest => dest.AssignedByName,
                        opt => opt.MapFrom(src =>
                            $"{src.AssignedByNavigation.Fname} {src.AssignedByNavigation.Lname}"));

                CreateMap<AssignmentViewModel, Assignment>();

                CreateMap<Update, UpdateViewModel>()
                    .ForMember(dest => dest.UpdatedByName,
                        opt => opt.MapFrom(src =>
                            $"{src.UpdatedByNavigation.Fname} {src.UpdatedByNavigation.Lname}"));

                CreateMap<Attachment, AttachmentViewModel>()
                    .ForMember(dest => dest.UploadedByName,
                        opt => opt.MapFrom(src =>
                            $"{src.UploadedByNavigation.Fname} {src.UploadedByNavigation.Lname}"));
                CreateMap<AttachmentViewModel, Attachment>();

                CreateMap<ArticleAttachment, ArticleAttachmentViewModel>()
                    .ForMember(dest => dest.UploadedByName,
                        opt => opt.MapFrom(src =>
                            $"{src.UploadedByNavigation.Fname} {src.UploadedByNavigation.Lname}"));

                CreateMap<FeedbackViewModel, Feedback>();
                CreateMap<Feedback, FeedbackViewModel>()
                    .ForMember(dest => dest.UserId,
                        opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.AgentId,
                        opt => opt.MapFrom(src => src.AgentId));

                CreateMap<User, AgentViewModel>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.Fname, opt => opt.MapFrom(src => src.Fname))
                    .ForMember(dest => dest.Lname, opt => opt.MapFrom(src => src.Lname))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.ProfilePictureUrl, 
                        opt => opt.MapFrom(src => 
                            string.IsNullOrEmpty(src.ProfilePicUrl)
                                ? AvatarHelper.GetInitialAvatar(src.Fname, src.Lname)
                                : src.ProfilePicUrl));

                CreateMap<PreferenceViewModel, Preference>();
                CreateMap<Preference, PreferenceViewModel>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.IsEmailingOn, opt => opt.MapFrom(src => src.IsEmailingOn))
                    .ForMember(dest => dest.ViewType, opt => opt.MapFrom(src => src.ViewType));
            }
        }
    }
}
