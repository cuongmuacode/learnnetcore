using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnNetCore.Domain;

public interface IIdEntity
{
    public Guid Id { get; set; }
}
