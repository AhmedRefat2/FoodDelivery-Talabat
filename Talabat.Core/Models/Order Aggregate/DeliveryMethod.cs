using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    public class DeliveryMethod : BaseModel
    {
        public string ShortName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Cost { get; set; }
        public string DeliveryTime { get; set; } = null!;
    }
}
