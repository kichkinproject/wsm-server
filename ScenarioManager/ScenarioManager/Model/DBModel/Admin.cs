using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class Admin
    {
        [Key]
        [Required]
        public string Login { get; set; }


        /// <summary>
        /// для Артемочки
        /// </summary>
        public bool IsMainAdmin { get; set; }
    }
}
