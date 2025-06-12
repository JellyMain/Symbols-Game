namespace Utils
{
    public static class StringUtility
    {
        private static readonly string[] byteUnits = { "b", "kb", "mb", "gb", "tb" };
    
    
        public static (int value, string unit) FormatMemoryAllocation(int bytes)
        {
            if (bytes <= 0)
            {
                return (0, byteUnits[0]);
            }

            int order = 0;
            int size = bytes;

            while (size >= 1024 && order < byteUnits.Length - 1)
            {
                order++;
                size /= 1024;
            }

            string unit = byteUnits[order];

            return (size, unit);
        }
        
        
        public static string AddColorTag(string str, string colorName)
        {
            return $"<color={colorName}>{str}</color>";
        }
        
        
        public static string AddColorTag(char c, string colorName)
        {
            return $"<color={colorName}>{c}</color>";
        }
    }
}
