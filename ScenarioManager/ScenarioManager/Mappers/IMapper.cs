using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Mappers
{
    public interface IMapper <output, input>
    {
        output Map(input input);
    }
}
