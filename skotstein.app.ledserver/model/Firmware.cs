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
    /// This class represents the meta information of a single firmware.
    /// A JSON representation is returnd by calling <see cref="FirmwareController.GetFirmwareCollection(SKotstein.Net.Http.Context.HttpContext)"/>.
    /// </summary>
    [DisplayName("Firmware")]
    [Description("Represents the meta information of a single firmware")]
    public class Firmware
    {
        public const string SCHEMA_NAME = "firmware";

        private int _minorVersion;
        private int _majorVersion;
        private string _firmwareId;
        private string _deviceName;
        private string _fileExtension;

        private string _uuid;
        private string _ledCount;

        /// <summary>
        /// Gets or sets the firmware identifier
        /// </summary>
        [DisplayName("Firmware ID")]
        [Description("The ID of the firmware")]
        [JsonProperty("id")]
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
        /// Gets or sets the minor version of the firmware
        /// </summary>
        [DisplayName("Minor Version")]
        [Description("The minor version of the firmware")]
        [JsonProperty("minor_version")]
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
        /// Gets or sets the major version of the firmware
        /// </summary>
        [DisplayName("Major Version")]
        [Description("The major version of the firmware")]
        [JsonProperty("major_version")]
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
        /// Gets or sets the device name
        /// </summary>
        [DisplayName("Device Name")]
        [Description("The name of the device which is compatible with this firmware")]
        [JsonProperty("device_name")]
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
        /// Gets or sets the extension of the firmware file
        /// </summary>
        [DisplayName("File Extension")]
        [Description("The file extension of the linked firmware file")]
        [JsonProperty("file_extension")]
        public string FileExtension
        {
            get
            {
                return _fileExtension;
            }

            set
            {
                _fileExtension = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the linked firmware file
        /// </summary>
        [DisplayName("Firmware file")]
        [Description("Hyperlink pointing to the linked firmware file")]
        [JsonProperty("firmwareFile")]
        public Hyperlink File
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Uuid) || String.IsNullOrWhiteSpace(LedCount))
                {
                    return new Hyperlink(ApiBase.API_V1 + "/firmware/" + _firmwareId + "/file");
                }
                else
                {
                    return new Hyperlink(ApiBase.API_V1 + "/firmware/" + FirmwareId + "/file?uuid="+Uuid+"&leds=" +LedCount);
                }
                
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to this resource itself
        /// </summary>
        [DisplayName("Self")]
        [Description("Hyperlink to this resource")]
        [JsonProperty("self")]
        public Hyperlink Self
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Uuid) || String.IsNullOrWhiteSpace(LedCount))
                {
                    return new Hyperlink(ApiBase.API_V1 + "/firmware/" + _firmwareId);
                }
                else
                {
                    return new Hyperlink(ApiBase.API_V1 + "/firmware/" + FirmwareId + "?uuid=" + Uuid + "&leds=" + LedCount);
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for changing this firmware meta data
        /// </summary>
        [DisplayName("Change Firmware Meta Data")]
        [Description("Hyperlink for changing the firmware meta data")]
        [JsonProperty("change_firmware_meta_data")]
        public Hyperlink Change
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/firmware/" + _firmwareId) { Action = "PUT" };
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for deleting this firmware (meta data and file)
        /// </summary>
        [DisplayName("Remove Firmware")]
        [Description("Hyperlink for deleting the firmware (i.e. its meta data and the linked firmware file)")]
        [JsonProperty("remove_firmware")]
        public Hyperlink Delete
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/firmware/" + _firmwareId) { Action = "DELETE" };
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object for upload the firmware file
        /// </summary>
        [DisplayName("Upload Firmware File")]
        [Description("Hyperlink for uploading the firmware file")]
        [JsonProperty("upload_firmware_file")]
        public Hyperlink UploadFile
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/firmware/" + _firmwareId+"/file") { Action = "PUT" };
            }
        }

        /// <summary>
        /// Gets or sets the UUID which should be injected into the firmware file such that it can be installed on the associated <see cref="Controller"/> having this UUID.
        /// </summary>
        [JsonIgnore]
        public string Uuid
        {
            get
            {
                return _uuid;
            }

            set
            {
                _uuid = value;
            }
        }

        /// <summary>
        /// Gets or sets the LED count which should be injected into the firmware file such that it can be installed on the associated <see cref="Controller"/> having this LED count.
        /// </summary>
        [JsonIgnore]
        public string LedCount
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

    }
}
