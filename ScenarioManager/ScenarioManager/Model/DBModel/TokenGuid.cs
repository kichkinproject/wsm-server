using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class TokenGuid
    {
        [Required]
        [Key]
        public string Login { get; set; }
        public Guid Guid { get; set; }
    }
}
