using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using Shouldly;
using Xunit;
using Details = ContosoUniversity.Pages.Departments.Details;

namespace ContosoUniversity.IntegrationTests.Pages.Departments
{
    
    [Collection(nameof(SliceFixture))]
    public class DetailsTests
    {
        private readonly SliceFixture _fixture;

        public DetailsTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_get_department_details()
        {
            var adminId = await _fixture.SendAsync(new CreateEdit.Command
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
            await _fixture.InsertAsync(dept);

            var query = new Details.Query
            {
                Id = dept.Id
            };

            var result = await _fixture.SendAsync(query);
            var admin = await _fixture.FindAsync<Instructor>(adminId);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(dept.Name);
            result.AdministratorFullName.ShouldBe(admin.FullName);
        }

    }
}