using FileToEmailLinker.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileToEmailLinker.Models.ViewModels
{
    public class MailingPlanListViewModel : IPaginationInfo
    {
        public ListViewModel<MailingPlan> MailingPlanList { get; set; }
        public string Search { get; set; } = string.Empty;
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public List<SelectListItem> PageLimitOptions { get; set; }

        public string? Action { get; set; }

        public string? TargetId { get; set; }

        int IPaginationInfo.CurrentPage => Page;

        int IPaginationInfo.TotalResults => MailingPlanList.TotalCount;

        int IPaginationInfo.ResultsPerPage => Limit;

        string IPaginationInfo.Search => Search;
    }
}
