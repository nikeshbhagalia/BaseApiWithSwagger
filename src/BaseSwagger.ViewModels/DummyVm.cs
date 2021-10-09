using AutoMapper;
using BaseSwagger.Data.Models;

namespace BaseSwagger.ViewModels
{
    public class DummyVm
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class DummyProfile : Profile
    {
        public DummyProfile()
        {
            CreateMap<Dummy, DummyVm>();
            CreateMap<DummyVm, Dummy>();
        }
    }
}
