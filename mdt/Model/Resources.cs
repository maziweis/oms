using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdt.Model
{
    public class Resources
    {
        public string ResId { get; set; }
        public string BookId { get; set; }
        public string Catalog1 { get; set; }
        public string ResType { get; set; }
        public string ResBigType { get; set; }
        public string TzResType { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string SchoolStage { get; set; }
        public string Grade { get; set; }
        public string Subject { get; set; }
        public string Edition { get; set; }
        public string BookReel { get; set; }
        public string ResourceClass { get; set; }
        public string ResourceStyle { get; set; }
        public string ResourceType { get; set; }
        public string ComeFrom { get; set; }

        public string Cover { get; set; }
        public string KeyWords { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ParentID { get; set; }
    }

    public class Files
    {
        public Guid ID { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string FileExtension { get; set; }
        public int FileSize { get; set; }
        public string FilePath { get; set; }
        public int FileType { get; set; }
        public bool FinishStatus { get; set; }
        public int EncryptStatus { get; set; }
        public int ConvertStatus { get; set; }
        public DateTime? UploadTime { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Catalog { get; set; }

        public int BookID { get; set; }

        public int? SchoolStage { get; set; }
        public int? Grade { get; set; }
        public int? Subject { get; set; }
        public int? Edition { get; set; }
        public int? BookReel { get; set; }
        public int? ResourceClass { get; set; }
        public int? ResourceStyle { get; set; }
        public int? ResourceType { get; set; }
        public string ComeFrom { get; set; }
        public string KeyWords { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int ParentID { get; set; }
    }

    public class Resource
    {
        public Guid FileID { get; set; }
        public int Catalog { get; set; }

        public int BookID { get; set; }

        public int? SchoolStage { get; set; }
        public int? Grade { get; set; }
        public int? Subject { get; set; }
        public int? Edition { get; set; }
        public int? BookReel { get; set; }
        public int? ResourceClass { get; set; }
        public int? ResourceStyle { get; set; }
        public int? ResourceType { get; set; }
        public string ComeFrom { get; set; }
        public string KeyWords { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int ParentID { get; set; }
    }
}