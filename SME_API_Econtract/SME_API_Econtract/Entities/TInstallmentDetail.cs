using System;
using System.Collections.Generic;

namespace SME_API_Econtract.Entities;

public partial class TInstallmentDetail
{
    public int Id { get; set; }

    public string? ProjectCode { get; set; }

    public int? InstallmentsNo { get; set; }

    public DateTime? InstallmentsDate { get; set; }

    public decimal? Amount { get; set; }

    public virtual MProjectContract? ProjectCodeNavigation { get; set; }
}
