using GUIForCLI.Api.Entities;
using GUIForCLI.Api.Entities.Helpers;

namespace GUIForCLI.Services
{
    public interface IDiskManagementService
    {
        public Task<List<DriveInfoModel>> GetDrives();
    }
}
