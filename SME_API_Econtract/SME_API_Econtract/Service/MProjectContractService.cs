using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SME_API_Econtract.Entities;
using SME_API_Econtract.Models;
using SME_API_Econtract.Repository;
using SME_API_Econtract.Services;
public class MProjectContractService 
{
    private readonly MProjectContractRepository _repository;
    private readonly ICallAPIService _serviceApi;
    private readonly IApiInformationRepository _repositoryApi;
    private readonly string _FlagDev;
    public MProjectContractService(MProjectContractRepository repository
            , IConfiguration configuration, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)
    {
        _repository = repository;
        _serviceApi = serviceApi;
        _repositoryApi = repositoryApi;
        _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");

    }

    public async Task<IEnumerable<MProjectContract>> GetAllAsync()
    {
        try
        {
            return await _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Service error retrieving models", ex);
        }
    }

    public async Task<MProjectContract?> GetByIdAsync(string WorkflowCode)
    {
        try
        {
            return await _repository.GetByIdAsync(WorkflowCode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Service error retrieving model with id {WorkflowCode}", ex);
        }
    }

    public async Task AddAsync(MProjectContract model)
    {
        try
        {
            await _repository.AddAsync(model);
        }
        catch (Exception ex)
        {
            throw new Exception("Service error adding model", ex);
        }
    }

    public async Task UpdateAsync(MProjectContract model)
    {
        try
        {
            await _repository.UpdateAsync(model);
        }
        catch (Exception ex)
        {
            throw new Exception("Service error updating model", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Service error deleting model", ex);
        }
    }

    public async Task BatchEndOfDay_MProjectContract(string xmodel)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var ProjectEcontractDataResponse = new ProjectEcontractDataResponse();
       
        var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "SME-ECONTRACT" });
        var apiParam = LApi.Select(x => new MapiInformationModels
        {
            ServiceNameCode = x.ServiceNameCode,
            ApiKey = x.ApiKey,
            AuthorizationType = x.AuthorizationType,
            ContentType = x.ContentType,
            CreateDate = x.CreateDate,
            Id = x.Id,
            MethodType = x.MethodType,
            ServiceNameTh = x.ServiceNameTh,
            Urldevelopment = x.Urldevelopment,
            Urlproduction = x.Urlproduction,
            Username = x.Username,
            Password = x.Password,
            UpdateDate = x.UpdateDate,
            Bearer = x.Bearer,
            AccessToken = x.AccessToken

        }).FirstOrDefault(); // Use FirstOrDefault to handle empty lists

        var apiResponse = await _serviceApi.GetDataApiAsync(apiParam, xmodel);
        var result = JsonSerializer.Deserialize<ProjectEcontractDataResponse>(apiResponse, options);

