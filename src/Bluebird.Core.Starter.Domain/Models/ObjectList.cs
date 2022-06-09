using System;
using System.Collections.Generic;
using System.Text;

namespace Bluebird.Core.Starter.Domain.Models
{
    public class ObjectList<T>
    {
        public long TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
        public string RawQuery { get; set; }
    }
}
