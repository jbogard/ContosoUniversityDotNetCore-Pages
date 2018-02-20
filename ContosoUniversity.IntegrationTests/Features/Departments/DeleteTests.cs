using System.Linq;
using ContosoUniversity.Pages.Departments;

namespace ContosoUniversity.IntegrationTests.Features.Departments
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using Models;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class DeleteTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_delete_department()
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