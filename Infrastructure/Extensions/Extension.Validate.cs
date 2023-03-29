namespace Infrastructure.Extensions
{
    public static partial class Extensions
    {
        public static bool IsEmpty(this object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ParseToString()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsNotEmpty(this object value)
        {
            return !IsEmpty(value);
        }
        public static bool IsNullOrZero(this object value)
        {
            if (value == null || value.ParseToString().Trim() == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
