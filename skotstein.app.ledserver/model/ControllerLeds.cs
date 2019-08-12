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
    /// This class represents a sub resource of <see cref="Controller"/> containing a list of all <see cref="Led"/> resource which are controlled by the controller.
    /// A JSON representation is returned by calling <see cref="ControllerController.GetLeds(SKotstein.Net.Http.Context.HttpContext, string)"/>.
    /// </summary>
    [DisplayName("Controller LEDs")]
    [Description("Represents a collection of LEDs being controlled by the underlying controller")]
    public class ControllerLeds
    {
        public const string SCHEMA_NAME = "controllerleds";

        IList<Led> _leds = new List<Led>();

        private int _controllerId;

        /// <summary>
        /// Gets or sets the <see cref="IList{T}"/> containing the <see cref="Led"/>s being controlled by the underlying controller
        /// </summary>
        [DisplayName("Leds")]
        [Description("List containing all LEDs being controlled by the underlying controller")]
        [JsonProperty("leds")]
        public IList<Led> Leds
        {
            get
            {
                return _leds;
            }

            set
            {
                _leds = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the underlying <see cref="Controller"/> resource
        /// </summary>
        [DisplayName("Controller")]
        [Description("Hyperlink to the underlying controller")]
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
                return new Hyperlink(ApiBase.API_V1 + "/controllers/" + _controllerId + "/leds");
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to action for setting the color of all LEDs being controlled by this controller
        /// </summary>
        [DisplayName("Set Color of LEDs")]
        [Description("Hyperlink for changing the color of all LEDs being controlled by the underlying controller")]
        [JsonProperty("set_color_of_leds")]
        public Hyperlink SetLeds
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/controllers/" + _controllerId + "/leds") { Action = "POST" };
            }
        }

        /// <summary>
        /// Gets or sets the ID of the underlying <see cref="Controller"/>
        /// </summary>
        [JsonIgnore]
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


    }
}
