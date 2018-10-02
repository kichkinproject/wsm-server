using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.AdminDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Mappers.AdminMapper
{
    public class AdminWithPasswordMapper:IMapper<Admin, AdminWithPassword>
    {
        public Admin Map(AdminWithPassword input)
        {
            return new Admin
            {
                Login = input.Login
            };
        }
    }
}
