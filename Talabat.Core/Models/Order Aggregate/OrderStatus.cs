using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

    
namespace Talabat.Core.Models.Order_Aggregate
{
    public enum OrderStatus
    {
        // [EnumMember(Value = "Pending")]
        Pending, // Order has been placed but not yet processed
        PaymentReceived, // Payment has been received for the order
        PaymentFailed, // Payment attempt failed    
    }
}
