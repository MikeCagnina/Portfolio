using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TFSData.Entities;

namespace TFSData.Process
{
    public static class ProcessMonthData
    {
        public static List<DataPoint> Months(List<AnalysisItem> analysisItems)
        {

            List<DataPoint> monthsData = new List<DataPoint>();
            for(int monthCounter = 1; monthCounter <= 12; monthCounter++)
            {
                DataPoint month = new DataPoint
                {
                    ID = monthCounter,
                    Description = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthCounter),
                    OrphanCount = analysisItems.Count(m => m.CreateDateTime.Month == monthCounter),
                    TaskCount = analysisItems.Where(c => c.CreateDateTime.Month == monthCounter).GroupBy(u => u.CreateDateTime).Select(f => f.First()).ToList().Count
                };

                month.OrphanRatio = string.Format("{0} to {1}", Math.Round((double)month.OrphanCount / month.TaskCount, 3), 1);
                monthsData.Add(month);
            }

            return monthsData;
        }
    }
}