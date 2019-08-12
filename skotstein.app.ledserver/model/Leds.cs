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
    /// This class represents a list resource containing a set of <see cref="Led"/>s-
    /// A JSON representation is returned by calling <see cref="LedController.GetLeds(SKotstein.Net.Http.Context.HttpContext)"/>.
    /// </summary>
    [DisplayName("Leds")]
    [Description("Represents a collection of LEDs")]
    public class Leds
    {
        public const string SCHEMA_NAME = "leds";

        private IList<Led> _leds = new List<Led>();
        private string _controllerId;

        /// <summary>
        /// Gets or sets the list of <see cref="Led"/>s
        /// </summary>
        [DisplayName("Leds")]
        [Description("Represents a collection of LEDs")]
        [JsonProperty("leds")]
        public IList<Led> LedList
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
        /// Gets a <see cref="Hyperlink"/> object pointing to this resource itself
        /// </summary>
        [DisplayName("Self")]
        [Description("Hyperlink to this resource")]
        [JsonProperty("self")]
        public Hyperlink ToSelf
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/leds");                
            }
        }
    }
}
