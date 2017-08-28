using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using FluentValidation;
using MediatR;

namespace ContosoUniversity.Features.Students
{
    public class Create
    {
        public class Command : IRequest
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

        public class Handler : IRequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db) => _db = db;

            public void Handle(Command message) 
                => _db.Students.Add(Mapper.Map<Command, Student>(message));
        }
    }
}