using SME_API_Econtract.Models;

namespace SME_API_Econtract.Services
{
    public interface ICallAPIService
    {
        Task<string> GetDataApiAsync(MapiInformationModels apiModels, object xdata);

    }
}
