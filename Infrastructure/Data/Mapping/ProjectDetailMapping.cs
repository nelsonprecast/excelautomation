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
            builder
                .Property(b => b.ProjectId)
                .HasColumnName("ProjectId");
            builder
                .Property(b => b.Wd)
                .HasColumnName("WD");
            builder
                .Property(b => b.ItemName)
                .HasColumnName("ItemName");
            builder
                .Property(b => b.DispositionSpecialNote)
                .HasColumnName("DispositionSpecialNote");
            builder
                .Property(b => b.DetailPage)
                .HasColumnName("DetailPage");
            builder
                .Property(b => b.TakeOffColor)
                .HasColumnName("TakeOffColor");
            builder
                .Property(b => b.Length)
                .HasColumnName("Length");
            builder
                .Property(b => b.Width)
                .HasColumnName("Width");
            builder
                .Property(b => b.Height)
                .HasColumnName("Height");
            builder
                .Property(b => b.Pieces)
                .HasColumnName("Pieces");
            builder
                .Property(b => b.ImagePath)
                .HasColumnName("ImagePath");
            builder
                .Property(b => b.TotalLf)
                .HasColumnName("TotalLF");
            builder
                .Property(b => b.ActSfcflf)
                .HasColumnName("ActSFCFLF");
            builder
                .Property(b => b.ActCfpcs)
                .HasColumnName("ActCFPcs");
            builder
                .Property(b => b.TotalActCf)
                .HasColumnName("TotalActCF");
            builder
                .Property(b => b.NomCflf)
                .HasColumnName("NomCFLF");
            builder
                .Property(b => b.NomCfpcs)
                .HasColumnName("NomCFPcs");
            builder
                .Property(b => b.TotalNomCf)
                .HasColumnName("TotalNomCF");
            builder
                .Property(b => b.MoldQty)
                .HasColumnName("MoldQty");
            builder
                .Property(b => b.LineItemCharge)
                .HasColumnName("LineItemCharge");
            builder
                .Property(b => b.TotalActualNominalValue)
                .HasColumnName("TotalActualNominalValue");
            builder
                .Property(b => b.Category)
                .HasColumnName("Category");
            builder
                .Property(b => b.GroupId)
                .HasColumnName("GroupId");
            builder
                .Property(b => b.HoursPerMold)
                .HasColumnName("HoursPerMold")
                .IsRequired(false);

            base.Configure(builder);
        }
    }
}
