using System;
using System.Threading.Tasks;
using Xunit;
using static ContosoUniversity.IntegrationTests.SliceFixture;
using ContosoUniversity.Pages.Students;
using ContosoUniversity.Models;
using Shouldly;

namespace ContosoUniversity.IntegrationTests.Features.Students
{
    public class EditTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_get_edit_details()
        {
            var cmd = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = DateTime.Today
            };

            var studentId = await SendAsync(cmd);

            var query = new Edit.Query
            {
                Id = studentId
            };

            var result = await SendAsync(query);

            result.FirstMidName.ShouldBe(cmd.FirstMidName);
            result.LastName.ShouldBe(cmd.LastName);
            result.EnrollmentDate.ShouldBe(cmd.EnrollmentDate);
        }

        [Fact]
        public async Task Should_edit_student()
        {
            var createCommand = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = DateTime.Today
            };

            var studentId = await SendAsync(createCommand);

            var editCommand = new Edit.Command
            {
                Id = studentId,
                FirstMidName = "Mary",
                LastName = "Smith",
                EnrollmentDate = DateTime.Today.AddYears(-1)
            };

            await SendAsync(editCommand);

            var student = await FindAsync<Student>(studentId);

            student.ShouldNotBeNull();
            student.FirstMidName.ShouldBe(editCommand.FirstMidName);
            student.LastName.ShouldBe(editCommand.LastName);
            student.EnrollmentDate.ShouldBe(editCommand.EnrollmentDate.GetValueOrDefault());
        }
    }
}