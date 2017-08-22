using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public class Query : IRequest<IEnumerable<Model>>
        {
        }

        public class Model
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }
            [Display(Name = "Department")]
            public string DepartmentName { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, IEnumerable<Model>>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db)
            {
                _db = db;
            }

            public async Task<IEnumerable<Model>> Handle(Query message)
            {
                var courses = await _db.Courses
                    .OrderBy(d => d.Id)
                    .ProjectTo<Model>()
                    .ToListAsync()
                    //.ProjectToListAsync<Result.Course>()
                    ;

                return courses;
            }
        }
    }
}