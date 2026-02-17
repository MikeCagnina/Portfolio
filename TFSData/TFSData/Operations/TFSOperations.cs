using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFSData.Entities;
using TFSData.Extensions;

namespace TFSData.Operations
{
    public static class TFSOperations
    {
        /// <summary>
                    /// Uses the provided UrlPath to connect to the TFS instance.
                    /// This connection automatically checks Active Directory Permissions
                    /// No additional permissions are needed for querying
                    /// </summary>
        public static WorkItemStore ConnectToTfs(string urlPath)
        {
            Uri tfsUri = new Uri(urlPath);
            TfsTeamProjectCollection server = new TfsTeamProjectCollection(tfsUri, CredentialCache.DefaultNetworkCredentials);
            return server.GetService<WorkItemStore>();
        }

        public static VersionControlServer ConnectToVcs(string urlPath)
        {
            Uri tfsUri = new Uri(urlPath);
            TfsTeamProjectCollection server = new TfsTeamProjectCollection(tfsUri, CredentialCache.DefaultNetworkCredentials);
            return (VersionControlServer)server.GetService(typeof(VersionControlServer));
        }

        public static List<ExportWorkItem> ChangeSetDataByYear(string urlPath, string FI, int year)
        {
            VersionControlServer vcs = ConnectToVcs(urlPath);
            List<Changeset> changeSets = QueryChangeSets(vcs, FI, year);

            List<int> ignoreChangeSetIds = IgnoreOperations.IgnoreChangeSetIds(FI, year.ToString());
            List<string> ignoreDescriptions = new List<string>(ConfigurationManager.AppSettings["IgnoreDescriptions"].Split(';'));
            List<string> ignorePeople = new List<string>(ConfigurationManager.AppSettings["IgnorePeople"].Split(';'));

            return (from changeSet in changeSets
                    let filterPerson = ignorePeople.Contains(changeSet.CommitterDisplayName)
                    let filterDescription = (from description in ignoreDescriptions where changeSet.Comment.Contains(description, StringComparison.OrdinalIgnoreCase) select description).Any()
                    let filterId = ignoreChangeSetIds.Contains(changeSet.ChangesetId)
                    where !filterId && !filterPerson && !filterDescription
                    orderby changeSet.OwnerDisplayName
                    select new ExportWorkItem
                    {
                        Id = changeSet.ChangesetId,
                        CreationDate = changeSet.CreationDate,
                        Description = changeSet.Comment,
                        CommitterDisplayName = changeSet.CommitterDisplayName,
                        OwnerDisplayName = changeSet.OwnerDisplayName,
                        PolicyOverrideComment = changeSet.PolicyOverride != null ? changeSet.PolicyOverride.Comment : "No Comment",
                        ChangeSetIds = new List<int>(),
                        CodeReviewIds = new List<int>()
                    }).ToList();
        }

        public static List<int> ChangeSetIds(WorkItem workItem)
        {
            return (from link in workItem.Links.OfType<ExternalLink>()
            .Where(l => l.LinkedArtifactUri.ToString().Contains("Changeset"))
                    select Convert.ToInt32(link.LinkedArtifactUri.ToString().Substring(link.LinkedArtifactUri.ToString().LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1))
            ).ToList();
        }

