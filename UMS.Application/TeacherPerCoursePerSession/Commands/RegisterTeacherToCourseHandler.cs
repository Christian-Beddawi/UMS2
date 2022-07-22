using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using UMS.Application.Common;
using UMS.Application.Entities.TeacherPerCourse.Commands;
using UMS.Domain.Models;
using UMS.Infrastructure.Abstraction.EmailServiceInterface;
using UMS.Persistence;

namespace UMS.Application.Entities.TeacherPerCoursePerSession.Commands;

public class RegisterTeacherToCourseHandler :IRequestHandler<RegisterTeacherToCourseCommand,string>
{
    private readonly umsContext _context;
    private readonly ICommonServices _common;
    private readonly IEEmailService _emailSender;
    public RegisterTeacherToCourseHandler(ICommonServices common, umsContext context, IEEmailService emailSender)
    {
        _context = context;
        _common = common;
        _emailSender = emailSender;
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
            string teacherName = _common.GetTeacherName(request.RegToCourse.TeacherId);
            if (!_common.TeacherPerCourseExists(request.RegToCourse.CourseName, teacherName))
            {
                await _context.TeacherPerCourses.AddAsync(teacherPerCourse, cancellationToken);
                await _context.SaveChangesAsync();
            }
            Domain.Models.SessionTime sessionTime = new Domain.Models.SessionTime()
            {
                StartTime = request.RegToCourse.StartTime, 
                EndTime = request.RegToCourse.EndTime
            };
            int sessionTimeId = _common.CheckSessionExists(request.RegToCourse.StartTime, request.RegToCourse.EndTime);
            if (sessionTimeId==0)
            {
                await _context.SessionTimes.AddAsync(sessionTime);
                await _context.SaveChangesAsync();
                sessionTimeId = (int) sessionTime.Id;
            }
            
            TeacherPerCoursePerSessionTime tPC=new TeacherPerCoursePerSessionTime()
            {
                TeacherPerCourseId = teacherPerCourse.Id,
                SessionTimeId = sessionTimeId
            };
            

            if (!_common.CheckClassExists((int)teacherPerCourse.Id, sessionTimeId))
            {
                await _context.TeacherPerCoursePerSessionTimes.AddAsync(tPC);
                _context.SaveChanges();
                List<Domain.Models.User> students = _common.GetStudents();
                foreach (var student in students)
                {
                    var emailAddress = _common.GetUserEmail((int)student.Id);
                    string emailBody = "Dear student, Kindly note note that a new schedule of the course '"
                                       + request.RegToCourse.CourseName + "' has been registered. Best";
                    await _emailSender.SendEmailAsync(emailAddress, "New Course Schedule", emailBody);
                }
                return "Course Registered Successfully!";
            }
            return "Class already exists!";
        }
        return "You are not a teacher ";
    }
}