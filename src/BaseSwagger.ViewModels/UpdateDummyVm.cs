using AutoMapper;
using BaseSwagger.Data.Models;

namespace BaseSwagger.ViewModels
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
