using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
namespace TaskManager.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasMany(e => e.Tasks)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Comments)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.Color).IsRequired().HasMaxLength(7).HasDefaultValue("#3B82F6");
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasMany(e => e.Tasks)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.ToTable("Tasks");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).IsRequired().HasConversion<int>();
                entity.Property(e => e.Priority).IsRequired().HasConversion<int>();
                entity.Property(e => e.DueDate);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CategoryId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.CategoryId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasMany(e => e.Comments)
                .WithOne(e => e.Task)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<TaskComment>(entity =>
            {
                entity.ToTable("TaskComments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.TaskId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.HasIndex(e => e.TaskId);
                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Personal", Description = "Tareas personales", Color = "#3B82F6" },
            new Category { Id = 2, Name = "Trabajo", Description = "Tareas relacionadas con el trabajo", Color = "#10B981" },
            new Category { Id = 3, Name = "Estudios", Description = "Tareas académicas y de aprendizaje", Color = "#F59E0B" }
            );
        }
    }
}
