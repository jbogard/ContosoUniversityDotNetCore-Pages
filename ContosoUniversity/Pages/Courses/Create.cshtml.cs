using System.Threading;
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


        public class Handler : IRequestHandler<Command, int>
        {
            private readonly SchoolContext _db;
            private readonly IMapper _mapper;

            public Handler(SchoolContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<int> Handle(Command message, CancellationToken token)
            {
                var course = _mapper.Map<Command, Course>(message);
                course.Id = message.Number;

                _db.Courses.Add(course);

                await _db.SaveChangesAsync(token);

                return course.Id;
            }
        }
    }
}