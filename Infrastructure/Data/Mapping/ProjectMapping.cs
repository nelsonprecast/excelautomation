using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mapping
{
    public class ProjectMapping : ExcelAutomationEntityTypeConfiguration<Project>
    {
        /// <summary>
        /// Configures the Expression entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable(nameof(Project));
            builder
                .Property(b => b.Id)
                .HasColumnName("ProjectId");
            builder
                .Property(e => e.ProjectName).HasMaxLength(200);

            builder
                .Property(e => e.NominalCf).IsRequired(false).HasMaxLength(200);

            builder
                .Property(e => e.ActualCf).IsRequired(false).HasMaxLength(200);

            builder
                .Property(e => e.CreatedDate).IsRequired(false);

            builder
                .Property(e => e.LineItemTotal).IsRequired(false);

            builder
                .Property(e => e.RevisionDate).IsRequired(false);

            builder
                .Property(e => e.ContactSpecs).IsRequired(false);

            builder
                .Property(e => e.Notes).IsRequired(false);

            builder
                .Property(e => e.OpportunityId).IsRequired(false);

            builder
                .Property(e => e.AccountName).IsRequired(false);

            builder
                .Property(e => e.Status).IsRequired(false);
            base.Configure(builder);
        }
    }
}
