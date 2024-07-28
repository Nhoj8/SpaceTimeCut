using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using static SpaceTimeCut.Level;
using SharpDX.MediaFoundation;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

namespace SpaceTimeCut
{

    public class Level
    {
        public static bool IsGrabbing = true;
        public static bool IsCutting;
        public static bool IsRotating;
        public static Texture2D circle;
        public static bool timeBarHasMouse;
        //public static int player = 0;
        public static int lengthpercircle = 80;
        public static int height = 16;
        public static int TotalamounOfCircles = 15;
        

        //public static SpaceTimeLocation destination;// = new SpaceTimeLocation(0, 0, 1, 0);
        //public static SpaceTimeLocation location;// = new SpaceTimeLocation(0,0,0,0);
        //public static int directionOfMovement;
        
        public static int PLAYERENTITY = 0;
        public static float progress;

        public static int timeperblock = 30;

        public static int currentMaxCut;
        public static int totalCuts;
        public static int currentMaxRotations;
        public static int totalRotations;

        public static int blocksize;

        public static Point mouseOriginalPosition = new Point(0, 0);
        public static SpaceTimeLocation[] LocationsOfSelectedBlock;
        public static bool anyGridHasMouse = false;

        //things that shouldnt be static
        public static Time[] times;
        public static Entity[] entitys;
        public static Entity[] originalEntitys;
        public static int currentCircle = 0;


        
        public static LevelSelector[] levelSelectors;
        public static int levelNum;
        public static Button cutButton;
        public static Button moveButton;
        public static Button rotateButton;
        //info
        public static void initLevelSelector()
        {
            int B = Grid.BARRIER;
            int M = Grid.MOVABLEWALL;
            int O = Grid.GOAL;//objective
            int A = Grid.AIR;
            int P = Grid.PLAYERTYPE;
            int R = Grid.ENTITYNUM;
            int D = Grid.ENTITYNUM + 1;
            int L = Grid.ENTITYNUM + 2;
            int U = Grid.ENTITYNUM + 3;
            int H = Grid.MOVEABLEENTITYNUM;
            int G = Grid.MOVEABLEENTITYNUM + 1;
            int F = Grid.MOVEABLEENTITYNUM + 2;
            int T = Grid.MOVEABLEENTITYNUM + 3;

            //level 1
            int[,] level1 = {
{ A,A,A,A,A,A, A, B,B,B },
{ A,P,A,M,A,A, A, A,O,B },
{ A,A,A,A,A,A, A, B,B,B },
};
            int[,] level2 = {
{ A,A,A,A,A,A, A, B,B,B },
{ A,P,A,A,A,A, A, D,O,B },
{ A,A,A,A,A,A, A, D,B,B },
            };
            //level 2
            int[,] level3 = {
            { B,B,B,A,A,A, B, A,B,B },
            { B,P,A,A,A,A, A, M,O,B },
            { B,B,B,A,A,A, B, A,B,B },
            { B,R,A,A,A,A, A, A,B,B },
            { B,B,B,A,A,A, B, B,B,B },
            };
            //level 3
            int[,] level4 = {
            { B,B,B,A},
            { B,P,A,M},
            { B,B,B,M},
            { B,R,O,M},
            { B,B,B,M},
            };
            //level 4
            int[,] level5 = {
{ B,B,B,B,B,B, B, B,B,B },
{ P,A,A,A,A,A, A, A,O,B },
{ B,B,B,B,B,B, B, B,M,B },
{ B,A,A,A,A,R, D, A,A,B },
{ M,M,M,M,M,M, M, B,A,B },
{ M,M,M,M,M,M, M, B,A,B },
{ M,M,M,M,M,M, M, B,A,B },
{ M,M,M,M,M,M, M, B,A,B },
{ M,M,M,M,M,M, M, B,A,B },
{ M,M,M,M,M,M, M, B,U,M },
{ M,M,M,M,M,M, M, M,U,M },
};
            int[,] level6 = {
{ A,A,A,A,A,A, A, B,B,B },
{ A,P,A,A,A,A, A, M,O,B },
{ A,A,A,A,A,A, A, B,B,B },
};

            int[,] level7 = {
            { B,B,B,B,A,B, A, B,B,B },
            { B,B,B,B,A,B, A, B,O,B },
            { P,A,A,A,A,B, A, B,A,B },
            { B,B,B,B,B,B, A, B,A,B },
            };
            int[,] level8 = {
{ B,B,B,B,B,B, B, B,B,B,B },
{ B,P,A,A,A,A, A, A,O,B,B },
{ B,B,B,B,B,B, B, B,M,B,B },
{ B,R,A,A,A,A, A, A,A,A,B },
{ M,R,A,A,A,A, A, A,A,A,B },
{ M,M,M,M,M,M, M, B,A,B,B },
{ M,M,M,M,M,M, M, B,A,B,B },
{ M,M,M,M,M,M, M, B,T,B,B },
{ M,M,M,M,M,M, M, M,T,T,T },
};

            int[,] level9 = {
{ P,M,M,M,M, M, H,H,H },
{ M,B,B,B,B, B, M,B,M },
{ M,M,A,A,M, A, A,B,M },
{ M,B,A,A,M, A, A,B,M },
{ M,B,M,M,O, M, M,B,M },
{ M,B,A,A,M, A, A,B,M },
{ M,B,A,A,M, A, A,M,M },
{ M,B,M,B,B, B, B,B,M },
{ L,L,L,M,M, M, M,M,M },
};
            int[,] level10 = {
{ A,B,B,B,B,B, B, B,B,B },
{ P,A,A,A,A,A, A, A,O,B },
{ A,B,A,A,A,A, A, D,A,B },
{ A,B,A,A,A,A, A, B,A,B },
{ A,B,A,A,A,A, A, B,A,B },
{ A,B,A,A,A,A, A, B,A,B },
{ A,B,A,A,A,A, A, B,A,B },
{ A,B,A,A,A,A, A, B,A,B },
{ A,B,A,A,A,A, A, B,U,B },
{ A,B,B,B,B,B, B, B,B,B },
};
            int[,] leveltest = {
{ A,A,A,A,A,A, A, B,B,B },
{ A,P,M,M,M,M, A, A,O,B },
{ A,A,A,A,A,A, A, B,B,B },
{ A,A,A,H,A,F, A, B,B,B },
};
            int[][,] theBlocks = { level1, level2, level3, level4, level5, level6, level7,level8,level9,level10,leveltest };

            levelSelectors = new LevelSelector[theBlocks.Length];
            for(int i = 0; i < theBlocks.Length; i++)
            {
                levelSelectors[i] = new LevelSelector(i,ref theBlocks[i],theBlocks.Length);
            }
            moveButton = new Button("M", Button.TOPMIDDLE, -75, 75, moveButtonClick, false, 50, 50, false, Color.Green, Color.DarkOliveGreen);
            rotateButton = new Button("R", Button.TOPMIDDLE, -25, 75, rotateButtonClick, false, 50, 50, false, Color.Green, Color.DarkOliveGreen);
            cutButton = new Button("C", Button.TOPMIDDLE, 25, 75, cutButtonClick, false, 50, 50, false, Color.Green, Color.DarkOliveGreen);
        }

