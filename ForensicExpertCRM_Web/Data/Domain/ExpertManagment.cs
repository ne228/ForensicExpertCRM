using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForensicExpertCRM_Web.Data.Domain;

public class ExpertManagment
{

    public int Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public List<Expert>? Experts { get; set; }
}
