using System;
using System.Threading.Tasks;
using Xunit;
using static ContosoUniversity.IntegrationTests.SliceFixture;
using ContosoUniversity.Pages.Students;
using ContosoUniversity.Models;
using Shouldly;

namespace ContosoUniversity.IntegrationTests.Features.Students
{
    public class DeleteTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_get_delete_details()
        {
            var cmd = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = DateTime.Today
            };

            var studentId = await SendAsync(cmd);

            var query = new Delete.Query
            {
                Id = studentId
            };

            var result = await SendAsync(query);

            result.FirstMidName.ShouldBe(cmd.FirstMidName);
            result.LastName.ShouldBe(cmd.LastName);
            result.EnrollmentDate.ShouldBe(cmd.EnrollmentDate.GetValueOrDefault());
        }

        [Fact]
        public async Task Should_delete_student()
        {
            var createCommand = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = DateTime.Today
            };

            var studentId = await SendAsync(createCommand);

            var deleteCommand = new Delete.Command
            {
                ID = studentId
            };

            await SendAsync(deleteCommand);

            var student = await FindAsync<Student>(studentId);

            student.ShouldBeNull();
        }
    }
}