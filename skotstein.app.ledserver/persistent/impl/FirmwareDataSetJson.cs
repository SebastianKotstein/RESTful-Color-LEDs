using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// This implementation of <see cref="IFirmwareDataSet"/> allows to store the meta data of a firmware on the disk in the form of a JSON File.
    /// Use <see cref="JsonSerializer"/> to serialize or deserialize an instance of this class into or from JSON.
    /// </summary>
    public class FirmwareDataSetJson : IFirmwareDataSet
    {
        private string _id;
        private int _minorVersion;
        private int _majorVersion;
        private string _deviceName;
        private string _fileExtension;

        [JsonProperty("id")]
        public string Id
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

        [JsonProperty("minorVersion")]
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

        [JsonProperty("majorVersion")]
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

        [JsonProperty("deviceName")]
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

        [JsonProperty("firmwareFileExtension")]
        public string FirmwareFileExtension
        {
            get
            {
                return _fileExtension;
            }

            set
            {
                _fileExtension = value;
            }
        }
    }
}
