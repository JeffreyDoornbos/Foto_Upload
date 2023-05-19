using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Foto_Upload.Pages
{
    public class FotoGridModel : PageModel
    {
        private readonly string _dbPath = "photocloud.db";
        private readonly string _uploadFolderPath;

        public List<PhotoModel> Photos { get; set; }

        public FotoGridModel(IWebHostEnvironment webHostEnvironment)
        {
            _uploadFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
        }

        public void OnGet()
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();

                var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT FileName, FilePath, UploadedBy, Evenement FROM Files";

                var photos = new List<PhotoModel>();

                using (var reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var photo = new PhotoModel
                        {
                            FileName = reader.GetString(0),
                            FilePath = reader.GetString(1),
                            UploadedBy = reader.GetString(2),
                            Evenement = reader.GetString(3)
                        };

                        photos.Add(photo);
                    }
                }

                Photos = photos;
            }
        }
    }

    public class PhotoModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string UploadedBy { get; set; }
        public string Evenement { get; set; }
        public string FullImagePath => Path.Combine("/uploads", Path.GetFileName(FilePath));
    }
}
