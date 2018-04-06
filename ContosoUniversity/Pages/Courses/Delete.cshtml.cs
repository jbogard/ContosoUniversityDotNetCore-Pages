using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Courses
{
    public class Delete : PageModel
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public Command Data { get; set; }

        public async Task OnGetAsync(Query query) => Data = await _mediator.Send(query);

        public async Task<IActionResult> OnPostAsync()
        {
            await _mediator.Send(Data);

            return this.RedirectToPageJson(nameof(Index));
        }

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Course, Command>();
        }

        public class QueryHandler : AsyncRequestHandler<Query, Command>
        {
            private readonly SchoolContext _db;

            public QueryHandler(SchoolContext db) => _db = db;

            protected override Task<Command> Handle(Query message) =>
                _db.Courses
                    .Where(c => c.Id == message.Id)
                    .ProjectTo<Command>()
                    .SingleOrDefaultAsync();
        }

        public class Command : IRequest
        {
            [Display(Name = "Number")]
            public int Id { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }

            [Display(Name = "Department")]
            public string DepartmentName { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public CommandHandler(SchoolContext db) => _db = db;

            protected override async Task Handle(Command message)
            {
                var course = await _db.Courses.FindAsync(message.Id);

                _db.Courses.Remove(course);
            }
        }
    }
}