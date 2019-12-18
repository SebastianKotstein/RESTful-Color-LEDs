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
    /// This class represents a sub resource of <see cref="Group"/> containing a list of all <see cref="Led"/>s being member of the group.
    /// A JSON representation is returned by calling <see cref="GroupController.SetLedsOfGroup(SKotstein.Net.Http.Context.HttpContext, string)"/>
    /// </summary>
    [DisplayName("Group LEDs")]
    [Description("Represents a collection of LEDs being member of a group")]
    public class GroupLeds
    {
        public const string SCHEMA_NAME = "groupleds";

        IList<Led> _leds = new List<Led>();
        private int _groupId;

        /// <summary>
        /// Gets or sets the list of <see cref="Led"/>s being member of the group 
        /// </summary>
        [DisplayName("Group LEDs")]
        [Description("Represents a collection of LEDs being member of a group")]
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
        /// Gets a <see cref="Hyperlink"/> object pointing to the underlying <see cref="Group"/> resource
        /// </summary>
        [DisplayName("Group")]
        [Description("Hyperlink to the underlying group")]
        [JsonProperty("group", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink ToGroup
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/groups/" + _groupId);
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
                    return new Hyperlink(ApiBase.API_V1 + "/groups/" + _groupId + "/leds");
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for changing the membership of the group.
        /// </summary>
        [DisplayName("Create Controller")]
        [Description("Hyperlink for changing the membership of the group")]
        [JsonProperty("set_leds_of_group", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink SetLeds
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/groups/" + _groupId + "/leds") { Action = "PUT" };
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for setting the color of all <see cref="Leds"/> being member of the group.
        /// </summary>
        [DisplayName("Set LED Color")]
        [Description("Hyperlink for setting the color of all LEDs being member of the group")]
        [JsonProperty("set_color_of_group_leds", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink SetLedColor
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/groups/" + _groupId + "/leds") { Action = "POST" };
                }
                else
                {
                    return null;
                }
             }
        }


        /// <summary>
        /// Gets or sets the ID of the underlying <see cref="Group"/>
        /// </summary>
        [JsonIgnore]
        public int GroupId
        {
            get
            {
                return _groupId;
            }

            set
            {
                _groupId = value;
            }
        }
    }
}
