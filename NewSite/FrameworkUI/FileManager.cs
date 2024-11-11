using DomainModel.Models.Framework;
using NewSite.FrameworkUI.Services;

namespace NewSite.FrameworkUI
{
    public class FileManager(IHostEnvironment env) : IFileManager
    {
        public bool RemoveFile(string? path)
        {
            if (path == null || path.ToLower() == "~/NoImage/noImage.jpg")
            {
                return false;
            }
            if (!System.IO.File.Exists(path))
            {
                return false;
            }
            System.IO.File.Delete(path);
            return true;

        }

        public OperationResult SaveFile(IFormFile?  file, string folderName)
        {
            OperationResult op = new OperationResult();

            var address = Path.GetFileName(file?.FileName);

            string uniqueFile = ToUniqueFileName(address);

            address = ToPhysicalAddress(uniqueFile, folderName);

            FileStream fs = new FileStream(address, FileMode.Create);
            try
            {
                file?.CopyTo(fs);
                return op.ToSuccess(ToRelativeAddress(uniqueFile, folderName));//32b6f602_e4e5_44cb_b1e9_fc15dbf7a1bbiphone.jpg
            }
            catch (Exception ex)
            {
                return op.ToError(ex.Message);
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }

        }

        public string ToPhysicalAddress(string fileName, string folderName)
        {
            return env.ContentRootPath + @"\wwwroot\" + folderName + @"\" + fileName;
        }

        public string ToUniqueFileName(string fileName)
        {
            return Guid.NewGuid().ToString().Replace("-", "_") + fileName;
        }

        public OperationResult ValidateFileSize(IFormFile? file, long minCapacity, long maxCapacity)
        {
            var op = new OperationResult();

            if (file == null) return op.ToSuccess("File Size Is Valid");

            if (file.Length < minCapacity || file.Length > maxCapacity)
            {
                return op.ToError("Invalid File Size");
            }
            return op.ToSuccess("File Size Is Valid");
        }

        public bool ValidateFileName(string? fileName)
        {
            if (fileName == null)
            {
                return true;
            }
            fileName = fileName.Trim().ToLower();
            if (fileName.Contains(".php") || fileName.Contains(".asp") || fileName.Contains(".ascx"))
            {
                return false;
            }
            return true;
        }

        public string ToRelativeAddress(string uniqueFileName, string folder)
        {
            return @"~/" + folder + @"/" + uniqueFileName;
        }
    }
}
