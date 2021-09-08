using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Enums
{
    public enum OrderStatus
    {
        NOT_AVAILABLE,
        CANCELLED,
        RECEIVED,
        PREPARING,
        READY,
        DELAYED,
        OUT_FOR_DELIVERY,
        DELIVERED,
        RETURNED
    }
}
