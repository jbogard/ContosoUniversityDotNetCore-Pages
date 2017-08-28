using AutoMapper;
using ContosoUniversity.Models;

namespace ContosoUniversity.Features.Instructors
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Instructor, Index.Model.Instructor>();
            CreateMap<CourseAssignment, Index.Model.CourseAssignment>();
            CreateMap<Course, Index.Model.Course>();
            CreateMap<Enrollment, Index.Model.Enrollment>();

            CreateMap<Instructor, CreateEdit.Command>();
            CreateMap<CourseAssignment, CreateEdit.Command.CourseAssignment>();

            CreateMap<Instructor, Details.Model>();
            CreateMap<Instructor, Delete.Command>();
        }
    }
}