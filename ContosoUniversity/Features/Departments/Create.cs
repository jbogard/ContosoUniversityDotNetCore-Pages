using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using AutoMapper;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using FluentValidation;
using MediatR;

namespace ContosoUniversity.Features.Departments
{
    public class Create
    {
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

        public class Command : IRequest<int>
        {
            [StringLength(50, MinimumLength = 3)]
            public string Name { get; set; }

            [DataType(DataType.Currency)]
            [Column(TypeName = "money")]
            public decimal? Budget { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? StartDate { get; set; }

            public Instructor Administrator { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command, int>
        {
            private readonly SchoolContext _context;

            public CommandHandler(SchoolContext context) => _context = context;

            protected override async Task<int> HandleCore(Command message)
            {
                var department = Mapper.Map<Command, Department>(message);

                _context.Departments.Add(department);

                await _context.SaveChangesAsync();

                return department.Id;
            }
        }
    }
}