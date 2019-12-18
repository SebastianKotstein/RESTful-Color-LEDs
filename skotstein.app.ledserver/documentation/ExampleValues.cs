// MIT License
//
// Copyright (c) 2019 Sebastian Kotstein
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using skotstein.app.ledserver.exceptions;
using skotstein.app.ledserver.model;
using skotstein.app.ledserver.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.documentation
{
    /// <summary>
    /// This class generates JSON-formatted example values which are injected into the OpenAPI documentation produced by <see cref="OpenApiDocumentBuilder"/>.
    /// <see cref="OpenApiDocumentBuilder.AsJson(Microsoft.OpenApi.Models.OpenApiDocument)"/> converts an OpenAPI document into its JSON representation (string).
    /// However, this JSON representation contains placeholders, e.g. "$EXAMPLE_Error_Message_MsgInvalidId", which must be replaced by appropriate JSON-formatted value examples provided by this class.
    /// Use <see cref="OpenApiDocumentBuilder.Replace(string, object)"/> to replace these placeholders in the OpenAPI document
    /// with the associated example value. The link between placeholder and example value is given over the <see cref="Replaces"/> attribute of each example value property.
    /// </summary>
    public class ExampleValues
    {
        #region ErrorMessage

        [Replaces("$EXAMPLE_Error_Message_MsgInvalidId")]
        public string JsonExampleErrorMessage_MsgInvalidId { get { return JsonSerializer.SerializeJson(new ErrorMessage(400, BadRequestException.MSG_INVALID_ID)); } }

        [Replaces("$EXAMPLE_Error_Message_MsgInvalidLedId")]
        public string JsonExampleErrorMessage_MsgInvalidLedId { get { return JsonSerializer.SerializeJson(new ErrorMessage(400, BadRequestException.MSG_INVALID_LED_ID)); } }

        [Replaces("$EXAMPLE_Error_Message_MsgInvalidPayload")]
        public string JsonExampleErrorMessage_MsgInvalidPayload { get { return JsonSerializer.SerializeJson(new ErrorMessage(400, BadRequestException.MSG_INVALID_PAYLOAD)); } }

        [Replaces("$EXAMPLE_Error_Message_MsgPayloadExpected")]
        public string JsonExampleErrorMessage_MsgPayloadExpected { get { return JsonSerializer.SerializeJson(new ErrorMessage(400, BadRequestException.MSG_PAYLOAD_EXPECTED)); } }

        [Replaces("$EXAMPLE_Error_Message_MsgInvaldidLedCount")]
        public string JsonExampleErrorMessage_MsgInvaldidLedCount { get { return JsonSerializer.SerializeJson(new ErrorMessage(400, BadRequestException.MSG_INVALID_LED_COUNT)); } }

        [Replaces("$EXAMPLE_Error_Message_MsgInvalidQueryParameter")]
        public string JsonExampleErrorMessage_MsgInvalidQueryParameter { get { return JsonSerializer.SerializeJson(new ErrorMessage(400, BadRequestException.MSG_INVALID_QUERY_PARAMETER_VALUE.Replace("{QUERY}", "equals"))); } }

        [Replaces("$EXAMPLE_Error_Message_MsgInvalidMinMajVersion")]
        public string JsonExampleErrorMessage_MsgInvalidMinMajVersion { get { return JsonSerializer.SerializeJson(new ErrorMessage(400, BadRequestException.MSG_INVALID_MIN_MAJ_VERSION)); } }

        [Replaces("$EXAMPLE_Error_Message_MsgFileExtensionNotSet")]
        public string JsonExampleErrorMessage_MsgFileExtensionNotSet { get { return JsonSerializer.SerializeJson(new ErrorMessage(400, BadRequestException.MSG_FILE_EXTENSION_NOT_SET)); } }

        [Replaces("$EXAMPLE_Error_Message_MsgControllerNotFound")]
        public string JsonExampleErrorMessage_MsgControllerNotFound { get { return JsonSerializer.SerializeJson(new ErrorMessage(404, ResourceNotFoundException.MSG_CONTROLLER_NOT_FOUND.Replace("{VALUE}", "0"))); } }

        [Replaces("$EXAMPLE_Error_Message_MsgGroupNotFound")]
        public string JsonExampleErrorMessage_MsgGroupNotFound { get { return JsonSerializer.SerializeJson(new ErrorMessage(404, ResourceNotFoundException.MSG_GROUP_NOT_FOUND.Replace("{VALUE}", "0"))); } }

        [Replaces("$EXAMPLE_Error_Message_MsgLedNotFound")]
        public string JsonExampleErrorMessage_MsgLedNotFound { get { return JsonSerializer.SerializeJson(new ErrorMessage(404, ResourceNotFoundException.MSG_LED_NOT_FOUND.Replace("{VALUE}", "0:0"))); } }

        [Replaces("$EXAMPLE_Error_Message_MsgFirmwareNotFound")]
        public string JsonExampleErrorMessage_MsgFirmwareNotFound { get { return JsonSerializer.SerializeJson(new ErrorMessage(404, ResourceNotFoundException.MSG_FIRMWARE_NOT_FOUND.Replace("{VALUE}", "F0"))); } }

        [Replaces("$EXAMPLE_Error_Message_NotFound")]
        public string JsonExapleErrorMessage_MsgNotFound { get { return JsonSerializer.SerializeJson(new ErrorMessage(404, ResourceNotFoundException.MSG_FILE_NOT_FOUND)); } }

        [Replaces("$EXAMPLE_Error_Message_MsgSchemaNotFound")]
        public string JsonExampleSchemaNotFound { get { return JsonSerializer.SerializeJson(new ErrorMessage(404, ResourceNotFoundException.MSG_SCHEMA_NOT_FOUND)); } }

        [Replaces("$EXAMPLE_Error_Message_MissingAccessToken")]
        public string JsonExampleMissingAccessToken { get { return JsonSerializer.SerializeJson(new ErrorMessage(401,skotstein.net.http.oauth.UnauthorizedException.MSG_MISSING_ACCESS_TOKEN)); } }

        [Replaces("$EXAMPLE_Error_Message_InvalidAccessToken")]
        public string JsonExampleInvalidAccessToken { get { return JsonSerializer.SerializeJson(new ErrorMessage(401, skotstein.net.http.oauth.UnauthorizedException.MSG_INVALID_ACCESS_TOKEN)); } }

        [Replaces("$EXAMPLE_Error_Message_TokenHasExpired")]
        public string JsonExampleTokenHasExpired { get { return JsonSerializer.SerializeJson(new ErrorMessage(401, skotstein.net.http.oauth.UnauthorizedException.MSG_TOKEN_HAS_EXPIRED)); } }

        [Replaces("$EXAMPLE_Error_Message_Forbidden")]
        public string JsonExampleForbidden { get { return JsonSerializer.SerializeJson(new ErrorMessage(403, skotstein.net.http.oauth.ForbiddenException.MSG_INVALID_SCOPE)); } }

        #endregion

        #region Controller
        [Replaces("$EXAMPLE_0Controllers")]
        public string JsonExampleControllers
        {
            get
            {
                Controllers controllers = new Controllers();
                for(int i = 0; i < 4; i++)
                {
                    controllers.ControllerList.Add(GenerateExampleController(i));
                }
                return JsonSerializer.SerializeJson(controllers);
            }
        }


        [Replaces("$EXAMPLE_1Controller")]
        public string JsonExampleController
        {
            get
            {
                return JsonSerializer.SerializeJson(GenerateExampleController(0));
            }
        }

        [Replaces("$EXAMPLE_2RgbValue")]
        public string JsonExampleRgbValue
        {
            get
            {
                return JsonSerializer.SerializeJson(new RgbValue() { Rgb = "#00FF00"});
            }
        }

        [Replaces("$EXAMPLE_3ControllerChange")]
        public string JsonExampleControllerChange
        {
            get
            {
                return "{\"name\":\"Controller A\",\"ledCount\":120}";
            }
        }

        [Replaces("$EXAMPLE_4ControllerFirmware")]
        public string JsonExampleControllerFirmware
        {
            get
            {
                ControllerFirmware firmware = new ControllerFirmware(0, "AAAAA-BBBBB-aaaaa-xxxx", 96);
                firmware.DeviceName = "ESP8266@NodeMCU";
                firmware.MajorVersion = 1;
                firmware.MinorVersion = 0;
                firmware.FirmwareId = "F1";
                firmware.TimeStamp = DateTime.UtcNow;
                return JsonSerializer.SerializeJson(firmware);
            }
        }

        [Replaces("$EXAMPLE_5ControllerLeds")]
        public string JsonExampleControllerLeds
        {
            get
            {
                ControllerLeds controllerLeds = new ControllerLeds();
                controllerLeds.ControllerId = 0;
                for(int i = 0; i < 96; i++)
                {
                    Led led = new Led();
                    led.ControllerId = 0;
                    led.LedNumber = i;
                    led.RgbValue = "#AA99FF";
                    controllerLeds.Leds.Add(led);
                }
                return JsonSerializer.SerializeJson(controllerLeds);
            }
        }

        [Replaces("$EXAMPLE_6Groups")]
        public string JsonExampleGroups
        {
            get
            {
                Groups groups = new Groups();
                groups.GroupList.Add(GenerateExampleGroup(0));
                groups.GroupList.Add(GenerateExampleGroup(1));
                groups.GroupList.Add(GenerateExampleGroup(2));
                groups.GroupList.Add(GenerateExampleGroup(3));
                return JsonSerializer.SerializeJson(groups);
            }
        }

        [Replaces("$EXAMPLE_7GroupName")]
        public string JsonExampleGroupName
        {
            get
            {
                return "{\"name\":\"Group A\"}";
            }
        }

        [Replaces("$EXAMPLE_8GroupLeds")]
        public string JsonExampleGroupLeds
        {
            get
            {
                GroupLeds leds = new GroupLeds();
                leds.GroupId = 1;
                for (int i = 0; i < 96; i++)
                {
                    if (i % 2 == 1)
                    {
                        continue;
                    }
                    Led led = new Led();
                    led.ControllerId = 1;
                    led.LedNumber = i;
                    led.RgbValue = "#AA99FF";
                    leds.Leds.Add(led);
                }
                return JsonSerializer.SerializeJson(leds);
            }
        }

        [Replaces("$EXAMPLE_9Group")]
        public string JsonExampleGroup
        {
            get
            {
                return JsonSerializer.SerializeJson(GenerateExampleGroup(1));
            }
        }

        [Replaces("$EXAMPLE_10GroupLEDsModification")]
        public string JsonExampleGroupLedsModification
        {
            get
            {
                string leds = "";
                for (int i = 0; i < 96; i++)
                {
                    if (i % 2 == 1)
                    {
                        continue;
                    }
                    leds += "{\"controller_id\":1,\"led_number\":" + i + "},";
                }
                leds.TrimEnd(',');

                return "{\"leds:\":[" + leds + "]}";
            }
        }

        [Replaces("$EXAMPLE_11Leds")]
        public string JsonExampleLeds
        {
            get
            {
                Leds leds = new Leds();
                for(int i = 0; i < 4; i++)
                {
                    for(int a = 0; a < 100 + (i*3)-4; a++)
                    {
                        Led led = new Led();
                        led.ControllerId = i;
                        led.LedNumber = a;
                        led.RgbValue = "#AA99FF";
                        leds.LedList.Add(led);
                    }
                }
                return JsonSerializer.SerializeJson(leds);
            }
        }

        [Replaces("$EXAMPLE_12Led")]
        public string JsonExampleLed
        {
            get
            {
                Led led = new Led();
                led.ControllerId = 0;
                led.LedNumber = 0;
                led.RgbValue = "#AA99FF";
                return JsonSerializer.SerializeJson(led);
            }
        }

        [Replaces("$EXAMPLE_13FirmwareCollection")]
        public string JsonExampleFirmwareCollectoion
        {
            get
            {
                FirmwareCollection firmwareCollection = new FirmwareCollection();

                Firmware firmware = new Firmware();
                firmware.FirmwareId = "F1";
                firmware.DeviceName = "ESP8266@NodeMCU";
                firmware.MajorVersion = 1;
                firmware.MinorVersion = 0;
                firmware.FileExtension = "ino";
                firmwareCollection.FirmwareList.Add(firmware);
                return JsonSerializer.SerializeJson(firmwareCollection);
            }
        }

        [Replaces("$EXAMPLE_14Firmware")]
        public string JsonExampleFirmware
        {
            get
            {
                Firmware firmware = new Firmware();
                firmware.FirmwareId = "F1";
                firmware.DeviceName = "ESP8266@NodeMCU";
                firmware.MajorVersion = 1;
                firmware.MinorVersion = 0;
                firmware.FileExtension = "ino";
                return JsonSerializer.SerializeJson(firmware);
            }
        }

        [Replaces("$EXAMPLE_15FirmwareCreate")]
        public string JsonExampleFirmwareCreate
        {
            get
            {
                return "{\"device_name\":\"ESP8266@NodeMCU\",\"major_version\":1,\"minor_version\":0,\"file_extension\":\"ino\"}";
            }
        }

        [Replaces("$EXAMPLE_16FirmwareChange")]
        public string JsonExampleFirmwareChange
        {
            get
            {
                return "{\"device_name\":\"ESP8266@NodeMCU\",\"major_version\":1,\"minor_version\":0}";
            }
        }

        private Controller GenerateExampleController(int id)
        {
            return new Controller()
            {
                Id = id,
                LedCount = 100 + (id * 3) - 4,
                Name = "Controller " + id,
                State = (id % 3 == 0) ? NetworkState.online : ((id % 2 == 0) ? NetworkState.connection_lost : NetworkState.offline)
            };
        }

        private Group GenerateExampleGroup(int id)
        {
            Group group = new Group();
            group.Id = id;
            if(id == 0)
            {
                group.Name = "All_LEDs_of_Controller_0";
            }
            else if(id == 1)
            {
                group.Name = "Even_LEDs_of_Controller_0";
            }
            else if(id == 2)
            {
                group.Name = "Odd_LEDs_of_Controller_0";
            }
            else
            {
                group.Name = "First_Ten_LEDs_of_Controller_1";
            }
            return group;
        }
        #endregion

    }
}
