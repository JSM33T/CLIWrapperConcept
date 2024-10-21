using GUIForCLI.Api.Entities.Helpers;

namespace GUIForCLI.Parsers
{
    public static class DriveInfoParser
    {
        public static List<DriveInfoModel>? ExtractDriveInfo(string text)
        {
            string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<DriveInfoModel> driveList = new List<DriveInfoModel>();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    string drive = parts[0];
                    long size = long.Parse(parts[1]);

                    driveList.Add(new DriveInfoModel { Drive = drive, Size = size });
                }
            }

            return driveList;
        }
    }

}
