using AutoMapper;
using SwitDish.Customer.API.Models;
using SwitDish.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Common.DomainModels.CustomerBooking, CustomerBookingViewModel>().ReverseMap();
            CreateMap<UserSecurityQuestionViewModel, Common.DomainModels.CustomerSecurityQuestion>().ReverseMap();
            CreateMap<DataModel.Models.VendorGalleryImage, Common.DomainModels.VendorGalleryImage>().ReverseMap();
            CreateMap<VendorGalleryImageViewModel, DataModel.Models.VendorGalleryImage>().ReverseMap();
            CreateMap<VendorGalleryImageViewModel, Common.DomainModels.VendorGalleryImage>().ReverseMap();
            CreateMap<VendorOfferViewModel, Common.DomainModels.VendorOffer>().ReverseMap();
            CreateMap<DataModel.Models.CustomerBooking, CustomerBookingViewModel>().ReverseMap();
            CreateMap<Common.DomainModels.CustomerFavouriteVendor, CustomerFavouriteVendorViewModel>().ReverseMap();
            CreateMap<Common.DomainModels.CustomerOrderFeedback, CustomerOrderFeedbackViewModel>().ReverseMap();
            CreateMap<Common.DomainModels.CustomerOrder, CustomerOrderPostViewModel>().ReverseMap();

            CreateMap<Common.DomainModels.CustomerDeliveryAddress, CustomerDeliveryAddressViewModel>()
                .ForMember(d => d.Type, opt => opt.MapFrom(src => src.CustomerDeliveryAddressType.ToString()));

            CreateMap<CustomerDeliveryAddressViewModel, Common.DomainModels.CustomerDeliveryAddress>()
                .ForMember(d => d.CustomerDeliveryAddressType, opt => opt.MapFrom(src => src.Type));

            CreateMap<CustomerDeliveryAddressType, string>().ConvertUsing(src => src.ToString());

            CreateMap<Common.DomainModels.Vendor, VendorDetailsViewModel>()
                .ForMember(d=> d.DeliveryTime, opt=> opt.MapFrom(src=> src.VendorDeliveryTime.ToString()))
                .ForMember(d=> d.Products, opt=> opt.MapFrom(src=> src.Products.GroupBy(d=> d.ProductCategory.Name).Select(d=> new { Category = d.Key, Products = d.ToList() }).ToList()))
                .ForMember(d=> d.MostPopularProducts, opt=> opt.MapFrom(src=> src.CustomerOrders.SelectMany(d=> d.CustomerOrderProducts).GroupBy(d=> d.ProductId).OrderByDescending(d=> d.Count()).Select(d=> d.FirstOrDefault().Product).Take(3)))
                .ForMember(d=> d.BestSellerProducts, opt => opt.MapFrom(src => src.CustomerOrders.SelectMany(d => d.CustomerOrderProducts).GroupBy(d => d.ProductId).OrderByDescending(d => d.Count()).Select(d => d.FirstOrDefault().Product).Take(10)))
                .ForMember(d => d.Rating, opt => opt.MapFrom(src => src.CustomerOrders.Where(d => d.CustomerOrderFeedback != null).Select(d => d.CustomerOrderFeedback).Select(d => d.FoodRating).DefaultIfEmpty(0).Average().ToString("F1")))
                .ForMember(d => d.ReviewCount, opt => opt.MapFrom(src => src.CustomerOrders.Where(d => d.CustomerOrderFeedback != null).Select(d => d.CustomerOrderId).DefaultIfEmpty(0).Count()))
                .ReverseMap();

            CreateMap<Common.DomainModels.Vendor, VendorInfoViewModel>()
                .ForMember(d => d.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.MapLocation, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(d => d.DeliveryTime, opt => opt.MapFrom(src => src.VendorDeliveryTime.ToString()))
                .ForMember(d => d.ProductImage, opt => opt.MapFrom(src => src.ProfileImage))
                .ForMember(d => d.ProductPrice, opt => opt.MapFrom(src => 0))
                //.ForMember(d => d.ProductImage, opt => opt.MapFrom(src => src.Products.OrderByDescending(d=> d.CustomerOrderProducts.Select(d=> d.CustomerOrderId).DefaultIfEmpty(0).Count()).FirstOrDefault().Image))
                //.ForMember(d => d.ProductPrice, opt => opt.MapFrom(src => src.Products.OrderByDescending(d=> d.CustomerOrderProducts.Select(d => d.CustomerOrderId).DefaultIfEmpty(0).Count()).FirstOrDefault().Price))
                .ForMember(d => d.IsPromoted, opt => opt.MapFrom(src=> src.IsPromoted))
                .ForMember(d => d.Rating, opt => opt.MapFrom(src => src.CustomerOrders.Where(d=> d.CustomerOrderFeedback != null).Select(d=> d.CustomerOrderFeedback).Select(d=> d.FoodRating).DefaultIfEmpty(0).Average().ToString("F1")))
                .ForMember(d => d.ReviewCount, opt => opt.MapFrom(src => src.CustomerOrders.Where(d=> d.CustomerOrderFeedback != null).Select(d=> d.CustomerOrderId).DefaultIfEmpty(0).Count()))
                .ReverseMap();

            CreateMap<SignUpViewModel, Common.DomainModels.Customer>()
                .ForPath(d=>d.ApplicationUser.IsActive, opt=> opt.MapFrom(src=> true))
                .ForPath(d=>d.ApplicationUser.FirstName, opt=> opt.MapFrom(src=> src.FirstName))
                .ForPath(d=>d.ApplicationUser.LastName, opt=> opt.MapFrom(src=> src.LastName))
                .ForPath(d=>d.ApplicationUser.Email, opt=> opt.MapFrom(src => src.EmailAddress))
                .ForPath(d=>d.ApplicationUser.PasswordHash, opt=> opt.MapFrom(src => src.Password))
                .ForPath(d=>d.ApplicationUser.Phone, opt=> opt.MapFrom(src => src.MobileNo))
                .ForPath(d=>d.ApplicationUser.DateOfBirth, opt=> opt.MapFrom(src => src.DateOfBirth))
                .ForPath(d=>d.ApplicationUser.Address.AddressId, opt=> opt.MapFrom(src => 0))
                .ReverseMap();

            CreateMap<Common.DomainModels.Customer, CustomerDetailViewModel>()
                .ForMember(d => d.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src.ApplicationUser.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(src => src.ApplicationUser.LastName))
                .ForMember(d => d.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(d => d.Phone, opt => opt.MapFrom(src => src.ApplicationUser.Phone))
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(src => src.ApplicationUser.DateOfBirth.ToString("MM/dd/yyyy")))
                .ForMember(d => d.Image, opt => opt.MapFrom(src => src.ApplicationUser.Image))
                .ReverseMap();

            CreateMap<Common.DomainModels.CustomerSecurityQuestion, CustomerSecurityQuestionViewModel>()
                .ForMember(d=> d.Question, opt=> opt.MapFrom(src=> src.SecurityQuestion.Question))
                .ReverseMap();

            CreateMap<CustomerProfileUpdateViewModel, Common.DomainModels.Customer>()
                .ForPath(d => d.ApplicationUser.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(d => d.ApplicationUser.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(d => d.ApplicationUser.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(d => d.ApplicationUser.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForPath(d => d.ApplicationUser.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ReverseMap();

            CreateMap<Common.DomainModels.CustomerOrder, CustomerOrderViewModel>()
                .ForPath(d=> d.OrderStatus, opt=> opt.MapFrom(src=> src.Status.ToString()))
                .ForPath(d=> d.IsReviewed, opt=> opt.MapFrom(src=> src.CustomerOrderFeedback != null))
                .ForPath(d=> d.RestaurantCharges, opt=> opt.MapFrom(src=> src.Vendor.RestaurantCharges))
                .ForPath(d=> d.DeliveryCharges, opt=> opt.MapFrom(src=> src.Vendor.DeliveryCharges))
                .ForPath(d=> d.VendorName, opt=> opt.MapFrom(src=> src.Vendor.Name))
                .ForPath(d=> d.VendorAddress, opt=> opt.MapFrom(src=> src.Vendor.Address))
                .ForPath(d=> d.VendorProfileImage, opt=> opt.MapFrom(src=> src.Vendor.ProfileImage))
                .ForPath(d=> d.CustomerOrderProducts, opt=> opt.MapFrom(src=> src.CustomerOrderProducts))
                .ReverseMap();

            CreateMap<Common.DomainModels.CustomerOrderProduct, CustomerOrderProductViewModel>()
                .ForPath(d=> d.ProductName, opt => opt.MapFrom(src=> src.Product.Name))
                .ForPath(d=> d.ProductPrice, opt => opt.MapFrom(src=> src.Product.Price))
                .ForPath(d=> d.ProductDescription, opt => opt.MapFrom(src=> src.Product.Description))
                .ReverseMap();

            CreateMap<Common.DomainModels.CustomerOrderFeedback, VendorFeedbackViewModel>()
                .ForPath(d=> d.Comments, opt=> opt.MapFrom(src=> src.FoodComments))
                .ForPath(d=> d.Rating, opt=> opt.MapFrom(src=> src.FoodRating))
                .ForPath(d=> d.CustomerName, opt=> opt.MapFrom(src=> $"{src.Customer.ApplicationUser.FirstName} {src.Customer.ApplicationUser.LastName}"))
                .ForPath(d=> d.FeedbackDate, opt=> opt.MapFrom(src=> src.DateTime))
                .ForPath(d=> d.Rating, opt=> opt.MapFrom(src=> src.FoodRating))
                .ForPath(d=> d.Likes, opt=> opt.MapFrom(src=> 
                    src.VendorFeedbackReactions
                    .Where(d=> d.ReactionType == VendorFeedbackReactionType.Like)
                    .Count()))
                .ForPath(d=> d.Dislikes, opt=> opt.MapFrom(src=> 
                    src.VendorFeedbackReactions
                    .Where(d=> d.ReactionType == VendorFeedbackReactionType.Dislike)
                    .Count()))
                .ReverseMap();

            CreateMap<Common.DomainModels.VendorFeedbackReaction, VendorFeedbackReactionViewModel>()
                .ForMember(d => d.ReactionType, opt => opt.MapFrom(src => src.ReactionType.ToString()));

            CreateMap<VendorFeedbackReactionViewModel, Common.DomainModels.VendorFeedbackReaction>()
                .ForMember(d => d.ReactionType, opt => opt.MapFrom(src => src.ReactionType));

            CreateMap<Common.DomainModels.ProductCategory, ProductCategoryViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.ProductCategoryId))
                .ReverseMap();

            CreateMap<Common.DomainModels.VendorFeature, VendorFeatureViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.VendorFeatureId))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Feature.Name))
                .ReverseMap();
        }
    }
}
