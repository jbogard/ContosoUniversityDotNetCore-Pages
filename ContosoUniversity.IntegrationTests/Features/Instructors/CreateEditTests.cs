namespace ContosoUniversity.IntegrationTests.Features.Instructors
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Pages.Instructors;
    using Models;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class CreateEditTests : IntegrationTestBase
    {
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
                Id = NextCourseNumber()
            };
            var english201 = new Course
            {
                Department = englishDept,
                Title = "English 201",
                Credits = 4,
                Id = NextCourseNumber()
            };

            await InsertAsync(englishDept, english101, english201);

            var command = new CreateEdit.Command
            {
                FirstMidName = "Jerry",
                LastName = "Seinfeld",
                HireDate = DateTime.Today,
                OfficeAssignmentLocation = "Houston",
                SelectedCourses = new [] {english101.Id.ToString(), english201.Id.ToString()}
            };

            var id = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Instructors.Where(i => i.Id == id).Include(i => i.CourseAssignments).Include(i => i.OfficeAssignment).SingleOrDefaultAsync());

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
                Id = NextCourseNumber()
            };
            var english201 = new Course
            {
                Department = englishDept,
                Title = "English 201",
                Credits = 4,
                Id = NextCourseNumber()
            };

            await InsertAsync(englishDept, english101, english201);

            var instructorId = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                OfficeAssignmentLocation = "Austin",
                HireDate = DateTime.Today,
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

            await SendAsync(command);

            var edited = await ExecuteDbContextAsync(db => db.Instructors.Where(i => i.Id == instructorId).Include(i => i.CourseAssignments).Include(i => i.OfficeAssignment).SingleOrDefaultAsync());

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
                Id = NextCourseNumber()
            };
            var english201 = new Course
            {
                Department = englishDept,
                Title = "English 201",
                Credits = 4,
                Id = NextCourseNumber()
            };
            await InsertAsync(englishDept, english101, english201);

            var instructorId = await SendAsync(new CreateEdit.Command
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

            await SendAsync(command);

            var edited = await ExecuteDbContextAsync(db => db.Instructors.Where(i => i.Id == instructorId).Include(i => i.CourseAssignments).Include(i => i.OfficeAssignment).SingleOrDefaultAsync());

            edited.FirstMidName.ShouldBe(command.FirstMidName);
            edited.LastName.ShouldBe(command.LastName);
            edited.HireDate.ShouldBe(command.HireDate.GetValueOrDefault());
            edited.OfficeAssignment.ShouldNotBeNull();
            edited.OfficeAssignment.Location.ShouldBe(command.OfficeAssignmentLocation);
            edited.CourseAssignments.Count.ShouldBe(1);
            edited.CourseAssignments.First().CourseID.ShouldBe(english201.Id);
        }

    }
}