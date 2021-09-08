using AutoMapper;

namespace SwitDish.Services.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DataModel.Models.ApplicationUser, Common.DomainModels.ApplicationUser>().ReverseMap();
            CreateMap<DataModel.Models.Address, Common.DomainModels.Address>().ReverseMap();
            CreateMap<DataModel.Models.Vendor, Common.DomainModels.Vendor>().ReverseMap();
            CreateMap<DataModel.Models.Cuisine, Common.DomainModels.Cuisine>().ReverseMap();
            CreateMap<DataModel.Models.VendorCuisine, Common.DomainModels.VendorCuisine>().ReverseMap();
            CreateMap<DataModel.Models.VendorDeliveryTime, Common.DomainModels.VendorDeliveryTime>().ReverseMap();
            CreateMap<DataModel.Models.VendorGalleryImage, Common.DomainModels.VendorGalleryImage>().ReverseMap();
            CreateMap<DataModel.Models.VendorOffer, Common.DomainModels.VendorOffer>().ReverseMap();
            CreateMap<DataModel.Models.Feature, Common.DomainModels.Feature>().ReverseMap();
            CreateMap<DataModel.Models.VendorFeature, Common.DomainModels.VendorFeature>().ReverseMap();
            CreateMap<DataModel.Models.VendorServiceCategory, Common.DomainModels.VendorServiceCategory>().ReverseMap();
            CreateMap<DataModel.Models.ServiceCategory, Common.DomainModels.ServiceCategory>().ReverseMap();
            CreateMap<DataModel.Models.Customer, Common.DomainModels.Customer>().ReverseMap();
            CreateMap<DataModel.Models.SecurityQuestion, Common.DomainModels.SecurityQuestion>().ReverseMap();
            CreateMap<DataModel.Models.CustomerSecurityQuestion, Common.DomainModels.CustomerSecurityQuestion>().ReverseMap();
            CreateMap<DataModel.Models.CustomerFavouriteVendor, Common.DomainModels.CustomerFavouriteVendor>().ReverseMap();
            CreateMap<DataModel.Models.Product, Common.DomainModels.Product>().ReverseMap();
            CreateMap<DataModel.Models.ProductCategory, Common.DomainModels.ProductCategory>().ReverseMap();
            CreateMap<DataModel.Models.CustomerOrder, Common.DomainModels.CustomerOrder>().ReverseMap();
            CreateMap<DataModel.Models.CustomerOrderProduct, Common.DomainModels.CustomerOrderProduct>().ReverseMap();
            CreateMap<DataModel.Models.CustomerDeliveryAddress, Common.DomainModels.CustomerDeliveryAddress>().ReverseMap();
            CreateMap<DataModel.Models.CustomerBooking, Common.DomainModels.CustomerBooking>().ReverseMap();
            CreateMap<DataModel.Models.CustomerOrderFeedback, Common.DomainModels.CustomerOrderFeedback>().ReverseMap();
            CreateMap<DataModel.Models.VendorFeedbackReaction, Common.DomainModels.VendorFeedbackReaction>().ReverseMap();
        }
    }
}
