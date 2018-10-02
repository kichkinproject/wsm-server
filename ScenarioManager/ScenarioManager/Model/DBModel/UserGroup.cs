using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class UserGroup
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public long? ParentGroupId { get; set; }

        [ForeignKey("ParentGropId")]
        public virtual UserGroup ParentGroup { get; set; }
        public virtual IEnumerable<UserGroup> ChildrenGroups { get; set; }
    }
}
