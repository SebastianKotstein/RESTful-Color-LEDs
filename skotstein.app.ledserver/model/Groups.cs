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
    /// This class represents a list resource containing a set of <see cref="Group"/>s.
    /// A JSON representation is returned by calling <see cref="GroupController.GetGroups(SKotstein.Net.Http.Context.HttpContext)"/>.
    /// </summary>
    [DisplayName("Groups")]
    [Description("Represents a collection of groups")]
    public class Groups
    {
        public const string SCHEMA_NAME = "groups";

        private IList<Group> _groups = new List<Group>();

        /// <summary>
        /// Gets or sets the list of <see cref="Group"/>s
        /// </summary>
        [DisplayName("Groups")]
        [Description("Represents a collection of groups")]
        [JsonProperty("groups")]
        public IList<Group> GroupList
        {
            get
            {
                return _groups;
            }

            set
            {
                _groups = value;
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
                    return new Hyperlink(ApiBase.API_V1 + "/groups");
                }
                else
                {
                    return null;
                }
                
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for creating a new group resource.
        /// </summary>
        [DisplayName("Create Controller")]
        [Description("Hyperlink for creating a new group")]
        [JsonProperty("create_new_group", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink CreateGroup
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/groups") { Action = "POST" };
                }
                else
                {
                    return null;
                }
                
            }
        }
    }
}
