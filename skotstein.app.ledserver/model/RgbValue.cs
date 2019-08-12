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
    /// Represents a resource which contains a RGB value.
    /// A JSON representation is used as a payload to the set the color of LEDs.
    /// </summary>
    [DisplayName("RGB Value")]
    [Description("A RGB color value of a LED")]
    public class RgbValue
    {
        public const string SCHEMA_NAME = "rgbvalue";

        private string _rgbValue;

        /// <summary>
        /// Gets or sets the RGB color value
        /// </summary>
        [DisplayName("RGB Value")]
        [Description("A RGB color value of a LED")]
        [JsonProperty("rgb")]
        public string Rgb
        {
            get
            {
                return _rgbValue;
            }

            set
            {
                _rgbValue = value;
            }
        }
    }
}
