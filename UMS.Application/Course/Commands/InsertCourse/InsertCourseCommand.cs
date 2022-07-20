using MediatR;

namespace UMS.Application.Entities.Course.Commands.InsertCourse;

public class InsertCourseCommand : IRequest<string>
{
    public Domain.Models.Course Course;
    public int UserID;

    public InsertCourseCommand(Domain.Models.Course course, int userId)
    {
        this.Course = course;
        UserID = userId;
    }
}