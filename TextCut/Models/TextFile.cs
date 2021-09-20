namespace TextCut.Models
{
    public class TextFile
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        //public int Id { get; set; }
        public TextFile()
        {

        }
        public TextFile(string filePath, string fileName)
        {
            FilePath = filePath;
            FileName = fileName;
        }
    }
}
