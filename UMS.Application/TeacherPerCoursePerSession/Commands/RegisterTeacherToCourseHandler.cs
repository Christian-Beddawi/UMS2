using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using UMS.Application.Common;
using UMS.Application.Entities.TeacherPerCourse.Commands;
using UMS.Domain.Models;
using UMS.Persistence;

namespace UMS.Application.Entities.TeacherPerCoursePerSession.Commands;

public class RegisterTeacherToCourseHandler :IRequestHandler<RegisterTeacherToCourseCommand,string>
{
    private readonly umsContext _context;
    private readonly ICommonServices _common;

    public RegisterTeacherToCourseHandler(ICommonServices common, umsContext context)
    {
        _context = context;
        _common = common;
    }
    
    public async Task<string> Handle(RegisterTeacherToCourseCommand request, CancellationToken cancellationToken)
    {
        string role = _common.GetRole(request.RegToCourse.TeacherId);
        if (role == "Teacher")
        {
            int? courseId = _common.CheckCourseExists(request.RegToCourse.CourseName);
            Domain.Models.TeacherPerCourse teacherPerCourse = new Domain.Models.TeacherPerCourse()
                {
                    CourseId = (long) courseId,
                    TeacherId = request.RegToCourse.TeacherId
                };
            await _context.TeacherPerCourses.AddAsync(teacherPerCourse, cancellationToken);
            await _context.SaveChangesAsync();
            Domain.Models.SessionTime sessionTime = new Domain.Models.SessionTime()
            {
                StartTime = request.RegToCourse.StartTime, 
                EndTime = request.RegToCourse.EndTime
            };
            await _context.SessionTimes.AddAsync(sessionTime);
            await _context.SaveChangesAsync();
            await _context.TeacherPerCoursePerSessionTimes.AddAsync(new TeacherPerCoursePerSessionTime()
            {
                TeacherPerCourseId = teacherPerCourse.Id,
                SessionTimeId = sessionTime.Id
            });
            _context.SaveChanges();
            return "Course Registered Successfully!";
        }
        return "You are not a teacher ";
    }
}