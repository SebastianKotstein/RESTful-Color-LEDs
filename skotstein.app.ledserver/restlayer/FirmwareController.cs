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
        /// <param name="context"></param>
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
        /// <param name="context"></param>
        [Path(ApiBase.API_V1+"/firmware",HttpMethod.POST)]
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
                context.Response.Headers.Set("Location", ApiBase.API_V1 + "/firmware/" + id);
                context.Response.Status = HttpStatus.Created;
            }
            else
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
            }
        }

        /// <summary>
        /// Returns the meta data of the firmware having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="firmwareId"></param>
        [Path(ApiBase.API_V1+"/firmware/{ID}",HttpMethod.GET)]
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
        /// Changes the meta data of the firmware having the specified ID. Note that the <see cref="Firmware.FileExtension"/> cannot be changed.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="firmwareId"></param>
        [Path(ApiBase.API_V1+"/firmware/{ID}",HttpMethod.PUT)]
        [AuthorizationScope(Scopes.FIRMWARE_WRITE)]
        public void ChangeFirmware(HttpContext context, string firmwareId)
        {
            string json = context.Request.Payload.ReadAll();
            Firmware firmware = JsonSerializer.DeserializeJson<Firmware>(json);
            if(firmware == null)
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_PAYLOAD);
            }
            //change firmware meta data
            _firmwareHandler.ChangeFirmware(firmwareId, firmware.DeviceName, firmware.MinorVersion, firmware.MajorVersion);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Deletes the firmware having the passed ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="firmwareId"></param>
        [Path(ApiBase.API_V1+"/firmware/{ID}",HttpMethod.DELETE)]
        [AuthorizationScope(Scopes.FIRMWARE_WRITE)]
        public void DeleteFirmware(HttpContext context, string firmwareId)
        {
            _firmwareHandler.DeleteFirmware(firmwareId);
            context.Response.Status = HttpStatus.OK;
        }

        /// <summary>
        /// Returns the content (raw data) of the firmware file having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="firmwareId"></param>
        [Path(ApiBase.API_V1+"/firmware/{ID}/file",HttpMethod.GET)]
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
        /// Changes the content (raw data) of the firmware file having the specified ID.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="firmwareId"></param>
        [Path(ApiBase.API_V1 + "/firmware/{ID}/file", HttpMethod.PUT)]
        [AuthorizationScope(Scopes.FIRMWARE_WRITE)]
        public void SetFirmwareFile(HttpContext context, string firmwareId)
        {
            string rawData = context.Request.Payload.ReadAll();
            _firmwareHandler.SetRawData(firmwareId, rawData);
            context.Response.Status = HttpStatus.OK;
        }
    }
}
