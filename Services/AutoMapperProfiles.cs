using AutoMapper;
using LMS.Areas.Admin.Models;
using LMS.Data;

namespace LMS.Services
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();

        }
    }
}
