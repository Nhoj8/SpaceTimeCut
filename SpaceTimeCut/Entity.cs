using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using static SpaceTimeCut.Level;
using tainicom.Aether.Physics2D.Common.PhysicsLogic;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
//using SharpDX.Direct2D1;

namespace SpaceTimeCut
{

    public class EntityType
    {
        public Entity[] entitys;

        public EntityType(Entity entity)
        {
            entitys = new Entity[1];
            entitys[0] = entity;
            entitys[0].originalLocation = entity.GetCopy();
            entitys[0].originalLocation.originalLocation = entitys[0].originalLocation;

        }
        public void UpdateLocation(int entity)
        {
            for (int i = 0; i < entitys.Length; i++)
            {
                entitys[i].UpdateLocation(entity, i);
            }
        }
        public bool CheckIfWin()
        {
            bool win = false;
            for (int i = 0; i < entitys.Length; i++)
            {
                if (entitys[i].CheckIfWin())
                    win = true;
            }
            return win;
        }
        public void Move(int entity,int time)
        {
            for (int i = 0; i < entitys.Length; i++)
            {

                entitys[i].Move(entity, i,time);
            }
        }
        public void cutTime(int entity, int time, int cutLocation, int currentCircle)
        {
            int numberOfCurrentEntities = entitys.Length;
            for (int i = 0; i < numberOfCurrentEntities; i++)
            {
                entitys[i].cutTime(entity, time, cutLocation, currentCircle);
            }
        }
        public void Rotate(int t, int index, int Ylength)
        {
            for (int i = 0; i < entitys.Length; i++)
                entitys[i].Rotate(t, index, Ylength);
        }

        public void Cut(int t, int index, int cutLocation, bool isHorizontal)
        {
            for (int e = 0; e < entitys.Length; e++)
                entitys[e].Cut(t, index, cutLocation, isHorizontal);
        }

        public void Draw(SpriteBatch _spriteBatch, Color colorclear, int t, int b)
        {
            for (int e = 0; e < entitys.Length; e++)
                entitys[e].Draw(_spriteBatch, colorclear, entitys[e].imageNum, t, b);
        }
        public void SelectTime(int t, int circleSelected)
        {
            for (int e = 0; e < entitys.Length; e++)
            {
                //if (entitys[e].originalLocation.location.time != t)
                //    continue;
                entitys[e] = entitys[e].originalLocation.GetCopy();
                SpaceTimeLocation currentLocation = times[entitys[e].location.time].GetLocationFromOriginalPoint(entitys[e].location.coords, entitys[e].location.time);
                int rotationAmount = times[currentLocation.time].grids[currentLocation.gridnum].rotation;
                entitys[e].DirectionOfMoveRotate(rotationAmount);
                entitys[e].location = currentLocation;
                SpaceTimeLocation currentDestination = times[entitys[e].destination.time].GetLocationFromOriginalPoint(entitys[e].destination.coords, entitys[e].destination.time);
                entitys[e].destination = currentDestination;
                entitys[e].active = true;

            }
            //entitys[e].SelectTime(t, circleSelected);

        }


    }



    public class Entity

    {
        public SpaceTimeLocation destination;
        public SpaceTimeLocation location;
        public int directionOfMovement;
        public bool isMoving;
        public bool hasMoved = false;
        public int imageNum;
        public bool active = true;
        public Entity originalLocation;



        public Entity(int x, int y, int imageNum, int directionOfMovement = 0, bool isMoving = true)
        {
            location = new SpaceTimeLocation(0, 0, x, y);
            destination = new SpaceTimeLocation(0, 0, x, y);
            this.directionOfMovement = directionOfMovement;
            this.isMoving = isMoving;
            this.imageNum = imageNum;

        }
        public void SelectTime(int t, int selectedCircle)
        {

        }

        public virtual void UpdateLocation(int entity, int version)
        {
            location = destination;

        }
        //what the program will attempt to call
        public virtual void Move(int entity, int version,int time)
        {
            return;
            //DoesMove(entity, directionOfMovement);
        }
        public virtual bool isBlocking(SpaceTimeLocation attemptedDestination)
        {
            if (!active)
                return false;
            if (attemptedDestination == destination || attemptedDestination == location)
                return true;
            return false;

        }
        public virtual bool testIfPushableAndPush(SpaceTimeLocation attemptedDestination, int i, int j, int directionOfMovement)
        {
            return (attemptedDestination == location && Push(i, j, directionOfMovement));
        }

