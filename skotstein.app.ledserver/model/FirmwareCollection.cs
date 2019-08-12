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
    /// This class represents a list resource containing a set of <see cref="Firmware"/>.
    /// A JSON representation is returnd by calling <see cref="FirmwareController.GetFirmwareCollection(SKotstein.Net.Http.Context.HttpContext)"/>.
    /// </summary>
    [DisplayName("Firmware Collection")]
    [Description("Represent a collection of firmware")]
    public class FirmwareCollection
    {
        public const string SCHEMA_NAME = "firmwares";

        private IList<Firmware> _firmwares = new List<Firmware>();

        /// <summary>
        /// Gets or sets the list of <see cref="Firmware"/>
        /// </summary>
        [DisplayName("Firmware Collection")]
        [Description("Represent a collection of firmware")]
        [JsonProperty("firmware")]
        public IList<Firmware> FirmwareList
        {
            get
            {
                return _firmwares;
            }
            set
            {
                _firmwares = value;
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
                return new Hyperlink(ApiBase.API_V1 + "/firmware");
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for adding a new firmware
        /// </summary>
        [DisplayName("Add Firmware")]
        [Description("Represent a collection of firmware")]
        [JsonProperty("add_firmware")]
        public Hyperlink AddFirmware
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/firmware") { Action = "Hyperlink" };
            }
        }
    }
}
