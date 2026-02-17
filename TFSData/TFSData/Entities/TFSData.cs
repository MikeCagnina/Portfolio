using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSData.Entities
{
    public class TFSData
    {
        public string? FI { get; set; }
        public int Year { get; set; }

        public int CodeReviewCount {  get; set; }

        public List<WorkItem>? CodeReviews { get; set; }

        public int ChangeSetCount { get; set; }

        public List<Changeset>? ChangeSets { get; set; }


    }
}
