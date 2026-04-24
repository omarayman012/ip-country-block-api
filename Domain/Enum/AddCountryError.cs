using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockedCountries.Domain.Enum
{
    public enum AddCountryError
    {
        None,
        InvalidCode,
        DuplicateCode,
        DuplicateName
    }
}
