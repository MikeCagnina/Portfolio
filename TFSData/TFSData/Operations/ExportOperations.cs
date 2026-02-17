using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFSData.Entities;
using Microsoft.Office.Interop.Excel;

namespace TFSData.Operations
{
    public class ExportOperations
    {
        #region Constructor
        public ExportOperations(ProcessOperations processOperations)
        {
            ProcessOperations = processOperations;
        }

        public ExportOperations(ProcessOperations processOperations, string FI, Workbook exportWorkbook)
        {
            ProcessOperations = processOperations;
            ExportWorkbook = exportWorkbook;
            ExportWorksheet = (Worksheet?)ExportWorkbook.Worksheets.Add(Type.Missing, exportWorkbook.Worksheets[exportWorkbook.Worksheets.Count], 1, XlSheetType.xlWorksheet);
            ExportWorksheet.Name = FI;
        }

        #endregion

        public List<string> ExportDataList { get; set; }

        public ProcessOperations ProcessOperations { get; set; }

        public Worksheet ExportWorksheet { get; set; }

        public Workbook ExportWorkbook { get; set; }

        public void AssembleExportData()
        {
            ExportDataList = new List<string>
{
"Month,Orphan Count,Task Count,Orphan Ratio"
};

            ExportWorksheet.Cells[1, 1] = "Month";
            ExportWorksheet.Cells[1, 2] = "Orphan Count";
            ExportWorksheet.Cells[1, 3] = "Task Count";
            ExportWorksheet.Cells[1, 4] = "Orphan Ratio";

            var counter = 1;

            foreach(DataPoint monthData in ProcessOperations.MonthsData.OrderBy(m => m.ID))
            {
                counter += 1;
                ExportDataList.Add(monthData.Description + "," + monthData.OrphanCount + "," + monthData.TaskCount + "," + monthData.OrphanRatio);

                ExportWorksheet.Cells[counter, 1] = monthData.Description;
                ExportWorksheet.Cells[counter, 2] = monthData.OrphanCount;
                ExportWorksheet.Cells[counter, 3] = monthData.TaskCount;
                ExportWorksheet.Cells[counter, 4] = monthData.OrphanRatio;
            }

            ExportDataList.Add("DayOfTheWeek,Orphan Count,Task Count,Orphan Ratio");
            counter += 1;
            ExportWorksheet.Cells[counter, 1] = "DayOfTheWeek";
            ExportWorksheet.Cells[counter, 2] = "Orphan Count";
            ExportWorksheet.Cells[counter, 3] = "Task Count";
            ExportWorksheet.Cells[counter, 4] = "Orphan Ratio";

            foreach(DataPoint dayData in ProcessOperations.DayOfTheWeekData.OrderBy(d => d.ID))
            {
                counter += 1;
                ExportDataList.Add(dayData.Description + "," + dayData.OrphanCount + "," + dayData.TaskCount + "," + dayData.OrphanRatio);

                ExportWorksheet.Cells[counter, 1] = dayData.Description;
                ExportWorksheet.Cells[counter, 2] = dayData.OrphanCount;
                ExportWorksheet.Cells[counter, 3] = dayData.TaskCount;
                ExportWorksheet.Cells[counter, 4] = dayData.OrphanRatio;
            }

            ExportDataList.Add("Hour,Orphan Count,Task Count,Orphan Ratio");
            counter += 1;
            ExportWorksheet.Cells[counter, 1] = "Hour";
            ExportWorksheet.Cells[counter, 2] = "Orphan Count";
            ExportWorksheet.Cells[counter, 3] = "Task Count";
            ExportWorksheet.Cells[counter, 4] = "Orphan Ratio";

            foreach(DataPoint hourData in ProcessOperations.HoursData.OrderBy(h => h.ID))
            {
                counter += 1;
                ExportDataList.Add(hourData.Description + "," + hourData.OrphanCount + "," + hourData.TaskCount + "," + hourData.OrphanRatio);

                ExportWorksheet.Cells[counter, 1] = hourData.Description;
                ExportWorksheet.Cells[counter, 2] = hourData.OrphanCount;
                ExportWorksheet.Cells[counter, 3] = hourData.TaskCount;
                ExportWorksheet.Cells[counter, 4] = hourData.OrphanRatio;
            }

            counter += 1;
            ExportDataList.Add("Total Task Count," + ProcessOperations.TaskCount);
            ExportWorksheet.Cells[counter, 1] = "Total Task Count," + ProcessOperations.TaskCount;
        }

