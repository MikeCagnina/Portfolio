using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using TFSData.Entities;
using TFSData.Extensions;
using TFSData.Operations;
using Excel = Microsoft.Office.Interop.Excel.Application;

namespace TFSData
{
    public partial class CaiaphasForm : Form
    {
        public CaiaphasForm()
        {
            InitializeComponent();
            txtUrl.Text = ConfigurationManager.AppSettings["TFSUrl"];
            txtCodeReviewQuery.Text = ConfigurationManager.AppSettings["CodeReviewQuery"];
            txtCodeReviewResponseQuery.Text = ConfigurationManager.AppSettings["CodeReviewResponseQuery"];
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(@"Close Caiaphas?", @"Caiaphas", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void btnChangeSet_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(@"Get ChangeSet Data?", @"Caiaphas", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string tfsUrl = txtUrl.Text;

                //TODO
                //Looking in the TFS data to find out who is or is not doing code review
                //Change sets for this year but no code reviews

                //I have 10 changesets of the ten 9 have at least one code review request with a status of completed or checked in
                //The overall percentage for closed completed code review to code commits must be > 85%

                List<int> Years = new List<string>(ConfigurationManager.AppSettings["Years"].Split(';')).Select(int.Parse).ToList();
                List<string> FIs = new List<string>(ConfigurationManager.AppSettings["FIs"].Split(';'));
                string dataOutputPath = ConfigurationManager.AppSettings["DataOutputPath"];
                string codeReviewDataFileName = ConfigurationManager.AppSettings["DataOutputFileName"];

                //Get All the Code Reviews for the FI
                //Get Code Review Requests from Q4 of previous year
                //month prior
                //separate into quarters
                //Violation of policy doesn't occur until the change set is created
                //State Change Date

                Excel exportExcel = new Excel();
                foreach(int year in Years)
                {
                    foreach(string FI in FIs)
                    {
                        //Get code review request for the year....when calculating the percentage we cant just arbitrarily grab X amount of Code Reviews
                        //Code Reviews have 2 States - Closed Requested
                        //Code Reviews have 4 Closed States - Null Completed Checked In Abandoned
                        //Changed By dcbuilder

                        //Get the code review responses for the year
                        List<ExportWorkItem> codeReviewResponseData = TFSOperations.CodeReviewResponsesByYear(tfsUrl, FI, year, txtCodeReviewResponseQuery.Text);

                        //Get the code review requests for the year
                        List<ExportWorkItem> codeReviewData = TFSOperations.CodeReviewRequestsByYear(tfsUrl, FI, year, txtCodeReviewQuery.Text);

                        //Get change sets for the year
                        List<ExportWorkItem> changeSetData = TFSOperations.ChangeSetDataByYear(tfsUrl, FI, year);

                        //Loop through the change sets and see if there is a corresponding code review
                        List<ExportWorkItem> filteredChangeSets = TFSOperations.FilteredChangeSetData(changeSetData, codeReviewData);

                        Workbook exportWorkbook = exportExcel.Workbooks.Add();
                        exportWorkbook = exportWorkbook.WriteAdminData(codeReviewData, codeReviewResponseData, changeSetData);
                        exportWorkbook = exportWorkbook.WriteChangeSetData(changeSetData, filteredChangeSets);
                        exportWorkbook = exportWorkbook.WriteCodeReviewData(codeReviewData);
                        exportWorkbook = exportWorkbook.WriteCodeReviewResponseData(codeReviewResponseData);

                        exportExcel.DisplayAlerts = false;
                        if(!Directory.Exists(dataOutputPath + FI))
                        {
                            Directory.CreateDirectory(dataOutputPath + FI);
                        }

                        exportWorkbook.SaveAs(dataOutputPath + FI + "\\" + string.Format(codeReviewDataFileName, FI, year));

                        exportExcel.Workbooks.Close();
                    }
                }

                exportExcel.Quit();
                MessageBox.Show(@"ChangeSet Complete", @"Caiaphas");
            }
        }

        //Who is providing the most code review responses
        //How long does a code review stay open
        //How long does a response take

        //What FIs are and are not going code reviews
    }
}
