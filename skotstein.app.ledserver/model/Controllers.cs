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
    /// This class represents a list resource containing a set of <see cref="Controller"/>. 
    /// A JSON representation is returned by calling <see cref="ControllerController.GetControllers(SKotstein.Net.Http.Context.HttpContext)"/>.
    /// </summary>
    [DisplayName("Controllers")]
    [Description("Represents a collection of controllers")]
    public class Controllers
    {
        public const string SCHEMA_NAME = "controllers";

        private IList<Controller> _controllers = new List<Controller>();

        /// <summary>
        /// Gets or sets the list of <see cref="Controller"/>s
        /// </summary>
        [DisplayName("Controllers")]
        [Description("Represents a collection of controllers")]
        [JsonProperty("controllers")]
        public IList<Controller> ControllerList
        {
            get
            {
                return _controllers;
            }

            set
            {
                _controllers = value;
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
                    return new Hyperlink(ApiBase.API_V1 + "/controllers");
                }
                else
                {
                    return null;
                }
                
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for creating a new controller resource.
        /// </summary>
        [DisplayName("Create Controller")]
        [Description("Hyperlink for creating a new controller")]
        [JsonProperty("create_new_controller", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink ActionCreate
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/controllers") { Action = "POST"};
                }
                else
                {
                    return null;
                }
            }
        }

        /*
        [DisplayName("Meta Information")]
        [Description("Provides meta information about the document representation such as a link to its schema")]
        [JsonProperty("_meta")]
        public MetaInformation Meta
        {
            get
            {
                MetaInformation meta = new MetaInformation();
                meta.Schema = new Hyperlink(ApiBase.API_DOC + "/schemas/controllers") { Description = "Schema of this document" };
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_DOCUMENTATION + ":curies", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_DOCUMENTATION + "/curies"));
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_CONTROLLER + ":create", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_CONTROLLER + "/create"));
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_DOCUMENTATION + ":schema", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_DOCUMENTATION + "/schema"));
                return meta;
            }
        }
        */
    }
}
