using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Features.Instructors
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
            public int? ID { get; set; }

            public string LastName { get; set; }
            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime? HireDate { get; set; }

            [Display(Name = "Location")]
            public string OfficeAssignmentLocation { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Model>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db) => _db = db;

            public Task<Model> Handle(Query message) => _db
                .Instructors
                .Where(i => i.Id == message.Id)
                .ProjectTo<Model>()
                .SingleOrDefaultAsync();
        }
    }
}