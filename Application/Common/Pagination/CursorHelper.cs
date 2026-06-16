namespace Application.Common.Pagination;

public static class CursorHelper
{
    public static string Encode<TValue, TKey>(TValue value, TKey key)
    {
        var text = $"{value}|{key}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
    }

    public static (TValue Value, TKey Key)? Decode<TValue, TKey>(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor))
            return null;

        try
        {
            var bytes = Convert.FromBase64String(cursor);
            var text = System.Text.Encoding.UTF8.GetString(bytes);
            var parts = text.Split('|');

            if (parts.Length != 2) return null;

            var converterValue = System.ComponentModel.TypeDescriptor.GetConverter(typeof(TValue));
            var converterKey = System.ComponentModel.TypeDescriptor.GetConverter(typeof(TKey));

            var value = (TValue)converterValue.ConvertFromString(parts[0])!;
            var key = (TKey)converterKey.ConvertFromString(parts[1])!;

            return (value, key);
        }
        catch
        {
            
            return null;
        }
    }
}