namespace UMS.Application.Exceptions;

public class SessionNotFoundException : Exception
{ 
   public SessionNotFoundException(string message) : base(message){ }
}