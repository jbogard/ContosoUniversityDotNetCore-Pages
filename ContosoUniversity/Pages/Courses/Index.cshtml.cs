using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Courses
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator) => _mediator = mediator;

        public Result Data { get; private set; }

        public async Task OnGetAsync() => Data = await _mediator.Send(new Query());

        public record Query : IRequest<Result>
        {
        }

        public record Result
        {
            public List<Course> Courses { get; init; }

            public record Course
            {
                public int Id { get; init; }
                public string Title { get; init; }
                public int Credits { get; init; }
                public string DepartmentName { get; init; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Course, Result.Course>();
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly SchoolContext _db;
            private readonly IConfigurationProvider _configuration;

            public Handler(SchoolContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Result> Handle(Query message, CancellationToken token)
            {
                var courses = await _db.Courses
                    .OrderBy(d => d.Id)
                    .ProjectToListAsync<Result.Course>(_configuration);

                return new Result
                {
                    Courses = courses
                };
            }
        }
    }
}