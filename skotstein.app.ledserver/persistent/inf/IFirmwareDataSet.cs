using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// An implementation of <see cref="IFirmwareDataSet"/> includes the meta data of a firmware file. By using an implementation of <see cref="IFirmwareStorage"/>, an instance of <see cref="IFirmwareDataSet"/> can be stored persistently.
    /// </summary>
    public interface IFirmwareDataSet
    {
        /// <summary>
        /// Gets or sets the identifier of the firmware.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the minor version of the firmware.
        /// </summary>
        int MinorVersion { get; set; }

        /// <summary>
        /// Gets or sets the major version of the firmware.
        /// </summary>
        int MajorVersion { get; set; }

        /// <summary>
        /// Gets or sets the name of the device which compatible to this firmware.
        /// </summary>
        string DeviceName { get; set; }
        
        /// <summary>
        /// Gets or sets the file extension of the firmware file.
        /// </summary>
        string FirmwareFileExtension { get; set; }
    
    }
}
