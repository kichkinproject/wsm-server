using ScenarioManager.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO
{
    public class ScenarioDTO
    {
        public long Id { get; set; }
        
        public string Name { get; set; }


        public string Description { get; set; }
        public string Text { get; set; }        
        
        public UserGroup UserGroup { get; set; }
    }
}
