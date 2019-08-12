using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// This interface defines methods for storing <see cref="IControllerDataSet"/> entities persistently.
    /// </summary>
    public interface IControllerStorage
    {
        /// <summary>
        /// Returns a list containing all <see cref="IControllerDataSet"/>s
        /// </summary>
        /// <returns>list with all <see cref="IControllerDataSet"/>s</returns>
        IList<IControllerDataSet> GetAllControllers();

        /// <summary>
        /// Returns the <see cref="IControllerDataSet"/> matching the passed ID or null, if there is no <see cref="IControllerDataSet"/> matching the ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IControllerDataSet"/></param>
        /// <returns>the <see cref="IControllerDataSet"/> or null</returns>
        IControllerDataSet GetControllerById(int id);

        /// <summary>
        /// Returns the <see cref="IControllerDataSet"/> matching the passed UUID or null, if there is no <see cref="IControllerDataSet"/> matching the UUID.
        /// </summary>
        /// <param name="uuid">UUID of the <see cref="IControllerDataSet"/></param>
        /// <returns>the <see cref="IControllerDataSet"/> or null</returns>
        IControllerDataSet GetControllerByUuId(string uuid);

        /// <summary>
        /// Returns true if there is a <see cref="IControllerDataSet"/> having the passed ID, else false.
        /// </summary>
        /// <param name="id">ID of the <see cref="IControllerDataSet"/></param>
        /// <returns>true or false</returns>
        bool HasControllerById(int id);

        /// <summary>
        /// Returns true if there is a <see cref="IControllerDataSet"/> haviong the passed UUI, else false.
        /// </summary>
        /// <param name="uudId">UUID of the <see cref="IControllerDataSet"/></param>
        /// <returns>true or false</returns>
        bool HasControllerByUuid(string uudId);

        /// <summary>
        /// Stores the passed <see cref="IControllerDataSet"/> as a new entity. Note that the <see cref="IControllerDataSet.Id"/> as well as the <see cref="IControllerDataSet.UuId"/> included in the passed <see cref="IControllerDataSet"/>
        /// must be set and must be unique. This method throws an <see cref="Exception"/> if there is already a <see cref="IControllerDataSet"/> having the specified ID or UUID, or if the ID or UUID is not set.
        /// </summary>
        /// <param name="controllerDataSet"><see cref="IControllerDataSet"/> to be added</param>
        void CreateController(IControllerDataSet controllerDataSet);

        /// <summary>
        /// Deletes the <see cref="IControllerDataSet"/> having the passed ID. If there is no <see cref="IControllerDataSet"/> matching the passed ID, nothing will happen.
        /// </summary>
        /// <param name="id">ID of the <see cref="IControllerDataSet"/> which should be deleted</param>
        void DeleteController(int id);

        /// <summary>
        /// Deletes the passed <see cref="IControllerDataSet"/>. Nothing will happen, if the passed <see cref="IControllerDataSet"/> is not handled by this <see cref="IControllerStorage"/> implementation.
        /// </summary>
        /// <param name="controllerDataSet"><see cref="IControllerDataSet"/> which should be deleted</param>
        void DeleteController(IControllerDataSet controllerDataSet);

        /// <summary>
        /// Changes the name of the <see cref="IControllerDataSet"/> having the passed ID. This method throws an <see cref="Exception"/> if there is no <see cref="IControllerDataSet"/> having the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IControllerDataSet"/> whose name should be changed</param>
        /// <param name="name">new name of the <see cref="IControllerDataSet"/></param>
        void SetName(int id, string name);

        /// <summary>
        /// Changes the number of LEDs of the <see cref="IControllerDataSet"/> having the passed ID. This method throws an <see cref="Exception"/> if there is no <see cref="IControllerDataSet"/> having the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IControllerDataSet"/> whose number of LEDs should be changed</param>
        /// <param name="count">new number of LEDs</param>
        void SetLedCount(int id, int count);

        /// <summary>
        /// Changes the name and the number of LEDs of the <see cref="IControllerDataSet"/> having the passed ID. This method throws an <see cref="Exception"/> if there is no <see cref="IControllerDataSet"/> having the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IControllerDataSet"/> whose name and number of LEDs should be changed</param>
        /// <param name="name">new name of the <see cref="IControllerDataSet"/></param>
        /// <param name="count">new number of LEDs</param>
        void SetNameAndLedCount(int id, string name, int count);

        /// <summary>
        /// Updates the firmware parameters of the <see cref="IControllerDataSet"/> having the passed UUID. This method throws an <see cref="Exception"/> if there is no <see cref="IControllerDataSet"/> having the passed UUID.
        /// Note that this method updates the <see cref="IControllerDataSet.Timestamp"/> automatically, showing when the firmware parameters has been updated. 
        /// </summary>
        /// <param name="uuId">UUID of the <see cref="IControllerDataSet"/> whose firmware parameters should be updated</param>
        /// <param name="minVersion">new minor version</param>
        /// <param name="majVersion">new major version</param>
        /// <param name="firmwareId">new firmware ID</param>
        /// <param name="deviceName">new device name</param>
        void SetFirmware(string uuId, int minVersion, int majVersion, string firmwareId, string deviceName);
    }
}
