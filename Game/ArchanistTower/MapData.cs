using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledLib;

namespace ArchanistTower
{
    public struct MapLink
    {
        public Map nextMap;
        public int nextMapsStartPoint;

        public MapLink(Map map, int startPoint)
        {
            nextMap = map;
            nextMapsStartPoint = startPoint;
        }
    };

    public class MapData
    {
        Map map;
        List<MapLink> mapLinkList = new List<MapLink>();

        public MapData(string mapLocation)
        {
            map = Globals.content.Load<Map>(mapLocation);
        }

        public void addMapLink(Map m, int startPoint)
        {
            mapLinkList.Add(new MapLink(m, startPoint));
        }

        public Map getMap()
        {
            return map;
        }

        public MapLink getNextMap(int mapIndex)
        {
            return mapLinkList[mapIndex];
        }
    }
}
