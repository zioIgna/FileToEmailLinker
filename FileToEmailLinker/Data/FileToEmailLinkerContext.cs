using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FileToEmailLinker.Models.Entities;

namespace FileToEmailLinker.Data
{
    public class FileToEmailLinkerContext : DbContext
    {
        public FileToEmailLinkerContext (DbContextOptions<FileToEmailLinkerContext> options)
            : base(options)
        {
        }

        public DbSet<Receiver> Receiver { get; set; } = default!;

        public DbSet<MailingPlan> MailingPlan { get; set; } = default!;

        public DbSet<FileToEmailLinker.Models.Entities.FileRef> FileRef { get; set; } = default!;
    }
}