        public static List<ExportWorkItem> WorkItemData(List<WorkItem> WorkItems)
        {
            return (from workItem in WorkItems
                    let changesetIds = ChangeSetIds(workItem)
                    let changedBy = QueryWorkItemFieldValue(workItem, "Changed By")
                    let createddBy = QueryWorkItemFieldValue(workItem, "Created By")
                    let closedBy = QueryWorkItemFieldValue(workItem, "Closed By")
                    let closedDate = QueryWorkItemFieldValue(workItem, "Closed Date")
                    let closedStatus = QueryWorkItemFieldValue(workItem, "Closed Status")
                    let closingComments = QueryWorkItemFieldValue(workItem, "Closing Comment")
                    let durationTimeSpan = closedDate.IsNullOrEmpty() ? new TimeSpan() : (DateTime.Parse(closedDate) - workItem.CreatedDate)
                    let finishDate = QueryWorkItemFieldValue(workItem, "Finish Date")
                    let rejectCount = QueryWorkItemFieldValue(workItem, "Reject Count")
                    let resolvedBy = QueryWorkItemFieldValue(workItem, "Resolved By")
                    let resolvedDate = QueryWorkItemFieldValue(workItem, "Resolved Date")
                    let reviewedBy = QueryWorkItemFieldValue(workItem, "Reviewed By")
                    let reviewedDate = QueryWorkItemFieldValue(workItem, "Reviewed Date")
                    let startDate = QueryWorkItemFieldValue(workItem, "Start Date")
                    let rejectedByQA = QueryWorkItemFieldValue(workItem, "Rejected By QA")
                    orderby createddBy, workItem.Id
                    select new ExportWorkItem
                    {
                        Id = workItem.Id,
                        AreaId = workItem.AreaId,
                        AreaPath = workItem.AreaPath,
                        AttachedFileCount = workItem.AttachedFileCount,
                        AuthorizedDate = workItem.AuthorizedDate,
                        ChangedBy = changedBy,
                        ChangedDate = workItem.ChangedDate,
                        ChangeSetIds = changesetIds.ToList(),
                        ChangesetCount = changesetIds.Count,
                        ClosedBy = closedBy,
                        ClosedDate = closedDate != string.Empty ? Convert.ToDateTime(closedDate) : (resolvedDate != string.Empty ? Convert.ToDateTime(resolvedDate) : DateTime.MinValue),
                        ClosedStatus = closedStatus,
                        ClosingComment = closingComments,
                        CreatedBy = createddBy,
                        CreatedDate = workItem.CreatedDate,
                        Description = workItem.Description == null ? string.Empty : workItem.Description.Replace(Environment.NewLine, ""),
                        DurationTimeSpan = durationTimeSpan,
                        ExternalLinkCount = workItem.ExternalLinkCount,
                        FinishDate = finishDate != string.Empty ? Convert.ToDateTime(finishDate) : DateTime.MinValue,
                        History = workItem.History,
                        HyperLinkCount = workItem.HyperLinkCount,
                        IsCodeReviewRequest = workItem.Type.Name.Equals("Code Review Request", StringComparison.OrdinalIgnoreCase),
                        IsCodeReviewResponse = workItem.Type.Name.Equals("Code Review Response", StringComparison.OrdinalIgnoreCase),
                        IsNew = workItem.IsNew,
                        IsOpen = workItem.IsOpen,
                        IsPartialOpen = workItem.IsPartialOpen,
                        IsReadOnly = workItem.IsReadOnly,
                        IsReadOnlyOpen = workItem.IsReadOnlyOpen,
                        IsWorkItem = !workItem.Type.Name.Equals("Code Review Response", StringComparison.OrdinalIgnoreCase) && !workItem.Type.Name.Equals("Code Review Request", StringComparison.OrdinalIgnoreCase),
                        IterationPath = workItem.IterationPath,
                        Links = workItem.Links,
                        NodeName = workItem.NodeName,
                        Project = workItem.Project,
                        Reason = workItem.Reason,
                        RejectCount = rejectCount != string.Empty ? Convert.ToInt32(rejectCount) : 0,
                        RejectedByQA = rejectedByQA,
                        RelatedLinkCount = workItem.RelatedLinkCount,
                        ResolvedBy = string.Empty,
                        ResolvedDate = resolvedDate != string.Empty ? Convert.ToDateTime(resolvedDate) : DateTime.MinValue,
                        ReviewedBy = string.Empty,
                        ReviewedDate = reviewedDate != string.Empty ? Convert.ToDateTime(reviewedDate) : DateTime.MinValue,
                        Rev = workItem.Rev,
                        RevisedDate = workItem.RevisedDate,
                        //Updated each time Task is updated for any reason
                        Revision = workItem.Revision,
                        StartDate = startDate != string.Empty ? Convert.ToDateTime(startDate) : DateTime.MinValue,
                        State = workItem.State,
                        Tags = workItem.Tags,
                        Title = workItem.Title,
                        Type = workItem.Type.Name,
                        Uri = workItem.Uri,
                        WorkItemLinkHistory = workItem.WorkItemLinkHistory,
                        WorkItemLinks = workItem.WorkItemLinks
                    }).ToList();
        }

        public static List<int> CodeReviewIds(WorkItem workItem, List<ExportWorkItem> codeReviewData)
        {
            return (from link in workItem.Links.OfType<RelatedLink>()
                    from codeReviewId in codeReviewData.Select(c => c.Id)
                    where link.RelatedWorkItemId.Equals(codeReviewId)
                    select link.RelatedWorkItemId).ToList();
        }

