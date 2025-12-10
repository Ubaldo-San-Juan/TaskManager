using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Configurations
{
    public class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTask>
    {
        public void Configure(EntityTypeBuilder<TodoTask> builder)
        {
            builder.ToTable("TodoTasks"); // Table name in SQL
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(t => t.Description)
                .HasMaxLength(1000);
            builder.Property(t => t.IsCompleted)
                .IsRequired();

            // Relationship with User
            builder.HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
