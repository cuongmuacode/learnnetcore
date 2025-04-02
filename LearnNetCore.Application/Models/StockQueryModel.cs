using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnNetCore.Application.Models;

public class StockQueryModel
{
    public Guid? Id { get; set; }
    public string? CompanyName { get; set; }
}
