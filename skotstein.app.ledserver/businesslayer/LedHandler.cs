using skotstein.app.ledserver.exceptions;
using skotstein.app.ledserver.model;
using skotstein.app.ledserver.persistent;
using skotstein.app.ledserver.restlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.businesslayer
{
    /// <summary>
    /// The class <see cref="LedHandler"/> implements the business logic for handling <see cref="Leds"/>s. More specifically,
    /// this class implements methods for obtaining all <see cref="Led"/>s (see <see cref="GetLeds(IDictionary{string, string})"/>) or a single <see cref="Led"/> (see <see cref="GetLed(string)"/>).
    /// Use <see cref="SetColor(string, string)"/> to set the color for a specific <see cref="Led"/>.
    /// </summary>
    public class LedHandler
    {
        //reference to the led storage
        private ILedStorage _ledStorage;

        /// <summary>
        /// Initializes an instance of this class and creates all <see cref="Led"/> resources in the non-persistent <see cref="ILedStorage"/>.
        /// </summary>
        /// <param name="ledStorage">reference to the <see cref="ILedStorage"/>  where all groups are stored</param>
        /// <param name="controllerStorage">reference to the <see cref="IGroupStorage"/> where all controllers are stored</param>
        public LedHandler(ILedStorage ledStorage, IControllerStorage controllerStorage)
        {
            _ledStorage = ledStorage;

            foreach(IControllerDataSet cds in controllerStorage.GetAllControllers())
            {
                for (int i = 0; i < cds.LedCount; i++)
                {
                    _ledStorage.AddLed(cds.Id, i);
                }
            }
        }

        /// <summary>
        /// Returns an instance of <see cref="Leds"/> which contains all <see cref="Led"/>s matching the passed query. The following query parameters can be applied:
        /// <list type="number">
        ///     <item>query parameter <code>id</code>: returns only the <see cref="Led"/> having the passed <see cref="Led.Id"/></item>
        ///     <item>query parameter <code>controllerId</code>: returns all <see cref="Led"/>s which are connected to the <see cref="Controller"/> having the passed <see cref="Controller.Id"/></item>
        ///     <item>query parameter <code>rgb</code>: returns all <see cref="Led"/>s which currently have the passed color (rgb value, case invariant, with or without leading '#', e.g. '#FFAA00')</item>
        ///     <item>query parameter <code>greater</code>: returns all <see cref="Led"/>s whose <see cref="Led.LedNumber"/> is greater than the passed value</item>
        ///     <item>query parameter <code>greater_equals</code>: returns all <see cref="Led"/>s whose <see cref="Led.LedNumber"/> is greater than or equals the passed value</item>
        ///     <item>query parameter <code>less</code>: returns all <see cref="Led"/>s whose <see cref="Led.LedNumber"/> is less than the passed value</item>
        ///     <item>query parameter <code>less_equals</code>: returns all <see cref="Led"/>s whose <see cref="Led.LedNumber"/> is less than or equals the passed value</item>
        ///     <item>query parameter <code>equals</code>: returns all <see cref="Led"/>s whose <see cref="Led.LedNumber"/> is equals the passed value</item>
        /// </list>
        /// If multiple query parameters are applied, the respective <see cref="Led"/> must match all query criterion in order to be in the returned list of <see cref="Leds"/>.
        /// If the query is 'null' or empty (i.e. query.Count == 0), all <see cref="Led"/>s handled by this class are returned.
        /// Unknown query parameters will be ignored.
        /// </summary>
        /// <param name="query">query (can be null or empty)</param>
        /// <returns>instance of <see cref="Leds"/> containing all <see cref="Led"/>s matching the passed query</returns>
        public Leds GetLeds(IDictionary<string,string> query)
        {
            Leds leds = new Leds();

            //return all LEDs, if no query parameters are specified
            if(query == null || query.Count == 0)
            {
                foreach(ILedDataSet lds in _ledStorage.GetAllLeds())
                {
                    Led led = new Led();
                    led.ControllerId = lds.ControllerId;
                    led.LedNumber = lds.LedNumber;
                    led.RgbValue = lds.RgbValue;

                    leds.LedList.Add(led);
                }
            }
            //filter LEDs based on the given query parameters
            else
            {
                foreach(ILedDataSet lds in _ledStorage.GetAllLeds())
                {
                    if (query.ContainsKey("controllerId"))
                    {
                        if(lds.ControllerId != ApiBase.ParseId(query["controllerId"]))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("id"))
                    {
                        if (lds.Id.CompareTo(query["id"]) != 0)
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("rgb"))
                    {
                        if (String.IsNullOrWhiteSpace(lds.RgbValue)||lds.RgbValue.Replace("#", "").ToLower().CompareTo(query["rgb"].Replace("#", "").ToLower()) != 0)
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("greater"))
                    {
                        if (!(lds.LedNumber > ApiBase.ParseInt(query["greater"], BadRequestException.MSG_INVALID_QUERY_PARAMETER_VALUE.Replace("{QUERY}", "greater"))))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("greater_equals"))
                    {
                        if (!(lds.LedNumber >= ApiBase.ParseInt(query["greater_equals"], BadRequestException.MSG_INVALID_QUERY_PARAMETER_VALUE.Replace("{QUERY}", "greater_equals"))))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("less"))
                    {
                        if (!(lds.LedNumber < ApiBase.ParseInt(query["less"], BadRequestException.MSG_INVALID_QUERY_PARAMETER_VALUE.Replace("{QUERY}", "less"))))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("less_equals"))
                    {
                        if (!(lds.LedNumber <= ApiBase.ParseInt(query["less_equals"], BadRequestException.MSG_INVALID_QUERY_PARAMETER_VALUE.Replace("{QUERY}", "less_equals"))))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("equals"))
                    {
                        if (!(lds.LedNumber == ApiBase.ParseInt(query["equals"], BadRequestException.MSG_INVALID_QUERY_PARAMETER_VALUE.Replace("{QUERY}", "equals"))))
                        {
                            continue;
                        }
                    }
                    Led led = new Led();
                    led.ControllerId = lds.ControllerId;
                    led.LedNumber = lds.LedNumber;
                    led.RgbValue = lds.RgbValue;

                    leds.LedList.Add(led);
                }
            }
            return leds;
        }

        /// <summary>
        /// Returns the <see cref="Led"/> having the passed ID.
        /// The method throws a <see cref="ResourceNotFoundException"/> if the requested <see cref="Led"/> does not exist.
        /// The method throws a <see cref="BadRequestException"/> if the passed ID is invalid.
        /// </summary>
        /// <param name="ledId">ID of the requested <see cref="Led"/></param>
        /// <returns>the <see cref="Led"/> having the passed ID</returns>
        public Led GetLed(string ledId)
        {
            string[] split = ledId.Split(':');
            if(split.Length != 2)
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_LED_ID);
            }
            else
            {
                int controllerId = ApiBase.ParseInt(split[0], BadRequestException.MSG_INVALID_LED_ID);
                int ledNumber = ApiBase.ParseInt(split[1], BadRequestException.MSG_INVALID_LED_ID);
                ILedDataSet lds = _ledStorage.GetLed(controllerId, ledNumber);
                if (lds == null)
                {
                    throw new ResourceNotFoundException(ResourceNotFoundException.MSG_LED_NOT_FOUND.Replace("{VALUE}", ledId));
                }
                else
                {
                    Led led = new Led();
                    led.ControllerId = lds.ControllerId;
                    led.LedNumber = lds.LedNumber;
                    led.RgbValue = lds.RgbValue;
                    return led;
                }
            }
        }

        /// <summary>
        /// Sets the color of the <see cref="Led"/> having the specified ID.
        /// The method throws a <see cref="ResourceNotFoundException"/> if the underlying <see cref="Led"/> does not exist.
        /// The method throws a <see cref="BadRequestException"/> if the passed ID is invalid.
        /// </summary>
        /// <param name="ledId">ID of the <see cref="Led"/> whose color should be changed</param>
        /// <param name="rbg">desired RGB value</param>
        public void SetColor(string ledId, string rgb)
        {
            string[] split = ledId.Split(':');
            if (split.Length != 2)
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_LED_ID);
            }
            else
            {
                int controllerId = ApiBase.ParseInt(split[0], BadRequestException.MSG_INVALID_LED_ID);
                int ledNumber = ApiBase.ParseInt(split[1], BadRequestException.MSG_INVALID_LED_ID);
                if (_ledStorage.HasLed(controllerId, ledNumber))
                {
                    _ledStorage.SetColor(controllerId, ledNumber, rgb);
                }
                else
                {
                    throw new ResourceNotFoundException(ResourceNotFoundException.MSG_LED_NOT_FOUND.Replace("{VALUE}", ledId));
                }
            }
        }
    }
}