        ProjectEcontractDataResponse = result ?? new ProjectEcontractDataResponse();
        if (ProjectEcontractDataResponse.Data != null)
        {
            foreach (var item in ProjectEcontractDataResponse.Data)
            {
                try
                {
                    var existing = await _repository.GetByIdAsync(item.ProjectCode);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MProjectContract
                        {
                            ProjectCode = item.ProjectCode,
                            ProjectName = item.ProjectName,
                            ContractingPartyName = item.ContractingPartyName,
                            StartDate = DateTime.TryParse(item.StartDate, out var startDate) ? startDate : (DateTime?)null,
                            ProjectBudget = decimal.TryParse(item.ProjectBudget?.Replace(",", ""), out var projectBudget) ? projectBudget : (decimal?)null,
                            EndDate = DateTime.TryParse(item.EndDate, out var endDate) ? endDate : (DateTime?)null,
                            AllocatedBudget = decimal.TryParse(item.AllocatedBudget?.Replace(",", ""), out var allocatedBudget) ? allocatedBudget : (decimal?)null,
                            Installments = int.TryParse(item.Installments, out var installments) ? installments : (int?)null,
                            TInstallmentDetails = item.InstallmentsDetails?.Select(a => new TInstallmentDetail
                            {
                                ProjectCode = item.ProjectCode,
                                InstallmentsNo = a.InstallmentsNo,
                                InstallmentsDate = DateTime.TryParse(a.InstallmentsDate, out var instDate) ? instDate : (DateTime?)null,
                                Amount = a.Amount
                            }).ToList() ?? new List<TInstallmentDetail>()
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MProjectContract with ProjectCode {newData.ProjectCode}");
                    }
                    else
                    {
                        // Update existing record
                        existing.ProjectName = item.ProjectName;
                        existing.ContractingPartyName = item.ContractingPartyName;
                        existing.StartDate = DateTime.TryParse(item.StartDate, out var startDate) ? startDate : (DateTime?)null;
                        existing.ProjectBudget = decimal.TryParse(item.ProjectBudget?.Replace(",", ""), out var projectBudget) ? projectBudget : (decimal?)null;
                        existing.EndDate = DateTime.TryParse(item.EndDate, out var endDate) ? endDate : (DateTime?)null;
                        existing.AllocatedBudget = decimal.TryParse(item.AllocatedBudget?.Replace(",", ""), out var allocatedBudget) ? allocatedBudget : (decimal?)null;
                        existing.Installments = int.TryParse(item.Installments, out var installments) ? installments : (int?)null;

                        // Update TInstallmentDetails
                        existing.TInstallmentDetails.Clear();
                        if (item.InstallmentsDetails != null)
                        {
                            foreach (var a in item.InstallmentsDetails)
                            {
                                existing.TInstallmentDetails.Add(new TInstallmentDetail
                                {
                                    ProjectCode = item.ProjectCode,
                                    InstallmentsNo = a.InstallmentsNo,
                                    InstallmentsDate = DateTime.TryParse(a.InstallmentsDate, out var instDate) ? instDate : (DateTime?)null,
                                    Amount = a.Amount
                                });
                            }
                        }

                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MProjectContract with ProjectCode {existing.ProjectCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process MProjectContract with ProjectCode {item.ProjectCode}: {ex.Message}");
                }
            }
        }


    }
    public async Task<ProjectEcontractDataResponse> GetAllAsyncSearch_MProjectContract(searchProjectData xmodel)
    {
        try
        {
            // Get data from repository
            var Ldata = await _repository.GetAllAsyncSearch_MProjectContract(xmodel);

            if (Ldata == null || !Ldata.Any())
            {
                await BatchEndOfDay_MProjectContract(xmodel.ProjectCode);

                var Ldata2 = await _repository.GetAllAsyncSearch_MProjectContract(xmodel);
                if (Ldata2 == null || !Ldata2.Any())
                {
                    return new ProjectEcontractDataResponse
                    {
                        ResponseCode = "200",
                        ResponseMsg = "Data not found.",

                        Data = new List<ProjectData>()
                    };
                }
                else
                {
                    var response = BuildApiResponse(Ldata2);
                    return response;
                }
            }
            else
            {
                var response = BuildApiResponse(Ldata);
                return response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to search MPlanKpiList: {ex.Message}");
            return new ProjectEcontractDataResponse
            {
                ResponseCode = "500",
                ResponseMsg = "Internal Server Error",

                Data = new List<ProjectData>()
            };
        }
    }

    private ProjectEcontractDataResponse BuildApiResponse(IEnumerable<MProjectContract> data)
    {
        return new ProjectEcontractDataResponse
        {
            ResponseCode = "200",
            ResponseMsg = "OK",
            Data = data.Select(d => new ProjectData
            {
                ProjectCode = d.ProjectCode,
                ProjectName = d.ProjectName,
                ContractingPartyName = d.ContractingPartyName,
                StartDate = d.StartDate?.ToString("yyyy-MM-dd"),
                ProjectBudget = d.ProjectBudget?.ToString("N0"),
                EndDate = d.EndDate?.ToString("yyyy-MM-dd"),
                AllocatedBudget = d.AllocatedBudget?.ToString("N0"),
                Installments = d.Installments?.ToString(),
                InstallmentsDetails = d.TInstallmentDetails?.Select(a => new InstallmentDetail
                {
                    InstallmentsNo = a.InstallmentsNo ?? 0,
                    InstallmentsDate = a.InstallmentsDate?.ToString("yyyy-MM-dd"),
                    Amount = a.Amount ?? 0
                }).ToList() ?? new List<InstallmentDetail>()
            }).ToList()
        };
    }


}