        public static List<ExportWorkItem> CodeReviewRequestsByYear(string tfsUrl, string FI, int year, string tfsQuery)
        {
            WorkItemStore workItemStore = ConnectToTfs(tfsUrl);

            List<string> ignoreIds = IgnoreOperations.IgnoreCodeReviewIds(FI, year.ToString());

            if(ignoreIds.Count == 0)
            {
                ignoreIds.Add("0");
            }

            if(tfsQuery.IsNullOrEmpty())
            {
                tfsQuery = string.Format(ConfigurationManager.AppSettings["CodeReviewQuery"], string.Join(",", ignoreIds), FI, year);
            }
            else
            {
                tfsQuery = string.Format(tfsQuery, string.Join(",", ignoreIds), FI, year);
            }

            List<WorkItem> workItems = QueryWorkItems(workItemStore, tfsQuery);
            List<ExportWorkItem> codeReviewData = WorkItemData(workItems);

            return codeReviewData;
        }

        public static List<ExportWorkItem> CodeReviewResponsesByYear(string tfsUrl, string FI, int year, string tfsQuery)
        {
            WorkItemStore workItemStore = ConnectToTfs(tfsUrl);

            List<string> ignoreIds = IgnoreOperations.IgnoreCodeReviewResponseIds(FI, year.ToString());

            if(ignoreIds.Count == 0)
            {
                ignoreIds.Add("0");
            }

            if(tfsQuery.IsNullOrEmpty())
            {
                tfsQuery = string.Format(ConfigurationManager.AppSettings["CodeReviewResponseQuery"], string.Join(",", ignoreIds), FI, year);
            }
            else
            {
                tfsQuery = string.Format(tfsQuery, string.Join(",", ignoreIds), FI, year);
            }

            List<WorkItem> workItems = QueryWorkItems(workItemStore, tfsQuery);
            List<ExportWorkItem> codeReviewResponseData = WorkItemData(workItems);

            return codeReviewResponseData;
        }

        public static List<ExportWorkItem> FilteredChangeSetData(List<ExportWorkItem> changeSetData, List<ExportWorkItem> codeReviewData)
        {
            return (from changeSet in changeSetData
                    let codeReviewCount = (from codeReview in codeReviewData where codeReview.ChangeSetIds.Contains(changeSet.Id) select codeReview).Count()
                    where codeReviewCount == 0
                    orderby changeSet.OwnerDisplayName, changeSet.Id
                    select changeSet).ToList();
        }

        private static VersionSpec GetDateVSpec(DateTime date)
        {
            string dateSpec = string.Format("D{0:yyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}", date);
            return VersionSpec.ParseSingleSpec(dateSpec, "");
        }

        public static List<WorkItem> QueryWorkItems(WorkItemStore workItemStore, string query)
        {
            return workItemStore.Query(query).OfType<WorkItem>().ToList();
        }

        public static List<Changeset> QueryChangeSets(VersionControlServer vcs, string FI, int year)
        {
            return (from repository in new List<string>(ConfigurationManager.AppSettings[FI + "Repositories"].Split(';')).ToList()
                    let versionFrom = GetDateVSpec(new DateTime(year, 1, 1))
                    let versionTo = GetDateVSpec(new DateTime(year, 12, 31))
                    let changeSets = vcs.QueryHistory(repository, VersionSpec.Latest, 0, RecursionType.Full, "", versionFrom, versionTo, int.MaxValue, false, false)
                    from changeSet in changeSets.Cast<Changeset>()
                    select changeSet).ToList();
        }

        /// <summary>
                    /// The field level variables are not returned by the <see cref="WorkItem"/> query
                    /// they must be requested as needed
                    /// if the user is filtering at the field level the field value must be subsequently obtained with a new call to TFS
                    /// </summary>
                    /// <param name="workItem"></param>
                    /// <param name="fieldName"></param>
                    /// <returns></returns>
        public static string QueryWorkItemFieldValue(WorkItem workItem, string fieldName)
        {
            try
            {
                var field = workItem.Fields.Cast<Field>().FirstOrDefault(item => item.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
                if(field != null && field.Value != null)
                {
                    return field.Value.ToString();
                }
            }
            catch(FieldDefinitionNotExistException)
            {
                //Invalid field names are ignored
            }

            return string.Empty;
        }
    }
}