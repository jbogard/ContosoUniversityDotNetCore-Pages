using AutoMapper;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;

namespace ContosoUniversity.Features.Courses
{
    public class Create
    {
        public class Command : IRequest
        {
            [IgnoreMap]
            public int Number { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }
            public Department Department { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db) => _db = db;

            public void Handle(Command message)
            {
                var course = Mapper.Map<Command, Course>(message);
                course.Id = message.Number;

                _db.Courses.Add(course);
            }
        }
    }
}