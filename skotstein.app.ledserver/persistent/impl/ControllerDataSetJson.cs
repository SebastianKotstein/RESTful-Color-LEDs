// MIT License
//
// Copyright (c) 2019 Sebastian Kotstein
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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
