using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Students
{
    public class Details : PageModel
    {
        private readonly IMediator _mediator;

        public Details(IMediator mediator) => _mediator = mediator;

        public Model Data { get; private set; }

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public record Query : IRequest<Model>
        {
            public int Id { get; init; }
        }

        public record Model
        {
            public int Id { get; init; }
            [Display(Name = "First Name")]
            public string FirstMidName { get; init; }
            public string LastName { get; init; }
            public DateTime EnrollmentDate { get; init; }
            public List<Enrollment> Enrollments { get; init; }

            public record Enrollment
            {
                public string CourseTitle { get; init; }
                public Grade? Grade { get; init; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Student, Model>();
                CreateMap<Enrollment, Model.Enrollment>();
            }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly SchoolContext _db;
            private readonly IConfigurationProvider _configuration;

            public Handler(SchoolContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public Task<Model> Handle(Query message, CancellationToken token) => _db
                    .Students
                    .Where(s => s.Id == message.Id)
                    .ProjectTo<Model>(_configuration)
                    .SingleOrDefaultAsync(token);
        }
    }
}