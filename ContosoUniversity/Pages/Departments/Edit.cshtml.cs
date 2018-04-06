using System;
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

namespace ContosoUniversity.Pages.Departments
{
    public class Edit : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        public Edit(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public async Task<ActionResult> OnPostAsync(int id)
        {
            await _mediator.Send(Data);

            return this.RedirectToPageJson("Index");
        }

        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
        {
            public string Name { get; set; }

            public decimal? Budget { get; set; }

            public DateTime? StartDate { get; set; }

            public Instructor Administrator { get; set; }
            public int Id { get; set; }
            public byte[] RowVersion { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name).NotNull().Length(3, 50);
                RuleFor(m => m.Budget).NotNull();
                RuleFor(m => m.StartDate).NotNull();
                RuleFor(m => m.Administrator).NotNull();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Department, Command>().ReverseMap();
        }

        public class QueryHandler : AsyncRequestHandler<Query, Command>
        {
            private readonly SchoolContext _db;

            public QueryHandler(SchoolContext db) => _db = db;

            protected override async Task<Command> Handle(Query message) => await _db
                .Departments
                .Where(d => d.Id == message.Id)
                .ProjectTo<Command>()
                .SingleOrDefaultAsync();
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public CommandHandler(SchoolContext db) => _db = db;

            protected override async Task Handle(Command message)
            {
                var dept = await _db.Departments.FindAsync(message.Id);
                message.Administrator = await _db.Instructors.FindAsync(message.Administrator.Id);

                Mapper.Map(message, dept);
            }
        }
    }
}