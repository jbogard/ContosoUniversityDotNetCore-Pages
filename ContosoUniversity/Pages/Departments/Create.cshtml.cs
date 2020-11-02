using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Departments
{
    public class Create : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        public Create(IMediator mediator) => _mediator = mediator;

        public async Task<ActionResult> OnPostAsync()
        {
            await _mediator.Send(Data);

            return this.RedirectToPageJson("Index");
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

        public record Command : IRequest<int>
        {
            [StringLength(50, MinimumLength = 3)]
            public string Name { get; init; }

            [DataType(DataType.Currency)]
            [Column(TypeName = "money")]
            public decimal? Budget { get; init; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? StartDate { get; init; }

            public Instructor Administrator { get; init; }
        }

        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly SchoolContext _context;

            public CommandHandler(SchoolContext context) => _context = context;

            public async Task<int> Handle(Command message, CancellationToken token)
            {
                var department = new Department
                {
                    Administrator = message.Administrator,
                    Budget = message.Budget!.Value,
                    Name = message.Name,
                    StartDate = message.StartDate!.Value
                };

                await _context.Departments.AddAsync(department, token);

                await _context.SaveChangesAsync(token);

                return department.Id;
            }
        }
    }
}