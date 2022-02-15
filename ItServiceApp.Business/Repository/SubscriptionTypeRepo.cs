using System;
using ItServiceApp.Business.Repository.Abstracts;
using ItServiceApp.Core.Entities;
using ItServiceApp.Data;

namespace ItServiceApp.Business.Repository
{
    public class SubscriptionTypeRepo : BaseRepository<SubscriptionType, Guid>
    {
        public SubscriptionTypeRepo(MyContext context) : base(context)
        {
        }
    }
}