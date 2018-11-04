using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class UserLoginInfo
    {
        [Key]
        [Required]
        public string Login { get; set; }

        public string HashedPassword { get; set; }
        public byte[] Salt { get; set; }
        public string Role { get; set; }
    }
}
