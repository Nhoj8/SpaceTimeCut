using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SpaceTimeCut
{
    [Serializable]
    public class Times
    {
        
        public static Texture2D circle;   
        public static bool timeBarHasMouse;
        //public static int player = 0;
        public static int lengthpercircle = 80;
        public static int height = 16;
        public static int TotalamounOfCircles = 10;

        public static SpaceTimeLocation destination;// = new SpaceTimeLocation(0, 0, 1, 0);
        public static SpaceTimeLocation Location;// = new SpaceTimeLocation(0,0,0,0);
        public static float Progress;
        public static int DirectionOfMovement;
        public static int timeperblock = 30; 

        public struct SpaceTimeLocation
        {
            public int time;
            public int block;
            public Point Coords;
            public SpaceTimeLocation(int time, int block,Point Coords)
            {
            this.time = time;
                this.block=block;
                this.Coords=Coords;
        }
            public SpaceTimeLocation(int time, int block, int X, int Y)
            {
                this.time = time;
                this.block = block;
                this.Coords.X = X;
                this.Coords.Y = Y;
            }
            public int X
            {
                get { return this.Coords.X; }
                set { this.Coords.X = value; }
            }
            public int Y
            {
                get { return this.Coords.Y; }
                set { this.Coords.Y = value; }
            }
            public static SpaceTimeLocation operator +(SpaceTimeLocation Dc1, Point dv)
            {
                Dc1.X += dv.X;
                Dc1.Y += dv.Y;
                return Dc1;
            }


        }

        public Rectangle timeBarRect;
        public int starttime = 0;
        public int amounOfCircles =10;

        public int cutLocation = -1;

        public Level[] level;
        int circleselected = -1;

        public void initilize()
        {
            level = new Level[1];
            level[0] = new Level();
            level[0].initilize();
            if (circle == null)
                circle = SpriteBatchExtensions.Resize(Game1._graphics.GraphicsDevice, Game1.Getborders(Game1.CreateCircle(Game1._graphics.GraphicsDevice, 8), 1, Color.Black, 0, false), 2);
            timeBarRefersh();
        }
        public void Move(Point movement)
        {
            level[Location.block].Move(movement);
        }
        public static void MoveTime()
        {
            Progress += 1 / (float)timeperblock;
            if (Progress >= 1)
            {
                Progress = 0;
                Location = destination;
                double direction = Times.DirectionOfMovement * Math.PI / 2;
                Game1.times[Times.Location.time].Move(new Point((int)Math.Cos(direction), (int)Math.Sin(direction)));
            }
        }
        public void release()
        {
            for (int i = 0; i < level.Length; i++)
            {
                if (level[i].IsGrabbed && level[i].isGhost)
                    level[i].connect();

                level[i].IsGrabbed = false;
            }
        }
        public bool press(int t, Point MousePosition)
        {
            bool hasMouse = false;
            if (timeBarRect.Contains(MousePosition)&& cutLocation > -1)
            {
                hasMouse = true;
                Cut(t);
            }


            for (int i = level.Length - 1; i > -1; i--)
            {
                if (level[i].blockRect.Contains(MousePosition))
                {
                    hasMouse = true;
                    if (Level.IsGrabbing)
                    {
                        level[i].Grab(MousePosition);
                        break;
                    }

                    else if (Level.IsCutting)
                    {
                        level[i].Cut(t,i);
                        break;
                    }
                    else if (Level.IsRotating)
                    {
                        level[i].Rotate(t,i);
                        break;
                    }
                    


                }
            }
            return hasMouse;
        }
        void timeBarRefersh()
        {
            timeBarRect = new Rectangle(Button.PositionAnchors[Button.BOTTOMMIDDLE].ToPoint() + new Point((starttime) * lengthpercircle, -100) - new Point(lengthpercircle * (TotalamounOfCircles - 1) / 2, height / 2), new Point(lengthpercircle * (amounOfCircles - 1) + circle.Width, height));

        }

        public void Update(Point MousePosition)
        {
            timeBarRefersh();
            timeBarHasMouse = false;
            circleselected = -1;
            cutLocation = -1;
            if (timeBarRect.Contains(MousePosition))
            {
                timeBarHasMouse = true;

                for (int i = 0; i < amounOfCircles; i++)
                {
                    if (new Rectangle(timeBarRect.Location + new Point((int)(lengthpercircle * i), 0), circle.Bounds.Size).Contains(MousePosition))
                    {
                        circleselected = i;
                        break;
                    }
                    else if (new Rectangle(timeBarRect.Location + new Point((int)(lengthpercircle * i + circle.Width), 0), new Point((int)(lengthpercircle - circle.Width), circle.Height)).Contains(MousePosition))
                    {
                        cutLocation = i;
                        break;
                    }
                }
            }
            for (int i = 0; i < level.Length; i++)
                level[i].Update(MousePosition);
        }
        void Cut(int t)
        {
            Array.Resize(ref Game1.times, Game1.times.Length+1);

            Game1.times[Game1.times.Length - 1] = (Times) this.MemberwiseClone();
            Game1.times[Game1.times.Length - 1].level = new Level[level.Length];

            for (int i = 0; i < level.Length; i++)
            {
                Game1.times[Game1.times.Length - 1].level[i] = level[i].GetCopy();
                Game1.times[Game1.times.Length - 1].level[i].Blocks = (int[,]) level[i].Blocks.Clone();
            }

            applyOffset(new Vector2(-300, 0));
            Game1.times[Game1.times.Length - 1].applyOffset(new Vector2(300, 0));

            amounOfCircles = cutLocation +1;
            Game1.times[Game1.times.Length - 1].amounOfCircles -= cutLocation+1;
            Game1.times[Game1.times.Length - 1].starttime = starttime + cutLocation +1;
            if (t == Location.time)
            {
                Game1.times[Game1.times.Length - 1].level[Location.block].Blocks[Location.X, Location.Y] =1;
                //Level.player = -1;
            }

        }


        void applyOffset(Vector2 offset)
        {
            for (int i = 0; i < level.Length; i++)
                level[i].position += offset;
        }

        public void Draw(SpriteBatch _spriteBatch, Color colorclear, int t)
        {
            for (int b = 0; b < level.Length; b++)
                level[b].Draw(_spriteBatch, colorclear,b,t);


            Rectangle timeRect = new Rectangle(timeBarRect.Location + new Point(0, timeBarRect.Height / 2 - 2 / 2), new Point(lengthpercircle * (amounOfCircles-1) +circle.Width, 2));
            
            
            //float deltaX = (float)(lengthpercircle * (TotalamounOfCircles-1))/ (TotalamounOfCircles - 1);
            for (int i = 0; i < amounOfCircles; i++)
            {
                Color color = i == circleselected ? Color.Red : colorclear;
                _spriteBatch.Draw(circle, timeBarRect.Location.ToVector2() + new Vector2((int)(lengthpercircle * i), 0), color);
            }

            if (cutLocation > -1)
                Game1.DrawRectangle(_spriteBatch, new Rectangle(timeBarRect.Location + new Point((int)(cutLocation* lengthpercircle + lengthpercircle / 2 + circle.Width/2),0), new Point(2, timeBarRect.Height)), colorclear);
            Game1.DrawRectangle(_spriteBatch, timeRect, Color.Red);
        }

    }
    //public static class Copy
    //{//https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net
    //    public static T DeepClone<T>(this T obj)
    //    {
    //        using (var ms = new MemoryStream())
    //        {
    //            var formatter = new BinaryFormatter();
    //            formatter.Serialize(ms, obj);
    //            ms.Position = 0;

    //            return (T)formatter.Deserialize(ms);
    //        }
    //    }
    //}

    public class Level
    {
        public static bool IsGrabbing = true;
        public static bool IsCutting;
        public static bool IsRotating;
        public static int timeBar = 0;
        //public static int player;
        public static int PLAYERTYPE = 3;
        public static int AIR = 2;
        //public static Point playerPos = new Point(0,0);


        public int[,] Blocks;
        public Vector2 position;
        public Point startMousePosition;
        public Rectangle blockRect;
        Rectangle ghostRect;
        public bool isGhost;
        bool IsCutLine = false;
        Rectangle CutLine;
        public bool IsGrabbed = false;
        int cutLocations;
        bool isHorizontal;
        float Rotation;


        public void initilize(int x = 10, int y = 10)
        {
            Random random = new Random();
            Blocks = new int[x, y];
            for (int i = 0; i < x; i++)
                for (int b = 0; b < y; b++)
                    Blocks[i, b] = random.Next(1, 3);
            Blocks[0, 0] = 3;
            position = new Vector2(-Blocks.GetLength(0) * Game1.blocksize / 2, -Blocks.GetLength(1) * Game1.blocksize / 2);
            GetRect();
            CleanUpWalls(1);
        }
        public Level GetCopy()
        {
            return (Level)this.MemberwiseClone();
        }
        public void GetRect()
        {
            blockRect = new Rectangle(Game1.middleposition.ToPoint() + position.ToPoint(), new Point(Blocks.GetLength(0) * Game1.blocksize, Blocks.GetLength(1) * Game1.blocksize));

        }
        public void connect()
        {
            position = ghostRect.Location.ToVector2() - Game1.middleposition;
        }
        public void Move(Point movement)
        {
            if (movement.X + Times.Location.X >= 0 && movement.Y + Times.Location.Y >= 0 && movement.X + Times.Location.X < Blocks.GetLength(0) && movement.Y + Times.Location.Y < Blocks.GetLength(1))
            {
                if (Blocks[Times.Location.X + movement.X, Times.Location.Y + movement.Y] == AIR)
                {
                    //Blocks[playerPos.X, playerPos.Y] = Blocks[playerPos.X + movement.X, playerPos.Y + movement.Y];
                    //Blocks[playerPos.X + movement.X, playerPos.Y + movement.Y] = PLAYERTYPE;
                    //playerPos += movement;
                    Times.destination = Times.Location + movement;
                }

            }
            else
            {
                for (int t = 0; t < Game1.times.Length; t++)
                    for (int i = 0; i < Game1.times[t].level.Length; i++)
                    {
                        if ((i != Times.Location.block || t != Times.Location.time) && !Game1.times[t].level[i].IsGrabbed && RectanglesShareLine(blockRect, Game1.times[t].level[i].blockRect))
                        {
                            Rectangle Otherrect = new Rectangle((Game1.times[t].level[i].blockRect.Location - blockRect.Location), Game1.times[t].level[i].blockRect.Size);
                            if (Otherrect.Contains((movement + Times.Location.Coords) * new Point(Game1.blocksize, Game1.blocksize)) && Game1.times[t].level[i].Blocks[Times.Location.X + movement.X - Otherrect.X / Game1.blocksize, Times.Location.Y + movement.Y - Otherrect.Y / Game1.blocksize] == AIR)
                            {

                                //index out of array

                                //Blocks[playerPos.X, playerPos.Y] = Game1.times[t].level[i].Blocks[playerPos.X + movement.X - Otherrect.X / Game1.blocksize, playerPos.Y + movement.Y - Otherrect.Y / Game1.blocksize];
                                //Game1.times[t].level[i].Blocks[playerPos.X + movement.X - Otherrect.X / Game1.blocksize, playerPos.Y + movement.Y - Otherrect.Y / Game1.blocksize] = PLAYERTYPE;
                                //playerPos += movement - Otherrect.Location / new Point(Game1.blocksize, Game1.blocksize);
                                //player = i;
                                //Times.player = t;
                                Times.destination = Times.Location + (movement - Otherrect.Location / new Point(Game1.blocksize, Game1.blocksize));
                                Times.destination.block = i;
                                Times.destination.time = t;
                                t = Game1.times.Length;//or just return
                                break;
                            }
                        }
                    }
            }

        }

        public void Update(Point MousePosition)
        {
            isGhost = false;
            cutLocations = 0;
            if (IsGrabbed)
            {
                position += MousePosition.ToVector2() - startMousePosition.ToVector2();
                startMousePosition = MousePosition;
                //Game1.Chat.NewLine(position.X.ToString()+ ", " + position.Y.ToString(),Game1.time);
                Point padding = new Point(Game1.blocksize, Game1.blocksize);
                Rectangle detectRect = new Rectangle(blockRect.Location - padding, blockRect.Size + padding + padding);

                for (int t = 0; t < Game1.times.Length; t++)
                    for (int i = 0; i < Game1.times[t].level.Length; i++)
                    {
                        if (!Game1.times[t].level[i].IsGrabbed && Game1.times[t].level[i].blockRect.Intersects(detectRect))
                        {
                            ghostRect = new Rectangle(Game1.times[t].level[i].blockRect.Location + new Point(((blockRect.X - Game1.times[t].level[i].blockRect.X) / 32) * 32, ((blockRect.Y - Game1.times[t].level[i].blockRect.Y) / 32) * 32), blockRect.Size);
                            isGhost = true;
                            if (ghostRect.Intersects(Game1.times[t].level[i].blockRect))
                            {
                                int XstartEnd = (Game1.times[t].level[i].blockRect.Left - blockRect.Right);
                                int YstartEnd = (Game1.times[t].level[i].blockRect.Top - blockRect.Bottom);
                                int XEndstart = (Game1.times[t].level[i].blockRect.Right - blockRect.Left);
                                int YEndstart = (Game1.times[t].level[i].blockRect.Bottom - blockRect.Top);
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

                            Game1.Chat.NewLine(position.X.ToString() + ", " + position.Y.ToString(), Game1.time);
                            t = Game1.times.Length;
                            break;
                        }
                    }
            }
            GetRect();
            IsCutLine = false;

            if (blockRect.Contains(MousePosition))
            {
                if (IsGrabbing)
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
                else if (IsCutting)
                {


                    IsCutLine = true;
                    Point relativeMouse = MousePosition - blockRect.Location;
                    int roundedX = (int)Math.Round((float)relativeMouse.X / Game1.blocksize);
                    int roundedY = (int)Math.Round((float)relativeMouse.Y / Game1.blocksize);
                    if (roundedX > 0 && roundedX < blockRect.Width / Game1.blocksize && Math.Abs(roundedX * Game1.blocksize - relativeMouse.X) <= Math.Abs(roundedY * Game1.blocksize - relativeMouse.Y))
                    {
                        CutLine = new Rectangle(blockRect.Location + new Point(roundedX * Game1.blocksize - 1, 0), new Point(2, blockRect.Height));
                        cutLocations = roundedX;
                        isHorizontal = true;
                    }
                    else if (roundedY > 0 && roundedY < blockRect.Height / Game1.blocksize)
                    {
                        CutLine = new Rectangle(blockRect.Location + new Point(0, roundedY * Game1.blocksize - 1), new Point(blockRect.Width, 2));
                        cutLocations = roundedY;
                        isHorizontal = false;
                    }

                    else
                        IsCutLine = false;
                    //Convert.ToInt32(relativeMouse.X/blocksize)
                    //Game1.Chat.NewLine("true " + relativeMouse.X.ToString() + ", " + relativeMouse.Y.ToString(), Game1.time, true);
                    //new WholeCoords((short)((MousePosition.X + position.X - GraphicsRectangle.Width / 2 + blocksize / 2) / blocksize), (short)((MousePosition.Y + position.Y - GraphicsRectangle.Height / 2 + blocksize / 2) / blocksize));
                }

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
            Rotation += 1;
            if (Rotation >= 3)
                Rotation = 0;
            for (int x = 0; x < Blocks.GetLength(0); x++)
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    newBlocks[Blocks.GetLength(1) - 1 - y, x] = Blocks[x, y];
                }
            if (Times.Location.time == t && Times.Location.block == index)
            {
                Times.Location.Coords = new Point(Blocks.GetLength(1) - 1 - Times.Location.Coords.Y, Times.Location.Coords.X);
                //Times.DirectionOfMovement -= (float)(Math.PI / 2);
                //Times.DirectionOfMovement = ((int)(Times.DirectionOfMovement * 1000)) / 1000;
                //if (Times.DirectionOfMovement <= -(float)(Math.PI / 2))
                //    Times.DirectionOfMovement = (float)(3 * Math.PI / 2);
                Times.DirectionOfMovement += 1;

                if (Times.DirectionOfMovement >= 4)
                    Times.DirectionOfMovement = 0;
            }

            if (Times.destination.time == t && Times.destination.block == index)
            {
                Times.destination.Coords = new Point(Blocks.GetLength(1) - 1 - Times.destination.Coords.Y, Times.destination.Coords.X);
            }
            Blocks = new int[Blocks.GetLength(1), Blocks.GetLength(0)];
            Blocks = newBlocks;
            int deltapos = Blocks.GetLength(1) - Blocks.GetLength(0);
            position += Game1.blocksize / 2 * (new Vector2(deltapos, -deltapos));
            GetRect();

        }
        public void Cut(int t, int index)
        {
            if (cutLocations > 0)
            {
                int x = Blocks.GetLength(0);
                int y = Blocks.GetLength(1);
                Array.Resize(ref Game1.times[t].level, Game1.times[t].level.Length + 1);
                Game1.times[t].level[Game1.times[t].level.Length - 1] = new Level();
                if (!isHorizontal)
                {
                    y -= cutLocations;
                    Game1.times[t].level[Game1.times[t].level.Length - 1].Blocks = new int[x, y];
                    Game1.times[t].level[Game1.times[t].level.Length - 1].position = position + new Vector2(0, cutLocations * Game1.blocksize);
                    for (int i = 0; i < x; i++)
                        for (int b = 0; b < y; b++)
                        {
                            Game1.times[t].level[Game1.times[t].level.Length - 1].Blocks[i, b] = Blocks[i, b + cutLocations];
                            if (Times.Location.time == t && Times.Location.block == index && Times.Location.Coords == new Point(i, b + cutLocations)) //Blocks[i, b + cutLocations] == PLAYERTYPE)
                            {
                                //playerPos = new Point(i, b);
                                //player = Game1.times[t].level.Length - 1;
                                Times.Location.Coords = new Point(i, b);
                                Times.Location.block = Game1.times[t].level.Length - 1;
                            }
                            if (Times.destination.time == t && Times.destination.block == index && Times.destination.Coords == new Point(i, b + cutLocations)) //Blocks[i, b + cutLocations] == PLAYERTYPE)
                            {
                                //playerPos = new Point(i, b);
                                //player = Game1.times[t].level.Length - 1;
                                Times.destination.Coords = new Point(i, b);
                                Times.destination.block = Game1.times[t].level.Length - 1;
                            }


                        }

                    int[,] newBlocks = new int[x, cutLocations];
                    for (int i = 0; i < x; i++)
                        for (int b = 0; b < cutLocations; b++)
                            newBlocks[i, b] = Blocks[i, b];
                    Blocks = newBlocks;

                }
                else
                {
                    x -= cutLocations;
                    Game1.times[t].level[Game1.times[t].level.Length - 1].Blocks = new int[x, y];
                    Game1.times[t].level[Game1.times[t].level.Length - 1].position = position + new Vector2(cutLocations * Game1.blocksize, 0);
                    for (int i = 0; i < x; i++)
                        for (int b = 0; b < y; b++)
                        {
                            Game1.times[t].level[Game1.times[t].level.Length - 1].Blocks[i, b] = Blocks[i + cutLocations, b];
                            //if (Blocks[i + cutLocations, b] == PLAYERTYPE)
                            //{
                            //    playerPos = new Point(i, b);
                            //    player = Game1.times[t].level.Length - 1;
                            //}
                            if (Times.Location.time == t && Times.Location.block == index && Times.Location.Coords == new Point(i + cutLocations, b)) //Blocks[i, b + cutLocations] == PLAYERTYPE)
                            {
                                //playerPos = new Point(i, b);
                                //player = Game1.times[t].level.Length - 1;
                                Times.Location.Coords = new Point(i, b);
                                Times.Location.block = Game1.times[t].level.Length - 1;
                            }
                            if (Times.destination.time == t && Times.destination.block == index && Times.destination.Coords == new Point(i + cutLocations, b)) //Blocks[i, b + cutLocations] == PLAYERTYPE)
                            {
                                //playerPos = new Point(i, b);
                                //player = Game1.times[t].level.Length - 1;
                                Times.destination.Coords = new Point(i, b);
                                Times.destination.block = Game1.times[t].level.Length - 1;
                            }
                        }

                    int[,] newBlocks = new int[cutLocations, y];
                    for (int i = 0; i < cutLocations; i++)
                        for (int b = 0; b < y; b++)
                            newBlocks[i, b] = Blocks[i, b];
                    Blocks = newBlocks;


                }


                Game1.times[t].level[Game1.times[t].level.Length - 1].GetRect();





            }



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
                        _spriteBatch.Draw(Game1.BlocksTexture[Blocks[x, y]], blockRect.Location.ToVector2() + new Vector2(x * Game1.blocksize, y * Game1.blocksize), colorclear);
                    if (t == Times.Location.time && b == Times.Location.block && Times.Location.Coords.X == x && Times.Location.Coords.Y == y)
                        DrawPlayer(_spriteBatch, x, y, colorclear, true);
                    //_spriteBatch.Draw(Game1.BlocksTexture[PLAYERTYPE], (Times.Progress) * (new Vector2((float)Math.Cos(Times.DirectionOfMovement), (float)Math.Sin(Times.DirectionOfMovement))) * Game1.blocksize + blockRect.Location.ToVector2() + new Vector2(x * Game1.blocksize, y * Game1.blocksize), new Rectangle(0, 0, (int)Math.Ceiling(Game1.blocksize - Game1.blocksize * Times.Progress * Math.Cos(Times.DirectionOfMovement)), (int)Math.Ceiling(Game1.blocksize - Game1.blocksize * Times.Progress * Math.Sin(Times.DirectionOfMovement))), colorclear);
                    if (t == Times.destination.time && b == Times.destination.block && Times.destination.Coords.X == x && Times.destination.Coords.Y == y)
                        //_spriteBatch.Draw(Game1.BlocksTexture[PLAYERTYPE], 0 * (Times.Progress) * (new Vector2((float)Math.Cos(Times.DirectionOfMovement), (float)Math.Sin(Times.DirectionOfMovement))) * Game1.blocksize + blockRect.Location.ToVector2() + new Vector2(x * Game1.blocksize, y * Game1.blocksize), new Rectangle((int)(Game1.blocksize * (1 - Times.Progress) * Math.Cos(Times.DirectionOfMovement)), (int)(Game1.blocksize * (1 - Times.Progress) * Math.Sin(Times.DirectionOfMovement)), (int)Math.Ceiling(Game1.blocksize - Game1.blocksize * (1 - Times.Progress) * Math.Cos(Times.DirectionOfMovement)), (int)Math.Ceiling(Game1.blocksize - Game1.blocksize * (1 - Times.Progress) * Math.Sin(Times.DirectionOfMovement))), colorclear);
                        DrawPlayer(_spriteBatch, x, y, colorclear, false);
                }
            if (IsCutLine)
                Game1.DrawRectangle(_spriteBatch, CutLine, colorclear);





        }
        void DrawPlayer(SpriteBatch _spriteBatch, int x, int y, Color colorclear, bool isLocation)
        {
            double direction = Times.DirectionOfMovement * Math.PI / 2;
            float progress = Times.Progress;
            float Invertprogress = 1 - Times.Progress;
            if (Times.DirectionOfMovement == 2 || Times.DirectionOfMovement == 3)
            {
                progress = 1 - Times.Progress;
                Invertprogress = Times.Progress;
                direction -= Math.PI;
            }

            if ((isLocation && (Times.DirectionOfMovement == 0 || Times.DirectionOfMovement == 1)) || (!isLocation && !(Times.DirectionOfMovement == 0 || Times.DirectionOfMovement == 1)))
                _spriteBatch.Draw(Game1.BlocksTexture[PLAYERTYPE], (progress) * (new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction))) * Game1.blocksize + blockRect.Location.ToVector2() + new Vector2(x * Game1.blocksize, y * Game1.blocksize), new Rectangle(0, 0, (int)Math.Ceiling(Game1.blocksize - Game1.blocksize * progress * Math.Cos(direction)), (int)Math.Ceiling(Game1.blocksize - Game1.blocksize * progress * Math.Sin(direction))), colorclear);
            else
                _spriteBatch.Draw(Game1.BlocksTexture[PLAYERTYPE], blockRect.Location.ToVector2() + new Vector2(x * Game1.blocksize, y * Game1.blocksize), new Rectangle((int)(Game1.blocksize * (Invertprogress) * Math.Cos(direction)), (int)(Game1.blocksize * (Invertprogress) * Math.Sin(direction)), (int)Math.Ceiling(Game1.blocksize - Game1.blocksize * (Invertprogress) * Math.Cos(direction)), (int)Math.Ceiling(Game1.blocksize - Game1.blocksize * (Invertprogress) * Math.Sin(direction))), colorclear);

        }

        public void CleanUpWalls(int BARRIER)
        {
            
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
                }
            for (int x = 0; x < Blocks.GetLength(0); x++)
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    if (Blocks[x, y] == BARRIER)
                        Blocks[x, y] = Newblocks[x, y] +6;
                }
        }
    }
    
}
