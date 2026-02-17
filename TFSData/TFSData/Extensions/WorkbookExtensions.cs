using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using TFSData.Entities;

namespace TFSData.Extensions
{
    public static class WorkbookExtensions
    {
        public static Worksheet RenameDataSetWorksheet(this Workbook exportWorkbook, string currentName, string dataSetName)
        {
            Worksheet exportWorksheet = (Worksheet)exportWorkbook.Worksheets[currentName];
            exportWorksheet.Name = dataSetName;

            return exportWorksheet;
        }

        public static Worksheet MakeDataSetWorksheet(this Workbook exportWorkbook, string dataSetName)
        {
            Worksheet exportWorksheet = (Worksheet)exportWorkbook.Worksheets.Add(Type.Missing, exportWorkbook.Worksheets[exportWorkbook.Worksheets.Count], 1, XlSheetType.xlWorksheet);
            exportWorksheet.Name = dataSetName;

            return exportWorksheet;
        }

        public static Workbook WriteAdminData(this Workbook adminWorkbook, List<ExportWorkItem> codeReviewData, List<ExportWorkItem> codeReviewResponseData, List<ExportWorkItem> changeSetData)
        {
            List<ExportWorkItem> filteredChangeSets = (from changeset in changeSetData
                                                       let codeReviewCount = (from codeReview in codeReviewData where codeReview.ChangeSetIds.Contains(changeset.Id) select codeReview).Count()
                                                       where codeReviewCount == 0
                                                       orderby changeset.OwnerDisplayName, changeset.Id
                                                       select changeset
            ).ToList();

            Worksheet adminWorksheet = adminWorkbook.RenameDataSetWorksheet("Sheet1", "Admin Data");

            adminWorksheet.WriteAdminHeader();

            //Code Review
            adminWorksheet.Cells[3, 1] = codeReviewData.Count;
            adminWorksheet.Cells[3, 2] = codeReviewData.Count(cr => cr.ClosedStatus.Equals("Abandoned"));
            adminWorksheet.Cells[3, 3] = codeReviewData.Count(cr => cr.ClosedStatus.Equals("Completed"));
            adminWorksheet.Cells[3, 4] = codeReviewData.Count(cr => cr.ClosedStatus.Equals("Checked-in"));
            adminWorksheet.Cells[3, 5] = codeReviewData.Count(cr => !cr.ClosedStatus.Equals("Abandoned") && !cr.ClosedStatus.Equals("Completed") && !cr.ClosedStatus.Equals("Checked-in"));
            adminWorksheet.Cells[6, 1] = codeReviewResponseData.Count;

            //ChangeSet
            adminWorksheet.Cells[9, 1] = changeSetData.Count;
            adminWorksheet.Cells[9, 2] = filteredChangeSets.Count;

            if(changeSetData.Count - filteredChangeSets.Count > 0 && changeSetData.Count > 0)
            {
                decimal annualReviewPercentage = decimal.Round(100.0M * ((changeSetData.Count - filteredChangeSets.Count) / (decimal)changeSetData.Count), 2);
                adminWorksheet.Cells[10, 2] = annualReviewPercentage.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                adminWorksheet.Cells[10, 2] = "ERROR";
            }

            ExportWorkItem longestCodeReview = (from codeReview in codeReviewData orderby codeReview.DurationTimeSpan descending select codeReview).FirstOrDefault();
            adminWorksheet.Cells[11, 2] = longestCodeReview != null ? longestCodeReview.Id.ToString() : "None";
            adminWorksheet.Cells[11, 3] = longestCodeReview != null ? longestCodeReview.DurationTimeSpan.ToReadableString() : "None";

            ExportWorkItem shortestCodeReview = (from codeReview in codeReviewData where codeReview.DurationTimeSpan.Ticks > 0 orderby codeReview.DurationTimeSpan select codeReview).FirstOrDefault();
            adminWorksheet.Cells[12, 2] = shortestCodeReview != null ? shortestCodeReview.Id.ToString() : "None";
            adminWorksheet.Cells[12, 3] = shortestCodeReview != null ? shortestCodeReview.DurationTimeSpan.ToReadableString() : "None";
            if(codeReviewData.Count > 0)
            {
                TimeSpan averageCodeReview = new TimeSpan((long)(from codeReview in codeReviewData select codeReview.DurationTimeSpan).Average(timeSpan => timeSpan.Ticks));
                adminWorksheet.Cells[13, 2] = averageCodeReview.ToReadableString();
            }

            int counter = 17;

            List<string> devNames = changeSetData.Select(d => d.OwnerDisplayName).Distinct().ToList();
            devNames.AddRange(codeReviewData.Select(d => d.CreatedBy).Distinct().ToList());
            devNames.AddRange(codeReviewResponseData.Select(d => d.ClosedBy).Distinct().ToList());

            foreach(string devName in devNames.Where(d => !string.IsNullOrEmpty(d)).Distinct().OrderBy(d => d))
            {
                int changeSetCount = changeSetData.Count(d => d.OwnerDisplayName.Equals(devName));
                int noCodeReviewChangeSetCount = filteredChangeSets.Count(d => d.OwnerDisplayName.Equals(devName));
                int codeReviewCount = codeReviewData.Count(d => d.CreatedBy != null && d.CreatedBy.Equals(devName));
                int codeReviewResponseCount = codeReviewResponseData.Count(d => d.ClosedBy != null && d.ClosedBy.Equals(devName));
                decimal percentChangeSet = changeSetCount == 0 || changeSetData.Count == 0 ? 0.0M : decimal.Round(100.0M * (changeSetCount / (decimal)changeSetData.Count), 2);
                decimal percentNoCodeReview = noCodeReviewChangeSetCount == 0 || filteredChangeSets.Count == 0 ? 0.0M : decimal.Round(100.0M * (noCodeReviewChangeSetCount / (decimal)filteredChangeSets.Count), 2);
                decimal percentCodeReview = codeReviewCount == 0 || codeReviewData.Count == 0 ? 0.0M : decimal.Round(100.0M * (codeReviewCount / (decimal)codeReviewData.Count), 2);
                decimal personalAdherenceRate = noCodeReviewChangeSetCount == 0 || changeSetCount == 0 ? 100.0M : decimal.Round(100.0M * (noCodeReviewChangeSetCount / (decimal)changeSetCount), 2);
                TimeSpan averageCodeReviewTime = codeReviewCount == 0 ? new TimeSpan() : new TimeSpan((long)(from review in codeReviewData where review.CreatedBy.Equals(devName) select review.DurationTimeSpan).Average(timeSpan => timeSpan.Ticks));
                TimeSpan averageCodeReviewResponseTime = codeReviewResponseCount == 0 ? new TimeSpan() : new TimeSpan((long)(from response in codeReviewResponseData where response.ClosedBy.Equals(devName) select response.DurationTimeSpan).Average(timeSpan => timeSpan.Ticks));
                adminWorksheet.Cells[counter, 1] = devName;
                adminWorksheet.Cells[counter, 2] = changeSetCount;
                adminWorksheet.Cells[counter, 3] = noCodeReviewChangeSetCount;
                adminWorksheet.Cells[counter, 4] = codeReviewCount;
                adminWorksheet.Cells[counter, 5] = codeReviewResponseCount;
                adminWorksheet.Cells[counter, 6] = percentChangeSet;
                adminWorksheet.Cells[counter, 7] = percentNoCodeReview;
                adminWorksheet.Cells[counter, 8] = percentCodeReview;
                adminWorksheet.Cells[counter, 9] = personalAdherenceRate;
                adminWorksheet.Cells[counter, 10] = averageCodeReviewTime.ToReadableString();
                adminWorksheet.Cells[counter, 11] = averageCodeReviewResponseTime.ToReadableString();
                counter++;
            }

            return adminWorkbook;
        }

