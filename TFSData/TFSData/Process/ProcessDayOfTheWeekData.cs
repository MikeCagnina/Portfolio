using System;
using System.Collections.Generic;
using System.Linq;
using TFSData.Entities;

namespace TFSData.Process
{
    public static class ProcessDayOfTheWeekData
    {
        public static List<DataPoint> Days(List<AnalysisItem> analysisItems)
        {
            List<DataPoint> daysData = new List<DataPoint>();
            foreach(DataPoint dayOfTheWeek in Enum.GetValues(typeof(DayOfWeek)).OfType<DayOfWeek>().ToList().Select(day => new DataPoint
            {
                ID = Convert.ToInt32(day.ToString("d")),
                Description = day.ToString(),
                OrphanCount = analysisItems.Count(m => m.CreateDateTime.DayOfWeek == day),
                TaskCount = analysisItems.Where(c => c.CreateDateTime.DayOfWeek == day).GroupBy(u => u.CreateDateTime).Select(f => f.First()).ToList().Count
            }))
            {
                dayOfTheWeek.OrphanRatio = string.Format("{0} to {1}", Math.Round((double)dayOfTheWeek.OrphanCount / dayOfTheWeek.TaskCount, 3), 1);
                daysData.Add(dayOfTheWeek);
            }

            return daysData;
        }
    }
}