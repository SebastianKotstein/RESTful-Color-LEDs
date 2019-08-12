using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// This implementation of <see cref="ILedDataSet"/> allows to store the data of a single LED into memory (i.e. not persistently).
    /// </summary>
    public class LedDataSetNonPersistent : ILedDataSet
    {
        private int _ledId;
        private int _controllerId;
        private string _rgb;

        public string Id
        {
            get
            {
                return ControllerId + ":" + LedNumber;
            }
        }

        public int LedNumber
        {
            get
            {
                return _ledId;
            }

            set
            {
                _ledId = value;
            }
        }

        public int ControllerId
        {
            get
            {
                return _controllerId;
            }

            set
            {
                _controllerId = value;
            }
        }

        public string RgbValue
        {
            get
            {
                return _rgb;
            }

            set
            {
                _rgb = value;
            }
        }

        public bool IsLedOfController(int controllerId)
        {
            return controllerId == _controllerId;
        }

        public bool HasId(string id)
        {
            return id.CompareTo(Id) == 0;
        }

        public bool HasId(int controllerId, int ledId)
        {
            return HasId(controllerId + ":" + ledId);
        }

     
    }
}
