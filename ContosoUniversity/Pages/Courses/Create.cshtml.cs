using System.Threading.Tasks;
using AutoMapper;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Courses
{
    public class Create : PageModel
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public Command Data { get; set; }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            await _mediator.Send(Data);

            return this.RedirectToPageJson("Index");
        }

        public class Command : IRequest<int>
        {
            [IgnoreMap]
            public int Number { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }
            public Department Department { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() =>
                CreateMap<Command, Course>(MemberList.Source)
                    .ForSourceMember(c => c.Number, opt => opt.Ignore());
        }


        public class Handler : AsyncRequestHandler<Command, int>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db) => _db = db;

            protected override async Task<int> HandleCore(Command message)
            {
                var course = Mapper.Map<Command, Course>(message);
                course.Id = message.Number;

                _db.Courses.Add(course);

                await _db.SaveChangesAsync();

                return course.Id;
            }
        }
    }
}