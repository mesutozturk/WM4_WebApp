using AutoMapper;
using ItServiceApp.Models.Entities;
using ItServiceApp.ViewModels;

namespace ItServiceApp.MapperProfiles
{
    public class SubscriptionProfiles : Profile
    {
        public SubscriptionProfiles()
        {
            CreateMap<SubscriptionType, SubscriptionTypeViewModel>().ReverseMap();
        }
    }
}