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
    /// This class represents a single group resource.
    /// A JSON representation is returned by calling <see cref="GroupController.GetGroup(SKotstein.Net.Http.Context.HttpContext, string)"/>.
    /// </summary>
    [DisplayName("Group")]
    [Description("Represents a group of LEDs")]
    public class Group
    {
        public const string SCHEMA_NAME = "group";

        private int _id;
        private string _name;

        /// <summary>
        /// Gets or sets the ID of the group
        /// </summary>
        [DisplayName("Group ID")]
        [Description("The ID of the group")]
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

        /// <summary>
        /// Gets or sets the name of the group
        /// </summary>
        [DisplayName("Group name")]
        [Description("The name of the group")]
        [JsonProperty("name")]
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

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to a sub resource containing all LEDs being member of this group
        /// </summary>
        [DisplayName("Group LEDs")]
        [Description("Hyperlink to the LEDs being member of this group")]
        [JsonProperty("leds", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink ToLeds
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/groups/" + _id + "/leds");
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to this group resource itself
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
                    return new Hyperlink(ApiBase.API_V1 + "/groups/" + _id);
                }
                else
                {
                    return null;
                }
            }
        }

    
        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for changing the name of this group resource
        /// </summary>
        [DisplayName("Change Name")]
        [Description("Hyperlink to the LEDs being member of this group")]
        [JsonProperty("change_name", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink ChangeName
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/groups/" + _id){Action="PUT"};
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for deleting this group resource
        /// </summary>
        [DisplayName("Remove Group")]
        [Description("Hyperlink for deleting this group")]
        [JsonProperty("remove_group", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink DeleteGroup
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/groups/" + _id) { Action = "DELETE" };
                }
                else
                {
                    return null;
                }
                
            }
        }

    }
}
