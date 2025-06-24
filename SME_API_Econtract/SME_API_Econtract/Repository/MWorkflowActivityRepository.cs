using SME_API_Econtract.Entities;
using Microsoft.EntityFrameworkCore;
using SME_API_Econtract.Models;

public class MProjectContractRepository 
{
    private readonly Si_EcontractDBContext _context;

    public MProjectContractRepository(Si_EcontractDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MProjectContract>> GetAllAsync()
    {
        try
        {
            return await _context.MProjectContracts.ToListAsync();
        }
        catch (Exception ex)
        {
            // Log or handle error
            throw new Exception("Error retrieving models", ex);
        }
    }

    public async Task<MProjectContract?> GetByIdAsync(string projectCode)
    {
        try
        {
            return await _context.MProjectContracts
                .Include(x => x.TInstallmentDetails)
                .FirstOrDefaultAsync(e=>e.ProjectCode == projectCode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving model with id {projectCode}", ex);
        }
    }

    public async Task AddAsync(MProjectContract model)
    {
        try
        {
            _context.MProjectContracts.Add(model);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding model", ex);
        }
    }

    public async Task UpdateAsync(MProjectContract model)
    {
        try
        {
            _context.MProjectContracts.Update(model);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating model", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var model = await _context.MProjectContracts.FindAsync(id);
            if (model != null)
            {
                _context.MProjectContracts.Remove(model);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting model", ex);
        }
    }
    public async Task<IEnumerable<MProjectContract>> GetAllAsyncSearch_MProjectContract(searchProjectData searchModel)
    {
        try
        {
            var query = _context.MProjectContracts
                .Include(x => x.TInstallmentDetails)
                .AsQueryable();

            query = query.Where(bu => bu.ProjectCode == searchModel.ProjectCode);
            // Add more filters if needed, e.g. for dimensionid

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<MProjectContract>();
        }
    }
}
