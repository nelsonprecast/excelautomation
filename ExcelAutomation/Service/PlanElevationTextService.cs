﻿using ExcelAutomation.Data;
using ExcelAutomation.Models;

namespace ExcelAutomation.Service;

public class PlanElevationTextService : IPlanElevationTextService
{
    private readonly ExcelAutomationContext _context;

    public PlanElevationTextService(ExcelAutomationContext context)
    {
        _context = context;
    }

    public int Save(ProjectPlanElevationTextDto projectPlanElevationTextDto)
    {
        var projectId = projectPlanElevationTextDto.ProjectId;
        foreach (var planElevationTextDto in projectPlanElevationTextDto.PlanElevationText)
        {
            var dbObject = new PlanElevationText();
            dbObject.Text = planElevationTextDto.Text;
            dbObject.CreatedDate = DateTime.Now;
            dbObject.ProjectId = projectId;
            _context.PlanElevationText.Add(dbObject);
        }

        return _context.SaveChanges();
    }
}