        public Level(int blocksize)
        {
            
            Level.blocksize = blocksize;
            totalCuts = 0;
            totalRotations = 0;
            currentMaxRotations = 1;
            currentMaxCut = 2;
            progress = 0;
            timeBarHasMouse = false;
            currentCircle = 0;
            if (circle == null)
                circle = SpriteBatchExtensions.Resize(Game1._graphics.GraphicsDevice, Game1.Getborders(Game1.CreateCircle(Game1._graphics.GraphicsDevice, 8), 1, Color.Black, 0, false), 2);
            times = new Time[1];
            times[0] = new Time();
            times[0].Initilize(ref levelSelectors[levelNum].blocks);
            moveButton.visible = true;
            cutButton.visible = true;
            rotateButton.visible = true;


        }
        static void moveButtonClick(object sender, EventArgs e)
        {
            Level.IsGrabbing = true;
            Level.IsCutting = false;
            Level.IsRotating = false;
            moveButton.color = Color.CornflowerBlue;
            moveButton.Selectedcolor = Color.CadetBlue;
            cutButton.color = Color.Green;
            cutButton.Selectedcolor = Color.DarkOliveGreen;
            rotateButton.color = Color.Green;
            rotateButton.Selectedcolor = Color.DarkOliveGreen;
        }
        static void rotateButtonClick(object sender, EventArgs e)
        {
            Level.IsGrabbing = false;
            Level.IsCutting = false;
            Level.IsRotating = true;
            rotateButton.color = Color.CornflowerBlue;
            rotateButton.Selectedcolor = Color.CadetBlue;
            moveButton.color = Color.Green;
            moveButton.Selectedcolor = Color.DarkOliveGreen;
            cutButton.color = Color.Green;
            cutButton.Selectedcolor = Color.DarkOliveGreen;
        }
        static void cutButtonClick(object sender, EventArgs e)
        {

            Level.IsGrabbing = false;
            Level.IsCutting = true;
            Level.IsRotating = false;
            cutButton.color = Color.CornflowerBlue;
            cutButton.Selectedcolor = Color.CadetBlue;
            moveButton.color = Color.Green;
            moveButton.Selectedcolor = Color.DarkOliveGreen;
            rotateButton.color = Color.Green;
            rotateButton.Selectedcolor = Color.DarkOliveGreen;
        }
        public void Update(Point mousePosition)
        {
            anyGridHasMouse = false;
            for (int i = 0; i < times.Length; i++)
                times[i].Update(mousePosition,i);

        }
        public void Press(Point MousePosition)
        {
            for (int i = 0; i < times.Length; i++)
            {
                if (times[i].Press(i, MousePosition))
                    break;
            }
        }
        public void Release()
        {
            for (int i = 0; i < times.Length; i++)
            {
                times[i].Release();
            }
        }
        public void MoveWithKEYS(Point movement)
        {
            entitys[PLAYERENTITY].MoveWithKEYS(movement,ref entitys[PLAYERENTITY].location,ref entitys[PLAYERENTITY].destination);
        }
        public void MoveTime()
        {
            progress += 1 / (float)timeperblock;
            if (progress >= 1)
            {
                
                NextTimeUnit();
            }
        }
        public static void NextTimeUnit()
        {
            currentCircle += 1;
            progress = 0;
            for (int i = 0; i < entitys.Length; i++)
            //for (int i = entitys.Length-1; i >=0; i--)
            {
                entitys[i].UpdateLocation(i);
            }
            entitys[PLAYERENTITY].CheckIfWin();
            for (int i = 0; i < entitys.Length; i++)
            //for (int i = entitys.Length - 1; i >= 0; i--)
            {
                
                entitys[i].Move(i);
            }
        }
        public static SpaceTimeLocation[] GetAllLocationsOfOriginalPoint(SpaceTimeLocation original)//original is in normal Coordinates not orignial
        {
            SpaceTimeLocation[] LocationsOfOriginalPoint = new SpaceTimeLocation[times.Length];
            Point originalPoint = times[original.time].grids[original.gridnum].GetOriginalLocationOfPoint(original.coords);
            for (int i = 0;i < times.Length; i++)
            {
                LocationsOfOriginalPoint[i].time = i;
                if (i == original.time)
                {
                    LocationsOfOriginalPoint[i] = original;
                    
                }
                else
                {
                    LocationsOfOriginalPoint[i] = times[i].GetLocationFromOriginalPoint(originalPoint , i);
                }

            }
            return LocationsOfOriginalPoint;
        }
        public static Point GetOriginalLocationOfPoint(SpaceTimeLocation location)
        {
            return times[location.time].grids[location.gridnum].GetOriginalLocationOfPoint(location.coords);
        }
        public void Draw(SpriteBatch _spriteBatch, Color colorclear)
        {
            for (int t = 0; t < times.Length; t++)
                times[t].Draw(_spriteBatch, colorclear, t);


        }

        public struct SpaceTimeLocation
        {
            public int time;
            public int gridnum;
            public Point coords;
            public SpaceTimeLocation(int time, int gridnum, Point coords)
            {
                this.time = time;
                this.gridnum = gridnum;
                this.coords = coords;
            }
            public SpaceTimeLocation(int time, int block, int X, int Y)
            {
                this.time = time;
                this.gridnum = block;
                this.coords.X = X;
                this.coords.Y = Y;
            }
            public int X
            {
                get { return this.coords.X; }
                set { this.coords.X = value; }
            }
            public int Y
            {
                get { return this.coords.Y; }
                set { this.coords.Y = value; }
            }
            public static SpaceTimeLocation operator +(SpaceTimeLocation Dc1, Point dv)
            {
                Dc1.X += dv.X;
                Dc1.Y += dv.Y;
                return Dc1;
            }
            public static bool operator ==(SpaceTimeLocation wc1, SpaceTimeLocation wc2)
            {
                return wc1.Equals(wc2); //(wc1.X == wc2.X && wc1.Y == wc2.Y);
            }

            public static bool operator !=(SpaceTimeLocation wc1, SpaceTimeLocation wc2)
            {
                return !wc1.Equals(wc2);
            }


        }
    }

    public class LevelSelector
    {
        int levelNum;
        public int[,] blocks;
        public Button button;
        public LevelSelector(int levelNum,ref int[,] blocks,int amountOfLevels)
        {
            this.levelNum = levelNum;
            this.blocks = blocks;
            //levelButtons = new Button[theBlocks.Length];
            //int amountOfButtons = levelButtons.Length;
            int buttonWidth = 50;
            int buttonMargin = 10;
            //for (int i = 0; i < amountOfButtons; i++)
            //{
            button = new Button((levelNum+1).ToString(), Button.TOPMIDDLE,-amountOfLevels*buttonWidth/2-(amountOfLevels-1)*buttonMargin/2 +buttonWidth * levelNum + buttonMargin * (levelNum-1), 200,Click, false, buttonWidth, buttonWidth,true);
            
            //}
        }
       
