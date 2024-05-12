using FileToEmailLinker.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileToEmailLinker.Models.ViewModels.Dashboard
{
    public class AlertsListViewModel : IPaginationInfo
    {
        public ListViewModel<Alert> Alerts { get; set; }
        public string Search { get; set; } = string.Empty;
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public List<SelectListItem> PageLimitOptions { get; set; }
        int IPaginationInfo.CurrentPage => Page;

        int IPaginationInfo.TotalResults => Alerts.TotalCount;

        int IPaginationInfo.ResultsPerPage => Limit;

        string IPaginationInfo.Search => Search;
    }
}
