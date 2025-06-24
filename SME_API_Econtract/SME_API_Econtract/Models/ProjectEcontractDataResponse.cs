using System.Text.Json.Serialization;
using System.Collections.Generic;

public class ProjectEcontractDataResponse
{
    [JsonPropertyName("responseCode")]
    public string ResponseCode { get; set; }

    [JsonPropertyName("responseMsg")]
    public string ResponseMsg { get; set; }

    [JsonPropertyName("data")]
    public List<ProjectData> Data { get; set; }
}

public class ProjectData
{
    [JsonPropertyName("projectCode")]
    public string ProjectCode { get; set; }

    [JsonPropertyName("projectName")]
    public string ProjectName { get; set; }

    [JsonPropertyName("contractingPartyName")]
    public string ContractingPartyName { get; set; }

    [JsonPropertyName("startDate")]
    public string StartDate { get; set; }

    [JsonPropertyName("projectBudget")]
    public string ProjectBudget { get; set; }

    [JsonPropertyName("endDate")]
    public string EndDate { get; set; }

    [JsonPropertyName("allocatedBudget")]
    public string AllocatedBudget { get; set; }

    [JsonPropertyName("installments")]
    public string Installments { get; set; }

    [JsonPropertyName("installmentsDetails")]
    public List<InstallmentDetail> InstallmentsDetails { get; set; }
}

public class InstallmentDetail
{
    [JsonPropertyName("installmentsNo")]
    public int InstallmentsNo { get; set; }

    [JsonPropertyName("installmentsDate")]
    public string InstallmentsDate { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }
}
public class searchProjectData
{
    [JsonPropertyName("projectCode")]
    public string ProjectCode { get; set; }

  
}
