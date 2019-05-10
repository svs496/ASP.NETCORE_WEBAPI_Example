using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaskManager.Entities
{
   public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }


        [MinLength(1)]
        [MaxLength(40)]
        [Required]
        public string FirstName { get; set; }

        [MinLength(1)]
        [MaxLength(40)]
        [Required]
        public string LastName { get; set; }

        [Column(TypeName = "char(6)")]
        public string EmployeeID { get; set; }

        public long TaskId { get; set; }
        public Task Task { get; set; }

    }
}
