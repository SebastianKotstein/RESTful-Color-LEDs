using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    public interface IGroupDataSet
    {
        int Id { get; set; }
        string Name { get; set; }
        IList<string> Leds { get; set; }

    }
}
