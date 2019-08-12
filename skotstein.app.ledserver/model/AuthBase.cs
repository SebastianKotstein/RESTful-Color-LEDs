using Newtonsoft.Json;
using skotstein.app.ledserver.restlayer;
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
        [JsonProperty("clients")]
        public Hyperlink ToClients
        {
            get
            {
                return new Hyperlink(skotstein.net.http.oauth.webkit.ApiBase.API_V1 + "/clients");
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to a resource containing a list of scopes
        /// </summary>
        [DisplayName("Scopes")]
        [Description("Hyperlink pointing to the list of scopes")]
        [JsonProperty("scopes")]
        public Hyperlink ToScopes
        {
            get
            {
                return new Hyperlink(skotstein.net.http.oauth.webkit.ApiBase.API_V1 + "/scopes");
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to this controller resource itself
        /// </summary>
        [DisplayName("Self")]
        [Description("Hyperlink to this resource")]
        [JsonProperty("self")]
        public Hyperlink ToSelf
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/auth");
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the endpoint where an access token can be obtained
        /// </summary>
        [DisplayName("Authorize")]
        [Description("Hyperlink pointing to the authorization endpoint")]
        [JsonProperty("get_access_token")]
        public Hyperlink GetAccessTokenAction
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/auth/token") { Action = "POST" };
            }
        }

       
    }
}