        void Click(object sender, EventArgs e)
        {
            for(int i = 0; i < levelSelectors.Length; i++)
            {
                levelSelectors[i].button.visible = false;
            }
            Level.levelNum = this.levelNum;
            Game1.CurrentlyDisplaying = Game1.LOADING;

        }
    }

    

    public class Time
    {
        

        

        public Rectangle timeBarRect;
        public int starttime = 0;
        public int amounOfCircles =TotalamounOfCircles;
        
        public int cutLocation = -1;

        public Grid[] grids;
        int circleHovering = -1;
        int circleSelected = -1;

        Rectangle timeSelectBar;
        bool timeSelectBarHovering;
        public bool visible=true;




        public void Initilize(ref int[,] theBlocks)
        {



            //destination = null;
            //location = null;
            grids = new Grid[1];
            grids[0] = new Grid();
            //destination =
            grids[0].Initilize(ref theBlocks);
            //location = destination;
            
            TimeBarRefersh();
        }
        //public void Move(Point movement)
        //{
        //    grids[location.gridnum].Move(movement,ref location,ref destination);
        //}


        public void Release()
        {
            for (int i = 0; i < grids.Length; i++)
            {
                if (grids[i].IsGrabbed && grids[i].isGhost)
                    grids[i].Connect();

                grids[i].IsGrabbed = false;
            }
        }
        public bool Press(int t, Point MousePosition)
        {
            bool hasMouse = false;//if anything in this time Contains the mouse
            if (timeBarRect.Contains(MousePosition)&& !timeSelectBarHovering)
            {

                if (cutLocation > -1)
                    Cut(t);
                else if (circleHovering > -1)
                    SelectTime(t);
                hasMouse = true;
                return hasMouse;
            }
            if (timeSelectBarHovering)
            {
                hasMouse = true;
                visible = !visible;
                return hasMouse;
            }
                


            for (int i = grids.Length - 1; i > -1; i--)
            {
                if (grids[i].blockRect.Contains(MousePosition))
                {
                    hasMouse = true;
                    if (Level.IsGrabbing)
                    {
                        grids[i].Grab(MousePosition);
                        break;
                    }

                    else if (Level.IsCutting)
                    {
                        grids[i].Cut(t,i);
                        break;
                    }
                    else if (Level.IsRotating)
                    {
                        grids[i].Rotate(t,i);
                        break;
                    }
                    


                }
            }
            return hasMouse;
        }

