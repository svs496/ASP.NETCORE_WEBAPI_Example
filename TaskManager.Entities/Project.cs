using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace TaskManager.Entities
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProjectId { get; set; }


        [MinLength(10)]
        [MaxLength(100)]
        [Required]
        public string ProjectName { get; set; }


        [Required]
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }

        [Range(0, 30)]
        public int? Priority { get; set; }

        public ICollection<Task> Tasks { get; set; }

        [Column("ManagerId")]
        public long? UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public int CompletedTaskCount
        {
            get
            {
                if (Tasks != null && Tasks.Count() > 0)
                    return Tasks.Where(p => p.Status == Statuses.Completed).Count();
                else
                    return 0;
            }
        }

    }
}
