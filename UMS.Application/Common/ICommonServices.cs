using NpgsqlTypes;
using UMS.WebAPI.DTO;

namespace UMS.Application.Common;

public interface ICommonServices
{
    public string? GetRole(int userId);
    public int? CheckCourseExists(string courseName);
    public int GetClassId(EnrollClass enrollmentInfo);
    public string GetUserEmail(int userId);
    public NpgsqlRange<DateOnly>? GetCourseDateRange(string courseName);
    public bool CheckCourseCapacity(string courseName);
    public List<Domain.Models.User> GetStudents();
    public bool CourseExists(string courseName);
    public bool TeacherPerCourseExists(string courseName, string teacherName);
    public string GetTeacherName(int teacherId);
    public int CheckSessionExists(DateTime startTime, DateTime endTime);
    public bool CheckClassExists(int classId, int sessionId);
}