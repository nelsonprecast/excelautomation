using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mapping
{
    public class PlanElevationReferenceMapping : ExcelAutomationEntityTypeConfiguration<PlanElevationReference>
    {
        /// <summary>
        /// Configures the Expression entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PlanElevationReference> builder)
        {
            builder.ToTable(nameof(PlanElevationReference));
            builder
                .Property(b => b.Id)
                .HasColumnName("PlanElevationReferenceId");
            base.Configure(builder);
        }
    }
}
