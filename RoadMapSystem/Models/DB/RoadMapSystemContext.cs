using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RoadMapSystem.Models.DB
{
    public partial class RoadMapSystemContext : DbContext
    {
        public RoadMapSystemContext()
        {
        }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeAccount> EmployeeAccount { get; set; }
        public virtual DbSet<EmployeeMentors> EmployeeMentors { get; set; }
        public virtual DbSet<EmployeeRank> EmployeeRank { get; set; }
        public virtual DbSet<EmployeeRole> EmployeeRole { get; set; }
        public virtual DbSet<EmployeeSkillValue> EmployeeSkillValue { get; set; }
        public virtual DbSet<MileStone> MileStone { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<SkillValue> SkillValue { get; set; }
        public virtual DbSet<SkillValueRank> SkillValueRank { get; set; }

        public RoadMapSystemContext(DbContextOptions<RoadMapSystemContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId).HasColumnName("commentId");

                entity.Property(e => e.AuthorId).HasColumnName("authorId");

                entity.Property(e => e.CommentValue)
                    .IsRequired()
                    .HasColumnName("commentValue")
                    .HasMaxLength(1000);

                entity.Property(e => e.MileStoneId).HasColumnName("mileStoneId");

                entity.HasOne(d => d.MileStone)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.MileStoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_MileStone");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.EmployeeRoleId).HasColumnName("employeeRoleId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Patronymic)
                    .HasColumnName("patronymic")
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.RankId).HasColumnName("rankId");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("surname")
                    .HasMaxLength(50);

                entity.HasOne(d => d.EmployeeRole)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.EmployeeRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_EmployeeRole");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.RankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_EmployeeRank");
            });

            modelBuilder.Entity<EmployeeAccount>(entity =>
            {
                entity.Property(e => e.EmployeeAccountId)
                    .HasColumnName("employeeAccountId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.HasOne(d => d.EmployeeAccountNavigation)
                    .WithOne(p => p.EmployeeAccount)
                    .HasForeignKey<EmployeeAccount>(d => d.EmployeeAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeAccount_Employee");
            });

            modelBuilder.Entity<EmployeeMentors>(entity =>
            {
                entity.HasKey(e => new { e.MentorId, e.InternId });

                entity.Property(e => e.MentorId).HasColumnName("mentorId");

                entity.Property(e => e.InternId).HasColumnName("internId");

                entity.Property(e => e.DataOfMileStone)
                    .HasColumnName("dataOfMileStone")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Intern)
                    .WithMany(p => p.EmployeeMentorsIntern)
                    .HasForeignKey(d => d.InternId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeMentors_Employee1");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.EmployeeMentorsMentor)
                    .HasForeignKey(d => d.MentorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeMentors_Employee");
            });

            modelBuilder.Entity<EmployeeRank>(entity =>
            {
                entity.Property(e => e.EmployeeRankId).HasColumnName("employeeRankId");

                entity.Property(e => e.EmployeeRankTitle)
                    .IsRequired()
                    .HasColumnName("employeeRankTitle")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeRole>(entity =>
            {
                entity.Property(e => e.EmployeeRoleId).HasColumnName("employeeRoleId");

                entity.Property(e => e.EmployeeRoleName)
                    .IsRequired()
                    .HasColumnName("employeeRoleName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeSkillValue>(entity =>
            {
                entity.Property(e => e.EmployeeSkillValueId).HasColumnName("employeeSkillValueId");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.SkillId).HasColumnName("skillId");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeSkillValue)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeSkillValue_Employee");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.EmployeeSkillValue)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeSkillValue_Skill");
            });

            modelBuilder.Entity<MileStone>(entity =>
            {
                entity.Property(e => e.MileStoneId).HasColumnName("mileStoneId");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmployeeSkillValueId).HasColumnName("employeeSkillValueId");

                entity.HasOne(d => d.EmployeeSkillValue)
                    .WithMany(p => p.MileStone)
                    .HasForeignKey(d => d.EmployeeSkillValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MileStone_EmployeeSkillValue");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.IdSkill);

                entity.Property(e => e.IdSkill).HasColumnName("idSkill");

                entity.Property(e => e.DescriptionOfSkill).HasMaxLength(500);

                entity.Property(e => e.SkillTitle)
                    .IsRequired()
                    .HasColumnName("skillTitle")
                    .HasMaxLength(50);

                entity.Property(e => e.SkillValueId).HasColumnName("skillValueId");

                entity.HasOne(d => d.SkillValue)
                    .WithMany(p => p.Skill)
                    .HasForeignKey(d => d.SkillValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Skill_SkillValue");
            });

            modelBuilder.Entity<SkillValue>(entity =>
            {
                entity.Property(e => e.SkillValueId).HasColumnName("skillValueId");

                entity.Property(e => e.SkillValueTitle)
                    .IsRequired()
                    .HasColumnName("skillValueTitle")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SkillValueRank>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeRankId, e.SkillId });

                entity.Property(e => e.EmployeeRankId)
                    .HasColumnName("employeeRankId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SkillId).HasColumnName("skillId");

                entity.HasOne(d => d.EmployeeRank)
                    .WithMany(p => p.SkillValueRank)
                    .HasForeignKey(d => d.EmployeeRankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SkillValueRank_EmployeeRank");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.SkillValueRank)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SkillValueRank_Skill");
            });
        }
    }
}
