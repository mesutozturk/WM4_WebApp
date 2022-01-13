﻿using AutoMapper;
using ItServiceApp.Models.Payment;
using Iyzipay.Model;

namespace ItServiceApp.MapperProfiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<CardModel, Card>().ReverseMap();
            CreateMap<BasketModel, BasketItem>().ReverseMap();
            CreateMap<AddressModel, Address>().ReverseMap();
            CreateMap<CustomerModel, Buyer>().ReverseMap();
            CreateMap<InstallmentModel, InstallmentDetail>().ReverseMap();
            CreateMap<InstallmentPriceModel, InstallmentPrice>().ReverseMap();
        }
    }
}