        public void ExportData()
        {
            File.WriteAllLines(@"C:\Temp\Data.csv", ExportDataList);
        }

        public void ExportData(string FI)
        {
            File.WriteAllLines(@"C:\Temp\" + FI + "Data.csv", ExportDataList);
        }

        public static void WriteAnalysisItems(List<AnalysisItem> analysisItems)
        {
            List<string> exportAnalysisItems = new List<string>
{
"ID,CreateDate"
};
            exportAnalysisItems.AddRange(analysisItems.Select(item => string.Format("{0},{1:MM/dd/yyyy hh:mm:ss tt}", item.Id, item.CreateDateTime)));

            File.WriteAllLines(@"C:\Temp\AnalysisItems.csv", exportAnalysisItems);
        }

        public static void WriteAnalysisItems(List<AnalysisItem> analysisItems, string FI)
        {
            List<string> exportAnalysisItems = new List<string>
{
"ID,CreateDate"
};
            exportAnalysisItems.AddRange(analysisItems.Select(item => string.Format("{0},{1:MM/dd/yyyy hh:mm:ss tt}", item.Id, item.CreateDateTime)));

            File.WriteAllLines(@"C:\Temp\" + FI + "AnalysisItems.csv", exportAnalysisItems);
        }

        public static void ExportWorkItems(IEnumerable<ExportWorkItem> exportWorkItems)
        {
            List<string> exportData = new List<string>
{
"Id,AreaId,AreaPath,AttachedFileCount,AuthorizedDate,ChangedBy,ChangedDate,ClosedBy,ClosedDate,ClosedStatus,ClosingComment,CreatedBy,CreatedDate,Description,ExternalLinkCount,FinishDate,History,IsNew,IsOpen,IsPartialOpen,IsReadOnly,IsReadOnlyOpen,IterationPath,Links,NodeName,Project,Reason,RejectedByQA,ResolvedBy,ResolvedDate,Rev,RevisedDate,Revision,ReviewedBy,ReviewedDate,StartDate,State,Tags,Title,Type,Uri,WorkItemLinkHistory,WorkItemLinks"
};

            exportData.AddRange(from exportItem in exportWorkItems
                                select string.Format("{0},{1},{2},{3},{4:MM/dd/yyyy hh:mm tt},{5},{6:MM/dd/yyyy hh:mm tt},{7},{8:MM/dd/yyyy hh:mm tt},{9},{10},{11},{12:MM/dd/yyyy hh:mm tt},{13},{14},{15:MM/dd/yyyy hh:mm tt},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29:MM/dd/yyyy hh:mm tt},{30},{31:MM/dd/yyyy hh:mm tt},{32},{33},{34:MM/dd/yyyy hh:mm tt},{35:MM/dd/yyyy hh:mm tt},{36},{37},{38},{39},{40},{41},{42}",
                                exportItem.Id,
                                exportItem.AreaId,
                                exportItem.AreaPath,
                                exportItem.AttachedFileCount,
                                exportItem.AuthorizedDate,
                                exportItem.ChangedBy,
                                exportItem.ChangedDate,
                                exportItem.ClosedBy,
                                exportItem.ClosedDate,
                                exportItem.ClosedStatus,
                                exportItem.ClosingComment,
                                exportItem.CreatedBy,
                                exportItem.CreatedDate,
                                exportItem.Description,
                                exportItem.ExternalLinkCount,
                                exportItem.FinishDate,
                                exportItem.History,
                                exportItem.IsNew,
                                exportItem.IsOpen,
                                exportItem.IsPartialOpen,
                                exportItem.IsReadOnly,
                                exportItem.IsReadOnlyOpen,
                                exportItem.IterationPath,
                                exportItem.Links,
                                exportItem.NodeName,
                                exportItem.Project,
                                exportItem.Reason,
                                exportItem.RejectedByQA,
                                exportItem.ResolvedBy,
                                exportItem.ResolvedDate,
                                exportItem.Rev,
                                exportItem.RevisedDate,
                                exportItem.Revision,
                                exportItem.ReviewedBy,
                                exportItem.ReviewedDate,
                                exportItem.StartDate,
                                exportItem.State,
                                exportItem.Tags,
                                exportItem.Title,
                                exportItem.Type,
                                exportItem.Uri,
                                exportItem.WorkItemLinkHistory,
                                exportItem.WorkItemLinks
                                ));

            File.WriteAllLines(@"C:\Temp\WorkItems.csv", exportData);
        }

        public static void ExportWorkItems(IEnumerable<ExportWorkItem> exportWorkItems, string FI)
        {
            List<string> exportData = new List<string>
{
"Id,AreaId,AreaPath,AttachedFileCount,AuthorizedDate,ChangedBy,ChangedDate,ClosedBy,ClosedDate,ClosedStatus,ClosingComment,CreatedBy,CreatedDate,Description,ExternalLinkCount,FinishDate,History,IsNew,IsOpen,IsPartialOpen,IsReadOnly,IsReadOnlyOpen,IterationPath,Links,NodeName,Project,Reason,RejectedByQA,ResolvedBy,ResolvedDate,Rev,RevisedDate,Revision,ReviewedBy,ReviewedDate,StartDate,State,Tags,Title,Type,Uri,WorkItemLinkHistory,WorkItemLinks"
};

            exportData.AddRange(from exportItem in exportWorkItems
                                select string.Format("{0},{1},{2},{3},{4:MM/dd/yyyy hh:mm tt},{5},{6:MM/dd/yyyy hh:mm tt},{7},{8:MM/dd/yyyy hh:mm tt},{9},{10},{11},{12:MM/dd/yyyy hh:mm tt},{13},{14},{15:MM/dd/yyyy hh:mm tt},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29:MM/dd/yyyy hh:mm tt},{30},{31:MM/dd/yyyy hh:mm tt},{32},{33},{34:MM/dd/yyyy hh:mm tt},{35:MM/dd/yyyy hh:mm tt},{36},{37},{38},{39},{40},{41},{42}",
                                exportItem.Id,
                                exportItem.AreaId,
                                exportItem.AreaPath,
                                exportItem.AttachedFileCount,
                                exportItem.AuthorizedDate,
                                exportItem.ChangedBy,
                                exportItem.ChangedDate,
                                exportItem.ClosedBy,
                                exportItem.ClosedDate,
                                exportItem.ClosedStatus,
                                exportItem.ClosingComment,
                                exportItem.CreatedBy,
                                exportItem.CreatedDate,
                                exportItem.Description,
                                exportItem.ExternalLinkCount,
                                exportItem.FinishDate,
                                exportItem.History,
                                exportItem.IsNew,
                                exportItem.IsOpen,
                                exportItem.IsPartialOpen,
                                exportItem.IsReadOnly,
                                exportItem.IsReadOnlyOpen,
                                exportItem.IterationPath,
                                exportItem.Links,
                                exportItem.NodeName,
                                exportItem.Project,
                                exportItem.Reason,
                                exportItem.RejectedByQA,
                                exportItem.ResolvedBy,
                                exportItem.ResolvedDate,
                                exportItem.Rev,
                                exportItem.RevisedDate,
                                exportItem.Revision,
                                exportItem.ReviewedBy,
                                exportItem.ReviewedDate,
                                exportItem.StartDate,
                                exportItem.State,
                                exportItem.Tags,
                                exportItem.Title,
                                exportItem.Type,
                                exportItem.Uri,
                                exportItem.WorkItemLinkHistory,
                                exportItem.WorkItemLinks
                                ));

            File.WriteAllLines(@"C:\Temp\" + FI + "WorkItems.csv", exportData);
        }
    }
}