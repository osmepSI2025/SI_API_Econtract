using System;
using System.Collections.Generic;

namespace SME_API_Econtract.Entities;

public partial class MProjectContract
{
    public string ProjectCode { get; set; } = null!;

    public string? ProjectName { get; set; }

    public string? ContractingPartyName { get; set; }

    public DateTime? StartDate { get; set; }

    public decimal? ProjectBudget { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? AllocatedBudget { get; set; }

    public int? Installments { get; set; }

    public virtual ICollection<TInstallmentDetail> TInstallmentDetails { get; set; } = new List<TInstallmentDetail>();
}
