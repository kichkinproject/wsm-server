using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class Scenario
    {
        [Required]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }


        public string Description { get; set; }
        [Required]
        public string Text { get; set; }


        [Required]
        public long UserGroupId { get; set; }
        [Required]
        [ForeignKey("UserGroupId")]
        public UserGroup UserGroup { get; set;}

    }
}
