namespace FileToEmailLinker.Models.ViewModels
{
    public interface IPaginationInfo
    {
        int CurrentPage { get; }
        int TotalResults { get; }
        int ResultsPerPage { get; }
        string Search { get; }
        string? Action { get; }
        string? TargetId { get; }
    }
}
