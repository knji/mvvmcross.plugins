using System.Text.RegularExpressions;

namespace Pidac.MvvmCross.Plugins.Mapping
{
    public static class StringHelper
    {
        public static int GetUniqueIdOfString(string inputString)
        {
            return RemoveAllSpacesFromStrng(inputString).GetHashCode();
        }

        public static string RemoveAllSpacesFromStrng(string inputString)
        {
            if (inputString == null) return null;
            return Regex.Replace(inputString, @"\s+", string.Empty);
        }

        public static string RemoveAllSpacesFromEndofStrng(string inputString)
        {
            return inputString.TrimEnd(' ');
        }

        public static bool IsNullOrWhiteSpace(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return true;
            return !Regex.Match(inputString, @"\S").Success;
        }

        public static bool IsEmptyString(string inputString)
        {
            // match any non=white space character 
            return !Regex.Match(inputString, @"\S").Success;
        }

        public static string RemoveAllSpacesFromStartOfStrng(string inputString)
        {
            return inputString.TrimStart(' ');
        }

        public static string RemoveAllSpacesFromStartAndEndOfStrng(string inputString)
        {
            return inputString.Trim();
        }

        public static bool FindPartOfString(string target, string partToFind)
        {
            bool found = false;
            if (!string.IsNullOrEmpty(target) && partToFind != null)
                if (Regex.Match(target, partToFind, RegexOptions.IgnoreCase).Success)
                    found = true;
            return found;
        }
    }
}