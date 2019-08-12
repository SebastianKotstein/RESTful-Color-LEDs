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
    /// Wrapper class nesting several hyperlink objects pointing to different entry points of the API. These objects are initialized automatically whenever the corresponding read-only property is accessed.
    /// A JSON representation is returned by calling <see cref="BaseController.GetBase(SKotstein.Net.Http.Context.HttpContext)"/> and serves as a central entry point for the API.
    /// </summary>
    [DisplayName("Base")]
    [Description("Serves as a central entry point for the API")]
    public class Base
    {
        public const string SCHEMA_NAME = "base";

        /// <summary>
        /// Gets the <see cref="Hyperlink"/> object pointing to the authorization endpoint 
        /// </summary>
        [DisplayName("Authorize")]
        [Description("Hyperlink pointing to the authorization endpoint")]
        [JsonProperty("authorization")]
        public Hyperlink Authorization
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/auth");
            }
        }

        /// <summary>
        /// Gets the <see cref="Hyperlink"/> object pointing to the API endpoints for handling LED controllers
        /// </summary>
        [DisplayName("Controllers")]
        [Description("Hyperlink pointing to the API endpoints for handling LED controllers")]
        [JsonProperty("controllers")]
        public Hyperlink Controllers
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/controllers");
            }
        }

        /// <summary>
        /// Gets the <see cref="Hyperlink"/> object pointing to the API endpoints for handling LED
        /// </summary>
        [DisplayName("Leds")]
        [Description("Hyperlink pointing to the API endpoints for handling LEDs")]
        [JsonProperty("leds")]
        public Hyperlink Leds
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/leds");
            }
        }

        /// <summary>
        /// Gets the <see cref="Hyperlink"/> object pointing to the API endpoints for managing groups of LEDs
        /// </summary>
        [DisplayName("Groups")]
        [Description("Hyperlink pointing to the API endpoints for managing groups of LEDs")]
        [JsonProperty("groups")]
        public Hyperlink Groups
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups");
            }
        }

        /// <summary>
        /// Gets the <see cref="Hyperlink"/> object pointing to the API endpoints for managing firmware settings and files of the LED controllers
        /// </summary>
        [DisplayName("Firmware")]
        [Description("Hyperlink pointing to the API endpoints for managing firmware settings and files of the LED controllers")]
        [JsonProperty("firmware")]
        public Hyperlink Firmware
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups");
            }
        }

        /// <summary>
        /// Gets the <see cref="Hyperlink"/> object pointing to the documentation
        /// </summary>
        [DisplayName("Documentation")]
        [Description("Hyperlink pointing to the documentation")]
        [JsonProperty("docs")]
        public Hyperlink Documentation
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/docs");
            }
        }
    }
}
