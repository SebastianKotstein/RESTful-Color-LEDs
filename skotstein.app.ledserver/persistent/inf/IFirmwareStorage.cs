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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    public interface IFirmwareStorage
    {

        /// <summary>
        /// Returns a list containing all <see cref="IFirmwareDataSet"/>s.
        /// </summary>
        /// <returns>list with all <see cref="IFirmwareDataSet"/>s</returns>
        IList<IFirmwareDataSet> GetAllFirmwares();

        /// <summary>
        /// Returns the <see cref="IFirmwareDataSet"/> matching the passed ID or null, if there is no <see cref="IFirmwareDataSet"/> matching the ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IFirmwareDataSet"/></param>
        /// <returns>the <see cref="IFirmwareDataSet"/> or null</returns>
        IFirmwareDataSet GetFirmware(string id);

        /// <summary>
        /// Returns the raw data of the firmware matching the passed ID or null, if there is no <see cref="IFirmwareDataSet"/> matching the ID or the raw data does not exist.
        /// </summary>
        /// <param name="id">ID of the <see cref="IFirmwareDataSet"/></param>
        /// <returns>the raw data</returns>
        string GetRawData(string id);

        /// <summary>
        /// Sets the raw data of the firmware matching the passed ID. This method throws an <see cref="Exception"/> if there is no <see cref="IFirmwareDataSet"/> having the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IFirmwareDataSet"/></param>
        /// <param name="rawData">the raw data</param>
        void SetRawData(string id, string rawData);

        /// <summary>
        /// Returns true, if the <see cref="IFirmwareDataSet"/> having the passed ID has raw data, else false.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        bool HasRawData(string id);

        /// <summary>
        /// Returns true, if there is a <see cref="IFirmwareDataSet"/> having the passed ID, else false.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true or false</returns>
        bool HasFirmware(string id);

        /// <summary>
        /// Stores the passed <see cref="IFirmwareDataSet"/> as a new entity. Note that the <see cref="IFirmwareDataSet.Id"/> included in
        /// <see cref="IFirmwareDataSet"/> must be set and must be unique. This methods throws an <see cref="Exception"/> if there is already a <see cref="IFirmwareDataSet"/> having the specified ID, or if
        /// the ID is not set. Note that the raw data must be created and stored separately (see <see cref="SetRawData(string, string)"/>).
        /// </summary>
        /// <param name="firmwareDataSet"><see cref="IFirmwareDataSet"/> to be added</param>
        /// <param name="rawData">raw data of the firmware to be added</param>
        void CreateFirmware(IFirmwareDataSet firmwareDataSet, string rawData);

        /// <summary>
        /// Deletes the <see cref="IFirmwareDataSet"/> having the passed ID. If there is not <see cref="IFirmwareDataSet"/> matching the passed ID, nothing will happen.
        /// </summary>
        /// <param name="id">ID of the <see cref="IFirmwareDataSet"/> which should be deleted</param>
        void DeleteFirmware(string id);

        /// <summary>
        /// Deletes the passed<see cref="IFirmwareDataSet"/>. Nothing will happen, if the passed <see cref="IFirmwareDataSet"/> is not handled by this <see cref="IFirmwareStorage"/> implementation.
        /// </summary>
        /// <param name="firmwareDataSet"><see cref="IFirmwareDataSet"/> which should be deleted</param>
        void DeleteFirmware(IFirmwareDataSet firmwareDataSet);

        /// <summary>
        /// Changes the name of the device which compatible to the firmware having the passed ID. This method throws an <see cref="Exception"/> if there is no <see cref="IFirmwareDataSet"/> having the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IFirmwareDataSet"/></param>
        /// <param name="deviceName">new name of the compatible device</param>
        void ChangeMetaInformation(string id, string deviceName);

        /// <summary>
        /// Changes the minor and major version of the <see cref="IFirmwareDataSet"/> having the passed ID. This method throws an <see cref="Exception"/> if there is no <see cref="IFirmwareDataSet"/> having the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IFirmwareDataSet"/></param>
        /// <param name="minorVersion">new minor version</param>
        /// <param name="majorVersion">new major version</param>
        void ChangeMetaInformation(string id, int minorVersion, int majorVersion);

        /// <summary>
        /// Changes the name of the device which compatible to the firmware and the minor and major version of the <see cref="IFirmwareDataSet"/> having the passed ID. This method throws an <see cref="Exception"/> if there is no <see cref="IFirmwareDataSet"/> having the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IFirmwareDataSet"/></param>
        /// <param name="deviceName">new name of the compatible device</param>
        /// <param name="minorVersion">new minor version</param>
        /// <param name="majorVersion">new major version</param>
        void ChangeMetaInformation(string id, string deviceName, int minorVersion, int majorVersion);

    }
}
