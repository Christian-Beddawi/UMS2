using System.Net;
using UMS.Application.Common;
using UMS.Persistence;

namespace UMS.Application.Entities.Course.Commands.InsertCourse;

public class InsertCourseHandler : MediatR.IRequestHandler<InsertCourseCommand, string>
{
    private readonly umsContext _context = new umsContext();
    public async Task<string> Handle(InsertCourseCommand request, CancellationToken cancellationToken)
    {   
        //request.course.EnrolmentDateRange = _context.Courses.First().EnrolmentDateRange;
        string? role = _context.Users.Where(u => u.Id == request.UserID).Select(r => r.Role.Name).FirstOrDefault()?.ToString();
        Console.WriteLine("Role : : : : : " + role);
        if (role == "Admin")
        {
            var res = _context.Courses.AddAsync(request.Course);
            var r=_context.SaveChanges();
            return "Inserted Successfully!";
        }
        return "You are not allowed to add courses !!";
    }
}