using AutoMapper;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Departments;

namespace ContosoUniversity.Features.Departments
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department, Pages.Departments.Index.Model>();
            CreateMap<Department, Details.Model>();
            CreateMap<Create.Command, Department>(MemberList.Source);
            CreateMap<Department, Edit.Command>().ReverseMap();
            CreateMap<Department, Delete.Command>();
        }
    }
}