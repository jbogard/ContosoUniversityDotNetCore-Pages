using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages
{
    public class AboutPage : PageModel
    {
        private readonly SchoolContext _context;

        public AboutPage(SchoolContext context)
        {
            _context = context;
        }

        public IEnumerable<EnrollmentDateGroup> Data { get; private set; }

        public async Task OnGetAsync()
        {
            var groups = await _context
                .Students
                .GroupBy(x => x.EnrollmentDate)
                .Select(x => new EnrollmentDateGroup
                {
                    EnrollmentDate = x.Key,
                    StudentCount = x.Count()
                })
                .ToListAsync();

            Data = groups;
        }
    }
}