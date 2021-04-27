namespace WareHouseSys.Models
{
    public class Attachment
    {
        public int AttachmentType { get; set; }
        public int ObjectType { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string NoteText { get; set; }
        public string MimeType { get; set; }
        public string Content { get; set; }
        public int FileSizeInBytes { get; set; }
    }
}