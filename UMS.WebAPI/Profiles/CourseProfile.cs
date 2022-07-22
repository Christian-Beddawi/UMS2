using AutoMapper;
using NpgsqlTypes;
using UMS.Domain.Models;
using UMS.WebAPI.DTO;

namespace UMS.WebAPI.Profiles;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<CreateCourse, Course>()
            .ForPath(course => course.EnrolmentDateRange,
                options => options.MapFrom(createCourse => new NpgsqlRange<DateOnly>(DateOnly.FromDateTime(createCourse.StartDate), DateOnly.FromDateTime(createCourse.EndDate)))).ReverseMap();
    }
}