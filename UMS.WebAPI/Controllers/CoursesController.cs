using System.Web.Http.OData;
using AutoMapper;
using EmailServiceTools;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UMS.Application.ClassEnrollment.Commands;
using UMS.Application.Entities.Course.Commands.InsertCourse;
using UMS.Application.Entities.Course.Queries.GetCourses;
using UMS.Application.Entities.TeacherPerCoursePerSession.Commands;
using UMS.Domain.Models;
using UMS.WebAPI.DTO;
using UMS.WebAPI.Filters;

namespace UMS.WebAPI.Controllers;

[ApiController]
//[Authorize]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public CoursesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    // GET
    [HttpGet("GetCourses")]
    [EnableQuery]
    public async Task<IActionResult> GetCourse()
    {
        //NpgsqlRange<LocalDate> range = default;
        //Console.WriteLine("Date range : "+range);
        return Ok(await _mediator.Send(new GetCoursesQuery()));
    }
    
    //POST
    [HttpPost("AddCourse")]
    [TypeFilter(typeof(UserAuthorizationFilter))]
    public async Task<IActionResult> InsertCourse([FromHeader] int userId,[FromBody] CreateCourse c)
    {
        Course course = _mapper.Map<Course>(c);
        return Ok(await _mediator.Send(new InsertCourseCommand(course,userId)));
    }
    
    // Teacher to Course Registration
    [HttpPost("RegisterToCourse")]
    public async Task<IActionResult> InsertTeacherPerCourse([FromBody] RegisterToCourse regToCourse)
    {
        return Ok(await _mediator.Send(new RegisterTeacherToCourseCommand(regToCourse)));
    }

    /*
    [HttpPost("SendEmail")]
    public async Task<IActionResult> SendEmailTo([FromHeader] string address, string displayName, string subject, string content)
    {
        EmailAddress emailAddress = new EmailAddress() {Address = address, DisplayName = displayName};
        EmailSend emailSend = new EmailSend(_emailSender);
        return Ok(emailSend.SendEmail(emailAddress,subject,content));
    }
    */
    
    [HttpPost("EnrollCourse")]
    public async Task<IActionResult> EnrollCourse([FromBody] EnrollClass enrollClass)
    {
        return Ok(await _mediator.Send(new EnrollClassCommand(enrollClass.ClassName, enrollClass.TeacherName,enrollClass.StudentId)));
    }
}