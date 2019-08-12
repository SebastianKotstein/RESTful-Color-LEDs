using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// An implementation of <see cref="ILedDataSet"/> includes the data of a single LED. By using an implementation of <see cref="ILedStorage"/>, an instance of <see cref="ILedDataSet"/> can
    /// be stored persistently.
    /// </summary>
    public interface ILedDataSet
    {
        /// <summary>
        /// Returns the full ID of the LED which has the format CONTROLLER_ID:LED_NUMBER.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets or sets the number of the LED
        /// </summary>
        int LedNumber { get; set; }

        /// <summary>
        /// Gets or sets the ID of the <see cref="IControllerDataSet"/> this LED belongs to.
        /// </summary>
        int ControllerId { get; set; }

        /// <summary>
        /// Gets or sets the RGB value of this LED
        /// </summary>
        string RgbValue { get; set; }


        /// <summary>
        /// Returns true if this LED is handled by the <see cref="IControllerDataSet"/> having the passed ID, else false.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <returns>true or false</returns>
        bool IsLedOfController(int controllerId);

        /// <summary>
        /// Returns true if the LED has the passed ID, else false.
        /// </summary>
        /// <param name="id">ID of the LED having the format CONTROLLER_ID:LED_ID</param>
        /// <returns>true or false</returns>
        bool HasId(string id);

        /// <summary>
        /// Returns true if the LED belongs to the <see cref="IControllerDataSet"/> having the passed ID and if the LED has the passed number.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <param name="number">number of the LED</param>
        /// <returns>true or false</returns>
        bool HasId(int controllerId, int number);

    }
}