        //actually try to move
        public virtual void DoesMove(int entity, int version, int directionOfMovement)
        {
            if (!active)
                return;
            SpaceTimeLocation oldDestination = destination;

            //FOR DIRECTION OF MULTIPLE TIMES
            //int direction =  times[ location.time].directionOfMovement;
            times[location.time].grids[location.gridnum].Move(new Point(Grid.Cos[directionOfMovement], Grid.Sin[directionOfMovement]), ref location, ref destination);
            if (oldDestination != destination)
            {

                int blockingAmount = 0;
                for (int i = 0; i < entitys.Length; i++)
                    for (int j = 0; j < entitys[i].entitys.Length; j++)
                    {

                        if (i == entity && j == version)
                            continue;
                        if (entitys[i].entitys[j].isBlocking(destination))
                            blockingAmount++;
                    }
                if (blockingAmount > 1)//never push a chain of moving blocks, too heavy
                {
                    Game1.Chat.NewLine("blockingamountbig", Game1.time);
                    destination = oldDestination;
                    return;
                }
                for (int i = 0; i < entitys.Length; i++)
                    for (int j = 0; j < entitys[i].entitys.Length; j++)
                    {
                        if (i == entity && j == version)
                            continue;
                        if (entitys[i].entitys[j].isBlocking(destination))
                        {
                            if (entitys[i].entitys[j].testIfPushableAndPush(destination, i, j, directionOfMovement))
                                //if (entitys[i].testIfPushableAndPush(destination,i,directionOfMovement))

                                continue;

                            //if failed to push and moving to the future, then move to future
                            if (destination.time != oldDestination.time && directionOfMovement != Grid.ZERODIRECTION)
                            {
                                destination = oldDestination;
                                DoesMove(entity, version, Grid.ZERODIRECTION);
                            }

                            else

                                destination = oldDestination;


                            break;
                        }
                    }
            }
        }
        public virtual bool Push(int entity, int version, int directionOfMovement)
        {
            return false;
        }
        public void Rotate(int t, int index, int Ylength)
        {
            //Ylength = times.
            if (location.time == t && location.gridnum == index)
            {
                location.coords = new Point(Ylength - 1 - location.coords.Y, location.coords.X);
                //DirectionOfMoveRotate();

            }

            if (destination.time == t && destination.gridnum == index)
            {
                destination.coords = new Point(Ylength - 1 - destination.coords.Y, destination.coords.X);
                DirectionOfMoveRotate();
            }
        }
        public void Cut(int t, int index, int cutLocation, bool isHorizontal)
        {
            if (location.time == t && location.gridnum == index && (isHorizontal && location.Y >= cutLocation || !isHorizontal && location.X >= cutLocation))// && location.coords == new Point(i, b + cutLocation)) 
            {

                //location.coords = new Point(i, b);
                if (isHorizontal)
                    location.coords.Y -= cutLocation;
                else
                    location.coords.X -= cutLocation;
                location.gridnum = times[t].grids.Length - 1;

            }
            if (destination.time == t && destination.gridnum == index && (isHorizontal && destination.Y >= cutLocation || !isHorizontal && destination.X >= cutLocation))// && destination.coords == new Point(i, b + cutLocation))
            {

                //destination.coords = new Point(i, b);
                if (isHorizontal)
                    destination.coords.Y -= cutLocation;
                else
                    destination.coords.X -= cutLocation;
                destination.gridnum = times[t].grids.Length - 1;
            }
        }
        public virtual void DirectionOfMoveRotate(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                directionOfMovement += 1;

                if (directionOfMovement >= 4)
                    directionOfMovement = 0;
            }

        }

        public Entity GetCopy()
        {
            return (Entity)this.MemberwiseClone();
        }

