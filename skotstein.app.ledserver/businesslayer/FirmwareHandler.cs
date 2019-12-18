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
using skotstein.app.ledserver.persistent;
using skotstein.app.ledserver.restlayer;
using skotstein.app.ledserver.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.businesslayer
{
    /// <summary>
    /// the class <see cref="FirmwareHandler"/> implements the business logic for handling <see cref="Firmware"/>. More specifically, this class implements methods
    /// for obtaining all <see cref="Firmware"/> meta information entities (see <see cref="GetFirmwareCollection(IDictionary{string, string})"/>) or a single <see cref="Firmware"/> meta information entity (see <see cref="GetFirmware(string, IDictionary{string, string})"/>),
    /// for creating (see <see cref="CreateFirmware(string, int, int, string)"/>), changing (see <see cref="ChangeFirmware(string, string, int, int)"/>) and deleting (see <see cref="DeleteFirmware(string)"/>) a <see cref="Firmware"/>.
    /// Every <see cref="Firmware"/> consists of the aforementioned meta information and, additionally, raw data (i.e. the firmware file) which can be flashed on the controllers.
    /// Use <see cref="GetRawData(string, IDictionary{string, string})"/> to obtain and <see cref="SetRawData(string, string)"/> to update this raw data.
    /// </summary>
    public class FirmwareHandler
    {
        public const string PLACEHOLDER_UUID = "{[UUID]}";
        public const string PLACEHOLDER_MIN_VERSION = "{[MIN]}";
        public const string PLACEHOLDER_MAJ_VERSION = "{[MAJ]}";
        public const string PLACEHOLDER_DEVICE_NAME_VERSION = "{[DEVICE]}";
        public const string PLACEHOLDER_FIRMWARE_ID = "{[FW_ID]}";
        public const string PLACEHOLDER_WIFI_SSID = "{[WIFI_SSID]}";
        public const string PLACEHOLDER_WIFI_PWD = "{[WIFI_PWD]}";
        public const string PLACEHOLDER_TARGET_URL = "{[TARGET_URL]}";
        public const string PLACEHOLDER_TIMEOUT = "{[TIMEOUT]}";
        public const string PLACEHOLDER_LED_COUNT = "{[LED_COUNT]}";

        //reference to the firmware storage where all firmware files and their meta information are stored persistently
        private FileBasedFirmwareStorage _firmwareStorage;

        //reference to the settings as the returned firmware files are injected with several parameters from the setting file
        private Settings _settings;

        /// <summary>
        /// Initializes an instance of this class
        /// </summary>
        /// <param name="storage">reference to the <see cref="IFirmwareStorage"/> where all firmware files and their meta information are stored persistently</param>
        /// <param name="settings">a reference to the <see cref="Settings"/> is required as the returned firmware files are injected with several parameters from the setting file</param>
        public FirmwareHandler(FileBasedFirmwareStorage storage, Settings settings)
        {
            _firmwareStorage = storage;
            _settings = settings;
        }

        /// <summary>
        /// Returns an instance of <see cref="FirmwareCollection"/> which contains all <see cref="Firmware"/> matching the passed query. The following query parameters can be applied:
        ///<list type="number">
        ///     <item>query parameter <code>id</code>: returns only the <see cref="Firmware"/> having the passed <see cref="Firmware.Id"/></item>
        ///     <item>query parameter <code>device</code>: returns all <see cref="Firmware"/> whose <see cref="Firmware.DeviceName"/> contains the passed value (case invariant)</item>
        ///     <item>query parameter <code>maj</code>: returns all <see cref="Firmware"/> having the passed <see cref="Firmware.MajorVersion"/></item>
        ///     <item>query parameter <code>min</code>: returns all <see cref="Firmware"/> having the passed <see cref="Firmware.MinorVersion"/></item>
        /// </list>
        /// If multiple query parameters are applied, the respective <see cref="Firmware"/> must match all query criterion in order to be in the returned list of <see cref="FirmwareCollection"/>.
        /// If the query is 'null' or empty (i.e. query.Count == 0), all <see cref="Firmware"/> handled by this class are returned.
        /// Unknown query parameters will be ignored.
        /// Additionally, the <code>uuid</code> and <code>leds</code> query parameter can be set. These parameter values will be added as query parameters in all hyperlinks of the returned resource representation such that
        /// a consumer of the REST API is navigated to an individual firmware file where these values are already injected.
        /// </summary>
        /// <param name="query">query (can be null or empty)</param>
        /// <returns>instance of <see cref="FirmwareCollection"/> containing all <see cref="Firmware"/> matching the passed query</returns>
        public FirmwareCollection GetFirmwareCollection(IDictionary<string,string> query)
        {
            FirmwareCollection firmwareCollection = new FirmwareCollection();
            
            //return all firmware, if no query parameters are specified
            if(query == null || query.Count == 0)
            {
                foreach(IFirmwareDataSet fds in _firmwareStorage.GetAllFirmwares())
                {
                    Firmware fw = new Firmware();
                    fw.FirmwareId = fds.Id;
                    fw.MajorVersion = fds.MajorVersion;
                    fw.MinorVersion = fds.MinorVersion;
                    fw.DeviceName = fds.DeviceName;
                    fw.FileExtension = fds.FirmwareFileExtension;
                    if (query.ContainsKey("uuid") && query.ContainsKey("leds"))
                    {
                        fw.Uuid = query["uuid"];
                        fw.LedCount = query["leds"];
                    }
                    fw.HasRawData = _firmwareStorage.HasRawData(fds.Id);
                    firmwareCollection.FirmwareList.Add(fw);
                }
            }
            //filter firmware based on given query parameters
            else
            {
                foreach(IFirmwareDataSet fds in _firmwareStorage.GetAllFirmwares())
                {
                    if (query.ContainsKey("id"))
                    {
                        if (fds.Id.CompareTo(query["id"]) != 0)
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("device"))
                    {
                        if (String.IsNullOrWhiteSpace(fds.DeviceName) || !fds.DeviceName.ToLower().Contains(query["device"].ToLower()))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("min"))
                    {
                        if(fds.MinorVersion != ApiBase.ParseInt(query["min"], BadRequestException.MSG_INVALID_MIN_MAJ_VERSION))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("maj"))
                    {
                        if (fds.MinorVersion != ApiBase.ParseInt(query["maj"], BadRequestException.MSG_INVALID_MIN_MAJ_VERSION))
                        {
                            continue;
                        }
                    }
                    Firmware fw = new Firmware();
                    fw.FirmwareId = fds.Id;
                    fw.MajorVersion = fds.MajorVersion;
                    fw.MinorVersion = fds.MinorVersion;
                    fw.DeviceName = fds.DeviceName;
                    fw.FileExtension = fds.FirmwareFileExtension;
                    if (query.ContainsKey("uuid") && query.ContainsKey("leds"))
                    {
                        fw.Uuid = query["uuid"];
                        fw.LedCount = query["leds"];
                    }
                    fw.HasRawData = _firmwareStorage.HasRawData(fds.Id);
                    firmwareCollection.FirmwareList.Add(fw);
                }
            }
            return firmwareCollection;
        }

        /// <summary>
        /// Creates a new <see cref="Firmware"/> meta information resource having the passed parameters.
        /// The methods throws a <see cref="BadRequestException"/> of the file extension is null or empty.
        /// </summary>
        /// <param name="deviceName">name of the device the firmware is compatible with</param>
        /// <param name="minorVersion">minor version of the firmware</param>
        /// <param name="majorVersion">major version of the firmware</param>
        /// <param name="fileExtension">file extension for the firmware file (cannot be null or empty)</param>
        /// <returns></returns>
        public string CreateFirmware(string deviceName, int minorVersion, int majorVersion, string fileExtension)
        {
            IFirmwareDataSet fds = new FirmwareDataSetJson();
            fds.DeviceName = deviceName;
            if (String.IsNullOrWhiteSpace(fileExtension))
            {
                throw new BadRequestException(BadRequestException.MSG_FILE_EXTENSION_NOT_SET);
            }
            fds.FirmwareFileExtension = fileExtension;
            fds.MajorVersion = majorVersion;
            fds.MinorVersion = minorVersion;
            fds.Id = "F" + _firmwareStorage.IdCounter++;
            _firmwareStorage.CreateFirmware(fds, "");
            return fds.Id;
        }

        /// <summary>
        /// Returns the <see cref="Firmware"/> having the passed <see cref="Firmware.Id"/>.
        /// Additionally, the<code> uuid</code> and<code> leds</code> query parameter can be set. These parameter values will be added as query parameters in all hyperlinks of the returned resource representation such that
        /// a consumer of the REST API is navigated to an individual firmware file where these values are already injected.
        /// The methods throws a <see cref="ResourceNotFoundException"/> if the requested <see cref="Firmware"/> does not exist.
        /// </summary>
        /// <param name="id"><see cref="Firmware.Id"/> of the requested firmeware</param>
        /// <param name="query">additional query parameters containing the UUID and the LED count (can be null)</param>
        /// <returns>the <see cref="Firmware"/> having the passed ID</returns>
        public Firmware GetFirmware(string id, IDictionary<string,string> query)
        {
            IFirmwareDataSet fds = _firmwareStorage.GetFirmware(id);
            if (fds != null)
            {
                Firmware fw = new Firmware();
                fw.FirmwareId = id;
                fw.MajorVersion = fds.MajorVersion;
                fw.MinorVersion = fds.MinorVersion;
                fw.DeviceName = fds.DeviceName;
                fw.FileExtension = fds.FirmwareFileExtension;
                if (query != null && query.ContainsKey("uuid") && query.ContainsKey("leds"))
                {
                    fw.Uuid = query["uuid"];
                    fw.LedCount = query["leds"];
                }
                fw.HasRawData = _firmwareStorage.HasRawData(id);
                return fw;
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_FIRMWARE_NOT_FOUND.Replace("{VALUE}",id));
            }
        }

        /// <summary>
        /// Changes the meta information of the firmware having the passed ID.
        /// The methods throws a <see cref="ResourceNotFoundException"/> if the requested <see cref="Firmware"/> does not exist.
        /// Note that the <see cref="Firmware.FileExtension"/> cannot not be changed.
        /// </summary>
        /// <param name="id"><see cref="Firmware.Id"/> of the firmeware which should be changed</param>
        /// <param name="deviceName">name of the device the firmware is compatible with</param>
        /// <param name="minorVersion">minor version of the firmware</param>
        /// <param name="majorVersion">major version of the firmware</param>
        public void ChangeFirmware(string id, string deviceName, int minorVersion, int majorVersion)
        {
            if (_firmwareStorage.HasFirmware(id))
            {
                _firmwareStorage.ChangeMetaInformation(id, deviceName, minorVersion, majorVersion);
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_FIRMWARE_NOT_FOUND.Replace("{VALUE}", id));
            }
        }

        /// <summary>
        /// Removes the firmware having the specified ID.
        /// </summary>
        /// <param name="id">see cref="Firmware.Id"/> of the firmeware which should be deleted</param>
        public void DeleteFirmware(string id)
        {
            _firmwareStorage.DeleteFirmware(id);
        }

        /// <summary>
        /// Returns the raw data of the firmware file linked to the firmware meta information having the passed ID.
        /// Additionally, the <code>uuid</code> and <code>leds</code> query parameter can be set. These parameter values will be injected into the raw data such that
        /// the returned firmware file can be flashed on the corresponding controller without any modifications.
        /// The methods throws a <see cref="ResourceNotFoundException"/> if the requested <see cref="Firmware"/> does not exist.
        /// </summary>
        /// <param name="id">see cref="Firmware.Id"/> of the firmeware whose firmware file is requested</param>
        /// <param name="query">additional query parameters containing the UUID and the LED count which will be injected into the returned raw data (can be null)</param>
        /// <returns>the raw data (firmware file)</returns>
        public string GetRawData(string id, IDictionary<string,string> query)
        {
            string uuid = "";
            string leds = "";
            if (query != null && query.ContainsKey("uuid") && query.ContainsKey("leds"))
            {
                uuid = query["uuid"];
                leds = query["leds"];
            }

            IFirmwareDataSet fds = _firmwareStorage.GetFirmware(id);
            if (fds != null)
            {
                if (!_firmwareStorage.HasRawData(id))
                {
                    throw new ResourceNotFoundException(ResourceNotFoundException.MSG_FILE_NOT_FOUND);
                }
                string rawData = _firmwareStorage.GetRawData(id);
                return rawData.Replace(PLACEHOLDER_UUID, uuid)
                    .Replace(PLACEHOLDER_MIN_VERSION, fds.MinorVersion + "")
                    .Replace(PLACEHOLDER_MAJ_VERSION, fds.MajorVersion + "")
                    .Replace(PLACEHOLDER_DEVICE_NAME_VERSION, fds.DeviceName)
                    .Replace(PLACEHOLDER_FIRMWARE_ID, fds.Id)
                    .Replace(PLACEHOLDER_WIFI_SSID, _settings.WifiSsid)
                    .Replace(PLACEHOLDER_WIFI_PWD, _settings.WifiPwd)
                    .Replace(PLACEHOLDER_TARGET_URL, _settings.TargetUrl)
                    .Replace(PLACEHOLDER_TIMEOUT, _settings.ClientTimeout + "")
                    .Replace(PLACEHOLDER_LED_COUNT, leds);
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_FIRMWARE_NOT_FOUND.Replace("{VALUE}", id));
            }
        }

        /// <summary>
        /// Creates or changes the raw data of the firmware having the passed ID. The method returns true, if a the raw data is created or false, if the raw data is changed.
        /// The methods throws a <see cref="ResourceNotFoundException"/> if the requested <see cref="Firmware"/> does not exist.
        /// </summary>
        /// <param name="id">see cref="Firmware.Id"/> of the firmeware whose firmware file content should be changed</param>
        /// <param name="rawData">the raw data</param>
        /// <returns>true or false</returns>
        public bool SetRawData(string id, string rawData)
        {
            if (_firmwareStorage.HasFirmware(id))
            {
                bool created = !_firmwareStorage.HasRawData(id);
                _firmwareStorage.SetRawData(id, rawData);
                return created;
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_FIRMWARE_NOT_FOUND.Replace("{VALUE}", id));
            }
        }

    }
}
