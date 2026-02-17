using System;
using System.Collections.Generic;
using System.Text;

namespace CliniFlow.Domain.Enums;

public enum AppointmentStatus
{
    Scheduled = 1,
    Completed = 2,
    Cancelled = 3,
    NoShow = 4
}