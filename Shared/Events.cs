using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Event1:Event
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Id, Name);
        }
    }

    public class Event2:Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Id, Name, CreationDate);
        }
    }

    public abstract class Event { }
}
