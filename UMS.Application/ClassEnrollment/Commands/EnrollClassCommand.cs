using MediatR;
using UMS.WebAPI.DTO;

namespace UMS.Application.ClassEnrollment.Commands;

public class EnrollClassCommand : IRequest<string>
{
    public EnrollClass EnrollmentInfo;
    public EnrollClassCommand(EnrollClass enrollmentInfo)
    {
        EnrollmentInfo = enrollmentInfo;
    }
}