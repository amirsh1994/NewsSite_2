namespace DomainModel.ViewModel;

public class PageModel
{
    private int _pageSize;
    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value==0)
            {
                value = 10;
            }
            _pageSize = value;
        }
    }

    public int RecordCount { get; set; }

    public int PageCount
    {
        get
        {
            if (RecordCount % PageSize == 0)
            {
                return RecordCount / PageSize;
            }

            return RecordCount / PageSize + 1;
        }
    }

    public int PageIndex { get; set; }
}