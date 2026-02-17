using System.Collections.Generic;
using System.Linq;
using TFSData.Entities;
using TFSData.Process;

namespace TFSData.Operations
{
    public class ProcessOperations
    {
        #region Constructor

        public ProcessOperations(List<AnalysisItem> analysisItems)
        {
            AnalysisItems = analysisItems;
        }

        #endregion

        public List<AnalysisItem> AnalysisItems { get; }
        public List<DataPoint>? DayOfTheWeekData { get; set; }
        public List<DataPoint>? HoursData { get; set; }
        public List<DataPoint>? MonthsData { get; set; }
        public int TaskCount { get; set; }

        public void CountUniqueRequests()
        {
            //Since the review requests are created automatically the time stamp should be within the same second.
            //group by the time stamp and count to total the number of tasks
            TaskCount = AnalysisItems.GroupBy(u => u.CreateDateTime).Select(f => f.First()).ToList().Count;
        }

        public void ProcessData()
        {
            MonthsData = ProcessMonthData.Months(AnalysisItems);
            DayOfTheWeekData = ProcessDayOfTheWeekData.Days(AnalysisItems);
            HoursData = ProcessHourData.Hours(AnalysisItems);
        }
    }
}