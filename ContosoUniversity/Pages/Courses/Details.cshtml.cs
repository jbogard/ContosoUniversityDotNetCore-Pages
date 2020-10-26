using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Courses
{
    public class Details : PageModel
    {
        private readonly IMediator _mediator;

        public Details(IMediator mediator) => _mediator = mediator;

        public Model Data { get; private set; }

        public async Task OnGetAsync(Query query) => Data = await _mediator.Send(query);

        public record Query : IRequest<Model>
        {
            public int? Id { get; init; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public record Model
        {
            public int Id { get; init; }
            public string Title { get; init; }
            public int Credits { get; init; }
            [Display(Name = "Department")]
            public string DepartmentName { get; init; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Course, Model>();
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

            public Task<Model> Handle(Query message, CancellationToken token) => 
                _db.Courses
                .Where(i => i.Id == message.Id)
                .ProjectTo<Model>(_configuration)
                .SingleOrDefaultAsync(token);
        }
    }
}