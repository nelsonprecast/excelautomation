using ExcelAutomation.Data;
using ExcelAutomation.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


namespace ExcelAutomation.Service
{
    public class ProjectService : IProjectService
    {
        private readonly ExcelAutomationContext _context;

        public ProjectService(ExcelAutomationContext context)
        {
            _context = context;
        }
        
        public int DeleteProjectDetailRow(int id)
        {
            var projectDetail = _context.ProjectDetails.FirstOrDefault(x => x.ProjectDetailId == id);
            if(projectDetail  != null)
            {
                _context.Remove(projectDetail);
                _context.SaveChanges();
            }

            return projectDetail.ProjectId;
        }
        
        public int SaveGroup(ProjectGroupDto projectGroupDto)
        {
            var newGroup = new ProjectGroup();
            newGroup.GroupName = projectGroupDto.GroupName;
            newGroup.CreatedDate = DateTime.Now;
            _context.ProjectGroups.Add(newGroup);
            _context.SaveChanges();


            var projectDetails = _context.ProjectDetails.Where(x => projectGroupDto.ProjectDetailIds.Contains(x.ProjectDetailId)).ToList();


            foreach (var projectDetail in projectDetails)
            {
               projectDetail.GroupId = newGroup.GroupId;
                _context.Update(projectDetail);
            }
            _context.SaveChangesAsync();
            return projectDetails.FirstOrDefault().ProjectId;

        }
        
        public void ChangeGroup(int projectDetailId, int groupId){
            var dbObject = _context.ProjectDetails.FirstOrDefault(p => p.ProjectDetailId == projectDetailId);
            if (dbObject != null)
            {
                dbObject.GroupId = groupId < 0 ? null : groupId;
                _context.Update(dbObject);
                _context.SaveChanges();
            }
        }
        
        public bool RemoveFromGroup(string projectDetailIds)
        {
            var projectDetailIdArray = projectDetailIds.Split(',').Select(x => int.Parse(x));
            var projectDetails = _context.ProjectDetails.Where(x => projectDetailIdArray.Contains(x.ProjectDetailId)).ToList();
            foreach (var projectDetail in projectDetails)
            {
                projectDetail.GroupId = null;
            }
            _context.UpdateRange(projectDetails);
            _context.SaveChanges();
            return true;
        }




        private string GetGroupName(int? groupId)
        {
            if (groupId == null)
            {
                return "";
            }
            var gName = _context.ProjectGroups.FirstOrDefault(p => p.GroupId == groupId).GroupName;
            return gName;
        }
    }
}
