using AutoMapper;
using LMS.Areas.Admin.Models;
using LMS.Data;

namespace LMS.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BookCategory, BookCategoryModel>().ReverseMap();
            CreateMap<Book, BookModel>().ReverseMap();
            CreateMap<Course, CourseModel>().ReverseMap();
            CreateMap<Student, StudentModel>().ReverseMap();
            CreateMap<RentBook, RentBookModel>().ReverseMap();
            CreateMap<RentBookDetail, RentBookDetailModel>().ReverseMap();

        }
    }
}
