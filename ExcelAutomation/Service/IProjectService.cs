﻿using ExcelAutomation.Models;

namespace ExcelAutomation.Service
{
    public interface IProjectService
    {
        int SaveGroup(ProjectGroupDto projectGroupDto);
        Task<int> SaveProject(ProjectDto project);

        ICollection<ProjectDto> GetProjects();

        ProjectDto GetProjectById(int  projectId);

        void UpdateProjectDetail(ProjectDto projectDto);

        int DeleteProjectDetailRow(int id);

        int CopyProject(int id);

        void DeleteProjectPlanElevationReferances(int id);
        void ChangeGroup(int projectDetailId, int groupId);
    }
}
