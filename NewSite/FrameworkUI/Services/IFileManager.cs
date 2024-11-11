using DomainModel.Models.Framework;

namespace NewSite.FrameworkUI.Services
{
    public interface IFileManager
    {
       bool RemoveFile(string path);

       string ToPhysicalAddress(string fileName,string folderName);

        OperationResult SaveFile(IFormFile file, string folderName);

        OperationResult ValidateFileSize(IFormFile file,long minCapacity,long maxCapacity);

        bool ValidateFileName(string fileName);

        string ToUniqueFileName(string fileName);

        string ToRelativeAddress(string uniqueFileName,string folder);


    }
}
