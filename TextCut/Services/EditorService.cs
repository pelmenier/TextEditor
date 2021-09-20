using System.Text;
using System.Text.RegularExpressions;

namespace TextCut.Services
{
    public static class EditorService
    {
        public static string RemoveWorld(string line, int value)
        {
            StringBuilder text = new StringBuilder();
            if (line == "")
                return "";
            //line = line.Trim();
            string[] text_arr = line.Split(' ');
            foreach (string item in text_arr)
            {
                if (item != "")
                    if (item.Length < value)
                    {
                        //text.Append(item.Remove(0, item.Length) + " ");
                    }
                    else
                        text.Append(item + " ");
            }
            return text.ToString();
        }

        public static string RemovePunctuationMarks(string line)
        {
            //return line.Trim(new Char[] { ',', '*', '.', '!', '?', ':', ';' });  
            return Regex.Replace(line, "(\\p{P})", "");
        }
    }
}
