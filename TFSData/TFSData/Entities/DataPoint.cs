using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSData.Entities
{
    public class DataPoint
    {
        public int ID { get; set; }

        public string? Description { get; set; }

        public int OrphanCount { get; set; }

        public int TaskCount { get; set; }

        public string? OrphanRatio { get; set; }
    }
}
