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
    /// Wrapper class nesting several hyperlink objects pointing to different entry points of the API. These objects are initialized automatically whenever the corresponding read-only property is accessed.
    /// A JSON representation is returned by calling <see cref="BaseController.GetBase(SKotstein.Net.Http.Context.HttpContext)"/> and serves as a central entry point for the API.
    /// </summary>
    [DisplayName("Base")]
    [Description("Serves as a central entry point for the API")]
    public class Base
    {
        public const string SCHEMA_NAME = "base";

        private bool _isOauthEnabled;

        /// <summary>
        /// Gets the <see cref="Hyperlink"/> object pointing to the authorization endpoint 
        /// </summary>
        [DisplayName("Authorize")]
        [Description("Hyperlink pointing to the authorization endpoint")]
        [JsonProperty("authorization", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink Authorization
        {
            get
            {
                if(_isOauthEnabled && Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/auth");
                }
                else
                {
                    return null;
                }
                
            }
        }

        /// <summary>
        /// Gets the <see cref="Hyperlink"/> object pointing to the API endpoints for handling LED controllers
        /// </summary>
        [DisplayName("Controllers")]
        [Description("Hyperlink pointing to the API endpoints for handling LED controllers")]
        [JsonProperty("controllers", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink Controllers
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
        /// Gets the <see cref="Hyperlink"/> object pointing to the API endpoints for handling LED
        /// </summary>
        [DisplayName("Leds")]
        [Description("Hyperlink pointing to the API endpoints for handling LEDs")]
        [JsonProperty("leds", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink Leds
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/leds");
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Hyperlink"/> object pointing to the API endpoints for managing groups of LEDs
        /// </summary>
        [DisplayName("Groups")]
        [Description("Hyperlink pointing to the API endpoints for managing groups of LEDs")]
        [JsonProperty("groups", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink Groups
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
        /// Gets the <see cref="Hyperlink"/> object pointing to the API endpoints for managing firmware settings and files of the LED controllers
        /// </summary>
        [DisplayName("Firmware")]
        [Description("Hyperlink pointing to the API endpoints for managing firmware settings and files of the LED controllers")]
        [JsonProperty("firmware", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink Firmware
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
        /// Gets the <see cref="Hyperlink"/> object pointing to the documentation
        /// </summary>
        [DisplayName("Documentation")]
        [Description("Hyperlink pointing to the documentation")]
        [JsonProperty("docs", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink Documentation
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/docs");
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore]
        public bool IsOauthEnabled
        {
            get
            {
                return _isOauthEnabled;
            }

            set
            {
                _isOauthEnabled = value;
            }
        }
    }
}
