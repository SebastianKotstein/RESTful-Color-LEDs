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
using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.businesslayer
{
    /// <summary>
    /// The class <see cref="ControllerHandler"/> implements the business logic for handling <see cref="Controller"/>s. More specifically,
    /// this class implements methods for obtaining all <see cref="Controller"/>s (see <see cref="GetControllers(IDictionary{string, string})"/>) or a single <see cref="Controller"/> (see <see cref="GetController(int)"/>), for
    /// creating (see <see cref="CreateController(string, int)"/> or changing (see <see cref="ChangeController(int, string, int)"/>) and deleting (see <see cref="DeleteController(int)"/> a <see cref="Controller"/>.
    /// As well as querying the <see cref="ControllerLeds"/> (see <see cref="GetLedsOfController(int)"/>) and the <see cref="ControllerFirmware"/> (see <see cref="GetFirmware(int)"/>). Use <see cref="SetColor(int, string)"/> to set
    /// the LED color for all LEDs of a specific <see cref="Controller"/> by issuing only one method call.
    /// </summary>
    public class ControllerHandler
    {
        //reference to the controller storage, where all controllers are stored persistently
        private IControllerStorage _controllerStorage;

        //a reference to the LED storage is required for setting the LED color and for creating or deleting LEDs if a controller is created, removed or its LED count is changed
        private ILedStorage _ledStorage;

        //a reference to the group storage is required since if a controller is deleted or the LED count is decreased, the non-assigned LEDs must be removed from groups
        private IGroupStorage _groupStorage;

        /// <summary>
        /// Initializes an instance of this class
        /// </summary>
        /// <param name="controllerStorage">reference to the <see cref="IControllerStorage"/>, where all controllers are stored persistently</param>
        /// <param name="groupStorage">reference to the <see cref="IGroupStorage"/>, where all groups are stored persistently</param>
        /// <param name="ledStorage">reference to the <see cref="ILedStorage"/>, where all LEDs are stored persistently</param>
        public ControllerHandler(IControllerStorage controllerStorage, IGroupStorage groupStorage, ILedStorage ledStorage)
        {
            _controllerStorage = controllerStorage;
            _ledStorage = ledStorage;
            _groupStorage = groupStorage;
        }

        /// <summary>
        /// Returns the current <see cref="NetworkState"/> based on the passed timestamp of the last verified connection.
        /// The network state is:
        /// - <see cref="NetworkState.offline"/> if the timestamp is older than 60 seconds
        /// - <see cref="NetworkState.connection_lost"/> if the timestamp is older than 30 seconds but younger than 60 seconds
        /// - <see cref="NetworkState.online"/> if the the timestamp is younger than 30 seconds
        /// </summary>
        /// <param name="timestamp">timestamp of the last verified connection</param>
        /// <returns>current network state</returns>
        private NetworkState CalculateState(DateTime timestamp)
        {
            if(DateTime.UtcNow.Subtract(timestamp).TotalSeconds > 60)
            {
                return NetworkState.offline;
            }
            else if(DateTime.UtcNow.Subtract(timestamp).TotalSeconds > 30)
            {
                return NetworkState.connection_lost;
            }
            else
            {
                return NetworkState.online;
            }
    
        }

        /// <summary>
        /// Returns an instance of <see cref="Controllers"/> which contains all <see cref="Controller"/>s matching the passed query. The following query parameters can be applied:
        /// <list type="number">
        ///     <item>query parameter <code>id</code>: returns only the <see cref="Controller"/> having the passed <see cref="Controller.Id"/></item>
        ///     <item>query parameter <code>name</code>: returns all <see cref="Controller"/> whose <see cref="Controller.Name"/> contains the passed value (case invariant).</item>
        ///     <item>query parameter <code>device_name</code>: returns all <see cref="Controller"/> whose device name contains the passed value (case invariant). Note that the device name is only visible if the respective <see cref="Controller"/> is online.</item>
        ///     <item>query parameter <code>firmware_id</code>: returns all <see cref="Controller"/> whose firmware has the passed firmware ID. Note that the firmware ID is only visible if the respective <see cref="Controller"/> is online.</item>
        ///     <item>query parameter <code>network_state</code>: returns all <see cref="Controller"/> whose <see cref="Controller.NetworkState"/> has the passed value (case invariant).</item>
        /// </list>
        /// If multiple query parameters are applied, the respective <see cref="Controller"/> must match all query criterion to be in the list of <see cref="Controllers"/>.
        /// If the query is 'null' or empty (i.e. query.Count == 0), all <see cref="Controller"/>s handled by this class are returned.
        /// Unknown query parameters will be ignored.
        /// </summary>
        /// <param name="query">query (can be null or empty)</param>
        /// <returns>instance of <see cref="Controllers"/> containg all <see cref="Controller"/>s matching the passed query</returns>
        public Controllers GetControllers(IDictionary<string,string> query)
        {
            Controllers controllers = new Controllers();

            //return all controllers, if no query parameters are specified
            if (query == null || query.Count == 0)
            {
                foreach (IControllerDataSet cds in _controllerStorage.GetAllControllers())
                {
                    //create controller entity object
                    Controller controller = new Controller();
                    controller.Id = cds.Id;
                    controller.Name = cds.FriendlyName;
                    controller.LedCount = cds.LedCount;
                    controller.State = CalculateState(cds.Timestamp);

                    controllers.ControllerList.Add(controller);
                }
            }
            //filter controllers based on the given query parameters
            else
            {
                foreach(IControllerDataSet cds in _controllerStorage.GetAllControllers())
                {
                    if (query.ContainsKey("id"))
                    {
                        if(cds.Id != ApiBase.ParseId(query["id"]))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("name"))
                    {
                        if (String.IsNullOrWhiteSpace(cds.FriendlyName) || !cds.FriendlyName.ToLower().Contains(query["name"].ToLower()))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("device_name"))
                    {
                        if (String.IsNullOrWhiteSpace(cds.DeviceName) || !cds.DeviceName.ToLower().Contains(query["device_name"].ToLower()))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("firmware_id"))
                    {
                        if (String.IsNullOrWhiteSpace(cds.FirmwareId) ||  cds.FirmwareId.CompareTo(query["firmware_id"])!=0)
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("uuid"))
                    {
                        if (String.IsNullOrWhiteSpace(cds.UuId) || cds.UuId.CompareTo(query["uuid"]) != 0)
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("network_state"))
                    {
                        NetworkState networkState = CalculateState(cds.Timestamp);
                        if(networkState != NetworkState.connection_lost && query["network_state"].ToLower().CompareTo("connection_lost") == 0)
                        {
                            continue;
                        }
                        else if (networkState != NetworkState.online && query["network_state"].ToLower().CompareTo("online") == 0)
                        {
                            continue;
                        }
                        else if (networkState != NetworkState.offline && query["network_state"].ToLower().CompareTo("offline") == 0)
                        {
                            continue;
                        }
                        else if (query["network_state"].ToLower().CompareTo("offline") != 0 ||
                                 query["network_state"].ToLower().CompareTo("connection_lost") != 0 ||
                                 query["network_state"].ToLower().CompareTo("online") != 0)
                        {
                            continue;
                        }
                    }

                    Controller controller = new Controller();
                    controller.Id = cds.Id;
                    controller.Name = cds.FriendlyName;
                    controller.LedCount = cds.LedCount;
                    controller.State = CalculateState(cds.Timestamp);

                    controllers.ControllerList.Add(controller);

                }
            }
            return controllers;
        }

        /// <summary>
        /// Creates a new controller having the passed name and LED count. If the LED count is negative, zero or greater than 256, a <see cref="BadRequestException"/> is thrown.
        /// The <see cref="Controller.Id"/> of the created controller is returned.
        /// </summary>
        /// <param name="name">name of the controller</param>
        /// <param name="ledCount">number of LEDs</param>
        /// <returns>the <see cref="Controller.Id"/> of the created controller</returns>
        public int CreateController(string name, int ledCount)
        {
            if (ledCount <= 0 || ledCount > 256)
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_LED_COUNT);
            }
            int id = ((FileBasedControllerStorage)_controllerStorage).IdCounter++;

            ControllerDataSetJson cds = new ControllerDataSetJson();
            cds.FriendlyName = name;
            cds.Id = id;
            cds.UuId = Guid.NewGuid().ToString();
            cds.LedCount = ledCount;

            _controllerStorage.CreateController(cds);

            //create LEDs
            CreateLeds(id, ledCount);
            return id;
        }

        /// <summary>
        /// Creates the LED resources for the controller having the passed <see cref="Controller.Id"/>.
        /// </summary>
        /// <param name="controllerId">the <see cref="Controller.Id"/> of the controller</param>
        /// <param name="ledCount">number of LEDs to be created</param>
        private void CreateLeds(int controllerId, int ledCount)
        {
            //create LEDs
            for (int i = 0; i < ledCount; i++)
            {
                _ledStorage.AddLed(controllerId, i);
            }
        }

        /// <summary>
        /// Returns the <see cref="Controller"/> having the passed ID or throws a <see cref="ResourceNotFoundException"/> if a <see cref="Controller"/>
        /// with the passed ID does not exist.
        /// </summary>
        /// <param name="id">id of the controller</param>
        /// <returns>controller</returns>
        public Controller GetController(int id)
        {
            if (_controllerStorage.HasControllerById(id))
            {
                Controller controller = new Controller();
                IControllerDataSet cds = _controllerStorage.GetControllerById(id);
                controller.Id = cds.Id;
                controller.Name = cds.FriendlyName;
                controller.LedCount = cds.LedCount;
                controller.State = CalculateState(cds.Timestamp);

                return controller;
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_CONTROLLER_NOT_FOUND.Replace("{VALUE}",id+""));
            }
        }

        /// <summary>
        /// Changes the name and the LED count of the controller having the passed <see cref="Controller.Id"/>.
        /// If the LED count is negative, zero or greater than 256, a <see cref="BadRequestException"/> is thrown.
        /// The method throws an <see cref="ResourceNotFoundException"/> if the specified <see cref="Controller"/> does not exist.
        /// </summary>
        /// <param name="id"><see cref="Controller.Id"/> of the controller</param>
        /// <param name="name">new <see cref="Controller.Name"/></param>
        /// <param name="ledCount">new number of LEDs to be created</param>
        public void ChangeController(int id, string name, int ledCount)
        {
            if(ledCount <= 0 || ledCount > 256)
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_LED_COUNT);
            }

            if (_controllerStorage.HasControllerById(id))
            {
                int oldLedCount = _controllerStorage.GetControllerById(id).LedCount;
                _controllerStorage.SetNameAndLedCount(id, name, ledCount);
                if(ledCount != oldLedCount)
                {
                    //change LEDs
                    _ledStorage.CleanLeds(id, 0); //remove all LEDs,...
                    CreateLeds(id, ledCount); //... then add them as new LED, as the offset might be higher than before
                    _groupStorage.CleanLeds(id, ledCount); //remove all LEDs until the offset (i.e. all LEDs which are greater than or equals the offset)
                }
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_CONTROLLER_NOT_FOUND.Replace("{VALUE}", id + ""));
            }
        }

        /// <summary>
        /// Deletes the controller having the passed <see cref="Controller.Id"/> but only if it exists.
        /// Moreover, all associated LED resources will be removed.
        /// </summary>
        /// <param name="id"><see cref="Controller.Id"/> of the controller which should be deleted</param>
        public void DeleteController(int id)
        {
            if (_controllerStorage.HasControllerById(id))
            {
                _controllerStorage.DeleteController(id);
                //change LEDs
                _ledStorage.CleanLeds(id, 0); //remove all LEDs
                _groupStorage.CleanLeds(id, 0); //remove all LEDs
            }
        }

        /// <summary>
        /// Returns an instance of <see cref="ControllerFirmware"/> encompassing all firmware details of the controller having the passed <see cref="Controller.Id"/>.
        /// The method throws an <see cref="ResourceNotFoundException"/> if the specified <see cref="Controller"/> does not exists.
        /// </summary>
        /// <param name="id"><see cref="Controller.Id"/> of the controller</param>
        /// <returns>instance of <see cref="ControllerFirmware"/></returns>
        public ControllerFirmware GetFirmware(int id)
        {
            if (_controllerStorage.HasControllerById(id))
            {
                IControllerDataSet cds = _controllerStorage.GetControllerById(id);
                ControllerFirmware firmware = new ControllerFirmware(id, cds.UuId, cds.LedCount);
                firmware.DeviceName = cds.DeviceName;
                firmware.FirmwareId = cds.FirmwareId;
                firmware.MajorVersion = cds.MajorVersion;
                firmware.MinorVersion = cds.MinorVersion;
                firmware.TimeStamp = cds.Timestamp;

                return firmware;
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_CONTROLLER_NOT_FOUND.Replace("{VALUE}", id + ""));
            }
        }

        /// <summary>
        /// Returns true if the controller with the passed <see cref="Controller.Id"/> exists, else false.
        /// </summary>
        /// <param name="id"><see cref="Controller.Id"/> of the controller</param>
        /// <returns>true or false</returns>
        public bool HasController(int id)
        {
            return _controllerStorage.HasControllerById(id);
        }

        /// <summary>
        /// Returns an instance of <see cref="ControllerLeds"/> encompassing all <see cref="Led"/>s of the controller having the passed <see cref="Controller.Id"/>.
        /// The method throws an <see cref="ResourceNotFoundException"/> if the specified <see cref="Controller"/> does not exists.
        /// </summary>
        /// <param name="id"><see cref="Controller.Id"/> of the controller</param>
        /// <returns>instance of <see cref="ControllerLeds"/> encompassing the Leds</returns>
        public ControllerLeds GetLedsOfController(int id)
        {
            if (!_controllerStorage.HasControllerById(id))
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_CONTROLLER_NOT_FOUND.Replace("{VALUE}", id + ""));
            }
            ControllerLeds controllerLeds = new ControllerLeds();
            controllerLeds.ControllerId = id;
            foreach (ILedDataSet lds in _ledStorage.GetAllLedsOfController(id))
            {
                Led led = new Led();
                led.ControllerId = lds.ControllerId;
                led.LedNumber = lds.LedNumber;
                led.RgbValue = lds.RgbValue;

                controllerLeds.Leds.Add(led);
            }
            return controllerLeds;

        }

        /// <summary>
        /// Sets the color of all LEDs of the controller having the specified <see cref="Controller.Id"/>.
        /// The method throws an <see cref="ResourceNotFoundException"/> if the specified <see cref="Controller"/> does not exists.
        /// </summary>
        /// <param name="id"><see cref="Controller.Id"/> of the controller whose LEDs should be set</param>
        /// <param name="rgb">the RGB value of the LEDs</param>
        public void SetColor(int id, string rgb)
        {
            if (_controllerStorage.HasControllerById(id))
            {
                foreach(ILedDataSet lds in _ledStorage.GetAllLedsOfController(id))
                {
                    _ledStorage.SetColor(lds.Id, rgb);
                }
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_CONTROLLER_NOT_FOUND.Replace("{VALUE}", id + ""));
            }
        }
    }
}
