using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Models
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
