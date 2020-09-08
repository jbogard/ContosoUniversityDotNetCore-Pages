using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Delete = ContosoUniversity.Pages.Departments.Delete;

namespace ContosoUniversity.IntegrationTests.Pages.Departments
{
    
    [Collection(nameof(SliceFixture))]
    public class DeleteTests
    {
        private readonly SliceFixture _fixture;

        public DeleteTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_delete_department()
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

            var command = new Delete.Command
            {
                Id = dept.Id,
                RowVersion = dept.RowVersion
            };

            await _fixture.SendAsync(command);

            var any = await _fixture.ExecuteDbContextAsync(db => db.Departments.Where(d => d.Id == command.Id).AnyAsync());

            any.ShouldBeFalse();
        }
    }
}