namespace UMS.Application.Common;

public interface ICommonServices
{
    public string? GetRole(int userId);
    public int? CheckCourseExists(string courseName);
}