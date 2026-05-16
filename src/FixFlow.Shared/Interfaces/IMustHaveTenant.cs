using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Shared.Interfaces
{
    public interface IMustHaveTenant
    {
        Guid TenantId { get; set; }
    }
}
