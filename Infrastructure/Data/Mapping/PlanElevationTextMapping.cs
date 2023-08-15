using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mapping
{
    public class PlanElevationTextMapping : ExcelAutomationEntityTypeConfiguration<PlanElevationText>
    {
        /// <summary>
        /// Configures the Expression entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PlanElevationText> builder)
        {
            builder.ToTable(nameof(PlanElevationText));

            base.Configure(builder);
        }
    }
}
