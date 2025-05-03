using AutoMapper;
using KutuphaneOtomasyon.Models;
using KutuphaneOtomasyon.Models.ViewModel;

namespace KutuphaneOtomasyon.Mapping
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
                CreateMap<User,UserViewModel>().ReverseMap();
        }
    }
}