        void SelectTime(int t)
        {
            
            for (int i = 0; i < times.Length; i++)
            {
                times[i].circleSelected = -1;
            }
            circleSelected = circleHovering;
            currentCircle = 0;
            entitys = new Entity[originalEntitys.Length];
            for(int i = 0; i < entitys.Length; i++)
            {
                entitys[i] = originalEntitys[i].GetCopy();
                SpaceTimeLocation currentLocation= times[entitys[i].location.time].GetLocationFromOriginalPoint(entitys[i].location.coords, entitys[i].location.time);
                int rotationAmount = times[currentLocation.time].grids[currentLocation.gridnum].rotation;
                entitys[i].DirectionOfMoveRotate(rotationAmount);
                entitys[i].location = currentLocation;
                SpaceTimeLocation currentDestination = times[entitys[i].destination.time].GetLocationFromOriginalPoint(entitys[i].destination.coords, entitys[i].destination.time);
                entitys[i].destination = currentDestination;
                
            }
            int timeSelected = circleSelected + starttime;
            for(int i = 0; i < timeSelected; i++)
            {
                NextTimeUnit();
            }
            progress = 1;
        }
        void TimeBarRefersh()
        {
            timeBarRect = new Rectangle(Button.PositionAnchors[Button.BOTTOMMIDDLE].ToPoint() + new Point((starttime) * lengthpercircle, -100) - new Point(lengthpercircle * (TotalamounOfCircles - 1) / 2, height / 2), new Point(lengthpercircle * (amounOfCircles - 1) + circle.Width, height));
            int BarThickness = 10;
            timeSelectBar = new Rectangle(Button.PositionAnchors[Button.BOTTOMMIDDLE].ToPoint() + new Point((starttime) * lengthpercircle- BarThickness, -100 - BarThickness) - new Point(lengthpercircle * (TotalamounOfCircles - 1) / 2, height / 2), new Point(lengthpercircle * (amounOfCircles - 1) + circle.Width+BarThickness*2, height+ BarThickness*2));

        }
        public void Update(Point MousePosition,int t)
        {
            TimeBarRefersh();
            timeBarHasMouse = false;
            timeSelectBarHovering= false;
            circleHovering = -1;
            cutLocation = -1;
            if (timeSelectBar.Contains(MousePosition))
            {
                timeSelectBarHovering = true;

                if (timeBarRect.Contains(MousePosition))
                {
                    timeBarHasMouse = true;

                    for (int i = 0; i < amounOfCircles; i++)
                    {
                        if (new Rectangle(timeBarRect.Location + new Point((int)(lengthpercircle * i), 0), circle.Bounds.Size).Contains(MousePosition))
                        {
                            circleHovering = i;
                            timeSelectBarHovering = false;
                            break;
                        }
                        else if (new Rectangle(timeBarRect.Location + new Point((int)(lengthpercircle * i + circle.Width), 0), new Point((int)(lengthpercircle - circle.Width), circle.Height)).Contains(MousePosition) && IsCutting)
                        {
                            cutLocation = i;
                            timeSelectBarHovering = false;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < grids.Length; i++)
                grids[i].Update(MousePosition, t, i);

        }
        void Cut(int t)
        {
            Array.Resize(ref times, times.Length + 1);
            Time newTime = (Time)this.MemberwiseClone();
            newTime.grids = new Grid[grids.Length];

            for (int i = 0; i < grids.Length; i++)
            {
                newTime.grids[i] = grids[i].GetCopy();
                newTime.grids[i].Blocks = (int[,])grids[i].Blocks.Clone();
            }

            ApplyOffset(new Vector2(-300, 0));
            newTime.ApplyOffset(new Vector2(300, 0));

            amounOfCircles = cutLocation + 1;
            newTime.amounOfCircles -= cutLocation + 1;
            newTime.starttime = starttime + cutLocation + 1;
            //if (t == location.time)
            //{
            //    //THIS IS FOR MAKING PLAYER IN OTHER TIME A GRAY BLOCK
            //    // times[ times.Length - 1].grids[location.gridnum].Blocks[location.X, location.Y] =1;
            //    ////Level.player = -1;
            //}
            if (t < times.Length - 2)//if the time that was cut is not the last time
            {
                Time oldTime = times[t + 1];
                times[t + 1] = newTime;
                for (int i = t + 2; i < times.Length; i++)
                {
                    Time holdTime = times[i];
                    times[i] = oldTime;
                    oldTime = holdTime;
                }
            }
            else
                times[times.Length - 1] = newTime;
            totalCuts++;

        }
        public SpaceTimeLocation GetLocationFromOriginalPoint(Point originalPoint, int t)
        {
            SpaceTimeLocation locationInThisInstance = new SpaceTimeLocation();
            locationInThisInstance.time = t;
            for (int i = 0; i < grids.Length;i++)
            {

                Rectangle area = new Rectangle(grids[i].originalLocation, new Point(grids[i].Blocks.GetLength(0), grids[i].Blocks.GetLength(1)));
                if (originalPoint.X < area.X)
                    continue;
                if (originalPoint.Y < area.Y)
                    continue;
                if (grids[i].rotation % 2 == 1)//get whether or not the x,y length were inverted because of rotation, if so reverse back
                {
                    int hold = area.Width;
                    area.Width = area.Height;
                    area.Height = hold;
                }

                if (originalPoint.X >= area.X + area.Width)
                    continue;
                if (originalPoint.Y >= area.Y + area.Height)
                    continue;

                locationInThisInstance.gridnum = i;//found the grids
                locationInThisInstance.coords = originalPoint - grids[i].originalLocation;//get the originallocation in terms of gridnum coords

                int xcoord2 = area.Width;
                int ycoord2 = area.Height;
                for (int r = 0; r < grids[i].rotation; r++)//Rotate by the amount the gird was
                {
                    locationInThisInstance.coords = new Point(ycoord2 - 1 - locationInThisInstance.coords.Y, locationInThisInstance.coords.X);
                    int hold2 = xcoord2;
                    xcoord2 = ycoord2;
                    ycoord2 = hold2;

                }
                return locationInThisInstance;
            }
            System.Windows.Forms.MessageBox.Show("UhOh Grid not found");
            return locationInThisInstance;


            }


        void ApplyOffset(Vector2 offset)
        {
            for (int i = 0; i < grids.Length; i++)
                grids[i].position += offset;
        }

        public void Draw(SpriteBatch _spriteBatch, Color colorclear, int t)
        {
            Color colorForSelectRect = Color.DimGray;

            if (visible)
            {
                for (int b = 0; b < grids.Length; b++)
                    grids[b].Draw(_spriteBatch, colorclear, b, t);
                ////entitys[PLAYERENTITY].Draw(_spriteBatch, colorclear, Grid.PLAYERTEXTURENUM);           
                //for (int e = 0; e < entitys.Length; e++)
                    //entitys[e].Draw(_spriteBatch, colorclear, entitys[e].imageNum);

                if (anyGridHasMouse)
                {
                    grids[LocationsOfSelectedBlock[t].gridnum].DrawSelected(_spriteBatch, LocationsOfSelectedBlock[t]);

                }
                for (int b = 0; b < grids.Length; b++)
                    grids[b].DrawCutLine(_spriteBatch, colorclear);
                colorForSelectRect = new Color(180, 180, 180);
            }
            if (timeSelectBarHovering)
                colorForSelectRect = new Color(150, 150, 150);

            //draw SelctRect for Time
            _spriteBatch.DrawRoundedRect(timeSelectBar, // The coordinates of the Rectangle to be drawn
circle, // Texture for the whole rounded rectangle
circle.Width / 2, // Distance from the edges of the texture to the "middle" patch
colorForSelectRect);


            Rectangle timeRect = new Rectangle(timeBarRect.Location + new Point(0, timeBarRect.Height / 2 - 2 / 2), new Point(lengthpercircle * (amounOfCircles-1) +circle.Width, 2));
            
            
            //float deltaX = (float)(lengthpercircle * (TotalamounOfCircles-1))/ (TotalamounOfCircles - 1);
            for (int i = 0; i < amounOfCircles; i++)
            {
                Color color = colorclear;
                if (i+ starttime <= currentCircle)
                {
                    if (i + starttime == currentCircle) 
                        color = new Color(1- progress, 1-progress*0.5f, 1 - progress);

                    else
                        color = Color.Green;
                }
                if (i == circleHovering)
                    color = Color.Red;
                else if (i == circleSelected)
                    color = Color.Orange;
                
                _spriteBatch.Draw(circle, timeBarRect.Location.ToVector2() + new Vector2((int)(lengthpercircle * i), 0), color);
            }

            if (cutLocation > -1)
                Game1.DrawRectangle(_spriteBatch, new Rectangle(timeBarRect.Location + new Point((int)(cutLocation* lengthpercircle + lengthpercircle / 2 + circle.Width/2),0), new Point(2, timeBarRect.Height)), colorclear);
            Game1.DrawRectangle(_spriteBatch, timeRect, Color.Red);
        }

    }


    public class Grid
    {

        public static bool IsValidCut = true;
        public static int timeBar = 0;
        //public static int player;
        public static int MOVABLEWALL = 1;
        public static int PLAYERTYPE = 3;
        public static int PLAYERTEXTURENUM = 0;
        public static int PUSHABLEBLOCKTEXTURENUM=1;
        public static int ENTITYTEXTURENUM = 2;
        public static int PUSHABLEENTITYTEXTURENUM = 3;
        //public static int ENTITYTYPE = 5;
        public static int AIR = 2;
        public static int GOAL = 4;
        public static int ENTITYNUM = 23;
        public static int MOVEABLEENTITYNUM = ENTITYNUM + 4;//Four Directions;
        public static int BARRIER = 22;
        static int FIRSTBARRIERNUM = 6;//Have to change the images in blocktexturs to change this
        static int[] barrierRotation = { 0, 1, 2, 3, 4, 5, 1 +FIRSTBARRIERNUM, 2 + FIRSTBARRIERNUM, 4 + FIRSTBARRIERNUM, 3 + FIRSTBARRIERNUM, 0 + FIRSTBARRIERNUM, 7 + FIRSTBARRIERNUM, 9 + FIRSTBARRIERNUM, 10 + FIRSTBARRIERNUM, 5 + FIRSTBARRIERNUM, 6 + FIRSTBARRIERNUM, 8 + FIRSTBARRIERNUM, 15 + FIRSTBARRIERNUM, 12 + FIRSTBARRIERNUM, 11 + FIRSTBARRIERNUM, 13 + FIRSTBARRIERNUM, 14 + FIRSTBARRIERNUM };
        public static int[] Sin = { 0, 1, 0, -1, 0, 1, 0, -1,0 };//Last One zero for both sin and cos so that when 8 is put in direction is zero
        public static int[] Cos = {1, 0, -1, 0, 1, 0, -1,0,0 };
        public static int ZERODIRECTION = 8;
        //public static Point playerPos = new Point(0,0);


        public int[,] Blocks;

        //position
        public Vector2 position;
        public int rotation;
        bool hasMouse = false;

        //moving
        public Point startMousePosition;
        public Rectangle blockRect;
        Rectangle ghostRect;
        public bool isGhost;
        public bool IsGrabbed = false;

        //Cut
        bool IsCutLine = false;
        Rectangle CutLine;
        int cutLocation;
        bool isHorizontal;
        public Point originalLocation;
        Vector2 speed;





        //SpaceTimeLocation
        public void Initilize(ref int[,] theBlocks)
        {
            //Random random = new Random();
            originalLocation = new Point(0, 0);
            //int B = BARRIER;
            //int M = MOVABLEWALL;
            //int G = GOAL;
            //int A = AIR;
            //int P = PLAYERTYPE;
            //int R = ENTITYNUM;
            //int D = ENTITYNUM + 1;
            //int L = ENTITYNUM + 2;
            //int U = ENTITYNUM + 3;
            
            //int[,] theBlocks = {

            //    {P,A,A,A,A,A, A, B,B,B },
            //    {B,B,B,B,B,B, A, B,A,B },
            //    {R,A,A,A,A,A, A, A,A,B },
            //    {A,B,B,B,B,B, B, A,B,B },
            //    {A,A,A,A,B,A, A, A,A,A },
            //    {B,B,B,A,B,A, B, B,B,A },
            //    {B,A,A,A,B,A, B, B,A,A },
            //    {B,B,A,B,A,A, B, A,B,A },
            //    {A,A,A,B,A,B, B, A,B,A },
            //    {A,B,B,B,A,A, A, A,B,G }
            //};
            ////theBlocks = {

            //level 1
            //{ B,B,B,B,A,B, A, B,B,B },
            //        { B,B,B,B,A,B, A, B,G,B },
            //        { P,A,A,A,A,B, A, B,A,B },
            //        { B,B,B,B,B,B, A, B,A,B },
            //    };
            //level 2
            //{ B,B,B,A,A,A, B, A,B,B },
            //        { B,P,A,A,A,A, A, M,G,B },
            //        { B,B,B,A,A,A, B, A,B,B },
            //        { B,R,A,A,A,A, A, A,B,B },
            //        { B,B,B,A,A,A, B, B,B,B },
            //    };
            //level 3
            //{ B,B,B,A},
            //        { B,P,A,M},
            //        { B,B,B,M},
            //        { B,R,G,M},
            //        { B,B,B,M},
            //    };
            //level 4
            //{ B,B,B,B,B,B, B, B,B,B },
            //{ B,P,A,A,A,A, A, A,G,B },
            //{ B,B,B,B,B,B, B, B,M,B },
            //{ B,A,A,A,A,R, D, A,A,B },
            //{ M,M,M,M,M,M, M, B,A,B },
            //{ M,M,M,M,M,M, M, B,A,B },
            //{ M,M,M,M,M,M, M, B,A,B },
            //{ M,M,M,M,M,M, M, B,A,B },
            //{ M,M,M,M,M,M, M, B,U,B },
            //{ M,M,M,M,M,M, M, M,U,M },
            //};

            //{ P,R,R,R,R,B, A, B,A,B },
            //        { R,R,R,R,R,B, A, B,G,A },
            //        { R,R,R,R,R,A, A, A,A,A },
            //        { B,B,A,A,B,B, A, B,A,B },
            //    };
            //int x, y;//return x, y is useless
            ParseLevel(theBlocks);

        //int x = 10, y = 10;
        //Blocks = new int[x, y];
        //for (int i = 0; i < x; i++)
        //    for (int b = 0; b < y; b++)
        //    //if (i == 0 || i == x - 1 || b == 0 || b == y - 1)
        //    //    Blocks[i, b] = 1;
        //    //else
        //    {
        //        int rand = random.Next(1, 6);
        //        if (rand == 1)
        //            Blocks[i, b] = M;
        //        else if (rand == 5)
        //            Blocks[i, b] = R;// 2;
        //        else if (rand == 2)
        //            Blocks[i, b] = B;// 2;
        //        else
        //            Blocks[i, b] = A;
        //    }



        //Blocks[0, 0] = P;
        //Blocks[9, 9] = G;

        position = new Vector2(-Blocks.GetLength(0) *  blocksize / 2, -Blocks.GetLength(1) *  blocksize / 2);
            GetRect();
            //SpaceTimeLocation location = new SpaceTimeLocation();
            //location.time = 0;
            //location.gridnum = 0;
            //location.coords =
            CleanUpWallsAndEntitys();

            return;// location;
        }
        void ParseLevel(int[,] theBlocks)
        {
            int y = theBlocks.GetLength(0);//Reversed because 2d arrays fill columns first
            int x = theBlocks.GetLength(1);
            Blocks = new int[x, y];
            for (int i = 0; i < x; i++)
                for (int b = 0; b < y; b++)
                {
                    Blocks[i, b] = theBlocks[b, i];
                }
                    
            return; //return amount of en
        }
        public Grid GetCopy()
        {
            return (Grid)this.MemberwiseClone();
        }
        public void GetRect()
        {
            blockRect = new Rectangle(Game1.middleposition.ToPoint() + position.ToPoint(), new Point(Blocks.GetLength(0) *  blocksize, Blocks.GetLength(1) *  blocksize));

        }
        public void Connect()
        {
            position = ghostRect.Location.ToVector2() - Game1.middleposition;
        }
        public void Move(Point movement, ref SpaceTimeLocation location, ref SpaceTimeLocation destination)
        {

            
            if (movement.X +  location.X >= 0 && movement.Y +  location.Y >= 0 && movement.X +  location.X < Blocks.GetLength(0) && movement.Y +  location.Y < Blocks.GetLength(1))
            {
                if (Blocks[ location.X + movement.X,  location.Y + movement.Y] == AIR || Blocks[ location.X + movement.X,  location.Y + movement.Y] == GOAL)
                {

                     destination =  location + movement;
                }

            }
            else
            {
                for (int t = 0; t <  times.Length; t++)
                    for (int i = 0; i <  times[t].grids.Length; i++)
                    {
                        if ((i !=  location.gridnum || t !=  location.time) && ! times[t].grids[i].IsGrabbed && !times[location.time].grids[location.gridnum].IsGrabbed && RectanglesShareLine(blockRect,  times[t].grids[i].blockRect))
                        {
                            Rectangle Otherrect = new Rectangle(( times[t].grids[i].blockRect.Location - blockRect.Location),  times[t].grids[i].blockRect.Size);
                            if (Otherrect.X % blocksize == 0 && Otherrect.Y % blocksize == 0 && Otherrect.Contains((movement +  location.coords) * new Point( blocksize,  blocksize)) && ( times[t].grids[i].Blocks[ location.X + movement.X - Otherrect.X /  blocksize,  location.Y + movement.Y - Otherrect.Y /  blocksize] == AIR||  times[t].grids[i].Blocks[ location.X + movement.X - Otherrect.X /  blocksize,  location.Y + movement.Y - Otherrect.Y /  blocksize] == GOAL))
                            {

                                //index out of array

                                 destination =  location + (movement - Otherrect.Location / new Point( blocksize,  blocksize));
                                 destination.gridnum = i;
                                 destination.time = t;
                                //FOR DIRECTION OF MULTIPLE TIMES
                                // times[t].directionOfMovement =  times[ location.time].directionOfMovement;

                                t =  times.Length;//or just return

                                break;
                            }
                        }
                    }
            }
            if ( currentCircle >=  times[ destination.time].amounOfCircles +  times[ destination.time].starttime &&  destination.time + 1 <  times.Length)
            {
                
                    destination.time++;

                //index out of bounds
                if ( times[ destination.time].grids.Length <=  location.gridnum ||  times[ destination.time-1].grids[ destination.gridnum].originalLocation !=  times[ destination.time].grids[ destination.gridnum].originalLocation||  times[ destination.time].grids[ destination.gridnum].rotation !=  times[ destination.time-1].grids[ destination.gridnum].rotation ||  times[ destination.time].grids[ destination.gridnum].Blocks.GetLength(0) <=  destination.X ||  times[ destination.time].grids[ destination.gridnum].Blocks.GetLength(1) <=  destination.Y)
                {//length of destination does not contain gridnum                               //orignial loctaion not the same                                                                        //rotation not the smae                                                                    //length of gridnum smaller than x location//needs rotation check                                                  ////length of gridnum smaller than y location//needs rotation check
                    
                    Point OriginalLocationOfPoint = times[destination.time - 1].grids[destination.gridnum].GetOriginalLocationOfPoint(destination.coords);//gets where the entity would be if there had been no rotations or cuts
                    destination = times[destination.time].GetLocationFromOriginalPoint(OriginalLocationOfPoint,destination.time);//from the orginalLocation, gets the place where it would be now
                    
                }
            }

        }

        public void Update(Point MousePosition, int time, int g)
        {
            isGhost = false;
            cutLocation = 0;
            if (IsGrabbed)
            {
                speed = new Vector2(0, 0);
                position += MousePosition.ToVector2() - startMousePosition.ToVector2();
                startMousePosition = MousePosition;
                // Chat.NewLine(position.X.ToString()+ ", " + position.Y.ToString(), time);
                Point padding = new Point(blocksize / 2, blocksize / 2);
                Rectangle detectRect = new Rectangle(blockRect.Location - padding, blockRect.Size + padding + padding);

                for (int t = 0; t < times.Length; t++)
                    for (int i = 0; i < times[t].grids.Length; i++)
                    {
                        if (!times[t].grids[i].IsGrabbed && times[t].grids[i].blockRect.Intersects(detectRect))
                        {
                            ghostRect = new Rectangle(times[t].grids[i].blockRect.Location + new Point((int)Math.Round((double)(blockRect.X - times[t].grids[i].blockRect.X) / blocksize) * blocksize, (int)Math.Round((double)(blockRect.Y - times[t].grids[i].blockRect.Y) / blocksize) * blocksize), blockRect.Size);
                            isGhost = true;
                            if (ghostRect.Intersects(times[t].grids[i].blockRect))
                            {
                                int XstartEnd = (times[t].grids[i].blockRect.Left - blockRect.Right);
                                int YstartEnd = (times[t].grids[i].blockRect.Top - blockRect.Bottom);
                                int XEndstart = (times[t].grids[i].blockRect.Right - blockRect.Left);
                                int YEndstart = (times[t].grids[i].blockRect.Bottom - blockRect.Top);
                                if (Math.Abs(XstartEnd) < Math.Abs(YstartEnd) && Math.Abs(XstartEnd) < Math.Abs(XEndstart) && Math.Abs(XstartEnd) < Math.Abs(YEndstart))
                                    ghostRect.Location = new Point(blockRect.Location.X + XstartEnd, ghostRect.Location.Y);
                                else if (Math.Abs(YstartEnd) < Math.Abs(XEndstart) && Math.Abs(YstartEnd) < Math.Abs(YEndstart))
                                    ghostRect.Location = new Point(ghostRect.Location.X, blockRect.Location.Y + YstartEnd);
                                else if (Math.Abs(XEndstart) < Math.Abs(YEndstart))
                                    ghostRect.Location = new Point(blockRect.Location.X + XEndstart, ghostRect.Location.Y);
                                else
                                    ghostRect.Location = new Point(ghostRect.Location.X, blockRect.Location.Y + YEndstart);
                                //isGhost = false;
                            }

                            // Chat.NewLine(position.X.ToString() + ", " + position.Y.ToString(),  time);
                            t = times.Length;
                            break;
                        }
                    }
            }

            position += speed;
            if (speed.X != 0 || speed.Y != 0)
            {
                speed.X *= speed.X > 0.01f? 0.75f :0f;

                speed.Y *= speed.Y > 0.01f ? 0.75f : 0f;

            }

            GetRect();
            IsCutLine = false;
            hasMouse = false;
            if (blockRect.Contains(MousePosition))
            {
                anyGridHasMouse = true;
                hasMouse = true;
                Point relativeMouse = MousePosition - blockRect.Location;

                if (Level.IsGrabbing)
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
                else if (Level.IsCutting)
                {


                    IsCutLine = true;
                    int roundedX = (int)Math.Round((float)relativeMouse.X / blocksize);
                    int roundedY = (int)Math.Round((float)relativeMouse.Y / blocksize);
                    if (roundedX > 0 && roundedX < blockRect.Width /  blocksize && Math.Abs(roundedX *  blocksize - relativeMouse.X) <= Math.Abs(roundedY *  blocksize - relativeMouse.Y))
                    {
                        CutLine = new Rectangle(blockRect.Location + new Point(roundedX *  blocksize - 1, 0), new Point(2, blockRect.Height));//vertical
                        cutLocation = roundedX;
                        isHorizontal = false;
                        IsValidCut = true;
                        for (int i = 0; i < Blocks.GetLength(1); i++)
                            if (Blocks[roundedX-1,i] >= FIRSTBARRIERNUM && Blocks[roundedX, i] >= FIRSTBARRIERNUM)
                            {
                                IsValidCut = false;
                                break;
                            }
                    }
                    else if (roundedY > 0 && roundedY < blockRect.Height /  blocksize)
                    {
                        CutLine = new Rectangle(blockRect.Location + new Point(0, roundedY *  blocksize - 1), new Point(blockRect.Width, 2));//horizontal
                        cutLocation = roundedY;
                        isHorizontal = true;
                        IsValidCut = true;
                        for (int i = 0; i < Blocks.GetLength(0); i++)
                            if (Blocks[i,roundedY-1] >= FIRSTBARRIERNUM && Blocks[i, roundedY] >= FIRSTBARRIERNUM)
                            {
                                IsValidCut = false;
                                break;
                            }
                    }
                    else
                        IsCutLine = false;
                    //Convert.ToInt32(relativeMouse.X/blocksize)
                    // Chat.NewLine("true " + relativeMouse.X.ToString() + ", " + relativeMouse.Y.ToString(),  time, true);
                    //new WholeCoords((short)((MousePosition.X + position.X - GraphicsRectangle.Width / 2 + blocksize / 2) / blocksize), (short)((MousePosition.Y + position.Y - GraphicsRectangle.Height / 2 + blocksize / 2) / blocksize));
                }
                int flooredX = (int)Math.Floor((float)relativeMouse.X / blocksize);
                int flooredY = (int)Math.Floor((float)relativeMouse.Y / blocksize);
                mouseOriginalPosition = GetOriginalLocationOfPoint(flooredX, flooredY);
                LocationsOfSelectedBlock = GetAllLocationsOfOriginalPoint(new SpaceTimeLocation(time, g, flooredX, flooredY));

            }
            
        }
        public void Grab(Point MousePosition)
        {
            IsGrabbed = true;
            startMousePosition = MousePosition;
        }
        public void Rotate(int t, int index)
        {


            int[,] newBlocks = new int[Blocks.GetLength(1), Blocks.GetLength(0)];
            rotation += 1;
            if (rotation >= 4)
                rotation = 0;
            for (int x = 0; x < Blocks.GetLength(0); x++)
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    newBlocks[Blocks.GetLength(1) - 1 - y, x] = barrierRotation[Blocks[x, y]];
                    //newBlocks[Blocks.GetLength(1) - 1 - y, x] = Blocks[x, y];rotatee position

                    //newBlocks[Blocks.GetLength(1) - 1 - y, x] = barrierRotation[newBlocks[Blocks.GetLength(1) - 1 - y, x]]; //roatate barriers
                }
            for (int i = 0; i < entitys.Length; i++)
            {
                entitys[i].Rotate(t,index, Blocks.GetLength(1));
            }

            Blocks = new int[Blocks.GetLength(1), Blocks.GetLength(0)];
            Blocks = newBlocks;
            int deltapos = Blocks.GetLength(1) - Blocks.GetLength(0);
            position +=  blocksize / 2 * (new Vector2(deltapos, -deltapos));
            GetRect();
            //CleanUpWallsAndEntitys(1);


            totalRotations++;
        }
        public void Cut(int t, int index)
        {
            float cutSpeed = 5;
            if (cutLocation > 0&& IsValidCut)
            {
                int x = Blocks.GetLength(0);
                int y = Blocks.GetLength(1);
                Array.Resize(ref  times[t].grids,  times[t].grids.Length + 1);
                 times[t].grids[ times[t].grids.Length - 1] = new Grid();



                 times[t].grids[ times[t].grids.Length - 1].rotation = rotation;
                int totalRotation = (int)rotation + Convert.ToInt32(!isHorizontal);
                Point translation;
                ref Point ogGetter = ref  times[t].grids[ times[t].grids.Length - 1].originalLocation;
                ref Point ogConstant = ref originalLocation;
                int length = isHorizontal ? y : x;
                if (totalRotation == 2 || totalRotation == 3)
                {
                    if (totalRotation == 2)
                        translation = new Point(0, length);
                    else
                        translation = new Point(length, 0);
                     ogGetter = ref originalLocation;
                     ogConstant = ref  times[t].grids[ times[t].grids.Length - 1].originalLocation;

                }
                else
                    translation = new Point(0, 0);


                //need to make sure the right one gets the og location
                // times[t].grids[ times[t].grids.Length - 1].originalLocation =
                Point originalLocationSaved = originalLocation;
                ogGetter = originalLocation + new Point(Sin[totalRotation] * cutLocation, Cos[totalRotation] * cutLocation) + translation;
                ogConstant = originalLocationSaved;
                Game1.Chat.NewLine(totalRotation.ToString(), Game1.time);
                Game1.Chat.NewLine( times[t].grids[ times[t].grids.Length - 1].originalLocation.ToString(), Game1.time);
                Game1.Chat.NewLine(originalLocation.ToString(), Game1.time);




                if (isHorizontal)
                {
                    y -= cutLocation;
                     times[t].grids[ times[t].grids.Length - 1].Blocks = new int[x, y];
                     times[t].grids[ times[t].grids.Length - 1].position = position + new Vector2(0, cutLocation *  blocksize);
                    times[t].grids[times[t].grids.Length - 1].speed.Y = cutSpeed;
                    for (int i = 0; i < x; i++)
                        for (int b = 0; b < y; b++)
                        {
                             times[t].grids[ times[t].grids.Length - 1].Blocks[i, b] = Blocks[i, b + cutLocation];


                        }

                    int[,] newBlocks = new int[x, cutLocation];
                    for (int i = 0; i < x; i++)
                        for (int b = 0; b < cutLocation; b++)
                            newBlocks[i, b] = Blocks[i, b];
                    Blocks = newBlocks;

                    // times[t].grids[ times[t].grids.Length - 1].originalLocation = originalLocation + new Point(cutLocation, 0);

                }
                else
                {
                    x -= cutLocation;
                     times[t].grids[ times[t].grids.Length - 1].Blocks = new int[x, y];
                     times[t].grids[ times[t].grids.Length - 1].position = position + new Vector2(cutLocation *  blocksize, 0);
                    times[t].grids[times[t].grids.Length - 1].speed.X = cutSpeed;
                    for (int i = 0; i < x; i++)
                        for (int b = 0; b < y; b++)
                        {
                            times[t].grids[times[t].grids.Length - 1].Blocks[i, b] = Blocks[i + cutLocation, b];
                        }

                    int[,] newBlocks = new int[cutLocation, y];
                    for (int i = 0; i < cutLocation; i++)
                        for (int b = 0; b < y; b++)
                            newBlocks[i, b] = Blocks[i, b];
                    Blocks = newBlocks;

                    // times[t].grids[ times[t].grids.Length - 1].originalLocation = originalLocation + new Point(cutLocation, 0);
                }
                for (int e = 0; e < entitys.Length; e++)
                {
                    entitys[e].Cut(t, index, cutLocation, isHorizontal);
                }

                times[t].grids[ times[t].grids.Length - 1].GetRect();


                totalCuts++;


            }



        }

        public Point GetOriginalLocationOfPoint(int x, int y)
        {
            return GetOriginalLocationOfPoint(new Point(x, y));
        }
        public Point GetOriginalLocationOfPoint(Point originalLocationOfPoint)//gets the 
        {

            int xlength = Blocks.GetLength(0);
            int ylength = Blocks.GetLength(1);
            for (int i = 0; i < rotation; i++)//-1 for the loctation instead of destination 
            {
                originalLocationOfPoint = new Point(originalLocationOfPoint.Y, xlength - 1 - originalLocationOfPoint.X);//reverse rotation
                int hold = xlength;
                xlength = ylength;
                ylength = hold;
            }
            return originalLocationOfPoint + originalLocation;

        }
        public bool RectanglesShareLine(Rectangle rect1, Rectangle rect2)
        {
            // Check if the rectangles do not share a line
            if (rect1.Right < rect2.Left || rect2.Right < rect1.Left ||
                rect1.Bottom < rect2.Top || rect2.Bottom < rect1.Top)
            {
                return false;
            }

            // Otherwise, the rectangles must share at least one line
            return true;
        }

        public void Draw(SpriteBatch _spriteBatch, Color colorclear, int b, int t)
        {
            if (isGhost)
                Game1.DrawRectangle(_spriteBatch, ghostRect, colorclear);
            for (int x = 0; x < Blocks.GetLength(0); x++)
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    if (Blocks[x, y] != 0)
                        _spriteBatch.Draw(Game1.BlocksTexture[Blocks[x, y]], blockRect.Location.ToVector2() + new Vector2(x *  blocksize, y *  blocksize), colorclear);

                }
            //Draw entitys here so that they get drawn in the right order
            for (int e = 0; e < entitys.Length; e++)
                entitys[e].Draw(_spriteBatch, colorclear, entitys[e].imageNum,t,b);




        }
        public void DrawSelected(SpriteBatch _spriteBatch, SpaceTimeLocation selected)
        {
            Rectangle rect = new Rectangle(blockRect.Location + new Point(selected.X*blocksize,selected.Y*blocksize), new Point (blocksize,blocksize));

            Game1.DrawRectangle(_spriteBatch, rect, new Color(0,0,0,50));
            
        }
        public void DrawCutLine(SpriteBatch _spriteBatch, Color colorclear)
        {
            if (IsCutLine)
            {
                Color color = IsValidCut ? colorclear : Color.Red;
                Game1.DrawRectangle(_spriteBatch, CutLine, color);
            }
        }


        public Point CleanUpWallsAndEntitys()//returns player location
        {
            int numberOfEntitys = 1; 
            int[,] Newblocks = new int[Blocks.GetLength(0), Blocks.GetLength(1)];
            for (int x = 0; x < Blocks.GetLength(0); x++)
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    if (Blocks[x, y] == BARRIER)
                    {
                        int adjacentNum = 0;
                        int blockNum = 0;
                        if (x > 0 && Blocks[x - 1, y] == BARRIER)
                            adjacentNum += 1;
                        if (y > 0 && Blocks[x, y - 1] == BARRIER)
                        {
                            adjacentNum += 1;
                            blockNum += 1;
                        }
                        if (x < Blocks.GetLength(0) - 1 && Blocks[x + 1, y] == BARRIER)
                        {
                            adjacentNum += 1;
                            blockNum += 2;
                        }
                        if (y < Blocks.GetLength(1) - 1 && Blocks[x, y + 1] == BARRIER)
                        {
                            adjacentNum += 1;
                            blockNum += 4;
                        }
                        if (adjacentNum == 0)
                            blockNum = 3;
                        else if (adjacentNum == 2)
                            blockNum += 4;
                        else if (adjacentNum == 3)
                            blockNum += 8;
                        else if (adjacentNum == 4)
                            blockNum = 12;
                        Newblocks[x, y] = blockNum;
                    }
                    else if((Blocks[x, y] >= ENTITYNUM && Blocks[x, y] < ENTITYNUM + 4) || Blocks[x, y] >= MOVEABLEENTITYNUM && Blocks[x, y] < MOVEABLEENTITYNUM + 4|| Blocks[x, y] == MOVABLEWALL)
                    {
                        numberOfEntitys += 1;
                    }
                }
            Entity playerEntity = null;
            Entity[] actualEntitys = new Entity[numberOfEntitys];
            Entity[] moveableAndPushableEntitys = new Entity[numberOfEntitys];
            int numberOfmoveableAndPushableEntitys = 0;
            entitys = new Entity[numberOfEntitys];
            numberOfEntitys = 0;
            int numberOfActualEntitys = 0;
            Point playerCoords = new Point();
            for (int x = 0; x < Blocks.GetLength(0); x++)
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    if (Blocks[x, y] == BARRIER)
                        Blocks[x, y] = Newblocks[x, y] + FIRSTBARRIERNUM;
                    else if (Blocks[x, y] == PLAYERTYPE)
                    {
                        //entitys[PLAYERENTITY] = new Entity(x,y);
                        playerEntity = new MoveableEntity(x, y,PLAYERTEXTURENUM);
                        Blocks[x, y] = 2;
                        playerCoords = new Point(x, y);
                    }
                    else if(Blocks[x, y] >= ENTITYNUM && Blocks[x, y] < ENTITYNUM+4)
                    {
                        actualEntitys[numberOfActualEntitys] = new MoveableEntity(x, y,ENTITYTEXTURENUM, Blocks[x, y] - ENTITYNUM);
                        //entitys[numberOfEntitys] = new Entity(x,y, Blocks[x, y]-ENTITYNUM);
                        Blocks[x, y] = 2;
                        numberOfActualEntitys++;
                        //numberOfEntitys++;
                    }
                    else if (Blocks[x, y] >= MOVEABLEENTITYNUM && Blocks[x, y] < MOVEABLEENTITYNUM + 4)
                    {
                        moveableAndPushableEntitys[numberOfmoveableAndPushableEntitys] = new MoveablePushableEntity(x, y, Blocks[x, y] - MOVEABLEENTITYNUM);
                        //entitys[numberOfEntitys] = new Entity(x,y, Blocks[x, y]-ENTITYNUM);
                        Blocks[x, y] = 2;
                        numberOfmoveableAndPushableEntitys++;
                        //numberOfEntitys++;
                    }
                    else if (Blocks[x, y] == MOVABLEWALL)
                    {
                        Blocks[x, y] = 2;
                        entitys[numberOfEntitys] = new PushableEntity(x, y);
                        numberOfEntitys++;
                    }

                }
            entitys[numberOfEntitys] = playerEntity;
            PLAYERENTITY = numberOfEntitys;
            numberOfEntitys++;
            for (int i = 0; i < numberOfActualEntitys; i++)
            {
                entitys[numberOfEntitys + i] = actualEntitys[i];
            }
            for (int i = 0; i < numberOfmoveableAndPushableEntitys; i++)
            {
                entitys[numberOfEntitys + i+numberOfActualEntitys] = moveableAndPushableEntitys[i];
            }
            originalEntitys = new Entity[entitys.Length];
            for (int i = 0; i < originalEntitys.Length; i++)
            {
                originalEntitys[i] = entitys[i].GetCopy();
            }
            return playerCoords;
        }
    }
    
}
