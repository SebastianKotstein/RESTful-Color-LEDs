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
    /// Wrapper class nesting a hyperlink object pointing to the authorization endpoint. This object is initialized automatically whenever the corresponding read-only property is accessed.
    /// A JSON representation is returned by calling <see cref="AuthController.GetAuthBase(SKotstein.Net.Http.Context.HttpContext)"/>.
    /// </summary>
    [DisplayName("Auth Base")]
    [Description("Contains a hyperlink to the authorization endpoint")]
    public class AuthBase
    {
        public const string SCHEMA_NAME = "authbase";

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to a resource containing a list of registered clients and the client management itself
        /// </summary>
        [DisplayName("Clients")]
        [Description("Hyperlink pointing to the client management endpoint")]
        [JsonProperty("clients", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink ToClients
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(skotstein.net.http.oauth.webkit.ApiBase.API_V1 + "/clients");
                }
                else
                {
                    return null;
                }
                
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to a resource containing a list of scopes
        /// </summary>
        [DisplayName("Scopes")]
        [Description("Hyperlink pointing to the list of scopes")]
        [JsonProperty("scopes", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink ToScopes
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(skotstein.net.http.oauth.webkit.ApiBase.API_V1 + "/scopes");
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to this controller resource itself
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
                    return new Hyperlink(ApiBase.API_V1 + "/auth");
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the endpoint where an access token can be obtained
        /// </summary>
        [DisplayName("Authorize")]
        [Description("Hyperlink pointing to the authorization endpoint")]
        [JsonProperty("get_access_token", NullValueHandling = NullValueHandling.Ignore)]
        public Hyperlink GetAccessTokenAction
        {
            get
            {
                if (Settings.ENABLE_HATEOAS)
                {
                    return new Hyperlink(ApiBase.API_V1 + "/auth/token") { Action = "POST" };
                }
                else
                {
                    return null;
                }
            }
        }

       
    }
}
