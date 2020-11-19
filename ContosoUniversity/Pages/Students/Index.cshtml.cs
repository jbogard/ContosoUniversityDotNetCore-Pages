using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Students
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator) => _mediator = mediator;

        public Result Data { get; private set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
            => Data = await _mediator.Send(new Query { CurrentFilter = currentFilter, Page = pageIndex, SearchString = searchString, SortOrder = sortOrder});

        public record Query : IRequest<Result>
        {
            public string SortOrder { get; init; }
            public string CurrentFilter { get; init; }
            public string SearchString { get; init; }
            public int? Page { get; init; }
        }

        public record Result
        {
            public string CurrentSort { get; init; }
            public string NameSortParm { get; init; }
            public string DateSortParm { get; init; }
            public string CurrentFilter { get; init; }
            public string SearchString { get; init; }

            public PaginatedList<Model> Results { get; init; }
        }

        public record Model
        {
            public int Id { get; init; }
            [Display(Name = "First Name")]
            public string FirstMidName { get; init; }
            public string LastName { get; init; }
            public DateTime EnrollmentDate { get; init; }
            public int EnrollmentsCount { get; init; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Student, Model>();
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly SchoolContext _db;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(SchoolContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Result> Handle(Query message, CancellationToken token)
            {
                var searchString = message.SearchString ?? message.CurrentFilter;

                IQueryable<Student> students = _db.Students;
                if (!string.IsNullOrEmpty(searchString))
                {
                    students = students.Where(s => s.LastName.Contains(searchString)
                                                   || s.FirstMidName.Contains(searchString));
                }

                students = message.SortOrder switch
                {
                    "name_desc" => students.OrderByDescending(s => s.LastName),
                    "Date" => students.OrderBy(s => s.EnrollmentDate),
                    "date_desc" => students.OrderByDescending(s => s.EnrollmentDate),
                    _ => students.OrderBy(s => s.LastName)
                };

                int pageSize = 3;
                int pageNumber = (message.SearchString == null ? message.Page : 1) ?? 1;

                var results = await students
                    .ProjectTo<Model>(_configuration)
                    .PaginatedListAsync(pageNumber, pageSize);

                var model = new Result
                {
                    CurrentSort = message.SortOrder,
                    NameSortParm = string.IsNullOrEmpty(message.SortOrder) ? "name_desc" : "",
                    DateSortParm = message.SortOrder == "Date" ? "date_desc" : "Date",
                    CurrentFilter = searchString,
                    SearchString = searchString,
                    Results = results
                };      

                return model;
            }
        }
    }
}