        public static Workbook WriteChangeSetData(this Workbook exportWorkbook, List<ExportWorkItem> changeSetData, List<ExportWorkItem> filteredChangeSets)
        {
            int counter = 1;
            Worksheet exportWorksheet = exportWorkbook.MakeDataSetWorksheet("No Code Review Change Sets");
            counter = exportWorksheet.WriteNoCodeReviewChangeSetDataHeader(counter);

            //Write the Change Sets with no Code Review
            foreach(ExportWorkItem workItem in filteredChangeSets)
            {
                exportWorksheet.Cells[counter, 1] = workItem.Id;
                exportWorksheet.Cells[counter, 2] = workItem.OwnerDisplayName;
                exportWorksheet.Cells[counter, 3] = workItem.CreationDate;
                exportWorksheet.Cells[counter, 4] = workItem.PolicyOverrideComment;
                exportWorksheet.Cells[counter, 6] = workItem.Description;
                counter++;
            }

            exportWorksheet = exportWorkbook.MakeDataSetWorksheet("Change Sets");

            counter = 1;
            counter = exportWorksheet.WriteChangeSetDataHeader(counter);

            //Write the Change Sets
            foreach(ExportWorkItem workItem in changeSetData)
            {
                exportWorksheet.Cells[counter, 1] = workItem.Id;
                exportWorksheet.Cells[counter, 2] = workItem.OwnerDisplayName;
                exportWorksheet.Cells[counter, 3] = workItem.CreationDate;
                exportWorksheet.Cells[counter, 4] = workItem.PolicyOverrideComment;
                exportWorksheet.Cells[counter, 6] = workItem.Description;
                counter++;
            }

            return exportWorkbook;
        }

