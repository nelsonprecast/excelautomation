using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mapping
{
    public class ProjectGroupMapping : ExcelAutomationEntityTypeConfiguration<ProjectGroup>
    {
        /// <summary>
        /// Configures the Expression entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProjectGroup> builder)
        {
            builder.ToTable(nameof(ProjectGroup));
            builder
                .Property(b => b.Id)
                .HasColumnName("GroupId");

            base.Configure(builder);
        }
    }
}
