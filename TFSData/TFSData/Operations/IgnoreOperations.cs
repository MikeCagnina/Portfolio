using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace TFSData.Operations
{
    public static class IgnoreOperations
    {
        public static List<int> IgnoreChangeSetIds(string FI, string year)
        {
            //string ignoreChangeSetIds = ConfigurationManager.AppSettings["IgnoreChangeSetIds"];
            //"NAFC~1,2,3,4,5.2018;NAFC~6,7,8,9.2019;NAFC~6,7,8,9.2021|FNBC~6,7,8.2019"
            //List<string> ignoreFIs = (from s in ignoreChangeSetIds.Split('|') where s.Contains(FI + ",") select s).ToList();
            //NAFC,1,2,3,4,5.2018;NAFC,6,7,8,9.2019
            //var query = (from yearSplit in (from FISplit in ignoreChangeSetIds.Split('|') where FISplit.Contains(FI + ",") && FISplit.Contains("." + year) select FISplit.Split(';')) select yearSplit).ToList();
            //var query = (from FISplit in ignoreChangeSetIds.Split('|') where FISplit.Contains(FI + ",") && FISplit.Contains("." + year)
            //          from yearSplit in FISplit.Split(';') where yearSplit.Contains("." + year)
            //          from IDSplit in yearSplit.Split('.')
            //                //NAFC,6,7,8,9.2021
            //                 select yearSplit).ToList();


            //foreach (string yearSplit in
            //from FISplit in ignoreChangeSetIds.Split('|') where FISplit.Contains(FI + ",") && FISplit.Contains("." + year)
            //from yearSplit in FISplit.Split(';')
            //from IDSplit in yearSplit.Split('.') select yearSplit) { }

            //var query = (from ignoreIdSplit in
            //          from FISplit in ignoreChangeSetIds.Split('|') where FISplit.Contains(FI + "~") && FISplit.Contains("." + year)
            //          from yearSplit in FISplit.Split(';') where yearSplit.Contains("." + year)
            //          from IDSplit in yearSplit.Split('.')
            //          from ignoreIdSplit in IDSplit.Replace(FI + "~", string.Empty).Split(',')
            //          select ignoreIdSplit);


            return (from FISplit in ConfigurationManager.AppSettings["IgnoreChangeSetIds"].Split('|')
                    where FISplit.Contains(FI + "~") && FISplit.Contains("." + year)
                    from yearSplit in FISplit.Split(';')
                    where yearSplit.Contains("." + year)
                    from IDSplit in yearSplit.Replace("." + year, string.Empty).Replace(FI + "~", string.Empty).Split('.')
                    from ignoreIdSplit in IDSplit.Split(',')
                    select int.Parse(ignoreIdSplit)).ToList();

            //List < int > returnValue = new List<int>();


            //foreach (string IDSplit in
            //    from FISplit in ignoreChangeSetIds.Split('|') where FISplit.Contains(FI + "~") && FISplit.Contains("." + year)
            //    from yearSplit in FISplit.Split(';') where yearSplit.Contains("." + year)
            //    from IDSplit in yearSplit.Split('.')
            //    from ignoreIdSplit in IDSplit.Replace(FI + "~", string.Empty).Split(',') select IDSplit) {
            //    //returnValue.Add(int.Parse(ignoreIdSplit));
            //}

            //foreach (var FISplit in ignoreChangeSetIds.Split('|'))
            //{
            //    if (FISplit.Contains(FI + "~") && FISplit.Contains("." + year))
            //    {
            //          foreach (var yearSplit in FISplit.Split(';'))
            //          {
            //                if (yearSplit.Contains("." + year))
            //                {
            //                      foreach (var IDSplit in yearSplit.Split('.'))
            //                      {
            //                            foreach (var ignoreIdSplit in IDSplit.Replace(FI + "~", string.Empty).Split(','))
            //                            {
            //                                  returnValue.Add( int.Parse(ignoreIdSplit));
            //                            }
            //                      }
            //                }
            //          }
            //    }
            //}

            //List<int> ignoreIds = new List<string>(ConfigurationManager.AppSettings["IgnoreChangeSetIds"].Split(';')).Select(int.Parse).ToList();

            //return null;
        }

        public static List<string> IgnoreCodeReviewIds(string FI, string year)
        {
            return (from FISplit in ConfigurationManager.AppSettings["IgnoreCodeReviewIds"].Split('|')
                    where FISplit.Contains(FI + "~") && FISplit.Contains("." + year)
                    from yearSplit in FISplit.Split(';')
                    where yearSplit.Contains("." + year)
                    from IDSplit in yearSplit.Replace("." + year, string.Empty).Replace(FI + "~", string.Empty).Split('.')
                    from ignoreIdSplit in IDSplit.Split(',')
                    select ignoreIdSplit).ToList();
        }

        public static List<string> IgnoreCodeReviewResponseIds(string FI, string year)
        {
            return (from FISplit in ConfigurationManager.AppSettings["IgnoreCodeReviewResponseIds"].Split('|')
                    where FISplit.Contains(FI + "~") && FISplit.Contains("." + year)
                    from yearSplit in FISplit.Split(';')
                    where yearSplit.Contains("." + year)
                    from IDSplit in yearSplit.Replace("." + year, string.Empty).Replace(FI + "~", string.Empty).Split('.')
                    from ignoreIdSplit in IDSplit.Split(',')
                    select ignoreIdSplit).ToList();
        }
    }
}