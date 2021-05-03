using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayDashing.API.Helper
{
    public class ManageFiles
    {
        private static readonly Random Rnd = new();
        private readonly IWebHostEnvironment HostEnviroment;
        public ManageFiles(IWebHostEnvironment hostEnviroment)
        {
            HostEnviroment = hostEnviroment;
        }
        public async Task<string> Save(IFormFile file, string folderName, string fileNamePreFix = "")
        {
            if (file != null)
            {
                string fileName = fileNamePreFix + "_" + new String(Path.GetFileNameWithoutExtension(file.FileName).Take(15).ToArray()).Replace(' ', '-') + "_" + GetPostFixFileName() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(HostEnviroment.WebRootPath, "Upload", folderName, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {                    
                    await file.CopyToAsync(fileStream);
                }
                return $"/Upload/{folderName}/{fileName}";
            }
            return null;
        }

        public string GetPostFixFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffffff") + "_" + Rnd.Next(1, 100000).ToString();
        }
    }
}
