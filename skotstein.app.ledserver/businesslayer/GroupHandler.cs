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
    /// The class <see cref="GroupHandler"/> implements the business logic for handling <see cref="Group"/>s. More specifically, this class implements methods for obtaining all <see cref="Group"/>s (see <see cref="GetGroupLeds(int)"/>) or a single <see cref="Group"/> (see <see cref="GetGroup(int)"/>), for
    /// creating (see <see cref="CreateGroup(string)"/> or changing (see <see cref="ChangeName(int, string)"/>) and deleting (see <see cref="DeleteGroup(int)"/> a <see cref="Group"/>.
    /// As well as querying the <see cref="Leds"/> being member of a <see cref="Group"/> (see <see cref="GetGroupLeds(int)"/>). Use <see cref="SetLedsOfGroup(int, GroupLeds)"/> to modify the list of <see cref="Led"/>s of a specific <see cref="Group"/>.
    /// Use <see cref="SetColorOfGroup(int, string)"/> to set the LED color for all <see cref="Led"/>s of a specific <see cref="Group"/> by issuing only one method call.
    /// </summary>
    public class GroupHandler
    {
        //reference to the group storage where all groups are stored persistently
        private IGroupStorage _groupStorage;

        //a reference to the LED storage is required for querying the LEDs of a group 
        private ILedStorage _ledStorage;

        /// <summary>
        /// Initializes an instance of this class
        /// </summary>
        /// <param name="groupStorage">reference to the <see cref="IGroupStorage"/> where all groups are stored persistently</param>
        /// <param name="ledStorage">reference to the <see cref="ILedStorage"/>  where all groups are stored</param>
        public GroupHandler(IGroupStorage groupStorage, ILedStorage ledStorage)
        {
            _groupStorage = groupStorage;
            _ledStorage = ledStorage;
        }

        /// <summary>
        /// Returns an instance of <see cref="Groups"/> which contains all <see cref="Group"/>s matching the passed query. The following query parameters can be applied:
        ///<list type="number">
        ///     <item>query parameter <code>id</code>: returns only the <see cref="Group"/> having the passed <see cref="Group.Id"/></item>
        ///     <item>query parameter <code>name</code>: returns all <see cref="Grpup"/>s whose <see cref="Group.Name"/> contains the passed value (case invariant).</item>
        ///     <item>query parameter <code>ledId</code>: returns all <see cref="Group"/>s where the <see cref="Led"/> having the passed ID is a member.</item>
        /// </list>
        /// If multiple query parameters are applied, the respective <see cref="Group"/> must match all query criterion in order to be in the returned list of <see cref="Groups"/>.
        /// If the query is 'null' or empty (i.e. query.Count == 0), all <see cref="Group"/>s handled by this class are returned.
        /// Unknown query parameters will be ignored.
        /// </summary>
        /// <param name="query">query (can be null or empty)</param>
        /// <returns>instance of <see cref="Groups"/> containing all <see cref="Group"/>s matching the passed query</returns>
        public Groups GetGroups(IDictionary<string, string> query)
        {
            Groups groups = new Groups();

            //return all groups, if no query parameters are specified
            if(query == null || query.Count == 0)
            {
                foreach (IGroupDataSet gds in _groupStorage.GetAllGroups())
                {
                    //create group entity object
                    Group group = new Group();
                    group.Id = gds.Id;
                    group.Name = gds.Name;

                    groups.GroupList.Add(group);
                }
            }
            //filter groups based on the given query parameters
            else
            {
                foreach(IGroupDataSet gds in _groupStorage.GetAllGroups())
                {
                    if (query.ContainsKey("id"))
                    {
                        if(gds.Id != ApiBase.ParseId(query["id"]))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("name"))
                    {
                        if (String.IsNullOrWhiteSpace(gds.Name) || !gds.Name.ToLower().Contains(query["name"].ToLower()))
                        {
                            continue;
                        }
                    }
                    if (query.ContainsKey("ledId"))
                    {
                        if (!gds.Leds.Contains(query["ledId"].ToLower()))
                        {
                            continue;
                        }
                    }

                    //create group entity object
                    Group group = new Group();
                    group.Id = gds.Id;
                    group.Name = gds.Name;

                    groups.GroupList.Add(group);
                }

            }
            return groups;
        }

        /// <summary>
        /// Creates a new <see cref="Group"/> having the passed <see cref="Group.Name"/>.
        /// The <see cref="Group.Id"/> of the created group is returned.
        /// </summary>
        /// <param name="name">name of the group</param>
        /// <returns>the <see cref="Group.Id"/> of the created group</returns>
        public int CreateGroup(string name)
        {
            int id = ((FileBasedGroupStorage)_groupStorage).IdCounter++;
            _groupStorage.AddGroup(id, name);
            return id;
        }

        /// <summary>
        /// Changes the name of the <see cref="Group"/> having the passed <see cref="Group.Id"/>.
        /// The methods throws an <see cref="ResourceNotFoundException"/> if the specified <see cref="Group"/> does not exist.
        /// </summary>
        /// <param name="id"><see cref="Group.Id"/> of the group</param>
        /// <param name="name">new <see cref="Group.Name"/></param>
        public void ChangeName(int id, string name)
        {
            if (_groupStorage.HasGroup(id))
            {
                _groupStorage.SetName(id, name);
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_GROUP_NOT_FOUND.Replace("{VALUE}",id+""));
            }
        }

        /// <summary>
        /// Returns the <see cref="Group"/> having the passed <see cref="Group.Id"/>.
        /// The methods throws a <see cref="ResourceNotFoundException"/> if the requested <see cref="Group"/> does not exist.
        /// </summary>
        /// <param name="id"><see cref="Group.Id"/></param>
        /// <returns>the <see cref="Group"/> having the passed ID</returns>
        public Group GetGroup(int id)
        {
            IGroupDataSet gds = _groupStorage.GetGroup(id);
            if(gds != null)
            {
                Group group = new Group();
                group.Id = gds.Id;
                group.Name = gds.Name;
                return group;
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_GROUP_NOT_FOUND.Replace("{VALUE}", id + ""));
            }
        }

        /// <summary>
        /// Removes the <see cref="Group"/> having the specified <see cref="Group.Id"/>.
        /// </summary>
        /// <param name="id"><see cref="Group.Id"/> of the <see cref="Group"/> which will be removed</param>
        public void DeleteGroup(int id)
        {
            _groupStorage.DeleteGroup(id);
        }

        /// <summary>
        /// Returns the list of <see cref="Leds"/> which are member of the <see cref="Group"/> having the specified <see cref="Group.Id"/>.
        /// The methods throws a <see cref="ResourceNotFoundException"/> if the underlying <see cref="Group"/> does not exist.
        /// </summary>
        /// <param name="id"><see cref="Group.Id"/></param>
        /// <returns><see cref="GroupLeds"/> containing the <see cref="Leds"/> being member of the <see cref="Group"/></returns>
        public GroupLeds GetGroupLeds(int id)
        {
            IGroupDataSet gds = _groupStorage.GetGroup(id);
            if(gds != null)
            {
                GroupLeds gLeds = new GroupLeds();
                gLeds.GroupId = gds.Id;
                foreach(string ledId in gds.Leds)
                {
                    ILedDataSet lds = _ledStorage.GetLed(ledId);
                    if (lds != null)
                    {
                        Led led = new Led();
                        led.ControllerId = lds.ControllerId;
                        led.LedNumber = lds.LedNumber;
                        led.RgbValue = lds.RgbValue;
                        gLeds.Leds.Add(led);
                    }
                }
                return gLeds;
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_GROUP_NOT_FOUND.Replace("{VALUE}", id + ""));
            }
        }

        /// <summary>
        /// Overwrites the list of <see cref="Leds"/> of the <see cref="Group"/> having the passed ID with the passed LED list.
        /// Note that the specified LEDs must exist, otherwise non-existing LEDs will not be added.
        /// The methods throws a <see cref="ResourceNotFoundException"/> if the underlying <see cref="Group"/> does not exist.
        /// </summary>
        /// <param name="id"><see cref="Group.Id"/></param>
        /// <param name="groupLeds">list with LEDs</param>
        public void SetLedsOfGroup(int id, GroupLeds groupLeds)
        {
            if (_groupStorage.HasGroup(id))
            {
                IList<string> ledIds = new List<string>();
                foreach(Led led in groupLeds.Leds)
                {
                    string ledId = led.ControllerId + ":" + led.LedNumber;
                    if (_ledStorage.HasLed(ledId))
                    {
                        ledIds.Add(ledId);
                    }
                }
                _groupStorage.SetLeds(id, ledIds);
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_GROUP_NOT_FOUND.Replace("{VALUE}", id + ""));
            }
        }

        /*
        public void RemoveLedsFromGroup(int id, IList<string> ledIds)
        {
            if (_groupStorage.HasGroup(id))
            {
                foreach (string ledId in ledIds)
                {
                    _groupStorage.RemoveLed(id, ledId);
                } 
            }
            else
            {
                throw new ResourceNotFoundException("The group having the ID: " + id + " does not exist");
            }
        }
        */

        /// <summary>
        /// Sets the color of all <see cref="Leds"/> being member of the <see cref="Group"/> having the specified <see cref="Group.Id"/>.
        /// The methods throws a <see cref="ResourceNotFoundException"/> if the underlying <see cref="Group"/> does not exist.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rgbValue"></param>
        public void SetColorOfGroup(int id, string rgbValue)
        {
            IGroupDataSet gds = _groupStorage.GetGroup(id);
            if(gds != null)
            {
                foreach (string ledId in gds.Leds)
                {
                    if (_ledStorage.HasLed(ledId))
                    {
                        _ledStorage.SetColor(ledId, rgbValue);
                    }
                }
            }
            else
            {
                throw new ResourceNotFoundException(ResourceNotFoundException.MSG_GROUP_NOT_FOUND.Replace("{VALUE}", id + ""));
            }
        }


    }
}
