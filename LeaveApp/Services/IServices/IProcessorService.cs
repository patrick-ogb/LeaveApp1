using LeaveApp.ReportModel;
using System.Threading.Tasks;

namespace LeaveApp.Services.IServices
{
    public interface IProcessorService
    {
        Task<HoldLevelVM> Processlevel(int? pageIndex, string? SearchText);
        Task<HoldDepartmentVM> ProcessDepartment(int? pageIndex, string? SearchText);
    }
}
