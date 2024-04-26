namespace FileToEmailLinker.Models.ViewModels
{
    public interface IPaginationInfo
    {
        int CurrentPage { get; }
        int TotalResults { get; }
        int ResultsPePage { get; }
        string Search { get; }
    }
}
