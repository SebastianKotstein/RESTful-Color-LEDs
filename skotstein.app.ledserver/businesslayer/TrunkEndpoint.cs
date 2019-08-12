using skotstein.app.ledserver.exceptions;
using skotstein.app.ledserver.persistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.businesslayer
{
    public class TrunkEndpoint
    {
        private IControllerStorage _controllerStorage;
        private ILedStorage _ledStorage;

        public TrunkEndpoint(IControllerStorage controllerStorage, ILedStorage ledStorage)
        {
            _controllerStorage = controllerStorage;
            _ledStorage = ledStorage;
        }

        public byte[] ProcessMessage(string msg, string authorization)
        {
            if (!_controllerStorage.HasControllerByUuid(authorization))
            {
                throw new UnauthorizedAccessException();
            }
            else
            {
                string[] split = msg.Split('#');
                if(split.Length != 6)
                {
                    throw new BadRequestException("Invalid message format: Unexpected count of parameter");
                }
                else
                {
                    if (split[0].CompareTo("HELLO") != 0)
                    {
                        throw new BadRequestException("Invalid message format: Invalid command");
                    }
                    if (split[1].CompareTo(authorization) != 0)
                    {
                        throw new BadRequestException("Invalid message format: Authorization mismatch");
                    }
                    string uuId = authorization;
                    string deviceName = split[2];
                    string firmwareId = split[3];
                    int minorVersion;
                    int majorVersion;

                    if(!Int32.TryParse(split[4],out minorVersion) || !Int32.TryParse(split[5], out majorVersion))
                    {
                        throw new BadRequestException("Invalid message format: Minor/Major version not convertible");
                    }
                    _controllerStorage.SetFirmware(uuId, minorVersion, majorVersion, firmwareId, deviceName);

                    //return preferred color

                    //return Encoding.ASCII.GetBytes("_NOPE");
                    int id = _controllerStorage.GetControllerByUuId(uuId).Id;
                    IList<ILedDataSet> leds = _ledStorage.GetAllLedsOfController(id);

                    //prepare the response message
                    byte[] responseMsg = new byte[(leds.Count * 4)];

                    for(int i = 0; i <leds.Count; i++)
                    {
                        byte[] val = ConvertRgbValue(leds[i].RgbValue);
                        responseMsg[(i * 4)] = (byte)i;
                        responseMsg[(i * 4) + 1] = val[0];
                        responseMsg[(i * 4) + 2] = val[1];
                        responseMsg[(i * 4) + 3] = val[2];
                    }
                    return responseMsg;
                }
            }
        }

        private byte[] ConvertRgbValue (string rgbValue)
        {
            byte[] val = new byte[3] { 0, 0, 0 }; //initialize with 0x000000

            if (rgbValue.StartsWith("#"))
            {
                rgbValue = rgbValue.TrimStart('#');
            }
            if(rgbValue.Length != 6)
            {
                return new byte[3] { 255, 255, 255 }; //return default 0xFFFFFF
            }
            for(int i = 0; i < 6; i++)
            {
                byte value = 0;
                switch (rgbValue.ElementAt(i))
                {
                    case '0':
                        value = 0;
                        break;
                    case '1':
                        value = 1;
                        break;
                    case '2':
                        value = 2;
                        break;
                    case '3':
                        value = 3;
                        break;
                    case '4':
                        value = 4;
                        break;
                    case '5':
                        value = 5;
                        break;
                    case '6':
                        value = 6;
                        break;
                    case '7':
                        value = 7;
                        break;
                    case '8':
                        value = 8;
                        break;
                    case '9':
                        value = 9;
                        break;
                    case 'A':
                        value = 10;
                        break;
                    case 'B':
                        value = 11;
                        break;
                    case 'C':
                        value = 12;
                        break;
                    case 'D':
                        value = 13;
                        break;
                    case 'E':
                        value = 14;
                        break;
                    case 'F':
                        value = 15;
                        break;
                    case 'a':
                        value = 10;
                        break;
                    case 'b':
                        value = 11;
                        break;
                    case 'c':
                        value = 12;
                        break;
                    case 'd':
                        value = 13;
                        break;
                    case 'e':
                        value = 14;
                        break;
                    case 'f':
                        value = 15;
                        break;
                    default:
                        return new byte[3] { 255, 255, 255 }; //return default 0xFFFFFF
                }
                if (i % 2 == 1)
                {
                    val[i / 2] += value;
                }
                else
                {
                    val[i / 2] += (byte)(value * 16);
                }
            }
            return val;
        }
    }
}
