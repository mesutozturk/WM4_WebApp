using ItServiceApp.Business.Repository.Abstracts;
using ItServiceApp.Core.Entities;
using ItServiceApp.Data;
using System;

namespace ItServiceApp.Business.Repository
{
    public class AddressRepo : BaseRepository<Address, Guid>
    {
        public AddressRepo(MyContext context) : base(context)
        {
        }
    }
}
