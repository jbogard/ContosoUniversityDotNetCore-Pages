using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Delete = ContosoUniversity.Pages.Departments.Delete;

namespace ContosoUniversity.IntegrationTests.Features.Departments
{
    using static SliceFixture;

    public class DeleteTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_delete_department()
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

            var command = new Delete.Command
            {
                Id = dept.Id,
                RowVersion = dept.RowVersion
            };

            await SendAsync(command);

            var any = await ExecuteDbContextAsync(db => db.Departments.Where(d => d.Id == command.Id).AnyAsync());

            any.ShouldBeFalse();
        }
    }
}