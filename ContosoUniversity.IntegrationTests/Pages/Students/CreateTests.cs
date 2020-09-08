using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Students;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Students
{
    [Collection(nameof(SliceFixture))]
    public class CreateTests
    {
        private readonly SliceFixture _fixture;

        public CreateTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_create_student()
        {
            var cmd = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = DateTime.Today
            };

            var studentId = await _fixture.SendAsync(cmd);

            var student = await _fixture.FindAsync<Student>(studentId);

            student.ShouldNotBeNull();
            student.FirstMidName.ShouldBe(cmd.FirstMidName);
            student.LastName.ShouldBe(cmd.LastName);
            student.EnrollmentDate.ShouldBe(cmd.EnrollmentDate.GetValueOrDefault());
        }
    }
}