using AutoMapper;
using Core.Domain;
using Core.Infrastructure.Mapper;
using Core.Model.Response;

namespace Facade.Framework
{
    public sealed class OrderedMapperConfiguration : Profile, IOrderedMapperProfile
    {
        public int Order => 0;

        public OrderedMapperConfiguration()
        {
            CreateMaps();
        }

        private void CreateMaps()
        {
            CreateMap<Project, ProjectResponse>();
            CreateMap<ProjectDetail, ProjectDetailResponse>();
            CreateMap<ProjectGroup, ProjectGroupResponse>();
            CreateMap<PlanElevationReference, PlanElevationReferenceResponse>();
            CreateMap<PlanElevationText, PlanElevationTextResponse>();

            CreateMap<ProjectResponse, Project>();
            CreateMap<ProjectDetailResponse, ProjectDetail>();
            CreateMap<ProjectGroupResponse, ProjectGroup>();
            CreateMap<PlanElevationReferenceResponse, PlanElevationReference>();
            CreateMap<PlanElevationTextResponse, PlanElevationText>();
        }
    }
}
