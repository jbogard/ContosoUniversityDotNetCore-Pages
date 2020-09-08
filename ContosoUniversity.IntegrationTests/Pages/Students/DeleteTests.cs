using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Students;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Students
{
    [Collection(nameof(SliceFixture))]
    public class DeleteTests
    {
        private readonly SliceFixture _fixture;

        public DeleteTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_get_delete_details()
        {
            var cmd = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = DateTime.Today
            };

            var studentId = await _fixture.SendAsync(cmd);

            var query = new Delete.Query
            {
                Id = studentId
            };

            var result = await _fixture.SendAsync(query);

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

            var studentId = await _fixture.SendAsync(createCommand);

            var deleteCommand = new Delete.Command
            {
                Id = studentId
            };

            await _fixture.SendAsync(deleteCommand);

            var student = await _fixture.FindAsync<Student>(studentId);

            student.ShouldBeNull();
        }
    }
}