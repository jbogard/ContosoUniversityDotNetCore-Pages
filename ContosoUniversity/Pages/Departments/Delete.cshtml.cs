﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Departments
{
    public class Delete : PageModel
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public Command Data { get; set; }

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public async Task<ActionResult> OnPostAsync()
        {
            await _mediator.Send(Data);

            return this.RedirectToPageJson("Index");
        }

        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
        {
            public string Name { get; set; }

            public decimal Budget { get; set; }

            public DateTime StartDate { get; set; }

            public int Id { get; set; }

            [Display(Name = "Administrator")]
            public string AdministratorFullName { get; set; }

            public byte[] RowVersion { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Department, Command>();
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly SchoolContext _db;

            public QueryHandler(SchoolContext db) => _db = db;

            public async Task<Command> Handle(Query message, CancellationToken token) => await _db
                .Departments
                .Where(d => d.Id == message.Id)
                .ProjectTo<Command>()
                .SingleOrDefaultAsync(token);
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public CommandHandler(SchoolContext db) => _db = db;

            protected override async Task Handle(Command message, CancellationToken token)
            {
                var department = await _db.Departments.FindAsync(new object[] { message.Id }, token);

                _db.Departments.Remove(department);
            }
        }
    }
}