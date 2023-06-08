using Microsoft.Xna.Framework;
using QwertyMod.Content.Items.Consumable.Tiles.Ores;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;
using System;
using QwertyMod.Content.Dusts;

namespace QwertyMod.Common
{
    public class Pathfinder
    {
        public static float? PathFindWithNodeSize(Vector2 worldStart, Vector2 worldEnd, int nodeSize, int extraDistFromTiles , bool debug = false)
        {
            //if(debug) Main.NewText("Method Called");
            Point startHere = worldStart.ToTileCoordinates();
            Point endHere = worldEnd.ToTileCoordinates();

            int padding = 60 / nodeSize;

            startHere.X = startHere.X - (startHere.X % nodeSize) + nodeSize / 2;
            startHere.Y = startHere.Y - (startHere.Y % nodeSize) + nodeSize / 2;
            endHere.X = endHere.X - (endHere.X % nodeSize) + nodeSize / 2;
            endHere.Y = endHere.Y -(endHere.Y % nodeSize) + nodeSize / 2;

            int width = (int)MathF.Abs(startHere.X - endHere.X) / nodeSize + 1 + padding * 2;
            int height = (int)MathF.Abs(startHere.Y - endHere.Y) / nodeSize + 1 + padding * 2;
            Point topLeft = new Point((int)MathF.Min(startHere.X, endHere.X) - padding, (int)MathF.Min(startHere.Y, endHere.Y) - padding);
            
            if(debug)
            {
               Main.NewText("Top Left: " + topLeft);
               Dust.NewDustPerfect( startHere.ToVector2() * 16 + Vector2.One * 8f, ModContent.DustType<InvaderGlow>(), Vector2.Zero);
               Dust.NewDustPerfect( endHere.ToVector2() * 16 + Vector2.One * 8f, ModContent.DustType<InvaderGlow>(), Vector2.Zero);
               Dust.NewDustPerfect( topLeft.ToVector2() * 16 + Vector2.One * 8f, ModContent.DustType<InvaderGlow>(), Vector2.Zero);
            }
            
            PathNode[,] nodes = new PathNode[width, height];
            int countObs = 0;
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    int x = i * nodeSize + nodeSize / 2;
                    int y = j * nodeSize + nodeSize / 2;
                    Point pos = topLeft + new Point(x, y);
                    bool walkable = true;
                    int extraSpacedPadding = nodeSize + extraDistFromTiles;
                    for(int k = 0; k < extraSpacedPadding; k++)
                    {
                        for(int l = 0; l < extraSpacedPadding; l++)
                        {
                            if(Main.tile[pos.X - extraSpacedPadding / 2 + k, pos.Y  - extraSpacedPadding / 2 + l].HasUnactuatedTile)
                            {
                                walkable = false;
                            }
                        }
                    }
                    countObs += !walkable ? 1 : 0;
                    nodes[i, j] = new PathNode(pos, -1, (pos - endHere).ToVector2().Length(), walkable);
                }
            }
            if(debug) Main.NewText("Created Nodes: " + width + " wide, " + height + " high, " + countObs + " obsticals");
            float maxDist = -1; 
            float maxDistFirst = -1;
            PathNode end = null;
            PathNode start = null;
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    Vector2 realLoc = (nodes[i, j].position).ToVector2() * 16 + Vector2.One * 8f;
                    if(nodes[i, j].walkable && (maxDist == -1 || (realLoc - worldEnd).Length() < maxDist))
                    {
                        end = nodes[i,j];
                        maxDist = (realLoc - worldEnd).Length();
                    }
                    if(nodes[i, j].walkable && (maxDistFirst == -1 || (realLoc - worldStart).Length() < maxDistFirst))
                    {
                        start = nodes[i,j];
                        maxDistFirst = (realLoc - worldStart).Length();
                    }
                    if(i > 0)
                    {
                        //if(j > 0) nodes[i, j].Neighbors.Add(nodes[i - 1, j - 1]);
                        nodes[i, j].Neighbors.Add(nodes[i - 1, j]);
                        //if(j < height - 1 ) nodes[i, j].Neighbors.Add(nodes[i - 1, j + 1]);
                    }
                    if(j > 0) nodes[i, j].Neighbors.Add(nodes[i, j - 1]);
                    if(j < height - 1 ) nodes[i, j].Neighbors.Add(nodes[i, j + 1]);
                    if(i < width - 1)
                    {
                        //if(j > 0) nodes[i, j].Neighbors.Add(nodes[i + 1, j - 1]);
                        nodes[i, j].Neighbors.Add(nodes[i + 1, j]);
                        //if(j < height - 1 ) nodes[i, j].Neighbors.Add(nodes[i + 1, j + 1]);
                    }
                }
            }
            if(end == null || start == null)
            {
                return null;
            }
            if(debug) Main.NewText("Assigned Neighbors");
            Point startRelative = new Point((startHere.X - topLeft.X) / nodeSize, (startHere.Y - topLeft.Y) / nodeSize); 
           
            List<PathNode> OPEN = new List<PathNode>();
            List<PathNode> CLOSED = new List<PathNode>();
            OPEN.Add(start);
            start.gCost = 0;
            int loopCount = 0;
            while(OPEN.Count > 0 && loopCount < 1000)
            {
                loopCount++;
                float bestF = -1;
                PathNode current = null;
                for(int i = 0; i < OPEN.Count; i++)
                {
                    if(i == 0 || OPEN[i].fCost() < bestF || (current != null && OPEN[i].fCost() == bestF && OPEN[i].hCost < current.hCost))
                    {
                        bestF = OPEN[i].fCost();
                        current = OPEN[i];
                    }
                }
                OPEN.Remove(current);
                CLOSED.Add(current);

                if(current == end) break;

                foreach(PathNode neighbor in current.Neighbors)
                {
                    if(neighbor.walkable && !CLOSED.Contains(neighbor))
                    {
                        float newTheoreticalFCost =  neighbor.hCost + current.gCost + (current.position - neighbor.position).ToVector2().Length();
                        if(!OPEN.Contains(neighbor) || newTheoreticalFCost < neighbor.fCost())
                        {
                            neighbor.parent = current;
                            neighbor.gCost = current.gCost + (current.position - neighbor.position).ToVector2().Length();
                            if(!OPEN.Contains(neighbor))
                            {
                                OPEN.Add(neighbor);
                            }
                            else
                            {
                                //if(debug) Main.NewText("Reroute");
                            }
                        }
                    }
                }
            }
            if(end.parent != null)
            {
                if(debug) Main.NewText("Pathfind complete with " + loopCount + " loops, cost: " + end.fCost());
                PathNode traceBack = end;
                loopCount = 0;
                while(traceBack.parent != null && loopCount < 1000)
                {
                    loopCount++;
                    if(debug) Dust.NewDustPerfect((traceBack.position).ToVector2() * 16 + Vector2.One * 8f, DustID.Torch, Vector2.Zero).noGravity = true;
                    if(traceBack.parent == start )
                    {
                        if(debug) Dust.NewDustPerfect((start.position).ToVector2() * 16 + Vector2.One * 8f, DustID.Torch, Vector2.Zero).noGravity = true;
                        Vector2 firstWorldPos = (start.position).ToVector2() * 16 + Vector2.One * 8f;
                        Vector2 nextWorldPos = (traceBack.position).ToVector2() * 16 + Vector2.One * 8f;
                        float distFromLine = (MathF.Abs((nextWorldPos.X - firstWorldPos.X) * (firstWorldPos.Y - worldStart.Y) - (firstWorldPos.X - worldStart.X) * (nextWorldPos.Y - firstWorldPos.Y)))
                         / ((firstWorldPos - nextWorldPos).Length());
                        if(distFromLine > 4)
                        {
                            return (firstWorldPos - worldStart).ToRotation();
                        }
                        return (traceBack.position - start.position).ToVector2().ToRotation();
                    }
                    traceBack = traceBack.parent;
                }
                if(debug) Main.NewText(loopCount);
                
            }
            else
            {
                if(debug) Main.NewText("Pathfind failed");
            }
            return null;
        }
    }
    public class PathNode
    {
        public List<PathNode> Neighbors;
        public PathNode parent = null;
        public Point position;
        public float gCost = -1;
        public float hCost = -1;
        public float fCost()
        {
            return gCost + hCost;
        }

        public bool walkable = false;
        public PathNode(Point position, float gCost, float hCost, bool walkable)
        {
            Neighbors = new List<PathNode>();
            this.position = position;
            this.gCost = gCost;
            this.hCost = hCost;
            this.walkable = walkable;
        }
    }
}