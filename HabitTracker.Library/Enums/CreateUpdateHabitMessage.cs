using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Library.Enums
{
    public enum CreateUpdateHabitMessage
    {
        OK,
        NameTooLong,
        DescriptionTooLong,
        ReasonTooLong,
        NameAlreadyExists
    }
}
