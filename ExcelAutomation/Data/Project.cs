using System;
using System.Collections.Generic;
using NuGet.Protocol.Core.Types;

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
    public string Notes { get; set; }
    public string OpportunityId { get; set; }
    public string AccountName { get; set; }

    public string Status { get; set; }
    public virtual ICollection<ProjectDetail> ProjectDetails { get; set; } = new List<ProjectDetail>();
}
