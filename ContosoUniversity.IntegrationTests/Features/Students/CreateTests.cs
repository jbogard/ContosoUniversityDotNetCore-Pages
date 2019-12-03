using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Students;
using Shouldly;
using Xunit;
using static ContosoUniversity.IntegrationTests.SliceFixture;

namespace ContosoUniversity.IntegrationTests.Features.Students
{
    public class CreateTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_create_student()
        {
            var cmd = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = DateTime.Today
            };

            var studentId = await SendAsync(cmd);

            var student = await FindAsync<Student>(studentId);

            student.ShouldNotBeNull();
            student.FirstMidName.ShouldBe(cmd.FirstMidName);
            student.LastName.ShouldBe(cmd.LastName);
            student.EnrollmentDate.ShouldBe(cmd.EnrollmentDate.GetValueOrDefault());
        }
    }
}