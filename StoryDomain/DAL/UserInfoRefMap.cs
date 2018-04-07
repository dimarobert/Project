using System.Data.Entity.ModelConfiguration;

namespace Project.StoryDomain.DAL {
    public class UserInfoRefMap : EntityTypeConfiguration<Models.UserInfoRef> {
        public UserInfoRefMap() {

            ToTable("AspNetUsers");

            HasKey(u => u.Id)
                .Property(u => u.Id)
                .HasMaxLength(128)
                .HasColumnName("Id")
                .HasColumnType("nvarchar")
                .IsRequired();

            Property(u => u.UserName)
                .HasMaxLength(256)
                .HasColumnName("UserName")
                .HasColumnType("nvarchar")
                .IsOptional();

        }
    }
}
