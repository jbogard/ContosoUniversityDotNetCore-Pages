using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using Shouldly;
using Xunit;
using Index = ContosoUniversity.Pages.Departments.Index;

namespace ContosoUniversity.IntegrationTests.Features.Departments
{
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_list_departments()
        {
            var adminId = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today
            });

            var dept = new Department
            {
                Name = "History",
                InstructorId = adminId,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            var dept2 = new Department
            {
                Name = "English",
                InstructorId = adminId,
                Budget = 456m,
                StartDate = DateTime.Today
            };

            await InsertAsync(dept, dept2);

            var query = new Index.Query();

            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThanOrEqualTo(2);
            result.Select(m => m.Id).ShouldContain(dept.Id);
            result.Select(m => m.Id).ShouldContain(dept2.Id);
        }

    }
}