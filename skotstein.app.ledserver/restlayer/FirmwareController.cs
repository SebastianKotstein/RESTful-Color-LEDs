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
using skotstein.app.ledserver.businesslayer;
using skotstein.app.ledserver.exceptions;
using skotstein.app.ledserver.model;
using skotstein.app.ledserver.tools;
using skotstein.net.http.oauth;
using SKotstein.Net.Http.Attributes;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.restlayer
{
    /// <summary>
    /// This class implements the API endpoints for interacting with <see cref="Firmware"/> resources
    /// </summary>
    public class FirmwareController : HttpController
    {
        private FirmwareHandler _firmwareHandler;

        /// <summary>
        /// Initializes an instance of this class
        /// </summary>
        /// <param name="handler">Reference to the business logic for processing incoming requests</param>
        public FirmwareController(FirmwareHandler handler)
        {
            _firmwareHandler = handler;
        }

        /// <summary>
        /// Returns a list of firmware
        /// </summary>
        /// <group>Firmware</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/firmware</url>
        /// <param name="id" cref="string" in="query">Returns only the firmware having the passed ID.</param>
        /// <param name="device" cref="string" in="query">Returns all firmware whose device name contains the passed value (case invariant).</param>
        /// <param name="maj" cref="int" in="query">Returns all firmware having the passed major version.</param>
        /// <param name="min" cref="int" in="query">Returns all firmware having the passed minor version.</param>
        /// <param name="uuid" cref="string" in="query">If set, this query parameter and the specified value will be added to all hyperlinks of the returned resource.</param>
        /// <param name="leds" cref="string" in="query">If set, this query parameter and the specified value will be added to all hyperlinks of the returned resource.</param>
        /// <response code="200">
        ///     <see cref="FirmwareCollection"/>
        ///     successful response
        ///     <example name="Firmware collection">
        ///         <value>
        ///             $EXAMPLE_13FirmwareCollection
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The major or minor version must be an integer
        ///     <example name="Invalid major or minor version">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidMinMajVersion
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1+"/firmware",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.FIRMWARE_READ)]
        public void GetFirmwareCollection(HttpContext context)
        {
            FirmwareCollection firmwareCollection = _firmwareHandler.GetFirmwareCollection(context.Request.ParsedQuery);
            string json = JsonSerializer.SerializeJson(firmwareCollection);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Creates a new firmware meta information resource
        /// </summary>
        /// <group>Firmware</group>
        /// <verb>POST</verb>
        /// <url>pseudo://localhost/api/v1/firmware</url>
        /// <param name="payload" in="body" required="true">
        ///     <see cref="Firmware"/>
        ///     The firmware meta information
        ///     <example name="Firmware meta information">
        ///         <value>
        ///             $EXAMPLE_15FirmwareCreate
        ///         </value>
        ///     </example>
        /// </param>
        /// <response code="201">
        ///     <header name="Location" cref="string"><description>Contains the URL pointing to the resource representing the firmware which has been successfully created</description></header>
        ///     <see cref="Firmware"/>
        ///     successful response
        ///     <example name="Firmware meta information">
        ///         <value>
        ///             $EXAMPLE_14Firmware
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The payload is empty, invalid or the file extension has not been set
        ///     <example name="Missing file extension">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgFileExtensionNotSet
        ///         </value>
        ///     </example>
        ///      <example name="Invalid payload">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidPayload
        ///         </value>
        ///     </example>
        ///     <example name="Payload expected">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgPayloadExpected
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1+"/firmware",HttpMethod.POST)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.FIRMWARE_WRITE)]
        public void CreateFirmware(HttpContext context)
        {
            if(context.Request.Payload.Length > 0)
            {
                string json = context.Request.Payload.ReadAll();
                Firmware firmware = JsonSerializer.DeserializeJson<Firmware>(json);
                if(firmware == null)
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
                //add firmware meta information
                string id = _firmwareHandler.CreateFirmware(firmware.DeviceName, firmware.MinorVersion, firmware.MajorVersion, firmware.FileExtension);
                Firmware result = _firmwareHandler.GetFirmware(id, null);
                string jsonResult = JsonSerializer.SerializeJson(result);
                context.Response.Payload.Write(jsonResult);
                context.Response.Headers.Set("Location", ApiBase.API_V1 + "/firmware/" + id);
                context.Response.Status = HttpStatus.Created;
            }
            else
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
            }
        }

        /// <summary>
        /// Returns the meta data of the firmware having the specified ID
        /// </summary>
        /// <group>Firmware</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/firmware/{firmwareId}</url>
        /// <param name="firmwareId" cref="string" in="path">The ID of the firmware</param>
        /// <param name="uuid" cref="string" in="query">If set, this query parameter and the specified value will be added to all hyperlinks of the returned resource.</param>
        /// <param name="leds" cref="string" in="query">If set, this query parameter and the specified value will be added to all hyperlinks of the returned resource.</param>
        /// <response code="200">
        ///     <see cref="Firmware"/>
        ///     successful response
        ///     <example name="Firmware meta information">
        ///         <value>
        ///             $EXAMPLE_14Firmware
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The firmware having the passed ID does not exist
        ///     <example name="Firmware not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgFirmwareNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1+"/firmware/{firmwareId}",HttpMethod.GET)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.FIRMWARE_READ)]
        public void GetFirmware(HttpContext context, string firmwareId)
        {
            Firmware firmware = _firmwareHandler.GetFirmware(firmwareId,context.Request.ParsedQuery);
            string json = JsonSerializer.SerializeJson(firmware);
            context.Response.Payload.Write(json);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Changes the meta information of the firmware having the specified ID. Note that the file extension cannot be changed.
        /// </summary>
        /// <group>Firmware</group>
        /// <verb>PUT</verb>
        /// <url>pseudo://localhost/api/v1/firmware/{firmwareId}</url>
        /// <param name="firmwareId" cref="string" in="path">The ID of the firmware</param>
        /// <param name="payload" in="body" required="true" type="application/json">
        ///     <see cref="Firmware"/>
        ///     Firmware meta information
        ///     <example name="Firmware meta information">
        ///         <value>
        ///             $EXAMPLE_16FirmwareChange
        ///         </value>
        ///     </example>
        /// </param>
        /// <response code="200">The response contains the updated firmware meta information
        ///     <see cref="Firmware"/>
        ///     <example name="Firmware meta information">
        ///         <value>
        ///             $EXAMPLE_14Firmware
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="400">
        ///     <see cref="ErrorMessage"/>
        ///     The payload is empty or invalid
        ///     <example name="Invalid payload">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgInvalidPayload
        ///         </value>
        ///     </example>
        ///     <example name="Payload expected">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgPayloadExpected
        ///         </value>
        ///     </example>
        /// </response>
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The firmware having the passed ID does not exist
        ///     <example name="Firmware not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgFirmwareNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1+"/firmware/{firmwareId}",HttpMethod.PUT)]
        [ContentType(MimeType.APPLICATION_JSON)]
        [AuthorizationScope(Scopes.FIRMWARE_WRITE)]
        public void ChangeFirmware(HttpContext context, string firmwareId)
        {
            if(context.Request.Payload.Length > 0)
            {
                string json = context.Request.Payload.ReadAll();
                Firmware firmware = JsonSerializer.DeserializeJson<Firmware>(json);
                if (firmware == null)
                {
                    throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
                }
                //change firmware meta data
                _firmwareHandler.ChangeFirmware(firmwareId, firmware.DeviceName, firmware.MinorVersion, firmware.MajorVersion);

                Firmware response = _firmwareHandler.GetFirmware(firmwareId, null);
                string jsonResponse = JsonSerializer.SerializeJson(response);
                context.Response.Payload.Write(jsonResponse);
                context.Response.Status = HttpStatus.OK;
            }
            else
            {
                throw new BadRequestException(BadRequestException.MSG_PAYLOAD_EXPECTED);
            }
        }

        /// <summary>
        /// Deletes the firmware having the specified ID
        /// </summary>
        /// <group>Firmware</group>
        /// <verb>DELETE</verb>
        /// <url>pseudo://localhost/api/v1/firmware/{firmwareId}</url>
        /// <param name="firmwareId" cref="string" in="path">The ID of the firmware</param>
        /// <response code="200">successful</response>
        [Path(ApiBase.API_V1+"/firmware/{firmwareId}",HttpMethod.DELETE)]
        [AuthorizationScope(Scopes.FIRMWARE_WRITE)]
        public void DeleteFirmware(HttpContext context, string firmwareId)
        {
            _firmwareHandler.DeleteFirmware(firmwareId);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Returns the content (raw data) of the firmware file having the specified ID
        /// </summary>
        /// <group>Firmware</group>
        /// <verb>GET</verb>
        /// <url>pseudo://localhost/api/v1/firmware/{firmwareId}/file</url>
        /// <param name="firmwareId" cref="string" in="path">The ID of the firmware</param>
        /// <param name="uuid" cref="string" in="query">If set, the value of this query parameter is injected into the returned raw file.</param>
        /// <param name="leds" cref="string" in="query">If set, the value of this query parameter is injected into the returned raw file.</param>
        /// <response code="200" type="application/[FILE_EXTENSION]">
        /// <see cref="string"/>
        /// successful response
        /// </response>
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The firmware having the passed ID does not exist or the firmware file has not been stored yet
        ///     <example name="Firmware not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgFirmwareNotFound
        ///         </value>
        ///     </example>
        ///     <see cref="ErrorMessage"/>
        ///     <example name="File not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_NotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1+"/firmware/{firmwareId}/file",HttpMethod.GET)]
        [AuthorizationScope(Scopes.FIRMWARE_READ)]
        public void GetFirmwareFile(HttpContext context, string firmwareId)
        {
            string rawData = _firmwareHandler.GetRawData(firmwareId,context.Request.ParsedQuery);
            string fileExtension = _firmwareHandler.GetFirmware(firmwareId, null).FileExtension;
            context.Response.Payload.Write(rawData);
            context.Response.Headers.Set("Content-Type", "application/" + fileExtension);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Creates or changes the content (raw data) of the firmware file having the specified ID
        /// </summary>
        /// <group>Firmware</group>
        /// <verb>PUT</verb>
        /// <url>pseudo://localhost/api/v1/firmware/{firmwareId}/file</url>
        /// <param name="firmwareId" cref="string" in="path">The ID of the firmware</param>
        /// <param name="payload" in="body" required="false" type="application/[FILE_EXTENSION]">
        /// <see cref="string"/>
        /// The raw content
        /// </param>
        /// <response code="200">
        ///     <see cref="string"/>
        ///     The response contains the updated raw data
        /// </response>
        /// <response code="201" type="application/[FILE_EXTENSION]">
        ///     <header name="Location" cref="string"><description>Contains the URL pointing to the raw data which has been successfully created</description></header>
        ///     <see cref="string"/>
        ///     The response contains the raw data which have been successfully created
        /// </response>
        /// <response code="404">
        ///     <see cref="ErrorMessage"/>
        ///     The firmware having the passed ID does not exist
        ///     <example name="Firmware not found">
        ///         <value>
        ///             $EXAMPLE_Error_Message_MsgFirmwareNotFound
        ///         </value>
        ///     </example>
        /// </response>
        [Path(ApiBase.API_V1 + "/firmware/{firmwareId}/file", HttpMethod.PUT)]
        [AuthorizationScope(Scopes.FIRMWARE_WRITE)]
        public void SetFirmwareFile(HttpContext context, string firmwareId)
        {
            string rawData = context.Request.Payload.ReadAll();
            bool created = _firmwareHandler.SetRawData(firmwareId, rawData);

            string response = _firmwareHandler.GetRawData(firmwareId, context.Request.ParsedQuery);
            string fileExtension = _firmwareHandler.GetFirmware(firmwareId, null).FileExtension;
            context.Response.Payload.Write(response);
            context.Response.Headers.Set("Content-Type", "application/" + fileExtension);

            if (created)
            {
                context.Response.Headers.Set("Location", ApiBase.API_V1 + "/firmware/" + firmwareId + "/file");
                context.Response.Status = HttpStatus.Created;
            }
            else
            {
                context.Response.Status = HttpStatus.OK;
            }
        }
    }
}
