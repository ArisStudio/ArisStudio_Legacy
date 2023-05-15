namespace ArisStudio.Utils
{
    public static class AsHelper
    {
        public static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }
    }
}
