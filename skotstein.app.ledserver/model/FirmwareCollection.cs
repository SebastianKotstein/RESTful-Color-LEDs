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
using skotstein.app.ledserver.restlayer;
using skotstein.app.ledserver.tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.model
{
    /// <summary>
    /// This class represents a list resource containing a set of <see cref="Firmware"/>.
    /// A JSON representation is returnd by calling <see cref="FirmwareController.GetFirmwareCollection(SKotstein.Net.Http.Context.HttpContext)"/>.
    /// </summary>
    [DisplayName("Firmware Collection")]
    [Description("Represent a collection of firmware")]
    public class FirmwareCollection
    {
        public const string SCHEMA_NAME = "firmwares";

        private IList<Firmware> _firmwares = new List<Firmware>();

        /// <summary>
        /// Gets or sets the list of <see cref="Firmware"/>
        /// </summary>
        [DisplayName("Firmware Collection")]
        [Description("Represent a collection of firmware")]
        [JsonProperty("firmware")]
        public IList<Firmware> FirmwareList
        {
            get
            {
                return _firmwares;
            }
            set
            {
                _firmwares = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to this resource itself
        /// </summary>
        [DisplayName("Self")]
        [Description("Hyperlink to this resource")]
        [JsonProperty("self", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink ToSelf
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/firmware");
                }
                else
                {
                    return null;
                }
                
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for adding a new firmware
        /// </summary>
        [DisplayName("Add Firmware")]
        [Description("Represent a collection of firmware")]
        [JsonProperty("add_firmware", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink AddFirmware
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/firmware") { Action = "Hyperlink" };
                }
                else
                {
                    return null;
                }
               
            }
        }
    }
}
