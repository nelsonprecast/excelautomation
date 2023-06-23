using System;
using System.Collections.Generic;

namespace ExcelAutomation.Data;

public partial class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public string NominalCf { get; set; }

    public string ActualCf { get; set; }

    public string LineItemTotal { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? RevisionDate { get; set; }

    public string ContactSpecs { get; set; }

    public virtual ICollection<ProjectDetail> ProjectDetails { get; set; } = new List<ProjectDetail>();
}
