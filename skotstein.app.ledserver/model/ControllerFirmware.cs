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
    /// This class represents a sub resource of <see cref="Controller"/> containing meta information about the installed firmware of the controller.
    /// A JSON representation is returned by calling <see cref="ControllerController.GetFirmware(SKotstein.Net.Http.Context.HttpContext, string)"/>.
    /// </summary>
    //[Newtonsoft.Json.JsonObject(Id ="#/api/v1/doc/schemas/controller_firmware")]
    [DisplayName("Controller Firmware Meta Information")]
    [Description("Represents the meta information about the firmware of a controller")]
    public class ControllerFirmware
    {
        public const string SCHEMA_NAME = "controllerfirmware";

        private DateTime _timeStamp;
        private int _minorVersion;
        private int _majorVersion;
        private string _firmwareId;
        private string _deviceName;

        private int _controllerId;
        private string _controllerUuId;
        private int _ledCounter;

        /// <summary>
        /// Initializes an instance
        /// </summary>
        /// <param name="controllerId">The <see cref="Controller.Id"/> this resource is linked to</param>
        /// <param name="controllerUuId">The UUID of the controller this resource is linked to</param>
        /// <param name="ledCounter">The <see cref="Controller.LedCount"/> of the controller this resource is linked to</param>
        public ControllerFirmware(int controllerId, string controllerUuId, int ledCounter)
        {
            _controllerId = controllerId;
            _controllerUuId = controllerUuId;
            _ledCounter = ledCounter;
        }

        /// <summary>
        /// Gets or sets the timestamp containing the point of time, when the firmware parameters has been refreshed
        /// </summary>
        [DisplayName("Last Update")]
        [Description("Point of time (timestamp) when the firmware parameters has been refreshed (might be null if the controller is offline)")]
        [JsonProperty("lastUpdate")]
        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp;
            }

            set
            {
                _timeStamp = value;
            }
        }

        /// <summary>
        /// Gets or sets the minor version of the installed firmware
        /// </summary>
        [DisplayName("Minor Version")]
        [Description("Minor version of the installed firmware (might be null if the controller is offline)")]
        [JsonProperty("minorVersion")]
        public int MinorVersion
        {
            get
            {
                return _minorVersion;
            }

            set
            {
                _minorVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the major version of the installed firmware
        /// </summary>
        [DisplayName("Major Version")]
        [Description("Major version of the installed firmware (might be null if the controller is offline)")]
        [JsonProperty("majorVersion")]
        public int MajorVersion
        {
            get
            {
                return _majorVersion;
            }

            set
            {
                _majorVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the firmware identifier
        /// </summary>
        [DisplayName("Firmware ID")]
        [Description("ID of the installed firmware (might be null if the controller is offline)")]
        [JsonProperty("firmwareId")]
        public string FirmwareId
        {
            get
            {
                return _firmwareId;
            }

            set
            {
                _firmwareId = value;
            }
        }

        /// <summary>
        /// Gets or sets the device name
        /// </summary>
        [DisplayName("Device Name")]
        [Description("Name of the device of this controller (might be null if the controller is offline)")]
        [JsonProperty("deviceName")]
        public string DeviceName
        {
            get
            {
                return _deviceName;
            }

            set
            {
                _deviceName = value;
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
        /// Gets a <see cref="Hyperlink"/> object pointing to the installed firmware
        /// </summary>
        [DisplayName("Installed Firmware")]
        [Description("Hyperlink to the installed firmware (might be null if the controller is offline)")]
        [JsonProperty("installedFirmware")]
        public Hyperlink ToFirmware
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_firmwareId))
                {
                    return null;
                }
                else
                {
                    return new Hyperlink(ApiBase.API_V1 + "/firmware/" + FirmwareId + "?uuid=" + _controllerUuId + "&leds=" + _ledCounter);
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the list of available firmware (i.e. firmware which are compatible with the underlying <see cref="Controller"/>)
        /// </summary>
        [DisplayName("Available Firmware")]
        [Description("Hyperlink to the list of available firmware for this controller")]
        [JsonProperty("availableFirmware")]
        public Hyperlink AvailableFirmwares
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/firmware?uuid=" + _controllerUuId + "&leds=" + _ledCounter);
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to this resource itself
        /// </summary>
        [DisplayName("Self")]
        [Description("Link to this resource")]
        [JsonProperty("self")]
        public Hyperlink ToSelf
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/controllers/" + _controllerId + "/firmware");
            }
        }

    }
}
