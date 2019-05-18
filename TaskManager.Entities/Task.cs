using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Entities
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TaskId { get; set; }

        [MinLength(10)]
        [MaxLength(100)]
        [Required]
        public string TaskName { get; set; }

        public long ? ParentTaskId { get; set; }

       
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime?  EndDate { get; set; }

        public bool? IsParentTask { get; set; }

        [Range(0,30)]
        public int? Priority { get; set; }

        public Statuses ? Status { get; set; }

        [NotMapped]
        public string ParentTaskName { get; set; }
        [NotMapped]
        public string ProjectName { get; set; }
        [NotMapped]
        public string UserName { get; set; }


        public DateTime CreateTime { get; set; }

       
        public DateTime ? ModifyDate { get; set; }


        public long ? ProjectId { get; set; }
        

        public long ? UserId { get; set; }
        public User User { get; set; }
    }

    public enum Statuses
    {
        InProgress = 1,
        Completed = 2
    }
}
