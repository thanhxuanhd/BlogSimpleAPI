using System.Collections.Generic;

namespace BlogSimple.BlazorApp.Models;

public class PagingViewModel<T> where T : class
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int TotalCount { get; set; }
    public List<T> Items { get; set; }
}