using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    /// This class represents a single controller resource.
    /// A JSON representation is returned by calling <see cref="ControllerController.GetController(SKotstein.Net.Http.Context.HttpContext, string)"/>.
    /// </summary>
    //[Newtonsoft.Json.JsonObject(Id = "#/api/docs/schemas/controller")]
    [DisplayName("Controller")]
    [Description("Represents a physical controller (e.g. an ESP8266) controlling a set of LEDs")]
    public class Controller
    {
        public const string SCHEMA_NAME = "controller";

        private int _id;
        private string _name;
        private int _ledCount;
        private NetworkState _state;

        /// <summary>
        /// Gets or sets the ID of the controller
        /// </summary>
        [DisplayName("Controller ID")]
        [Description("The ID of the controller")]
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
        /// Gets or sets the friendly name of the controller
        /// </summary>
        [DisplayName("Controller Name")]
        [Description("The friendly name of the controller")]
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
        /// Gets or sets the <see cref="NetworkState"/> of the controller
        /// </summary>
        [DisplayName("Network State")]
        [Description("The current network state of the controller")]
        [JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NetworkState State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of LEDs which are controlled by this controller
        /// </summary>
        [DisplayName("LED Count")]
        [Description("The number of LEDs controlled by this controller")]
        [JsonProperty("ledCount")]
        public int LedCount
        {
            get
            {
                return _ledCount;
            }

            set
            {
                _ledCount = value;
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to a sub resource containing meta information about the firmware of this controller
        /// </summary>
        [DisplayName("Controller Firmware")]
        [Description("Hyperlink to the firmware meta information of the controller")]
        [JsonProperty("firmware")]
        public Hyperlink ToFirmware
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/controllers/" + Id + "/firmware");
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to a sub resource containig a list of all LEDs being controlled by this controller
        /// </summary>
        [DisplayName("LEDs")]
        [Description("Link to the list of LEDs controlled by this controller")]
        [JsonProperty("leds")]
        public Hyperlink ToLeds
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/controllers/" + Id + "/leds");
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
                return new Hyperlink(ApiBase.API_V1 + "/controllers/" + Id);
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for changing this controller resource
        /// </summary>
        [DisplayName("Change Controller")]
        [Description("Hyperlink for chaning this controller")]
        [JsonProperty("change_controller")]
        public Hyperlink ActionChange
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/controllers/" + Id) { Action = "PUT" };
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for deleting this controller resource
        /// </summary>
        [DisplayName("Remove Controller")]
        [Description("Hyperlink for deleting this controller")]
        [JsonProperty("remove_controller")]
        public Hyperlink ActionDelete
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/controllers/" + Id) { Action = "DELETE" };
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
                meta.Schema = new Hyperlink(ApiBase.API_DOC + "/schemas/controller") { Description = "Schema of this document" };
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_DOCUMENTATION + ":curies", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_DOCUMENTATION + "/curies"));
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_CONTROLLER + ":firmware", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_CONTROLLER + "/firmware"));
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_CONTROLLER + ":leds", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_CONTROLLER + "/leds"));
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_CONTROLLER + ":remove_controller", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_CONTROLLER + "/remove_controller"));
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_CONTROLLER + ":change_controller", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_CONTROLLER + "/change_controller"));
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_DOCUMENTATION + ":schema", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_DOCUMENTATION + "/schema"));
                return meta;
            }
        }
        */


    }

    public enum NetworkState
    {
        
        offline, connection_lost, online
    }
}
