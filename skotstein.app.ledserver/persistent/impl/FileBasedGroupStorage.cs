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
using skotstein.app.ledserver.tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    public class FileBasedGroupStorage : IGroupStorage
    {
        private string _homeDirectory;

        private IList<IGroupDataSet> _groups;
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
        /// Gets the directory where all <see cref="IGroupDataSet"/> entites are stored persistently. Use <see cref="Initialize(string)"/> to specify the home directory.
        /// </summary>
        public string HomeDirectory
        {
            get
            {
                return _homeDirectory;
            }
        }

        /// <summary>
        /// Initializes this <see cref="FileBasedGroupStorage"/> and reads all <see cref="GroupDataSetJson"/> from the home directory as well as the current <see cref="IdCounter"/>.
        /// </summary>
        /// <param name="homeDirectory"></param>
        public void Initialize(string homeDirectory)
        {
            _homeDirectory = homeDirectory;

            if (!Directory.Exists(homeDirectory))
            {
                Directory.CreateDirectory(homeDirectory);
                File.WriteAllText(_homeDirectory + "ID.config", 0 + "");
            }

            _groups = new List<IGroupDataSet>();

            //load all groups
            foreach(string file in Directory.EnumerateFiles(_homeDirectory, "*.json"))
            {
                string json = File.ReadAllText(file);
                IGroupDataSet gds = JsonSerializer.DeserializeJson<GroupDataSetJson>(json);
                if(gds != null)
                {
                    _groups.Add(gds);
                }
            }

            //load ID counter
            string id = File.ReadAllText(_homeDirectory + "ID.config");
            _idCounter = Int32.Parse(id);
        }

        /// <summary>
        /// Saves the passed <see cref="IGroupDataSet"/> to disk.
        /// </summary>
        /// <param name="data"></param>
        private void SaveToDisk(IGroupDataSet data)
        {
            File.WriteAllText(_homeDirectory + data.Id + ".json", JsonSerializer.SerializeJson((GroupDataSetJson)data));
        }

        /// <summary>
        /// Deletes the <see cref="IGroupDataSet"/> from disk.
        /// </summary>
        /// <param name="data"></param>
        private void RemoveFromDisk(IGroupDataSet data)
        {
            File.Delete(_homeDirectory + data.Id + ".json");
        }

        public IList<IGroupDataSet> GetAllGroups()
        {
            IList<IGroupDataSet> copy = new List<IGroupDataSet>();
            foreach(IGroupDataSet gds in _groups)
            {
                copy.Add(gds);
            }
            return copy;
        }

        public IGroupDataSet GetGroup(int id)
        {
            foreach(IGroupDataSet gds in _groups)
            {
                if(gds.Id == id)
                {
                    return gds;
                }
            }
            return null;
        }

        public bool HasGroup(int id)
        {
            return GetGroup(id) != null;
        }

        public void AddGroup(int id, string name)
        {
            if (HasGroup(id))
            {
                throw new Exception("Group already existing");
            }
            else
            {
                IGroupDataSet gds = new GroupDataSetJson() { Id = id, Name = name };
                _groups.Add(gds);
                SaveToDisk(gds);
            }
        }
        public void SetName(int id, string name)
        {
            IGroupDataSet gds = GetGroup(id);
            if (gds == null)
            {
                throw new Exception("Group not found");
            }
            else
            {
                gds.Name = name;
                SaveToDisk(gds);
            }
        }

        public void DeleteGroup(int id)
        {
            for(int i = 0; i < _groups.Count; i++)
            {
                if(_groups[i].Id == id)
                {
                    RemoveFromDisk(_groups[i]);
                    _groups.RemoveAt(i);
                    return;
                }
            }
        }

        public void AddLed(int id, string ledId)
        {
            IGroupDataSet gds = GetGroup(id);
            if (gds == null)
            {
                throw new Exception("Group not found");
            }
            else
            {
                foreach(string lId in gds.Leds)
                {
                    if (ledId.CompareTo(lId) == 0)
                    {
                        throw new Exception("LED with ID: " + ledId + " is already member of the group with ID: " + id);
                    }
                }
                gds.Leds.Add(ledId);
                SaveToDisk(gds);
            }
        }

        public void SetLeds(int id, IList<string> ledIds)
        {
            IGroupDataSet gds = GetGroup(id);
            if (gds == null)
            {
                throw new Exception("Group not found");
            }
            else
            {
                gds.Leds.Clear();
                foreach(string ledId in ledIds)
                {
                    gds.Leds.Add(ledId);
                }
                SaveToDisk(gds);
            }
        }

        public void ClearLeds(int id)
        {
            IGroupDataSet gds = GetGroup(id);
            if (gds == null)
            {
                throw new Exception("Group not found");
            }
            else
            {
                gds.Leds.Clear();
                SaveToDisk(gds);
            }
        }

        public void RemoveLed(int id, string ledId)
        {
            IGroupDataSet gds = GetGroup(id);
            if (gds == null)
            {
                throw new Exception("Group not found");
            }
            else
            {
                for(int i = 0; i < gds.Leds.Count; i++)
                {
                    if (gds.Leds[i].CompareTo(ledId) == 0)
                    {
                        gds.Leds.RemoveAt(i);
                        SaveToDisk(gds);
                        return;
                    }
                }
            }
        }

        public void CleanLeds(int controllerId, int offset)
        {
            foreach(IGroupDataSet gds in GetAllGroups())
            {
                IList<string> toBeRemoved = new List<string>();
                for(int i = 0; i < gds.Leds.Count; i++)
                {
                    if (gds.Leds[i].Split(':')[0].CompareTo(controllerId+"") == 0)
                    {
                        int number = Int32.Parse(gds.Leds[i].Split(':')[1]);
                        if(number >= offset)
                        {
                            toBeRemoved.Add(controllerId + ":" + number);
                        }
                    }
                    foreach(string ledId in toBeRemoved)
                    {
                        gds.Leds.Remove(ledId);
                    }
                }
            }
        }


    }
}
