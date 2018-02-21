using System.Linq;

namespace ContosoUniversity.IntegrationTests.Features.Departments
{
    using System;
    using System.Threading.Tasks;
    using Models;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_list_departments()
        {
            var adminId = await SendAsync(new Pages.Instructors.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });

            var dept = new Department
            {
                Name = "History",
                InstructorID = adminId,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            var dept2 = new Department
            {
                Name = "English",
                InstructorID = adminId,
                Budget = 456m,
                StartDate = DateTime.Today
            };

            await InsertAsync(dept, dept2);

            var query = new Pages.Departments.Index.Query();

            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThanOrEqualTo(2);
            result.Select(m => m.Id).ShouldContain(dept.Id);
            result.Select(m => m.Id).ShouldContain(dept2.Id);
        }

    }
}