        public static Workbook WriteCodeReviewData(this Workbook exportWorkbook, List<ExportWorkItem> codeReviewData)
        {
            int counter = 1;
            Worksheet exportWorksheet = exportWorkbook.MakeDataSetWorksheet("Code Reviews");
            counter = exportWorksheet.WriteCodeReviewDataHeader(counter);
            counter++;

            //Write the Code Review Data
            foreach(ExportWorkItem workItem in codeReviewData)
            {
                exportWorksheet.Cells[counter, 1] = workItem.Id;
                exportWorksheet.Cells[counter, 2] = workItem.CreatedBy;
                exportWorksheet.Cells[counter, 3] = workItem.CreatedDate;
                exportWorksheet.Cells[counter, 4] = workItem.ChangedBy;
                exportWorksheet.Cells[counter, 5] = workItem.ChangedDate;
                exportWorksheet.Cells[counter, 6] = workItem.ClosedBy;
                exportWorksheet.Cells[counter, 7] = workItem.ClosedDate;
                exportWorksheet.Cells[counter, 8] = workItem.ClosedStatus;
                exportWorksheet.Cells[counter, 9] = workItem.DurationTimeSpan.ToReadableString();
                exportWorksheet.Cells[counter, 10] = workItem.ChangesetCount;
                exportWorksheet.Cells[counter, 11] = workItem.ChangeSetIds.Count > 0 ? string.Join(" ", workItem.ChangeSetIds) : string.Empty;
                exportWorksheet.Cells[counter, 12] = workItem.Title;
                exportWorksheet.Cells[counter, 13] = workItem.Description;
                exportWorksheet.Cells[counter, 14] = workItem.Reason;
                exportWorksheet.Cells[counter, 15] = workItem.RejectCount;
                exportWorksheet.Cells[counter, 16] = workItem.RejectedByQA;
                exportWorksheet.Cells[counter, 17] = workItem.RelatedLinkCount;
                exportWorksheet.Cells[counter, 18] = workItem.State;
                exportWorksheet.Cells[counter, 19] = workItem.AreaPath;
                exportWorksheet.Cells[counter, 20] = workItem.Revision;

                counter++;
            }

            return exportWorkbook;
        }

        public static Workbook WriteCodeReviewResponseData(this Workbook exportWorkbook, List<ExportWorkItem> codeReviewResponseData)
        {
            int counter = 1;
            Worksheet exportWorksheet = exportWorkbook.MakeDataSetWorksheet("Code Review Responses");
            counter = exportWorksheet.WriteCodeReviewResponseDataHeader(counter);
            counter++;

            //Write the Code Review Response Data
            foreach(ExportWorkItem workItem in codeReviewResponseData.OrderBy(d => d.ClosedBy))
            {
                exportWorksheet.Cells[counter, 1] = workItem.Id;
                exportWorksheet.Cells[counter, 2] = workItem.ClosedBy;
                exportWorksheet.Cells[counter, 3] = workItem.ClosedDate;
                exportWorksheet.Cells[counter, 4] = workItem.CreatedBy;
                exportWorksheet.Cells[counter, 5] = workItem.CreatedDate;
                exportWorksheet.Cells[counter, 6] = workItem.ChangedBy;
                exportWorksheet.Cells[counter, 7] = workItem.ChangedDate;
                exportWorksheet.Cells[counter, 8] = workItem.ClosedStatus + "---" + workItem.ClosingComment;
                exportWorksheet.Cells[counter, 9] = workItem.DurationTimeSpan.ToReadableString();
                exportWorksheet.Cells[counter, 10] = workItem.ChangesetCount;
                exportWorksheet.Cells[counter, 11] = workItem.ChangeSetIds.Count > 0 ? string.Join(" ", workItem.ChangeSetIds) : string.Empty;
                exportWorksheet.Cells[counter, 12] = workItem.Title;
                exportWorksheet.Cells[counter, 13] = workItem.Description;
                exportWorksheet.Cells[counter, 14] = workItem.Reason;
                exportWorksheet.Cells[counter, 15] = workItem.RejectCount;
                exportWorksheet.Cells[counter, 16] = workItem.RejectedByQA;
                exportWorksheet.Cells[counter, 17] = workItem.RelatedLinkCount;
                exportWorksheet.Cells[counter, 18] = workItem.State;
                exportWorksheet.Cells[counter, 19] = workItem.AreaPath;
                exportWorksheet.Cells[counter, 20] = workItem.Revision;

                counter++;
            }

            return exportWorkbook;
        }
    }
}