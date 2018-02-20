using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Pages.Students;
using ContosoUniversity.Models;
using Shouldly;
using Xunit;
using static ContosoUniversity.IntegrationTests.SliceFixture;

namespace ContosoUniversity.IntegrationTests.Features.Students
{
    public class DetailsTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_get_details()
        {
            var adminId = await SendAsync(new Pages.Instructors.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });

            var englishDept = new Department
            {
                InstructorID = adminId,
                Budget = 123m,
                Name = "English 101",
                StartDate = DateTime.Today,
            };
            await InsertAsync(englishDept);
            var deptId = englishDept.Id;

            var course1 = new Course
            {
                DepartmentID = deptId,
                Credits = 10,
                Id = NextCourseNumber(),
                Title = "Course 1"
            };
            var course2 = new Course
            {
                DepartmentID = deptId,
                Credits = 10,
                Id = NextCourseNumber(),
                Title = "Course 2"
            };
            await InsertAsync(course1, course2);

            var command = new Create.Command
            {
                FirstMidName = "Joe",
                LastName = "Schmoe",
                EnrollmentDate = new DateTime(2013, 1, 1)
            };
            var studentId = await SendAsync(command);

            var enrollment1 = new Enrollment
            {
                CourseID = course1.Id,
                Grade = Grade.A,
                StudentID = studentId,
            };
            var enrollment2 = new Enrollment
            {
                CourseID = course2.Id,
                Grade = Grade.F,
                StudentID = studentId,
            };
            await InsertAsync(enrollment1, enrollment2);

            var details = await SendAsync(new Details.Query {Id = studentId});

            details.ShouldNotBeNull();
            details.FirstMidName.ShouldBe(command.FirstMidName);
            details.LastName.ShouldBe(command.LastName);
            details.EnrollmentDate.ShouldBe(command.EnrollmentDate.GetValueOrDefault());
            details.Enrollments.Count.ShouldBe(2);
        }
    }
}