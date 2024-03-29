﻿// MIT License
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
    /// This class represents a sub resource of <see cref="Controller"/> containing a list of all <see cref="Led"/> resource which are controlled by the controller.
    /// A JSON representation is returned by calling <see cref="ControllerController.GetLeds(SKotstein.Net.Http.Context.HttpContext, string)"/>.
    /// </summary>
    [DisplayName("Controller LEDs")]
    [Description("Represents a collection of LEDs being controlled by the underlying controller")]
    public class ControllerLeds
    {
        public const string SCHEMA_NAME = "controllerleds";

        IList<Led> _leds = new List<Led>();

        private int _controllerId;

        /// <summary>
        /// Gets or sets the <see cref="IList{T}"/> containing the <see cref="Led"/>s being controlled by the underlying controller
        /// </summary>
        [DisplayName("Leds")]
        [Description("List containing all LEDs being controlled by the underlying controller")]
        [JsonProperty("leds")]
        public IList<Led> Leds
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

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the underlying <see cref="Controller"/> resource
        /// </summary>
        [DisplayName("Controller")]
        [Description("Hyperlink to the underlying controller")]
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
                    return new Hyperlink(ApiBase.API_V1 + "/controllers/" + _controllerId + "/leds");
                }
                else
                {
                    return null;
                } 
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to action for setting the color of all LEDs being controlled by this controller
        /// </summary>
        [DisplayName("Set Color of LEDs")]
        [Description("Hyperlink for changing the color of all LEDs being controlled by the underlying controller")]
        [JsonProperty("set_color_of_leds", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink SetLeds
        {
            get
            {

                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/controllers/" + _controllerId + "/leds") { Action = "POST" };
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ID of the underlying <see cref="Controller"/>
        /// </summary>
        [JsonIgnore]
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


    }
}
