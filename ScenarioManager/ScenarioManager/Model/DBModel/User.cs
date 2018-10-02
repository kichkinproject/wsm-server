using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class User
    {
        [Required]
        [Key]
        public string Login { get; set; }
        [Required]
        public UserType UserType { get; set; }

        [Required]
        public long UserGroupId { get; set; }
        [Required]
        [ForeignKey("UserGroupId")]
        public UserGroup UserGroup { get; set; }
    }
}
