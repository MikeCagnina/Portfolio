using Microsoft.Office.Interop.Excel;

namespace TFSData.Extensions
{
    public static class WorksheetExtensions
    {
        public static void WriteAdminHeader(this Worksheet adminWorksheet)
        {
            adminWorksheet.Cells[1, 1] = "Totals";
            adminWorksheet.Cells[2, 1] = "Code Review Count";
            adminWorksheet.Cells[2, 2] = "Code Review Closed Status Abandoned Count";
            adminWorksheet.Cells[2, 3] = "Code Review Closed Status Completed Count";
            adminWorksheet.Cells[2, 4] = "Code Review Closed Status Checked In Count";
            adminWorksheet.Cells[2, 5] = "Code Review Closed Status Other Count";
            adminWorksheet.Cells[5, 1] = "Code Review Response Count";

            adminWorksheet.Cells[8, 1] = "ChangeSet Count";
            adminWorksheet.Cells[8, 2] = "ChangeSet Without Code Review Count";

            adminWorksheet.Cells[10, 1] = "Code Review Request to Change Set Percentage";
            adminWorksheet.Cells[11, 1] = "Longest Code Review";
            adminWorksheet.Cells[12, 1] = "Shortest Code Review";
            adminWorksheet.Cells[13, 1] = "Average Code Review Length";
            adminWorksheet.Cells[15, 1] = "Developer Counts";
            adminWorksheet.Cells[16, 1] = "Developer Name";
            adminWorksheet.Cells[16, 2] = "ChangeSet Count";
            adminWorksheet.Cells[16, 3] = "ChangeSet Without Code Review Count";
            adminWorksheet.Cells[16, 4] = "Code Review Count";
            adminWorksheet.Cells[16, 5] = "Code Review Response Count";
            adminWorksheet.Cells[16, 6] = "Percentage of ChangeSets";
            adminWorksheet.Cells[16, 7] = "Percentage of No Code Review ChangeSets";
            adminWorksheet.Cells[16, 8] = "Percentage of Code Reviews";
            adminWorksheet.Cells[16, 9] = "Personal Adherence Rate";
            adminWorksheet.Cells[16, 10] = "Average Code Review Length";
            adminWorksheet.Cells[16, 11] = "Average Code Review Response Length";
        }

        public static int WriteNoCodeReviewChangeSetDataHeader(this Worksheet exportWorksheet, int counter)
        {
            exportWorksheet.Cells[counter, 1] = "No Code Review ChangeSet Results";
            counter++;

            exportWorksheet.Cells[counter, 1] = "Id";
            exportWorksheet.Cells[counter, 2] = "OwnerDisplayName";
            exportWorksheet.Cells[counter, 3] = "CreationDate";
            exportWorksheet.Cells[counter, 4] = "PolicyOverrideComment";
            exportWorksheet.Cells[counter, 6] = "Description";
            counter++;

            return counter;
        }

        public static int WriteChangeSetDataHeader(this Worksheet exportWorksheet, int counter)
        {
            exportWorksheet.Cells[counter, 1] = "All ChangeSet Results";
            counter++;

            exportWorksheet.Cells[counter, 1] = "Id";
            exportWorksheet.Cells[counter, 2] = "OwnerDisplayName";
            exportWorksheet.Cells[counter, 3] = "CreationDate";
            exportWorksheet.Cells[counter, 4] = "PolicyOverrideComment";
            exportWorksheet.Cells[counter, 6] = "Description";
            counter++;

            return counter;
        }

        public static int WriteCodeReviewDataHeader(this Worksheet exportWorksheet, int counter)
        {
            exportWorksheet.Cells[counter, 1] = "All Code Review Results";
            counter++;

            exportWorksheet.Cells[counter, 1] = "Id";
            exportWorksheet.Cells[counter, 2] = "Created By";
            exportWorksheet.Cells[counter, 3] = "Created Date";
            exportWorksheet.Cells[counter, 4] = "Changed By";
            exportWorksheet.Cells[counter, 5] = "Changed Date";
            exportWorksheet.Cells[counter, 6] = "Closed By";
            exportWorksheet.Cells[counter, 7] = "Closed Date";
            exportWorksheet.Cells[counter, 8] = "Closed Status";
            exportWorksheet.Cells[counter, 9] = "Duration";
            exportWorksheet.Cells[counter, 10] = "ChangeSet Count";
            exportWorksheet.Cells[counter, 11] = "ChangeSet Ids";
            exportWorksheet.Cells[counter, 12] = "Title";
            exportWorksheet.Cells[counter, 13] = "Description";
            exportWorksheet.Cells[counter, 14] = "Reason";
            exportWorksheet.Cells[counter, 15] = "RejectCount";
            exportWorksheet.Cells[counter, 16] = "RejectedByQA";
            exportWorksheet.Cells[counter, 17] = "RelatedLinkCount";
            exportWorksheet.Cells[counter, 18] = "State";
            exportWorksheet.Cells[counter, 19] = "Area Path";
            exportWorksheet.Cells[counter, 20] = "Revision";

            return counter;
        }

        public static int WriteCodeReviewResponseDataHeader(this Worksheet exportWorksheet, int counter)
        {
            exportWorksheet.Cells[counter, 1] = "All Code Review Response Results";
            counter++;

            exportWorksheet.Cells[counter, 1] = "Id";
            exportWorksheet.Cells[counter, 2] = "Closed By";
            exportWorksheet.Cells[counter, 3] = "Closed Date";
            exportWorksheet.Cells[counter, 4] = "Created By";
            exportWorksheet.Cells[counter, 5] = "Created Date";
            exportWorksheet.Cells[counter, 6] = "Changed By";
            exportWorksheet.Cells[counter, 7] = "Changed Date";
            exportWorksheet.Cells[counter, 8] = "Comments";
            exportWorksheet.Cells[counter, 9] = "Duration";
            exportWorksheet.Cells[counter, 10] = "ChangeSet Count";
            exportWorksheet.Cells[counter, 11] = "ChangeSet Ids";
            exportWorksheet.Cells[counter, 12] = "Title";
            exportWorksheet.Cells[counter, 13] = "Description";
            exportWorksheet.Cells[counter, 14] = "Reason";
            exportWorksheet.Cells[counter, 15] = "RejectCount";
            exportWorksheet.Cells[counter, 16] = "RejectedByQA";
            exportWorksheet.Cells[counter, 17] = "RelatedLinkCount";
            exportWorksheet.Cells[counter, 18] = "State";
            exportWorksheet.Cells[counter, 19] = "Area Path";
            exportWorksheet.Cells[counter, 20] = "Revision";

            return counter;
        }
    }
}