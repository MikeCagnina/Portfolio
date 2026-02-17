using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSData.Entities
{
    public class ExportWorkItem
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string? AreaPath { get; set; }
        public int AttachedFileCount { get; set; }
        public DateTime AuthorizedDate { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int ChangesetCount { get; set; }
        public int CodeReviewCount { get; set; }
        public string? CommitterDisplayName { get; set; }
        public string? OwnerDisplayName { get; set; }
        public string? PolicyOverrideComment { get; set; }
        public List<int>? ChangeSetIds { get; set; }
        public List<int>? CodeReviewIds { get; set; }
        public string? ClosedBy { get; set; }
        public DateTime ClosedDate { get; set; }
        public string? ClosedStatus { get; set; }
        public string? ClosingComment { get; set; }
        public int CodeReviewRequestCount { get; set; }
        public List<string>? CodeReviewRequestIds { get; set; }
        public int CodeReviewDeclinedByRequestorCount { get; set; }
        public int CodeReviewDeclinedByReviewerCount { get; set; }
        public int CodeReviewLooksGoodCount { get; set; }
        public int CodeReviewWithCommentsCount { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Description { get; set; }
        public TimeSpan DurationTimeSpan { get; set; }
        public int ExternalLinkCount { get; set; }
        public DateTime FinishDate { get; set; }
        public string? History { get; set; }
        public int HyperLinkCount { get; set; }
        public bool IsCodeReviewRequest { get; set; }
        public bool IsCodeReviewResponse { get; set; }
        public bool IsNew { get; set; }
        public bool IsOpen { get; set; }
        public bool IsPartialOpen { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsReadOnlyOpen { get; set; }
        public bool IsWorkItem { get; set; }
        public string? IterationPath { get; set; }
        public LinkCollection? Links { get; set; }
        public string? NodeName { get; set; }
        public Project? Project { get; set; }
        public string? Reason { get; set; }
        public int RejectCount { get; set; }
        public string? RejectedByQA { get; set; }
        public int RelatedLinkCount { get; set; }
        public string? ResolvedBy { get; set; }
        public DateTime ResolvedDate { get; set; }
        public int Rev { get; set; }
        public DateTime RevisedDate { get; set; }
        public int Revision { get; set; }
        public string? ReviewedBy { get; set; }
        public DateTime ReviewedDate { get; set; }
        public DateTime StartDate { get; set; }
        public string? State { get; set; }
        public string? Tags { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public Uri? Uri { get; set; }
        public WorkItemLinkCollection? WorkItemLinkHistory { get; set; }
        public WorkItemLinkCollection? WorkItemLinks { get; set; }
    }
}