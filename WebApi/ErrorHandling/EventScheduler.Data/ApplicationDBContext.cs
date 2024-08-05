using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventScheduler.Data.Model;
using System.Reflection.Emit;

namespace EventScheduler.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<Speaker> Speakers { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Location).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Start).HasColumnType("datetime");
                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Events_AspNetUsers");
            });

            builder.Entity<Participant>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Occupation).HasMaxLength(250);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Participants)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_Participants_Events");
            });

            builder.Entity<Schedule>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Schedule");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany()
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Schedule_Events");

                entity.HasOne(d => d.Speaker)
                    .WithMany()
                    .HasForeignKey(d => d.SpeakerId)
                    .HasConstraintName("FK_Schedule_Speakers");
            });
            builder.Entity<Speaker>(entity =>
                        {
                            entity.Property(e => e.Id).ValueGeneratedNever();

                            entity.Property(e => e.FirstName).HasMaxLength(150);

                            entity.Property(e => e.LastName).HasMaxLength(150);

                            entity.Property(e => e.Occupation).HasMaxLength(150);

                            entity.HasOne(d => d.Event)
                                .WithMany(p => p.Speakers)
                                .HasForeignKey(d => d.EventId)
                                .HasConstraintName("FK_Speakers_Events");
                        });

            base.OnModelCreating(builder);

        }
    }
}
