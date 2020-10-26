using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Departments
{
    public class Delete : PageModel
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public Command Data { get; set; }

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public async Task<ActionResult> OnPostAsync()
        {
            await _mediator.Send(Data);

            return this.RedirectToPageJson("Index");
        }

        public record Query : IRequest<Command>
        {
            public int Id { get; init; }
        }

        public record Command : IRequest
        {
            public string Name { get; init; }

            public decimal Budget { get; init; }

            public DateTime StartDate { get; init; }

            public int Id { get; init; }

            [Display(Name = "Administrator")]
            public string AdministratorFullName { get; init; }

            public byte[] RowVersion { get; init; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Department, Command>();
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly SchoolContext _db;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(SchoolContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Command> Handle(Query message, CancellationToken token) => await _db
                .Departments
                .Where(d => d.Id == message.Id)
                .ProjectTo<Command>(_configuration)
                .SingleOrDefaultAsync(token);
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public CommandHandler(SchoolContext db) => _db = db;

            public async Task<Unit> Handle(Command message, CancellationToken token)
            {
                var department = await _db.Departments.FindAsync(message.Id);

                _db.Departments.Remove(department);

                return default;
            }
        }
    }
}