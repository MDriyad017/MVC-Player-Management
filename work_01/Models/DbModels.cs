using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace work_01.Models
{
    public enum PlayerGrade
    {
        Grade_A = 1,
        Grade_B,
        Grade_C,
        Grade_D
    }
    public class Sports
    {
        public Sports()
        {
            this.Players = new List<Player>();
        }
        public int SportsId { get; set; }
        [Required, StringLength(50, ErrorMessage = "Sports name is required!!"), Display(Name = "Sports Name")]
        public string SportsName { get; set; }
        //nev
        public virtual ICollection<Player> Players { get; set; }

    }
    public class Player
    {
        public int PlayerId { get; set; }
        [Required, Display(Name = "Player Name")]
        public string PlayerName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Join Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoinDate { get; set; }
        [EnumDataType(typeof(PlayerGrade))]
        [Required, Display(Name = "Player Grade")]
        public PlayerGrade? PlayerGrade { get; set; }
        public string PicturePath { get; set; }
        //FK
        [ForeignKey("Sports"), Display(Name = "Sports")]
        public int SportsId { get; set; }
        //nev
        public virtual Sports Sports { get; set; }

    }
    public class ClubDbContext : DbContext
    {
        public ClubDbContext()
        {
            Database.SetInitializer(new DbInitializer());
        }
        public DbSet<Sports> Sports { get; set; }
        public DbSet<Player> Players { get; set; }
    }
    public class DbInitializer : DropCreateDatabaseIfModelChanges<ClubDbContext>
    {
        protected override void Seed(ClubDbContext context)
        {
            base.Seed(context);
            Sports s1 = new Sports { SportsName = "Cricket" };
            Sports s2 = new Sports { SportsName = "Football" };

            s2.Players.Add(new Player { PlayerName = "Lionel Messi", PlayerGrade = PlayerGrade.Grade_A, JoinDate = DateTime.Now.AddYears(-20), PicturePath = "~/Images/abc.png" });
            s2.Players.Add(new Player { PlayerName = "Alejandro Garnacho", PlayerGrade = PlayerGrade.Grade_B, JoinDate = DateTime.Now.AddYears(-3), PicturePath = "~/Images/abc.png" });
            s2.Players.Add(new Player { PlayerName = "David Neres", PlayerGrade = PlayerGrade.Grade_D, JoinDate = DateTime.Now.AddYears(-1), PicturePath = "~/Images/abc.png" });
            context.Sports.AddRange(new Sports[] { s1, s2});
            context.SaveChanges();
        }
    }
}