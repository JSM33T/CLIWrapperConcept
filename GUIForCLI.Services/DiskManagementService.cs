using GUIForCLI.Api.Entities;
using GUIForCLI.Api.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIForCLI.Services
{
    public class DiskManagementService : IDiskManagementService
    {
        public async Task<List<DriveInfoModel>> GetDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<DriveInfoModel> driveInfoList = new List<DriveInfoModel>();

            foreach (DriveInfo drive in allDrives)
            {
                DriveInfoModel driveModel = new DriveInfoModel
                {
                    Drive = drive.Name,
                    Size = drive.TotalSize,
                    FreeSpace = drive.TotalFreeSpace,
                    Type = drive.DriveType.ToString()
                };

                driveInfoList.Add(driveModel);
            }

            return driveInfoList;
        }

        static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        int order = 0;
        while (bytes >= 1024 && order < sizes.Length - 1)
        {
            order++;
            bytes = bytes / 1024;
        }

        return $"{bytes} {sizes[order]}";
    }
    }
}
