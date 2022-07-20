using MediatR;
using UMS.WebAPI.DTO;

namespace UMS.Application.Entities.TeacherPerCoursePerSession.Commands;

public class RegisterTeacherToCourseCommand : IRequest<string>
{
    public RegisterToCourse RegToCourse;

    public RegisterTeacherToCourseCommand(RegisterToCourse regToCourse)
    {
        RegToCourse = regToCourse;
    }
}