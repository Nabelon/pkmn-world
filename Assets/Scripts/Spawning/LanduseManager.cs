using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
namespace LanduseManager
{

    public class Point
    {
        public int X;
        public int Y;
        public Point() { X = 0; Y = 0; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public class PointF
    {
        public float X;
        public float Y;
        public PointF() { X = 0; Y = 0; }
        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
    public class Tuple<T, U>
    {
        public T Item1 { get; private set; }
        public U Item2 { get; private set; }

        public Tuple(T item1, U item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    public static class Tuple
    {
        public static Tuple<T, U> Create<T, U>(T item1, U item2)
        {
            return new Tuple<T, U>(item1, item2);
        }
    }
    //Stores boundaries of a rectangle and its landuse
    class Item
    {
        static Dictionary<string, int> landuseToIntDic = new Dictionary<string, int>();
        static Dictionary<int, string> intToLanduseDic = new Dictionary<int, string>();
        static int intLanduseCount = 0;
        public float left, top, right, bottom;
        public int landuseId;
        public Item(float l, float t, float r, float b, int lId)
        {
            left = l; top = t; right = r; bottom = b; landuseId = lId;
        }
        public Item(float l, float t, float r, float b, string landuse)
        {
            left = l; top = t; right = r; bottom = b; landuseId = landuseToInt(landuse);
        }
        public static string getLanduse(int landuseId)
        {
            return intToLanduseDic[landuseId];
        }
        private int landuseToInt(string landuse)
        {
            if (landuseToIntDic.ContainsKey(landuse)) { return landuseToIntDic[landuse]; }
            intLanduseCount++;
            landuseToIntDic.Add(landuse, intLanduseCount);
            intToLanduseDic.Add(intLanduseCount, landuse);
            return intLanduseCount;
        }
    }
    class Quadtree
    {
        Quadtree nw, ne, sw, se;        //childs
        float cx, cy;                   //central coords
        List<int> landuses;             //
        public Quadtree(List<Item> items, int depth, float leftB, float topB, float rightB, float bottomB)
        {
            depth--;
            landuses = new List<int>();
            if (depth == 0)     // If we've reached the maximum depth then insert all items into this quadrant.
            {
                foreach (Item i in items)
                {
                    if (!landuses.Contains(i.landuseId))
                    {
                        landuses.Add(i.landuseId);
                    }
                }
                return;
            }
            cx = Convert.ToSingle((leftB + rightB) * 0.5);      // Find this quadrant's centre.
            cy = Convert.ToSingle((topB + bottomB) * 0.5);

            List<Item> nw_items = new List<Item>();
            List<Item> sw_items = new List<Item>();
            List<Item> se_items = new List<Item>();
            List<Item> ne_items = new List<Item>();
            foreach (Item item in items)    //give items to children
            {
                //check if node is in item
                if (item.left < leftB && item.top > topB && item.right > rightB && item.bottom < bottomB)
                {
                    if (!landuses.Contains(item.landuseId)) { landuses.Add(item.landuseId); }
                    continue;
                }
                //Which side to insert item?
                bool in_nw = (item.left <= cx && item.top >= cy);
                bool in_sw = (item.left <= cx && item.bottom <= cy);
                bool in_ne = (item.right >= cx && item.top >= cy);
                bool in_se = (item.right >= cx && item.bottom <= cy);

                if (in_nw) { nw_items.Add(item); }
                if (in_ne) { ne_items.Add(item); }
                if (in_se) { se_items.Add(item); }
                if (in_sw) { sw_items.Add(item); }
            }
            //Create the sub-quadrants, recursively.
            if (nw_items.Count > 0) { nw = new Quadtree(nw_items, depth, leftB, topB, cx, cy); }
            if (ne_items.Count > 0) { ne = new Quadtree(ne_items, depth, cx, topB, rightB, cy); }
            if (se_items.Count > 0) { se = new Quadtree(se_items, depth, cx, cy, rightB, bottomB); }
            if (sw_items.Count > 0) { sw = new Quadtree(sw_items, depth, leftB, cy, cx, bottomB); }
        }
        public void insertItems(List<Item> items, int depth, float leftB, float topB, float rightB, float bottomB)
        {
            // If we've reached the maximum depth then insert all items into this
            // quadrant.
            depth--;
            if (depth == 0)
            {
                foreach (Item i in items)
                {
                    if (!landuses.Contains(i.landuseId)) { landuses.Add(i.landuseId); }
                }
                return;
            }
            // Find this quadrant's centre.
            cx = Convert.ToSingle((leftB + rightB) * 0.5);
            cy = Convert.ToSingle((topB + bottomB) * 0.5);

            List<Item> nw_items = new List<Item>();
            List<Item> sw_items = new List<Item>();
            List<Item> se_items = new List<Item>();
            List<Item> ne_items = new List<Item>();
            foreach (Item item in items)
            {
                //check if landuse already in landuses
                if (landuses.Contains(item.landuseId)) { continue; }
                //check if node is in item
                if (item.left < leftB && item.top > topB && item.right > rightB && item.bottom < bottomB)
                {
                    landuses.Add(item.landuseId);
                    continue;
                }
                //Which of the sub-quadrants does the item overlap?
                bool in_nw = (item.left <= cx && item.top >= cy);
                bool in_sw = (item.left <= cx && item.bottom <= cy);
                bool in_ne = (item.right >= cx && item.top >= cy);
                bool in_se = (item.right >= cx && item.bottom <= cy);

                if (in_nw) { nw_items.Add(item); }
                if (in_ne) { ne_items.Add(item); }
                if (in_se) { se_items.Add(item); }
                if (in_sw) { sw_items.Add(item); }
            }
            //Create the sub-quadrants, recursively.
            if (nw_items.Count > 0)
            {
                if (nw == null) { nw = new Quadtree(nw_items, depth, leftB, topB, cx, cy); }
                else { nw.insertItems(nw_items, depth, leftB, topB, cx, cy); }
            }
            if (ne_items.Count > 0)
            {
                if (ne == null) { ne = new Quadtree(ne_items, depth, cx, topB, rightB, cy); }
                else { ne.insertItems(ne_items, depth, cx, topB, rightB, cy); }
            }
            if (se_items.Count > 0)
            {
                if (se == null) { se = new Quadtree(se_items, depth, cx, cy, rightB, bottomB); }
                else { se.insertItems(se_items, depth, cx, cy, rightB, bottomB); }
            }
            if (sw_items.Count > 0)
            {
                if (sw == null) { sw = new Quadtree(sw_items, depth, leftB, cy, cx, bottomB); }
                else { sw.insertItems(sw_items, depth, leftB, cy, cx, bottomB); }
            }
        }
        //get landuse on position x,y
        public List<int> hit(float x, float y)
        {
            List<int> hits = new List<int>();
            foreach (int i in landuses)
            {
                hits.Add(i);
            }
            List<int> lowerHits = new List<int>();
            if (nw != null && x <= cx && y >= cy) { lowerHits.AddRange(nw.hit(x, y)); }
            if (sw != null && x <= cx && y <= cy) { lowerHits.AddRange(sw.hit(x, y)); }
            if (ne != null && x >= cx && y >= cy) { lowerHits.AddRange(ne.hit(x, y)); }
            if (se != null && x >= cx && y <= cy) { lowerHits.AddRange(se.hit(x, y)); }


            foreach (int i in lowerHits)
            {
                if (!hits.Contains(i)) hits.Add(i);
            }
            return hits;
        }
    }

    class QuadtreeRoot
    {
        //Catches hits out of bounds, 
        Quadtree tree;
        float leftB, topB, rightB, bottomB;
        int depth;
        public QuadtreeRoot(List<Item> items, int depth, float leftB, float topB, float rightB, float bottomB)
        {
            //remove Squares outside our boundaries
            List<Item> tmpList = new List<Item>();
            foreach (Item item in items)
            {
                if (leftB > item.right || rightB < item.left || topB < item.bottom && bottomB > item.top)
                {
                    //item outside
                    continue;
                }
                tmpList.Add(item);
            }
            tree = new Quadtree(tmpList, depth, leftB, topB, rightB, bottomB);
            this.leftB = leftB; this.topB = topB; this.rightB = rightB; this.bottomB = bottomB; this.depth = depth;
        }
        public List<string> hit(float x, float y)
        {
            if (leftB < x && rightB > x && topB > y && bottomB < y)
            {
                List<int> hits = tree.hit(x, y);
                List<string> hitsReturn = new List<string>();
                foreach (int i in hits)
                {
                    hitsReturn.Add(Item.getLanduse(i));
                }
                return hitsReturn;
            }
            else
            {
                return new List<string>();
            }
        }
        public void insertItems(List<Item> items)
        {
            tree.insertItems(items, depth, leftB, topB, rightB, bottomB);
        }
    }

    //stores multiple trees, replaces old ones with new ones
    //redirects landuse call to a tree
    public class LanduseManager
    {
        private WWW landuseRequest, waterRequest;
        int zoomSmall, zoomBig, maxTreesStored, treeDepth;
        bool treeInBuild = false;
        float buildingTreeLat, buildingTreeLng;
        Dictionary<string, Tuple<System.DateTime, QuadtreeRoot>> trees; //find the currently needed tree and find the oldest to for deleting

        public LanduseManager(int zoomSmall = 11, int zoomBig = 9, int maxTreesStored = 3, int treeDepth = 7)
        {
            this.zoomSmall = zoomSmall; this.zoomBig = zoomBig; this.maxTreesStored = maxTreesStored; this.treeDepth = treeDepth;
            trees = new Dictionary<string, Tuple<System.DateTime, QuadtreeRoot>>();
        }
        public List<string> getLanduse(float lat, float lng)
        {
            Point tile = WorldToTilePos(lng, lat, zoomSmall);
            string coordStr = tile.X + "," + tile.Y;
            QuadtreeRoot tree;
            if (trees.ContainsKey(coordStr))    //search for tree
            {
                tree = trees[coordStr].Item2;
            }
            else                                //create new tree
            {
                if (treeInBuild)                //finish last tree first
                {
                    lat = buildingTreeLat;
                    lng = buildingTreeLng;
                }
                tree = createTree(lat, lng, treeDepth, zoomSmall, zoomBig);
                if (tree == null)
                {
                    buildingTreeLat = lat; 
                    buildingTreeLng = lng; 
                    treeInBuild = true; 
                    return null;
                }
                tile = WorldToTilePos(lng, lat, zoomSmall);
                coordStr = tile.X + "," + tile.Y;
                trees[coordStr] = new Tuple<System.DateTime, QuadtreeRoot>(System.DateTime.Now, tree);
                if (trees.Count > maxTreesStored)        //remove oldest tree if we have too many
                {
                    string oldestKey = "";
                    System.DateTime oldestTime = System.DateTime.Now;
                    foreach (KeyValuePair<string, Tuple<System.DateTime, QuadtreeRoot>> kvp in trees)
                    {
                        if (kvp.Value.Item1 < oldestTime)
                        {

                            oldestTime = kvp.Value.Item1;
                            oldestKey = kvp.Key;
                        }
                    }
                    if (oldestKey == "") { throw new System.ArgumentException("no key found"); }
                    trees.Remove(oldestKey);
                }
                treeInBuild = false;
                return null;
            }

            return tree.hit(lng, lat);
        }
        private List<float[][]> polyToRecs(JSONNode poly)
        {
            List<float[][]> recs = new List<float[][]>();
            //create a rectangle for every triangle in polygon,  inefficent, should be changed
            for (int i = 1; i < poly.Count - 1; i++)
            {
                var format = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
                float lowLat = Math.Min(float.Parse(poly[i - 1][0].ToString().Replace("\"", ""), format), Math.Min(float.Parse(poly[i][0].ToString().Replace("\"", ""), format), float.Parse(poly[i + 1][0].ToString().Replace("\"", ""), format)));
                float upLat = Math.Max(float.Parse(poly[i - 1][0].ToString().Replace("\"", ""), format), Math.Max(float.Parse(poly[i][0].ToString().Replace("\"", ""), format), float.Parse(poly[i + 1][0].ToString().Replace("\"", ""), format)));
                float lowLon = Math.Min(float.Parse(poly[i - 1][1].ToString().Replace("\"", ""), format), Math.Min(float.Parse(poly[i][1].ToString().Replace("\"", ""), format), float.Parse(poly[i + 1][1].ToString().Replace("\"", ""), format)));
                float upLon = Math.Max(float.Parse(poly[i - 1][1].ToString().Replace("\"", ""), format), Math.Max(float.Parse(poly[i][1].ToString().Replace("\"", ""), format), float.Parse(poly[i + 1][1].ToString().Replace("\"", ""), format)));
                recs.Add(new[] { new[] { lowLat, lowLon }, new[] { upLat, upLon } });
            }
            return recs;
        }
        //creates a new tree
        private QuadtreeRoot createTree(float lat, float lng, int depth, int zoomSmall, int zoomBig)
        {
            Point tile = WorldToTilePos(lng, lat, zoomSmall);
            if (landuseRequest == null || waterRequest == null)
            {
                landuseRequest = getTileData("landuse", tile.X, tile.Y, zoomSmall);  //get landuse data
                waterRequest = getTileData("water", tile.X, tile.Y, zoomSmall);//get water data
            }
            if (!landuseRequest.isDone || !waterRequest.isDone) return null;
            JSONNode data = JSONNode.Parse(landuseRequest.text);
            JSONNode waterData = JSONNode.Parse(waterRequest.text);
            List<Item> items = getItemsFromJson(data);                          //get landuse items for tree
            PointF nwCorner = TileToWorldPos(tile.X, tile.Y, zoomSmall);        //find bounds for tree
            PointF seCorner = TileToWorldPos(tile.X + 1, tile.Y + 1, zoomSmall);
            QuadtreeRoot tree = new QuadtreeRoot(items, depth, nwCorner.X, nwCorner.Y, seCorner.X, seCorner.Y);     //create tree
            tree.insertItems(getItemsFromJson(waterData));                    //insert water layer
            //tree.insertItems(getItemsFromJson(getTileData("landuse", tile.X, tile.Y, zoomBig)));                    //insert landuse layer with higher zoom
            return tree;
        }
        //creates list with items from geoJson
        private List<Item> getItemsFromJson(JSONNode data)
        {
            JSONNode features = data["features"];
            List<Item> items = new List<Item>();
            for (int i = 0; i < features.Count; i++)
            {
                if (features[i]["geometry"]["type"].ToString().Equals("\"Polygon\""))
                {
                    List<float[][]> recs = polyToRecs(features[i]["geometry"]["coordinates"][0]);
                    foreach (float[][] r in recs)
                    {
                        items.Add(new Item(r[0][0], r[1][1], r[1][0], r[0][1], features[i]["properties"]["kind"].ToString()));
                    }
                }
                else if (features[i]["geometry"]["type"].ToString().Equals("\"MultiPolygon\""))
                {
                    for (int j = 0; j < features[i]["geometry"]["coordinates"][0].Count; j++)
                    {
                        List<float[][]> recs = polyToRecs(features[i]["geometry"]["coordinates"][0][j]);
                        foreach (float[][] r in recs)
                        {
                            items.Add(new Item(r[0][0], r[1][1], r[1][0], r[0][1], features[i]["properties"]["kind"].ToString()));
                        }
                    }
                }
                else if (features[i]["geometry"]["type"].ToString().Equals("\"Point\""))
                {
                    JSONNode coordJ = features[i]["geometry"]["coordinates"];
                    var format = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
                    float x = float.Parse(coordJ[0].ToString().ToString().Replace("\"", ""), format);
                    float y = float.Parse(coordJ[1].ToString().ToString().Replace("\"", ""), format);

                    items.Add(new Item(x, y, x + (float)0.0001, y + (float)0.0001, features[i]["properties"]["kind"].ToString()));
                }
                else
                {
                    //Console.Write("Could not parse: ");       //mainly Line and MultiLine
                    //Console.WriteLine(features[i]["geometry"]["type"].ToString());
                }
            }
            return items;
        }

        //later this should have a better api call and save data local
        private WWW getTileData(string layer, int xTile, int yTile, int zoom)
        {
            string url = "http://vector.mapzen.com/osm/" + layer + "/" +
                            Convert.ToString(zoom) + "/" + Convert.ToString(xTile) + "/" + Convert.ToString(yTile) + ".json?api_key=vector-tiles-vx5RUiN";
            // Create the request and wait for a response
            // Create the request and wait for a response
            WWW r = new WWW(url);

            return r;
        }
        private Point WorldToTilePos(double lon, double lat, int zoom)
        {
            Point p = new Point();
            p.X = (int)((lon + 180.0) / 360.0 * (1 << zoom));
            p.Y = (int)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) +
                1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom));

            return p;
        }
        private PointF TileToWorldPos(int tile_x, int tile_y, int zoom)
        {
            PointF p = new PointF();
            double n = Math.PI - ((2.0 * Math.PI * tile_y) / Math.Pow(2.0, zoom));

            p.X = (float)((tile_x / Math.Pow(2.0, zoom) * 360.0) - 180.0);
            p.Y = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            return p;
        }
    }
}