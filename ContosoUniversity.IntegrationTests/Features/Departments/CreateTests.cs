using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Pages.Departments;
using ContosoUniversity.Pages.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Features.Departments
{
    using static SliceFixture;

    public class CreateTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_create_new_department()
        {
            var adminId = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today
            });

            Create.Command command = null;

            await ExecuteDbContextAsync(async (db, mediator) =>
            {
                var admin = await db.Instructors.FindAsync(adminId);

                command = new Create.Command
                {
                    Budget = 10m,
                    Name = "Engineering",
                    StartDate = DateTime.Now.Date,
                    Administrator = admin
                };

                await mediator.Send(command);
            });

            var created = await ExecuteDbContextAsync(db => db.Departments.Where(d => d.Name == command.Name).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Budget.ShouldBe(command.Budget.GetValueOrDefault());
            created.StartDate.ShouldBe(command.StartDate.GetValueOrDefault());
            created.InstructorId.ShouldBe(adminId);
        }
    }
}