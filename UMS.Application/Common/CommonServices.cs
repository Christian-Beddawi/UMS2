using UMS.Application.Exceptions;
using UMS.Persistence;

namespace UMS.Application.Common;

public class CommonServices : ICommonServices
{
    private readonly umsContext _context;

    public CommonServices(umsContext context)
    {
        _context = context;
    }
    
    public string? GetRole(int userId)
    {
        string? role = _context.Users.Where(u => u.Id == userId).Select(r => r.Role.Name).FirstOrDefault()?.ToString();
        if (role == null)
            throw new UserNotFoundException("No user with the specified ID");
        return role;
    }

    public int? CheckCourseExists(string courseName)
    {
        var courseId = (int?)_context.Courses.Where(c => c.Name == courseName).Select(c => c.Id).FirstOrDefault();
        if (courseId == 0)
            throw new CourseNotFoundException("This course does not exist!");
        return courseId;
    }
}