using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// This implementation of <see cref="IControllerDataSet"/> allows to store a controller data set on the disk in the form of a JSON File. Note that only the properties
    /// <see cref="Id"/>, <see cref="UuId"/>, <see cref="FriendlyName"/> and <see cref="LedCount"/> are stored persistently. The firmware parameters such as <see cref="FirmwareId"/>, <see cref="MinorVersion"/>, <see cref="MajorVersion"/>, <see cref="DeviceName"/> and <see cref="Timestamp"/> are volatile (and are not part of the stored JSON File).
    /// Use <see cref="JsonSerializer"/> to serialize or deserialize an instance of this class into or from JSON.
    /// </summary>
    public class ControllerDataSetJson : IControllerDataSet
    {
        private int _id;
        private string _uuId;
        private string _friendlyName;
        private int _ledCount;
        private int _minorVersion;
        private int _majorVersion;
        private string _firmwareId;
        private string _deviceName;
        private DateTime _timestamp;

        [JsonProperty("id")]
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

        [JsonProperty("uuId")]
        public string UuId
        {
            get
            {
                return _uuId;
            }

            set
            {
                _uuId = value;
            }
        }

        [JsonProperty("friendlyName")]
        public string FriendlyName
        {
            get
            {
                return _friendlyName;
            }

            set
            {
                _friendlyName = value;
            }
        }

        [JsonProperty("ledCount")]
        public int LedCount
        {
            get
            {
                return _ledCount;
            }

            set
            {
                _ledCount = value;
            }
        }

        [JsonIgnore]
        public int MinorVersion
        {
            get
            {
                return _minorVersion;
            }

            set
            {
                _minorVersion = value;
            }
        }

        [JsonIgnore]
        public int MajorVersion
        {
            get
            {
                return _majorVersion;
            }

            set
            {
                _majorVersion = value;
            }
        }

        [JsonIgnore]
        public string FirmwareId
        {
            get
            {
                return _firmwareId;
            }

            set
            {
                _firmwareId = value;
            }
        }

        [JsonIgnore]
        public string DeviceName
        {
            get
            {
                return _deviceName;
            }

            set
            {
                _deviceName = value;
            }
        }

        [JsonIgnore]
        public DateTime Timestamp
        {
            get
            {
                return _timestamp;
            }

            set
            {
                _timestamp = value;
            }
        }
    }
}
