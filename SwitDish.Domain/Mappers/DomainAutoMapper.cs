using AutoMapper;
using SwitDish.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain.Mappers
{
    public class DomainAutoMapper : Profile
    {
        public DomainAutoMapper()
        {
            CreateAgentMapperProfile();
            CreateUserMapperProfile();
            CreateCompanyMapperProfile();
        }
        void CreateAgentMapperProfile()
        {
            this.CreateMap<DataModel.Agent, Models.Agent>()
                .ForMember(dest => dest.AgentId, opt => opt.MapFrom(src => src.AgentId))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.AgentFirstName, opt => opt.MapFrom(src => src.User != null ? src.User.FirstName : string.Empty))
                .ForMember(dest => dest.AgentLastName, opt => opt.MapFrom(src => src.User != null ? src.User.LastName : string.Empty))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User != null ? src.User.Active.HasValue ? src.User.Active.Value : false : false))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.User != null ? src.User.Password : string.Empty))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User != null ? src.User.Phone : string.Empty))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User != null ? src.User.Gender : string.Empty))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.User != null ? src.User.Title : string.Empty))
                .ForAllOtherMembers(opt => opt.Ignore());

            this.CreateMap<Models.Agent, DataModel.Agent>()
                 .ForMember(dest => dest.AgentId, opt => opt.MapFrom(src => src.AgentId))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForAllOtherMembers(opt => opt.Ignore()); ;
        }
        void CreateUserMapperProfile()
        {
            this.CreateMap<DataModel.User, Models.User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForAllOtherMembers(opt => opt.Ignore());

            this.CreateMap<Models.User, DataModel.User>()
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForAllOtherMembers(opt => opt.Ignore()); ;
        }
        void CreateCompanyMapperProfile()
        {
            this.CreateMap<DataModel.Company, Models.Company>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForAllOtherMembers(opt => opt.Ignore());

            this.CreateMap<Models.Company, DataModel.Company>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