        public void MoveWithKEYS(Point movement, ref SpaceTimeLocation Location, ref SpaceTimeLocation destination)//FOR Moving with ARROW KEYS
        {

            Level.times[Location.time].grids[Location.gridnum].Move(movement, ref Location, ref destination);


        }
        public bool CheckIfWin()
        {
            if (Level.times[location.time].grids[location.gridnum].Blocks[location.coords.X, location.coords.Y] == Grid.GOAL&& imageNum ==Grid.PLAYERTEXTURENUM)
                Game1.Chat.NewLine("You WIN", Game1.time);
            return false;
        }
        public virtual void cutTime(int entity, int time, int cutLocation, int currentCircle)
        {
            //ref Entity[] entitys = ref Level.entitys[entity].entitys;
            //if (destination.time == time && cutLocation <= currentCircle)
            //{
            //    Array.Resize(ref entitys, entitys.Length + 1);
            //    entitys[entitys.Length - 1] = this.GetCopy();


            //    //normalize the location stored
            //    Point originalLocation = times[location.time].grids[location.gridnum].GetOriginalLocationOfPoint(location.coords);
            //    int rotationAmount = times[location.time].grids[location.gridnum].rotation;
            //    entitys[entitys.Length - 1].DirectionOfMoveRotate(4 - rotationAmount);//get the complement of the rotation
            //    entitys[entitys.Length - 1].location.coords = originalLocation;
            //    entitys[entitys.Length - 1].location.gridnum = 0;
            //    entitys[entitys.Length - 1].location.time = time + 1;
            //    entitys[entitys.Length - 1].destination = entitys[entitys.Length - 1].location;

            //    entitys[entitys.Length - 1].originalLocation = entitys[entitys.Length - 1].GetCopy();



            //    entitys[entitys.Length - 1].active = false;
            //}
            ref Entity[] entitys = ref Level.entitys[entity].entitys;
            if (destination.time == time && cutLocation <= currentCircle)
            {
                Array.Resize(ref entitys, entitys.Length + 1);
                entitys[entitys.Length - 1] = this.GetCopy();
                entitys[entitys.Length - 1].location.time = time + 1;
                entitys[entitys.Length - 1].destination.time = time + 1;
                entitys[entitys.Length - 1].originalLocation = entitys[entitys.Length - 1].GetCopy();

                //normalize the location stored
                Point originalLocation = times[location.time].grids[location.gridnum].GetOriginalLocationOfPoint(location.coords);
                int rotationAmount = times[location.time].grids[location.gridnum].rotation;
                entitys[entitys.Length - 1].originalLocation.DirectionOfMoveRotate(4 - rotationAmount);//get the complement of the rotation
                entitys[entitys.Length - 1].originalLocation.location.coords = originalLocation;
                entitys[entitys.Length - 1].originalLocation.location.gridnum = 0;
                //entitys[entitys.Length - 1].originalLocation.location.time = time + 1;
                entitys[entitys.Length - 1].originalLocation.destination = entitys[entitys.Length - 1].originalLocation.location;
                entitys[entitys.Length - 1].originalLocation.originalLocation = entitys[entitys.Length - 1].originalLocation;





                entitys[entitys.Length - 1].active = true;
                //active = false;
               
            }

        }
        public virtual void Draw(SpriteBatch _spriteBatch, Color colorclear, int entityImage, int t, int b)
        {
            if (Level.times[location.time].visible && active && location.time == t && location.gridnum == b)
                DrawEntity(_spriteBatch, colorclear, location, true, entityImage, !isMoving, directionOfMovement);
            if (Level.times[destination.time].visible && active && destination.time == t && destination.gridnum == b)
                DrawEntity(_spriteBatch, colorclear, destination, false, entityImage, !isMoving, directionOfMovement);
        }
        public void DrawEntity(SpriteBatch _spriteBatch, Color colorclear, SpaceTimeLocation location, bool isLocation, int entityImage, bool isStationary, int directionOfMovement, int ImageDirection = -1)
        {
            Rectangle blockRect = times[location.time].grids[location.gridnum].blockRect;//isLocation ? times[location.time].grids[location.gridnum].blockRect : times[destination.time].grids[destination.gridnum].blockRect;
            int x = location.X;//isLocation ? location.X : destination.X;
            int y = location.Y;//isLocation ? location.Y : destination.Y;
                               //int directionOfMovement = this.directionOfMovement;
                               //FOR DIRECTION OF MULTIPLE TIMES
                               //int directionOfMovement;
                               //if (isLocation)
                               //{
                               //    directionOfMovement =  times[ location.time].directionOfMovement;
                               //}
                               //else
                               //{
                               //    directionOfMovement =  times[ destination.time].directionOfMovement;
                               //}
            int direction = directionOfMovement;
            //double direction =  directionOfMovement * Math.PI / 2;
            float progress = Level.progress;
            float Invertprogress = 1 - Level.progress;
            if (directionOfMovement == 2 || directionOfMovement == 3)
            {
                progress = 1 - Level.progress;
                Invertprogress = Level.progress;
                //direction -= Math.PI;
                direction += 2;
            }

            if (isStationary)
            {
                progress = 0;
                Invertprogress = 0;
            }
            if (ImageDirection == -1)
                ImageDirection = directionOfMovement;

            if ((isLocation && (directionOfMovement == 0 || directionOfMovement == 1)) || (!isLocation && !(directionOfMovement == 0 || directionOfMovement == 1)))
                _spriteBatch.Draw(Game1.EntityTextures[entityImage][ImageDirection], (progress) * (new Vector2(Grid.Cos[direction], Grid.Sin[direction])) * blocksize + blockRect.Location.ToVector2() + new Vector2(x * blocksize, y * blocksize), new Rectangle(0, 0, (int)Math.Round(blocksize - blocksize * progress * Grid.Cos[direction]), (int)Math.Round(blocksize - blocksize * progress * Grid.Sin[direction])), colorclear);
            else
                _spriteBatch.Draw(Game1.EntityTextures[entityImage][ImageDirection], blockRect.Location.ToVector2() + new Vector2(x * blocksize, y * blocksize), new Rectangle((int)(blocksize * (Invertprogress) * Grid.Cos[direction]), (int)(blocksize * (Invertprogress) * Grid.Sin[direction]), (int)Math.Round(blocksize - blocksize * (Invertprogress) * Grid.Cos[direction]), (int)Math.Round(blocksize - blocksize * (Invertprogress) * Grid.Sin[direction])), colorclear);

        }
    }

    public class VoidEntity : Entity//make a moving class and a pushable class, and make a combined one
    {

        public VoidEntity(int x, int y, int directionOfMovement = 0, bool isMoving = false) : base(x, y, Grid.VOIDENTITYTEXTURENUM, directionOfMovement, isMoving)
        {

        }
        public override void Move(int entity, int version, int time)
        {

        }
        public override void UpdateLocation(int entity, int version)
        {
            if (active == false)
                return;
            for (int i = 0; i < entitys.Length; i++)
                for (int j = 0; j < entitys[i].entitys.Length; j++)
                {
                    if (i == entity && version == j)
                        continue;
                    if (entitys[i].entitys[j].destination == destination)
                    {
                        active = false;
                        entitys[i].entitys[j].active = false;
                    }
                }

                    isMoving = false;
            base.UpdateLocation(entity, version);
        }


        public override void cutTime(int entity, int time, int cutLocation, int currentCircle)
        {
            ref Entity[] entitys = ref Level.entitys[entity].entitys;
            if (destination.time == time)
            {
                Array.Resize(ref entitys, entitys.Length + 1);
                entitys[entitys.Length - 1] = this.GetCopy();
                entitys[entitys.Length - 1].location.time = time + 1;
                entitys[entitys.Length - 1].destination.time = time + 1;
                entitys[entitys.Length - 1].originalLocation = entitys[entitys.Length - 1].GetCopy();

                //normalize the location stored
                Point originalLocation = times[location.time].grids[location.gridnum].GetOriginalLocationOfPoint(location.coords);
                int rotationAmount = times[location.time].grids[location.gridnum].rotation;
                entitys[entitys.Length - 1].originalLocation.DirectionOfMoveRotate(4 - rotationAmount);//get the complement of the rotation
                entitys[entitys.Length - 1].originalLocation.location.coords = originalLocation;
                entitys[entitys.Length - 1].originalLocation.location.gridnum = 0;
                //entitys[entitys.Length - 1].originalLocation.location.time = time + 1;
                entitys[entitys.Length - 1].originalLocation.destination = entitys[entitys.Length - 1].originalLocation.location;
                entitys[entitys.Length - 1].originalLocation.originalLocation = entitys[entitys.Length - 1].originalLocation;





                entitys[entitys.Length - 1].active = true;
            }


        }

        public override bool Push(int entity, int version, int directionOfMovement)
        {

            //active = false;

            return true;

        }
        public override void Draw(SpriteBatch _spriteBatch, Color colorclear, int entityImage, int t, int b)
        {
            base.Draw(_spriteBatch, colorclear, imageNum, t, b);
        }
    }




    public class PushableEntity : Entity//make a moving class and a pushable class, and make a combined one
    {

        public PushableEntity(int x, int y, int directionOfMovement = 0, bool isMoving = false) : base(x, y, Grid.PUSHABLEBLOCKTEXTURENUM, directionOfMovement, isMoving)
        {

        }
        public override void Move(int entity, int version, int time)
        {

        }
        public override void UpdateLocation(int entity, int version)
        {

            isMoving = false;
            base.UpdateLocation(entity, version);
        }


        public override void cutTime(int entity, int time, int cutLocation, int currentCircle)
        {
            ref Entity[] entitys = ref Level.entitys[entity].entitys;
            if (destination.time == time)
            {
                Array.Resize(ref entitys, entitys.Length + 1);
                entitys[entitys.Length - 1] = this.GetCopy();
                entitys[entitys.Length - 1].location.time = time + 1;
                entitys[entitys.Length - 1].destination.time = time + 1;
                entitys[entitys.Length - 1].originalLocation = entitys[entitys.Length - 1].GetCopy();

                //normalize the location stored
                Point originalLocation = times[location.time].grids[location.gridnum].GetOriginalLocationOfPoint(location.coords);
                int rotationAmount = times[location.time].grids[location.gridnum].rotation;
                entitys[entitys.Length - 1].originalLocation.DirectionOfMoveRotate(4 - rotationAmount);//get the complement of the rotation
                entitys[entitys.Length - 1].originalLocation.location.coords = originalLocation;
                entitys[entitys.Length - 1].originalLocation.location.gridnum = 0;
                //entitys[entitys.Length - 1].originalLocation.location.time = time + 1;
                entitys[entitys.Length - 1].originalLocation.destination = entitys[entitys.Length - 1].originalLocation.location;
                entitys[entitys.Length - 1].originalLocation.originalLocation = entitys[entitys.Length - 1].originalLocation;





                entitys[entitys.Length - 1].active = true;
            }


        }

        public override bool Push(int entity, int version, int directionOfMovement)
        {

            SpaceTimeLocation oldDestination = destination;

            times[location.time].grids[location.gridnum].Move(new Point(Grid.Cos[directionOfMovement], Grid.Sin[directionOfMovement]), ref location, ref destination);

            if (oldDestination != destination)

                for (int i = 0; i < entitys.Length; i++)
                    for (int j = 0; j < entitys[i].entitys.Length; j++)
                    {
                        if (i == entity && version == j)
                            continue;
                        if (entitys[i].entitys[j].isBlocking(destination))
                        {

                            if (entitys[i].entitys[j].testIfPushableAndPush(destination, i, j, directionOfMovement))
                            {
                                continue;
                            }
                            destination = oldDestination;
                            return false;
                            break;
                        }
                    }


            else
                return false;
            isMoving = true;
            this.directionOfMovement = directionOfMovement == Grid.ZERODIRECTION ? 0 : directionOfMovement;

            return true;

        }
        public override void Draw(SpriteBatch _spriteBatch, Color colorclear, int entityImage, int t, int b)
        {
            base.Draw(_spriteBatch, colorclear, imageNum, t, b);
            //for (int i = 0; i < oldOriginalLocations.Length; i++)
            //{
            //    if (times[i].visible && times[i].starttime + times[i].amounOfCircles <= currentCircle)
            //    {
            //        SpaceTimeLocation originalLocation = times[i].GetLocationFromOriginalPoint(oldOriginalLocations[i].coords, i);
            //        if (originalLocation.time != t || originalLocation.gridnum != b)
            //            continue;
            //        DrawEntity(_spriteBatch, colorclear, originalLocation, true, imageNum, true, directionOfMovement);//Grid.ZERODIRECTION);
            //    }

            //}

            //for (int i = 0; i < times.Length; i++)
            //{
            //    //if (i > 0)
            //    //System.Windows.Forms.MessageBox.Show(times[i].starttime.ToString() + " " + currentCircle.ToString());
            //    if (!times[i].visible || times[i].starttime <= currentCircle)
            //    {

            //        continue;
            //    }

            //    Point OriginalLocationOfPoint = times[location.time].grids[location.gridnum].GetOriginalLocationOfPoint(location.coords);//gets where the entityNum would be if there had been no rotations or cuts
            //    SpaceTimeLocation drawLocation = times[i].GetLocationFromOriginalPoint(OriginalLocationOfPoint, i);//from the orginalLocation, gets the place where it would be now
            //    if (drawLocation.time != t || drawLocation.gridnum != b)
            //        continue;
            //    Color futureColor = colorclear;
            //    //Color futureColor = new Color(100, 100, 100, 180);
            //    DrawEntity(_spriteBatch, futureColor, drawLocation, true, imageNum, !isMoving, (directionOfMovement + times[drawLocation.time].grids[drawLocation.gridnum].rotation - times[location.time].grids[location.gridnum].rotation + 8) % 4);
            //    Point OriginalDestinationOfPoint = times[location.time].grids[destination.gridnum].GetOriginalLocationOfPoint(destination.coords);//gets where the entityNum would be if there had been no rotations or cuts
            //    SpaceTimeLocation drawDestination = times[i].GetLocationFromOriginalPoint(OriginalDestinationOfPoint, i);//from the orginalLocation, gets the place where it would be now
            //    DrawEntity(_spriteBatch, futureColor, drawDestination, false, imageNum, !isMoving, (directionOfMovement + times[drawDestination.time].grids[drawDestination.gridnum].rotation - times[location.time].grids[location.gridnum].rotation + 8) % 4);

            //}



        }


    }
    public class MoveableEntity : Entity

    {


        public MoveableEntity(int x, int y, int imageNum, int directionOfMovement = 0, bool isMoving = true) : base(x, y, imageNum, directionOfMovement, isMoving)
        {
        }
        public override void Move(int entity, int version, int time)
        {
            if (location.time == time)
                DoesMove(entity, version, directionOfMovement);
        }


    }
    public class MoveablePushableEntity : Entity
    {
        int OverallDirectionOfMovement;
        //int OldDirectionOfMovement;
        //bool hasMoved = false;
        //void Move() { movableProperty.Move(); }

        public MoveablePushableEntity(int x, int y, int directionOfMovement = 0, bool isMoving = true) : base(x, y, Grid.PUSHABLEENTITYTEXTURENUM, directionOfMovement, isMoving)
        {

            OverallDirectionOfMovement = directionOfMovement;
            //this.movableProperty = new MovableProperty(this);
        }
        public override void UpdateLocation(int entity, int version)
        {
            hasMoved = false;
            isMoving = false;
            base.UpdateLocation(entity, version);
        }
        public override void DirectionOfMoveRotate(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                directionOfMovement += 1;
                OverallDirectionOfMovement++;
                //OldDirectionOfMovement++;

                if (directionOfMovement >= 4)
                    directionOfMovement = 0;
                if (OverallDirectionOfMovement >= 4)
                    OverallDirectionOfMovement = 0;
                //if (OldDirectionOfMovement >= 4)
                //    OldDirectionOfMovement = 0;
            }

        }

        public override void DoesMove(int entity, int version, int directionOfMovement)
        {

            if (!active)
                return;
            SpaceTimeLocation oldDestination = destination;

            //FOR DIRECTION OF MULTIPLE TIMES
            //int direction =  times[ location.time].directionOfMovement;
            times[location.time].grids[location.gridnum].Move(new Point(Grid.Cos[directionOfMovement], Grid.Sin[directionOfMovement]), ref location, ref destination);
            if (oldDestination != destination)
            {
                hasMoved = true;
                int blockingAmount = 0;
                for (int i = 0; i < entitys.Length; i++)
                    for (int j = 0; j < entitys[i].entitys.Length; j++)
                    {
                        if (i == entity && j == version)
                            continue;
                        if (entitys[i].entitys[j].isBlocking(destination))
                            blockingAmount++;
                    }
                if (blockingAmount > 1)
                {
                    Game1.Chat.NewLine("blockingamountbig", Game1.time);
                    destination = oldDestination;
                    hasMoved = false;
                    return;
                }

                for (int i = 0; i < entitys.Length; i++)
                    for (int j = 0; j < entitys[i].entitys.Length; j++)
                    {
                        if (i == entity && j == version)
                            continue;
                        if (entitys[i].entitys[j].isBlocking(destination))
                        {//OVERIDE DOES MOVE because of this to test if movable pusing movable
                         //hoever need to check if in the same direcvtion

                            //has moved is so that when we try to push a movablepushalble entity, if it has already moved and is pushing somthing else, it will still go there, but will not prevent this one from moving
                            //wait actually I have no Idea what its for
                            if ((entitys[i].entitys[j].imageNum != imageNum || entitys[i].entitys[j].directionOfMovement != (directionOfMovement + 2) % 4) && entitys[i].entitys[j].testIfPushableAndPush(destination, i, j, directionOfMovement))//||entitys[i].hasMoved)))
                            {
                                //bad if img==img and dir == dir+2
                                //good if !(img==img and dir == dir+2)
                                //good if (img!=img or dir != dir+2)
                                if (entitys[i].entitys[j].hasMoved)
                                {
                                    entitys[i].entitys[j].directionOfMovement = directionOfMovement;
                                    Game1.Chat.NewLine("HASMOVED GARBAGE", Game1.time);

                                }

                                continue;

                            }

                            //for when the one in front is a pushable that has already moved
                            //if (entitys[i].imageNum == imageNum && destination == entitys[i].location && destination != entitys[i].destination)// && (entitys[i].directionOfMovement + directionOfMovement) %2 ==1)
                            //{
                            //    //Game1.Chat.NewLine("hahaaaa", Game1.time);
                            //    entitys[i].directionOfMovement = directionOfMovement;
                            //    entitys[i].isMoving = true;
                            //    continue;
                            //}
                            //if failed to push and moving to the future, then move to future
                            if (destination.time != oldDestination.time && directionOfMovement != Grid.ZERODIRECTION)
                            {
                                destination = oldDestination;
                                hasMoved = false;
                                DoesMove(entity, version, Grid.ZERODIRECTION);
                            }

                            else
                            {
                                hasMoved = false;
                                destination = oldDestination;
                            }



                            break;
                        }
                    }
            }
        }
        public override void Move(int entity, int version, int time)
        {
            //OldDirectionOfMovement = directionOfMovement;
            //directionOfMovement = OverallDirectionOfMovement;
            //DoesMove(entity, directionOfMovement);
            if(location.time==time)
             DoesMove(entity, version, OverallDirectionOfMovement);

        }
        public override bool Push(int entity, int version, int directionOfMovement)
        {
            if (hasMoved)
                return false;
            SpaceTimeLocation oldDestination = destination;

            times[location.time].grids[location.gridnum].Move(new Point(Grid.Cos[directionOfMovement], Grid.Sin[directionOfMovement]), ref location, ref destination);

            if (oldDestination != destination)
            {
                int blockingAmount = 0;
                for (int i = 0; i < entitys.Length; i++)
                    for (int j = 0; j < entitys[i].entitys.Length; j++)
                    {
                        if (i == entity && j == version)
                            continue;
                        if (entitys[i].entitys[j].isBlocking(destination))
                            blockingAmount++;
                    }
                if (blockingAmount > 1)
                {
                    destination = oldDestination;
                    return false;
                }
                for (int i = 0; i < entitys.Length; i++)
                    for (int j = 0; j < entitys[i].entitys.Length; j++)
                    {
                        if (i == entity)
                            continue;
                        if (entitys[i].entitys[j].isBlocking(destination))
                        {

                            if (entitys[i].entitys[j].testIfPushableAndPush(destination, i, j, directionOfMovement))
                            {
                                continue;
                            }
                            destination = oldDestination;
                            return false;
                            break;
                        }
                    }
            }



            else
                return false;
            isMoving = true;
            this.directionOfMovement = directionOfMovement == Grid.ZERODIRECTION ? 0 : directionOfMovement;

            return true;

        }
        public override void Draw(SpriteBatch _spriteBatch, Color colorclear, int entityImage, int t, int b)
        {
            if (!isMoving)
                directionOfMovement = OverallDirectionOfMovement;
            if (location.time == t && location.gridnum == b)
                DrawEntity(_spriteBatch, colorclear, location, true, entityImage, false, directionOfMovement, OverallDirectionOfMovement);
            if (destination.time == t && destination.gridnum == b)
                DrawEntity(_spriteBatch, colorclear, destination, false, entityImage, false, directionOfMovement, OverallDirectionOfMovement);
        }


    }




}