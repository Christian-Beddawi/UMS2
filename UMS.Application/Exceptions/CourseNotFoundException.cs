namespace UMS.Application.Exceptions;

public class CourseNotFoundException : Exception
{
    public CourseNotFoundException(string message) : base(message){}
    
}