using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mapping
{
    public class ProjectDetailMapping : ExcelAutomationEntityTypeConfiguration<ProjectDetail>
    {
        /// <summary>
        /// Configures the Expression entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProjectDetail> builder)
        {
            builder.ToTable(nameof(ProjectDetail));
            builder
                .Property(b => b.Id)
                .HasColumnName("ProjectDetailId");

            base.Configure(builder);
        }
    }
}
