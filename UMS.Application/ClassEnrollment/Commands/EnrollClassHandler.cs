using MediatR;
using UMS.Application.Common;
using UMS.Domain.Models;
using UMS.Infrastructure.Abstraction.EmailServiceInterface;
using UMS.Persistence;
using UMS.WebAPI.DTO;

namespace UMS.Application.ClassEnrollment.Commands;

public class EnrollClassHandler : IRequestHandler<EnrollClassCommand, string>
{
    private readonly umsContext _context;
    private readonly ICommonServices _common;
    private readonly IEEmailService _emailSender;

    public EnrollClassHandler(ICommonServices common, umsContext context, IEEmailService emailSender)
    {
        _context = context;
        _common = common;
        _emailSender = emailSender;
    }
    public async Task<string> Handle(EnrollClassCommand request, CancellationToken cancellationToken)
    {
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
        var range = _common.GetCourseDateRange(request.EnrollmentInfo.ClassName);
        
        var classId = _common.GetClassId(request.EnrollmentInfo);
        var res = await _context.AddAsync(new Domain.Models.ClassEnrollment()
        {
            StudentId = request.EnrollmentInfo.StudentId,
            ClassId = classId
        });
        _context.SaveChanges();
        var emailAddress = _common.GetUserEmail(request.EnrollmentInfo.StudentId);
        string emailBody = "Dear student,\tKindly note note that you have been successfully enrolled to the "
            + request.EnrollmentInfo.ClassName + " course.\tBest";
        await _emailSender.SendEmailAsync(emailAddress, "Course Enrollment", emailBody);
        return "Enrolled successfully!";
    }
    
}