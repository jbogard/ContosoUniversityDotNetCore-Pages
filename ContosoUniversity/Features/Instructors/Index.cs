using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Features.Instructors
{
    public class Index
    {
        public class Query : IRequest<Model>
        {
            public int? Id { get; set; }
            public int? CourseID { get; set; }
        }

        public class Model
        {
            public int? InstructorID { get; set; }
            public int? CourseID { get; set; }

            public IList<Instructor> Instructors { get; set; }
            public IList<Course> Courses { get; set; }
            public IList<Enrollment> Enrollments { get; set; }

            public class Instructor
            {
                public int ID { get; set; }

                [Display(Name = "Last Name")]
                public string LastName { get; set; }

                [Display(Name = "First Name")]
                public string FirstMidName { get; set; }

                [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
                [Display(Name = "Hire Date")]
                public DateTime HireDate { get; set; }

                public string OfficeAssignmentLocation { get; set; }

                public IEnumerable<CourseAssignment> CourseAssignments { get; set; }
            }

            public class CourseAssignment
            {
                public int CourseID { get; set; }
                public string CourseTitle { get; set; }
            }

            public class Course
            {
                public int Id { get; set; }
                public string Title { get; set; }
                public string DepartmentName { get; set; }
            }

            public class Enrollment
            {
                [DisplayFormat(NullDisplayText = "No grade")]
                public Grade? Grade { get; set; }
                public string StudentFullName { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db) => _db = db;

            protected override async Task<Model> HandleCore(Query message)
            {
                var instructors = await _db.Instructors
                    .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                    .OrderBy(i => i.LastName)
                    .ProjectTo<Model.Instructor>()
                    .ToListAsync()
                    ;

                // EF Core cannot project child collections w/o Include
                // See https://github.com/aspnet/EntityFrameworkCore/issues/9128
                //var instructors = await _db.Instructors
                //    .OrderBy(i => i.LastName)
                //    .ProjectToListAsync<Model.Instructor>();

                var courses = new List<Model.Course>();
                var enrollments = new List<Model.Enrollment>();

                if (message.Id != null)
                {
                    courses = await _db.CourseAssignments
                        .Where(ci => ci.InstructorID == message.Id)
                        .Select(ci => ci.Course)
                        .ProjectTo<Model.Course>()
                        .ToListAsync();
                }

                if (message.CourseID != null)
                {
                    enrollments = await _db.Enrollments
                        .Where(x => x.CourseID == message.CourseID)
                        .ProjectTo<Model.Enrollment>()
                        .ToListAsync();
                }

                var viewModel = new Model
                {
                    Instructors = instructors,
                    Courses = courses,
                    Enrollments = enrollments,
                    InstructorID = message.Id,
                    CourseID = message.CourseID
                };

                return viewModel;
            }
        }

    }
}