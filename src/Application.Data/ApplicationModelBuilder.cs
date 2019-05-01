using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data
{
    public partial class ApplicationModelBuilder
    {
        /// <summary>Builds the model defined in this class with the modelbuilder specified. Called from the generated DbContext</summary>
		/// <param name="modelBuilder">The model builder to build the model with.</param>
		public virtual void BuildModel(ModelBuilder modelBuilder)
        {
            MapUser(modelBuilder.Entity<User>());
        }

        /// <summary>Defines the mapping information for the entity 'User'</summary>
		/// <param name="config">The configuration to modify.</param>
		protected virtual void MapUser(EntityTypeBuilder<User> config)
        {
            config.ToTable("User");
            config.HasKey(t => t.UserId);
            config.Property(t => t.UserId).HasColumnName("UserID").ValueGeneratedOnAdd();
            config.Property(t => t.UserType).HasColumnName("UserType");
            config.Property(t => t.UserName).HasMaxLength(50);
            config.Property(t => t.IsDeleted);
        }
    }
}