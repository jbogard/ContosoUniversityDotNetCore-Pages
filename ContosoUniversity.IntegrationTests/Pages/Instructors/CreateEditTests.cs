using System;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Pages.Instructors;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace ContosoUniversity.IntegrationTests.Pages.Instructors
{
    
    [Collection(nameof(SliceFixture))]
    public class CreateEditTests
    {
        private readonly SliceFixture _fixture;

        public CreateEditTests(SliceFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_create_new_instructor()
        {
            var englishDept = new Department
            {
                Name = "English",
                StartDate = DateTime.Today
            };
            var english101 = new Course
            {
                Department = englishDept,
                Title = "English 101",
                Credits = 4,
                Id = _fixture.NextCourseNumber()
            };
            var english201 = new Course
            {
                Department = englishDept,
                Title = "English 201",
                Credits = 4,
                Id = _fixture.NextCourseNumber()
            };

            await _fixture.InsertAsync(englishDept, english101, english201);

            var command = new CreateEdit.Command
            {
                FirstMidName = "Jerry",
                LastName = "Seinfeld",
                HireDate = DateTime.Today,
                OfficeAssignmentLocation = "Houston",
                SelectedCourses = new [] {english101.Id.ToString(), english201.Id.ToString()}
            };

            var id = await _fixture.SendAsync(command);

            var created = await _fixture.ExecuteDbContextAsync(db => db.Instructors.Where(i => i.Id == id).Include(i => i.CourseAssignments).Include(i => i.OfficeAssignment).SingleOrDefaultAsync());

            created.FirstMidName.ShouldBe(command.FirstMidName);
            created.LastName.ShouldBe(command.LastName);
            created.HireDate.ShouldBe(command.HireDate.GetValueOrDefault());
            created.OfficeAssignment.ShouldNotBeNull();
            created.OfficeAssignment.Location.ShouldBe(command.OfficeAssignmentLocation);
            created.CourseAssignments.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Should_edit_instructor_details()
        {
            var englishDept = new Department
            {
                Name = "English",
                StartDate = DateTime.Today
            };
            var english101 = new Course
            {
                Department = englishDept,
                Title = "English 101",
                Credits = 4,
                Id = _fixture.NextCourseNumber()
            };
            var english201 = new Course
            {
                Department = englishDept,
                Title = "English 201",
                Credits = 4,
                Id = _fixture.NextCourseNumber()
            };

            await _fixture.InsertAsync(englishDept, english101, english201);

            var instructorId = await _fixture.SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                OfficeAssignmentLocation = "Austin",
                HireDate = DateTime.Today
            });

            var command = new CreateEdit.Command
            {
                FirstMidName = "Jerry",
                LastName = "Seinfeld",
                HireDate = DateTime.Today,
                OfficeAssignmentLocation = "Houston",
                SelectedCourses = new string[0],
                Id = instructorId
            };

            await _fixture.SendAsync(command);

            var edited = await _fixture.ExecuteDbContextAsync(db => db.Instructors.Where(i => i.Id == instructorId).Include(i => i.CourseAssignments).Include(i => i.OfficeAssignment).SingleOrDefaultAsync());

            edited.FirstMidName.ShouldBe(command.FirstMidName);
            edited.LastName.ShouldBe(command.LastName);
            edited.HireDate.ShouldBe(command.HireDate.GetValueOrDefault());
            edited.OfficeAssignment.ShouldNotBeNull();
            edited.OfficeAssignment.Location.ShouldBe(command.OfficeAssignmentLocation);
        }

        [Fact]
        public async Task Should_merge_course_instructors()
        {
            var englishDept = new Department
            {
                Name = "English",
                StartDate = DateTime.Today
            };
            var english101 = new Course
            {
                Department = englishDept,
                Title = "English 101",
                Credits = 4,
                Id = _fixture.NextCourseNumber()
            };
            var english201 = new Course
            {
                Department = englishDept,
                Title = "English 201",
                Credits = 4,
                Id = _fixture.NextCourseNumber()
            };
            await _fixture.InsertAsync(englishDept, english101, english201);

            var instructorId = await _fixture.SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                OfficeAssignmentLocation = "Austin",
                HireDate = DateTime.Today,
                SelectedCourses = new[] { english101.Id.ToString() }
            });

            var command = new CreateEdit.Command
            {
                FirstMidName = "Jerry",
                LastName = "Seinfeld",
                HireDate = DateTime.Today,
                OfficeAssignmentLocation = "Houston",
                SelectedCourses = new[] { english201.Id.ToString() },
                Id = instructorId
            };

            await _fixture.SendAsync(command);

            var edited = await _fixture.ExecuteDbContextAsync(db => db.Instructors.Where(i => i.Id == instructorId).Include(i => i.CourseAssignments).Include(i => i.OfficeAssignment).SingleOrDefaultAsync());

            edited.FirstMidName.ShouldBe(command.FirstMidName);
            edited.LastName.ShouldBe(command.LastName);
            edited.HireDate.ShouldBe(command.HireDate.GetValueOrDefault());
            edited.OfficeAssignment.ShouldNotBeNull();
            edited.OfficeAssignment.Location.ShouldBe(command.OfficeAssignmentLocation);
            edited.CourseAssignments.Count.ShouldBe(1);
            edited.CourseAssignments.First().CourseId.ShouldBe(english201.Id);
        }

    }
}