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
    public class Details
    {
        public class Query : IRequest<Model>
        {
            public int? Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }
            [Display(Name = "Department")]
            public string DepartmentName { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db) => _db = db;

            protected override Task<Model> HandleCore(Query message) => 
                _db.Courses
                .Where(i => i.Id == message.Id)
                .ProjectTo<Model>()
                .SingleOrDefaultAsync();
        }
    }
}