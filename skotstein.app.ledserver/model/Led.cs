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
    /// This class represents a single LED resource.
    /// A JSON representation is returned by calling <see cref="LedController.GetLed(SKotstein.Net.Http.Context.HttpContext, string)"/>.
    /// </summary>
    [DisplayName("LED")]
    [Description("Represents a LED which is controlled by a controller")]
    public class Led
    {
        public const string SCHEMA_NAME = "led";

        private int _controllerId;
        private int _ledId;
        private string _rgb;

        /// <summary>
        /// Gets or sets the ID of the <see cref="Controller"/> which controls this LED.
        /// </summary>
        [DisplayName("Controller ID")]
        [Description("The ID of the controller which controls this LED")]
        [JsonProperty("controller_id")]
        public int ControllerId
        {
            get
            {
                return _controllerId;
            }

            set
            {
                _controllerId = value;
            }
        }

        /// <summary>
        /// Gets or sets the LED number of this LED.
        /// </summary>
        [DisplayName("LED Number")]
        [Description("LED number of this LED")]
        [JsonProperty("led_number")]
        public int LedNumber
        {
            get
            {
                return _ledId;
            }

            set
            {
                _ledId = value;
            }
        }

        /// <summary>
        /// Gets or sets the RGB color value of this LED.
        /// </summary>
        [DisplayName("RGB Color Value")]
        [Description("Current RGB color value of this LED")]
        [JsonProperty("rgb")]
        public string RgbValue
        {
            get
            {
                return _rgb;
            }
            set
            {
                _rgb = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the controller which controls this LED.
        /// </summary>
        [DisplayName("Controller")]
        [Description("Hyperlink to the controller which controls this LED")]
        [JsonProperty("controller")]
        public Hyperlink ToController
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/controllers/" + _controllerId);
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to this resource itself
        /// </summary>
        [DisplayName("Self")]
        [Description("Hyperlink to this resource")]
        [JsonProperty("self")]
        public Hyperlink ToSelf
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/leds/"+_controllerId+":"+_ledId);
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for setting the color this LED.
        /// </summary>
        [DisplayName("Set LED Color")]
        [Description("Hyperlink for setting the color of this LED")]
        [JsonProperty("set_color")]
        public Hyperlink SetLed
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/leds/" + _controllerId + ":" + _ledId) { Action = "PUT" };
            }
        }
    }
}
