using AutoMapper;
using ItServiceApp.Core.Identity;
using ItServiceApp.Core.ViewModels;

namespace ItServiceApp.Business.MapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<ApplicationUser, UserProfileViewModel>().ReverseMap();
            //CreateMap<UserProfileViewModel, ApplicationUser>(); //reversemap kullandığımız için gerek yok
        }
    }
}
