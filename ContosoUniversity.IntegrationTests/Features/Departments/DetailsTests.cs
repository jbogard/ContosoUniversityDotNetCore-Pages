using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using Shouldly;
using Xunit;
using Details = ContosoUniversity.Pages.Departments.Details;

namespace ContosoUniversity.IntegrationTests.Features.Departments
{
    using static SliceFixture;

    public class DetailsTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_get_department_details()
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
            await InsertAsync(dept);

            var query = new Details.Query
            {
                Id = dept.Id
            };

            var result = await SendAsync(query);
            var admin = await FindAsync<Instructor>(adminId);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(dept.Name);
            result.AdministratorFullName.ShouldBe(admin.FullName);
        }

    }
}