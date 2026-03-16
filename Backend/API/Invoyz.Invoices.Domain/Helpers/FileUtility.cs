namespace Invoyz.Invoices.Domain.Helpers
{
    public interface IFileUtility
    {
        void SaveFile(byte[] fileContent, string fileName);
    }
    public class FileUtility : IFileUtility
    {
        public void SaveFile(byte[] fileContent, string fileName)
        {
            string tempPath = Path.GetTempPath();
            string tempFilePath = Path.Combine(tempPath, Path.GetFileName(fileName));

            File.WriteAllBytes(tempFilePath, fileContent);
        }
    }
}
