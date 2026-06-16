namespace Application.Common.Pagination;
using System.Collections;

public class CursorPage<T>
{
    public IEnumerable<T> Items { get; set; }
    public string? NextCursorValue { get; set; }
    public int? NextCursorTieBreakerId { get; set; }
    public bool HasMore { get; set; }
}

public class CursorPagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public string? NextCursor { get; set; }
    public bool HasMore { get; set; }
}