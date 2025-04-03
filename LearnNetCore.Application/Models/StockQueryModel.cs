using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnNetCore.Application.Models;

public class StockQueryModel : BaseQueryModel
{
    public string? CompanyName { get;set;}
}
