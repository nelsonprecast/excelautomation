﻿using ExcelAutomation.Models;

namespace ExcelAutomation.Service;

public interface IPlanElevationTextService
{
    int Save(ProjectPlanElevationTextDto projectPlanElevationTextDto);
    int DeletePlanElevationText(int id);
}