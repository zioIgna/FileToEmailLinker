using FileToEmailLinker.Models.Entities;

namespace FileToEmailLinker.Models.ViewModels
{
    public class MailingPlanListViewModel : IPaginationInfo
    {
        public ListViewModel<MailingPlan> MailingPlanList { get; set; }
        public string Search { get; set; } = string.Empty;
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }

        int IPaginationInfo.CurrentPage => Page;

        int IPaginationInfo.TotalResults => MailingPlanList.TotalCount;

        int IPaginationInfo.ResultsPePage => Limit;

        string IPaginationInfo.Search => Search;
    }
}
