using System;
using System.Collections.Generic;
using System.Linq;
using TFSData.Entities;

namespace TFSData.Process
{
    public static class ProcessHourData
    {
        public static List<DataPoint> Hours(List<AnalysisItem> analysisItems)
        {
            List<DataPoint> hoursData = new List<DataPoint>();
            for(int hourCounter = 0; hourCounter < 24; hourCounter++)
            {
                DataPoint hour = new DataPoint
                {
                    ID = hourCounter,
                    Description = hourCounter + (hourCounter < 12 ? "AM" : "PM"),
                    OrphanCount = analysisItems.Count(m => m.CreateDateTime.Hour == hourCounter),
                    TaskCount = analysisItems.Where(c => c.CreateDateTime.Hour == hourCounter).GroupBy(u => u.CreateDateTime).Select(f => f.First()).ToList().Count,
                };

                hour.OrphanRatio = string.Format("{0} to {1}", Math.Round((double)hour.OrphanCount / hour.TaskCount, 3), 1);
                hoursData.Add(hour);
            }

            return hoursData;
        }
    }
}