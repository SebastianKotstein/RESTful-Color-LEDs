using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// An implementation of <see cref="IControllerDataSet"/> includes the data of a single controller. By using an implementation of <see cref="IControllerStorage"/>, an instance of <see cref="IControllerDataSet"/> can be stored persistently.
    /// </summary>
    public interface IControllerDataSet
    {
        /// <summary>
        /// Gets or sets the identifer
        /// </summary>
       
        int Id
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the UUID
        /// </summary>
        string UuId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the friendly name
        /// </summary>
        string FriendlyName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the LED Count
        /// </summary>
        int LedCount
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the minor version of the installed firmware
        /// </summary>
        int MinorVersion
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets the major version of the installed firmware
        /// </summary>
        int MajorVersion
        { 
            get;set;
        }

        /// <summary>
        /// Gets or sets the ID of the installed firmware
        /// </summary>
        string FirmwareId
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets the device name
        /// </summary>
        string DeviceName
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets the timestamp showing the time, when the state has been updated at last
        /// </summary>
        DateTime Timestamp
        {
            get;set;
        }
    }
}
