using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Features.Courses
{
    public class Delete
    {
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

        public class QueryHandler : AsyncRequestHandler<Query, Command>
        {
            private readonly SchoolContext _db;

            public QueryHandler(SchoolContext db) => _db = db;

            protected override Task<Command> HandleCore(Query message) =>
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

            protected override async Task HandleCore(Command message)
            {
                var course = await _db.Courses.FindAsync(message.Id);

                _db.Courses.Remove(course);
            }
        }
    }
}