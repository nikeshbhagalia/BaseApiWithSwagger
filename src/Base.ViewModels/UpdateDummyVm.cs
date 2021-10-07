using AutoMapper;
using Base.Data.Models;

namespace Base.ViewModels
{
    public class UpdateDummyVm
    {
        public string Name { get; set; }
    }

    public class UpdateDummyProfile : Profile
    {
        public UpdateDummyProfile()
        {
            CreateMap<UpdateDummyVm, Dummy>();
        }
    }
}
