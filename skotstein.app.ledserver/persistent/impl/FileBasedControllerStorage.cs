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
    /// The class is an <see cref="IControllerStorage"/> implementation which stores <see cref="IControllerDataSet"/> entities as JSON Files on the local disk (i.e. the <see cref="IControllerDataSet"/> entities are handled as <see cref="ControllerDataSetJson"/> objects internally).
    /// By calling <see cref="Initialize(string)"/> this storage is initialized, which means that the home directory is specified and all <see cref="ControllerDataSetJson"/> entities which have been store persistently are loaded. This class maintains an <see cref="IdCounter"/> which can be used to generate a unique identifier for each <see cref="IControllerDataSet"/> (see <see cref="IControllerDataSet.Id"/>).
    /// This <see cref="IdCounter"/> is stored persistently in the home directory as well. Use <see cref="Guid"/> to generate unique UUIDs (see <see cref="IControllerDataSet.UuId"/>).
    /// </summary>
    public class FileBasedControllerStorage : IControllerStorage
    {
        private string _homeDirectory;

        private IList<IControllerDataSet> _controllers;
        private int _idCounter;

        /// <summary>
        /// Gets or sets the current ID Counter. By setting the ID counter, its value is stored persistently in the home directory.
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
        /// Gets the home directory where all <see cref="IControllerDataSet"/> entities are stored persistently. Use <see cref="Initialize(string)"/> to specify the home directory.
        /// </summary>
        public string HomeDirectory
        {
            get
            {
                return _homeDirectory;
            }
        }

        /// <summary>
        /// Initializes this <see cref="FileBasedControllerStorage"/> and reads all <see cref="ControllerDataSetJson"/> from the home directory as well as the current <see cref="IdCounter"/>.
        /// </summary>
        /// <param name="homeDirectory">home directory path</param>
        public void Initialize(string homeDirectory)
        {
            _homeDirectory = homeDirectory;
            _controllers = new List<IControllerDataSet>();

            //load all controllers:
            foreach(string file in Directory.EnumerateFiles(_homeDirectory,"*.json"))
            {
                string json = File.ReadAllText(file);
                ControllerDataSetJson cds = JsonSerializer.DeserializeJson<ControllerDataSetJson>(json);
                if(cds != null)
                {
                    _controllers.Add(cds);
                }
            }

            //load ID counter
            string id = File.ReadAllText(_homeDirectory + "ID.config");
            _idCounter = Int32.Parse(id);
        }

        /// <summary>
        /// Saves the passed <see cref="ControllerDataSetJson"/> to disk.
        /// </summary>
        /// <param name="data"></param>
        private void SaveToDisk(IControllerDataSet data)
        {
            File.WriteAllText(_homeDirectory + data.Id + ".json", JsonSerializer.SerializeJson((ControllerDataSetJson)data));
        }

        /// <summary>
        /// Deletes the <see cref="ControllerDataSetJson"/> having the passed ID from disk
        /// </summary>
        /// <param name="id"></param>
        private void RemoveFromDisk(int id)
        {
            File.Delete(_homeDirectory + id + ".json");
        }

        public IList<IControllerDataSet> GetAllControllers()
        {
            IList<IControllerDataSet> copy = new List<IControllerDataSet>();
            foreach(IControllerDataSet cds in _controllers)
            {
                copy.Add(cds);
            }
            return copy;
        }

        public IControllerDataSet GetControllerById(int id)
        {
            foreach(IControllerDataSet cds in _controllers)
            {
                if(cds.Id == id)
                {
                    return cds;
                }
            }
            return null;
        }

        public IControllerDataSet GetControllerByUuId(string uuid)
        {
            foreach (IControllerDataSet cds in _controllers)
            {
                if (cds.UuId.CompareTo(uuid)==0)
                {
                    return cds;
                }
            }
            return null;
        }

        public bool HasControllerById(int id)
        {
            return GetControllerById(id) != null;
        }

        public bool HasControllerByUuid(string uudId)
        {
            return GetControllerByUuId(uudId) != null;
        }

        public void CreateController(IControllerDataSet controllerDataSet)
        {
            if(String.IsNullOrWhiteSpace(controllerDataSet.UuId))
            {
                throw new Exception("UUID is not set");
            }
            if (HasControllerById(controllerDataSet.Id) || HasControllerByUuid(controllerDataSet.UuId))
            {
                throw new Exception("Controller already existing");
            }
            else
            {
                _controllers.Add(controllerDataSet);
                SaveToDisk(controllerDataSet);
            }
        }

        public void DeleteController(int id)
        {
            for(int i = 0; i < _controllers.Count; i++)
            {
                if(_controllers[i].Id == id)
                {
                    _controllers.RemoveAt(i);
                    RemoveFromDisk(id);
                    return;
                }
            }
        }

        public void DeleteController(IControllerDataSet controllerDataSet)
        {
            DeleteController(controllerDataSet.Id);
        }

        public void SetName(int id, string name)
        {
            IControllerDataSet cds = GetControllerById(id);
            if(cds == null)
            {
                throw new Exception("Unknown Controller");
            }
            else
            {
                cds.FriendlyName = name;
                SaveToDisk(cds);
            }
        }

        public void SetLedCount(int id, int count)
        {
            IControllerDataSet cds = GetControllerById(id);
            if(cds == null)
            {
                throw new Exception("Unknown Controller");
            }
            else
            {
                cds.LedCount = count;
                SaveToDisk(cds);
            }
        }

        public void SetNameAndLedCount(int id, string name, int count)
        {
            IControllerDataSet cds = GetControllerById(id);
            if(cds == null)
            {
                throw new Exception("Unknown Controller");
            }
            else
            {
                cds.LedCount = count;
                cds.FriendlyName = name;
                SaveToDisk(cds);
            }
        }

        public void SetFirmware(string uuId, int minVersion, int majVersion, string firmwareId, string deviceName)
        {
            IControllerDataSet cds = GetControllerByUuId(uuId);
            if (cds == null)
            {
                throw new Exception("Unknown Controller");
            }
            else
            {
                cds.MinorVersion = minVersion;
                cds.MajorVersion = majVersion;
                cds.FirmwareId = firmwareId;
                cds.DeviceName = deviceName;
                cds.Timestamp = DateTime.UtcNow;
            }
        }


    }
}
