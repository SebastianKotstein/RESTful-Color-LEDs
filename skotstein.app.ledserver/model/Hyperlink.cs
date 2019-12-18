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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.model
{
    /// <summary>
    /// This class represents a sub resource containing a hyperlink to another resource as well as further, but mostly optional, meta information.
    /// </summary>
    [DisplayName("Hyperlink")]
    [Description("A hyperlink pointing to another resource")]
    public class Hyperlink
    {
        public const string SCHEMA_NAME = "hyperlink";

        private string _rel;
        private string _description;
        private string _href;
        private string _action = null;
        private bool? _templated = null;

        public Hyperlink(string href)
        {
            _href = href;
        }

        /// <summary>
        /// Gets or sets the relation type (optional)
        /// </summary>
        [DisplayName("Relation type")]
        [Description("The relation type of the hyperlink")]
        [JsonProperty("rel", NullValueHandling = NullValueHandling.Ignore)]
        public string Relation
        {
            get
            {
                return _rel;
            }
            set
            {
                _rel = value;
            }
        }

        /// <summary>
        /// Gets or sets the hyperlink description (optional)
        /// </summary>
        [DisplayName("Hyperlink Description")]
        [Description("The description of the hyperlink")]
        [JsonProperty("desc",NullValueHandling =NullValueHandling.Ignore)]
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Gets or sets the hyperlink (i.e. the URL pointing to another resource)
        /// </summary>
        [DisplayName("Hyper Reference")]
        [Description("The hyperlink to the linked resource")]
        [JsonProperty("href")]
        public string Href
        {
            get
            {
                return _href;
            }

            set
            {
                _href = value;
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating whether the URL contains URL-Parameters (optional)
        /// </summary>
        [DisplayName("Templated")]
        [Description("Indicates whether the provided hyper reference is templated")]
        [JsonProperty("templated", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Templated
        {
            get
            {
                return _templated;
            }

            set
            {
                _templated = value;
            }
        }

        /// <summary>
        /// Gets or sets the action which should be used to interact with the referenced resource in order to achieve a certain goal (optional)
        /// </summary>
        [DisplayName("Action")]
        [Description("Defines the HTTP verb which should be use ")]
        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public string Action
        {
            get
            {
                return _action;
            }

            set
            {
                _action = value;
            }
        }

    }
}
