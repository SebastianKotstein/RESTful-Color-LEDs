using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    public class GroupDataSetJson : IGroupDataSet
    {
        private int _id;
        private string _name;
        private IList<string> _leds = new List<string>();

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public IList<string> Leds
        {
            get
            {
                return _leds;
            }

            set
            {
                _leds = value;
            }
        }
    }
}
