namespace ArisStudio.Utils
{
    public static class ArrayHelper
    {
        public static bool IsIndexInRange<T>(T[] array, int index)
        {
            return index >= 0 && index < array.Length;
        }
    }
}
