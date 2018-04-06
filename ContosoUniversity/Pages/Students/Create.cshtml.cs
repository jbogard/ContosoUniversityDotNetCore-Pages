using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Students
{
    public class Create : PageModel
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public Command Data { get; set; }

        public void OnGet() => Data = new Command();

        public async Task<IActionResult> OnPostAsync()
        {
            await _mediator.Send(Data);

            return this.RedirectToPageJson(nameof(Index));
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Command, Student>(MemberList.Source);
        }

        public class Command : IRequest<int>
        {
            public string LastName { get; set; }

            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }

            public DateTime? EnrollmentDate { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.LastName).NotNull().Length(1, 50);
                RuleFor(m => m.FirstMidName).NotNull().Length(1, 50);
                RuleFor(m => m.EnrollmentDate).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command, int>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db) => _db = db;

            protected override async Task<int> Handle(Command message)
            {
                var student = Mapper.Map<Command, Student>(message);

                _db.Students.Add(student);

                await _db.SaveChangesAsync();

                return student.Id;
            }
        }
    }
}