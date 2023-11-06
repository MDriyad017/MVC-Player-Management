using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace work_01.Models.ViewModels
{
    public class PlayerViewModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Join Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoinDate { get; set; }
        [EnumDataType(typeof(PlayerGrade))]
        public PlayerGrade? PlayerGrade { get; set; }
        public string PicturePath { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        //FK
        [ForeignKey("Sports")]
        public int SportsId { get; set; }
        //nev
        public virtual Sports Sports { get; set; }
    }
}