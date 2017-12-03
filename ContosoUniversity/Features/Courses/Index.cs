using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Features.Courses
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
        }


        public class Result
        {
            public List<Course> Courses { get; set; }

            public class Course
            {
                public int Id { get; set; }
                public string Title { get; set; }
                public int Credits { get; set; }
                public string DepartmentName { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db)
            {
                _db = db;
            }

            protected override async Task<Result> HandleCore(Query message)
            {
                var courses = await _db.Courses
                    .OrderBy(d => d.Id)
                    .ProjectTo<Result.Course>()
                    .ToListAsync()
                    //.ProjectToListAsync<Result.Course>()
                    ;

                return new Result
                {
                    Courses = courses
                };
            }
        }
    }
}