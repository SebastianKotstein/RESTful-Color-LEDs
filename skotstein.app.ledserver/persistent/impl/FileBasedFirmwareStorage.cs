using skotstein.app.ledserver.tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// This class is an <see cref="IFirmwareStorage"/> implementation which stores <see cref="IFirmwareDataSet"/> entities as JSON file on the local disk (i.e. the <see cref="IFirmwareDataSet"/> entities are handled as <see cref="FirmwareDataSetJson"/> objects internally).
    /// By calling <see cref="Initialize(string)"/> this storage is initialized, which means that the home directory is specified and all <see cref="FirmwareDataSetJson"/> entities which have been store persistently are loaded. This class maintains an <see cref="IdCounter"/> which can be used to generate a unique identifier for each <see cref="IFirmwareDataSet"/> (see <see cref="IFirmwareDataSet.Id"/>).
    /// This <see cref="IdCounter"/> is stored persistently in the home directory as well.
    /// </summary>
    public class FileBasedFirmwareStorage : IFirmwareStorage
    {
        private string _homeDirectory;

        private IList<IFirmwareDataSet> _firmwares;
        private int _idCounter;

        /// <summary>
        /// Gets or sets the current ID counter. By setting the ID counter, its value is stored persistently in the home directory.
        /// </summary>
        public int IdCounter
        {
            get
            {
                return _idCounter;
            }

            set
            {
                _idCounter = value;
                File.WriteAllText(_homeDirectory + "ID.config", _idCounter + "");
            }
        }


        /// <summary>
        /// Gets the home directory where all <see cref="IFirmwareDataSet"/> entities are stored persistently. Use <see cref="Initialize(string)"/> to specify the home directory.
        /// </summary>
        public string HomeDirectory
        {
            get
            {
                return _homeDirectory;
            }
        }

        /// <summary>
        /// Initializes this <see cref="FileBasedFirmwareStorage"/> and reads all <see cref="FirmwareDataSetJson"/> from the home directory as well as the current <see cref="IdCounter"/>.
        /// </summary>
        /// <param name="homeDirectory">home directory path</param>
        public void Initialize(string homeDirectory)
        {
            _homeDirectory = homeDirectory;
            _firmwares = new List<IFirmwareDataSet>();

            //load all firmwares
            foreach(string file in Directory.EnumerateFiles(_homeDirectory, "*.json"))
            {
                string json = File.ReadAllText(file);
                FirmwareDataSetJson fds = JsonSerializer.DeserializeJson<FirmwareDataSetJson>(json);
                if (fds != null)
                {
                    _firmwares.Add(fds);
                }
            }

            //load ID counter
            string id = File.ReadAllText(_homeDirectory + "ID.config");
            _idCounter = Int32.Parse(id);
        }

        /// <summary>
        /// Saves the passed <see cref="IFirmwareDataSet"/> to disk.
        /// </summary>
        /// <param name="data"></param>
        private void SaveToDisk(IFirmwareDataSet data)
        {
            File.WriteAllText(_homeDirectory + data.Id + ".json", JsonSerializer.SerializeJson((FirmwareDataSetJson)data));
        }

        /// <summary>
        /// Saves the passed firmware file in the form of the passed raw data to disk
        /// </summary>
        /// <param name="rawData">raw data of the firmware file</param>
        /// <param name="fileType">file type extension</param>
        /// <param name="id">the id of the <see cref="IFirmwareDataSet"/>, this firmware belongs to</param>
        private void SaveToDisk(string rawData, string fileType, string id)
        {
            File.WriteAllText(_homeDirectory + id + "." + fileType, rawData);
        }

        /// <summary>
        /// Deletes the <see cref="IFirmwareDataSet"/> as well as its firmware file having the passed ID from disk
        /// </summary>
        /// <param name="id"></param>
        private void RemoveFromDisk(IFirmwareDataSet data)
        {
            File.Delete(_homeDirectory + data.Id + ".json");
            File.Delete(_homeDirectory + data.Id + "." + data.FirmwareFileExtension);
        }

        public IList<IFirmwareDataSet> GetAllFirmwares()
        {
            IList<IFirmwareDataSet> copy = new List<IFirmwareDataSet>();
            foreach(IFirmwareDataSet fds in _firmwares)
            {
                copy.Add(fds);
            }
            return copy;
        }

        public IFirmwareDataSet GetFirmware(string id)
        {
            foreach (IFirmwareDataSet fds in _firmwares)
            {
                if (fds.Id.CompareTo(id) == 0)
                {
                    return fds;
                }
            }
            return null;
        }

        public string GetRawData(string id)
        {
            if (HasFirmware(id))
            {
                return File.ReadAllText(_homeDirectory + id + "." + GetFirmware(id).FirmwareFileExtension);
            }
            else
            {
                return null;
            }
        }

        public void SetRawData(string id, string rawData)
        {
            IFirmwareDataSet fds = GetFirmware(id);
            if (fds == null)
            {
                throw new Exception("Unknown Firmware");
            }
            SaveToDisk(rawData, fds.FirmwareFileExtension, id);
        }

        public bool HasFirmware(string id)
        {
            return GetFirmware(id) != null;
        }

        public void CreateFirmware(IFirmwareDataSet firmwareDataSet, string rawData)
        {
            if (String.IsNullOrWhiteSpace(firmwareDataSet.Id))
            {
                throw new Exception("ID is not set");
            }
            if (HasFirmware(firmwareDataSet.Id))
            {
                throw new Exception("Firmware already existing");
            }
            else
            {
                _firmwares.Add(firmwareDataSet);
                SaveToDisk(firmwareDataSet);
                SetRawData(firmwareDataSet.Id, rawData);
            }
        }

        public void DeleteFirmware(string id)
        {
            for(int i = 0; i < _firmwares.Count; i++)
            {
                if(_firmwares[i].Id == id)
                {
                    RemoveFromDisk(_firmwares[i]);
                    _firmwares.RemoveAt(i);
                    return;
                }
            }
        }

        public void DeleteFirmware(IFirmwareDataSet firmwareDataSet)
        {
            DeleteFirmware(firmwareDataSet.Id);
        }

        public void ChangeMetaInformation(string id, string deviceName)
        {
            IFirmwareDataSet fds = GetFirmware(id);
            if(fds == null)
            {
                throw new Exception("Unknown Firmware");
            }
            fds.DeviceName = deviceName;
        }

        public void ChangeMetaInformation(string id, int minorVersion, int majorVersion)
        {
            IFirmwareDataSet fds = GetFirmware(id);
            if (fds == null)
            {
                throw new Exception("Unknown Firmware");
            }
            fds.MinorVersion = minorVersion;
            fds.MajorVersion = majorVersion;
        }

        public void ChangeMetaInformation(string id, string deviceName, int minorVersion, int majorVersion)
        {
            IFirmwareDataSet fds = GetFirmware(id);
            if (fds == null)
            {
                throw new Exception("Unknown Firmware");
            }
            fds.DeviceName = deviceName;
            fds.MinorVersion = minorVersion;
            fds.MajorVersion = majorVersion;
        }

    
    }
}
