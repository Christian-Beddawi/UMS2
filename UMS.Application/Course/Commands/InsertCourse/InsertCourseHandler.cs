using System.Net;
using UMS.Application.Common;
using UMS.Infrastructure.Abstraction.EmailServiceInterface;
using UMS.Persistence;

namespace UMS.Application.Entities.Course.Commands.InsertCourse;

public class InsertCourseHandler : MediatR.IRequestHandler<InsertCourseCommand, string>
{
    private readonly umsContext _context;
    private readonly IEEmailService _emailSender;
    private readonly ICommonServices _common;

    public InsertCourseHandler(umsContext context, IEEmailService emailSender, ICommonServices common)
    {
        _context = context;
        _emailSender = emailSender;
        _common = common;
    }
    public async Task<string> Handle(InsertCourseCommand request, CancellationToken cancellationToken)
    {   
        //request.course.EnrolmentDateRange = _context.Courses.First().EnrolmentDateRange;
        string? role = _context.Users.Where(u => u.Id == request.UserID).Select(r => r.Role.Name).FirstOrDefault()?.ToString();
        if (role == "Admin")
        {
            if (!_common.CourseExists(request.Course.Name))
            {
                var res = _context.Courses.AddAsync(request.Course);
                var r=_context.SaveChanges();
            
                List<Domain.Models.User> students = _common.GetStudents();
                foreach (var student in students)
                {
                    var emailAddress = _common.GetUserEmail((int)student.Id);
                    string emailBody = "Dear student,\tKindly note note that a new course named '"
                                       + request.Course.Name + "' is now open.\tBest";
                    await _emailSender.SendEmailAsync(emailAddress, "New Course Opened", emailBody);
                }
                return "Inserted Successfully!";
            }

            return "This course already exists!";

        }
        return "You are not allowed to add courses !!";
    }
}