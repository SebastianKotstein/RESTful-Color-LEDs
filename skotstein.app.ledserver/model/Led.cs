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
    /// This class represents a single LED resource.
    /// A JSON representation is returned by calling <see cref="LedController.GetLed(SKotstein.Net.Http.Context.HttpContext, string)"/>.
    /// </summary>
    [DisplayName("LED")]
    [Description("Represents a LED which is controlled by a controller")]
    public class Led
    {
        public const string SCHEMA_NAME = "led";

        private int _controllerId;
        private int _ledId;
        private string _rgb;

        /// <summary>
        /// Gets the ID of the <see cref="Led"/>.
        /// </summary>
        [DisplayName("LED ID")]
        [Description("The ID of the LED")]
        [JsonProperty("id")]
        public string LedId
        {
            get
            {
                return ControllerId + ":" + LedNumber;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the <see cref="Controller"/> which controls this LED.
        /// </summary>
        [DisplayName("Controller ID")]
        [Description("The ID of the controller which controls this LED")]
        [JsonProperty("controller_id")]
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

        /// <summary>
        /// Gets or sets the LED number of this LED.
        /// </summary>
        [DisplayName("LED Number")]
        [Description("LED number of this LED")]
        [JsonProperty("led_number")]
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

        /// <summary>
        /// Gets or sets the RGB color value of this LED.
        /// </summary>
        [DisplayName("RGB Color Value")]
        [Description("Current RGB color value of this LED")]
        [JsonProperty("rgb")]
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

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the controller which controls this LED.
        /// </summary>
        [DisplayName("Controller")]
        [Description("Hyperlink to the controller which controls this LED")]
        [JsonProperty("controller", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink ToController
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/controllers/" + _controllerId);
                }
                else
                {
                    return null;
                }
                
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
                    return new Hyperlink(ApiBase.API_V1 + "/leds/" + _controllerId + ":" + _ledId);
                }
                else
                {
                    return null;
                }
                
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for setting the color this LED.
        /// </summary>
        [DisplayName("Set LED Color")]
        [Description("Hyperlink for setting the color of this LED")]
        [JsonProperty("set_color", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink SetLed
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/leds/" + _controllerId + ":" + _ledId) { Action = "PUT" };
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
