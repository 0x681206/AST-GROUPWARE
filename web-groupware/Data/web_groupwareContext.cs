using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using web_groupware.Models;

namespace web_groupware.Data
{
    public class web_groupwareContext : DbContext
    {
        public web_groupwareContext(DbContextOptions<web_groupwareContext> options)
            : base(options)
        {
        }

        public DbSet<T_INFO> T_INFO { get; set; }
        public DbSet<T_INFO_FILE> T_INFO_FILE { get; set; }
        public DbSet<T_STAFFM>? T_STAFFM { get; set; }
        public DbSet<T_GROUPM> T_GROUPM { get; set; }
        public DbSet<T_GROUPSTAFF> T_GROUPSTAFF { get; set; }
        public DbSet<T_BUKKEN> T_BUKKEN { get; set; }
        public DbSet<T_BUKKENCOMMENT> T_BUKKENCOMMENT { get; set; }
        public DbSet<T_BUKKENCOMMENT_READ> T_BUKKENCOMMENT_READ { get; set; }
        public DbSet<T_NO> T_NO { get; set; }
        public DbSet<T_REPORT> T_REPORT { get; set; }
        public DbSet<T_REPORTCOMMENT> T_REPORTCOMMENT { get; set; }
        public DbSet<T_REPORTCOMMENT_READ> T_REPORTCOMMENT_READ { get; set; }
        public DbSet<R_RESTORATION_REPORT> R_RESTORATION_REPORT { get; set; }
        public DbSet<T_FILEINFO> T_FILEINFO { get; set; }
        public DbSet<T_AttendanceDate> T_AttendanceDate { get; set; }
        public DbSet<T_WORKFLOW> T_WORKFLOW { get; set; }
        public DbSet<T_DIC> T_DIC { get; set; }
        public DbSet<T_SCHEDULEPEOPLE> T_SCHEDULEPEOPLE { get; set; }
        public DbSet<T_SCHEDULEPLACE> T_SCHEDULEPLACE { get; set; }
        public DbSet<ScheduleViewModel> ScheduleViewModel { get; set; }
        public DbSet<T_PLACEM> T_PLACEM { get; set; }

        public DbSet<T_MEMO> T_MEMO { get; set; }
        public DbSet<T_MEMO_READ> T_MEMO_READ { get; set; }
        public DbSet<T_SCHEDULEFACILITY> T_SCHEDULEFACILITY { get; set; }
        public DbSet<T_TODO> T_TODO { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T_INFO>()
                .HasKey(c => new { c.info_cd });

            modelBuilder.Entity<T_BUKKEN>()
                .HasKey(c => new { c.bukken_cd });

            modelBuilder.Entity<T_BUKKENCOMMENT>()
                .HasKey(c => new { c.bukken_cd, c.comment_cd });
            modelBuilder.Entity<T_BUKKENCOMMENT_READ>()
                .HasKey(c => new { c.bukken_cd, c.comment_no, c.staf_cd });
            modelBuilder.Entity<T_DIC>()
                .HasKey(c => new { c.dic_kb, c.dic_cd });

            modelBuilder.Entity<T_FILEINFO>()
                .HasKey(c => new { c.file_no });
            modelBuilder.Entity<AddGEventViewModel>()
                .HasNoKey();
            modelBuilder.Entity<T_SCHEDULEPEOPLE>()
                .HasKey(t => new { t.schedule_no, t.staf_cd });
            modelBuilder.Entity<T_SCHEDULEPLACE>()
                .HasKey(t => new { t.schedule_no, t.place_cd });
            
            modelBuilder.Entity<Places>()
                .HasNoKey();
            modelBuilder.Entity<People>()
                .HasNoKey();
            modelBuilder.Entity<LoginViewModel>()
                .HasNoKey();
            modelBuilder.Entity<T_WORKFLOW>()
                .HasKey(c => new { c.id });
           

            modelBuilder.Entity<T_AttendanceDate>()
                .HasKey(c => new { c.id });
            modelBuilder.Entity<T_MEMO>()
                .HasKey(c => new { c.memo_no });
            modelBuilder.Entity<T_MEMO_READ>()
                .HasKey(c => new { c.memo_no, c.staff_cd });
            modelBuilder.Entity<T_GROUPSTAFF>()
                .HasKey(c => new { c.staf_cd, c.group_cd });
            modelBuilder.Entity<GroupViewModel>()
                .HasNoKey();
            modelBuilder.Entity<T_SCHEDULEFACILITY>()
                .HasKey(t => new { t.schedule_no, t.place_cd, t.staf_cd });
            modelBuilder.Entity<T_TODO>()
                .HasKey(k => new {k.id});

        }
        public DbSet<T_SCHEDULE>? T_SCHEDULE { get; set; }
        public DbSet<web_groupware.Models.LoginViewModel> LoginViewModel { get; set; }
        public DbSet<web_groupware.Models.AddGEventViewModel>? AddGEventViewModel { get; set; }
        public DbSet<web_groupware.Models.Places>? Places { get; set; }
        public DbSet<web_groupware.Models.People>? People { get; set; }
        public DbSet<web_groupware.Models.GroupViewModel>? GroupViewModel { get; set; }
    }
}
