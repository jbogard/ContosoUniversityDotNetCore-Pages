using System;
using System.Threading.Tasks;
using Xunit;
using static ContosoUniversity.IntegrationTests.SliceFixture;
using ContosoUniversity.Pages.Students;
using ContosoUniversity.Models;
using Shouldly;

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