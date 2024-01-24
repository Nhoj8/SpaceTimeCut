using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// to do: 
/// make predicted location for extrinzoid based on velocity in 1 sec and show time projectil shot from ship will take to get to mouse. 
/// DONE make projectiels and exrtinzoids move when tiem reversed
/// DONE makes player not be able to move when time revesed
/// DONE need to make dependancies
/// when reEneabaling events need to check if all things that it depends on are also enabled, use refeerces for that i think
/// make spreading corruption blocks
/// diversify world gen(biomes like in the cool slime video)
/// make big map image
/// make extrinzoid mode change into an event or just make them not be angry if no friends nearby
/// DONE need to transfer all calculations from the Draw phasde into the game phae ie: make th eblocks test for extrinzoids and corruption in there
///
/// </summary>

namespace SpaceTimeCut
{
    public struct DoubleCoordinates
    {
        public float X;
        public float Y;

        public DoubleCoordinates(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Point convertToPoint(DoubleCoordinates Dc1)
        {
            return new Point((int)Dc1.X, (int)Dc1.Y);
        }
        public static Vector2 convertToVector2(DoubleCoordinates Dc1)
        {
            return new Vector2((float)Dc1.X, (float)Dc1.Y);
        }
        public static DoubleCoordinates convertToDoubleCoordinates(Vector2 Dc1)
        {
            return new DoubleCoordinates(Dc1.X, Dc1.Y);
        }
        public static DoubleCoordinates operator +(DoubleCoordinates Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X += Dc2.X;
            Dc1.Y += Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator -(DoubleCoordinates Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X -= Dc2.X;
            Dc1.Y -= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X *= Dc2.X;
            Dc1.Y *= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator /(DoubleCoordinates Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X /= Dc2.X;
            Dc1.Y /= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator +(DoubleCoordinates Dc1, Vector2 Dc2)
        {
            Dc1.X += Dc2.X;
            Dc1.Y += Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator -(DoubleCoordinates Dc1, Vector2 Dc2)
        {
            Dc1.X -= Dc2.X;
            Dc1.Y -= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator +(DoubleCoordinates Dc1, Point Dc2)
        {
            Dc1.X += Dc2.X;
            Dc1.Y += Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator /(DoubleCoordinates Dc1, int dv)
        {
            Dc1.X /=dv;
            Dc1.Y /=dv;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, double dv)
        {
            Dc1.X *= (float)dv;
            Dc1.Y *= (float)dv;
            return Dc1;
        }
        public static DoubleCoordinates operator /(DoubleCoordinates Dc1, double dv)
        {
            Dc1.X /= (float)dv;
            Dc1.Y /= (float)dv;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, int dv)
        {
            Dc1.X *= dv;
            Dc1.Y *= dv;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, Point Dc2)
        {
            Dc1.X *= Dc2.X;
            Dc1.Y *= Dc2.Y;
            return Dc1;
        }
        public static DoubleCoordinates operator *(DoubleCoordinates Dc1, WholeCoords Dc2)
        {
            Dc1.X *= Dc2.X;
            Dc1.Y *= Dc2.Y;
            return Dc1;
        }
        public static Point operator +(Point Dc1, DoubleCoordinates Dc2)
        {
            Dc1.X += (int)Dc2.X;
            Dc1.Y += (int)Dc2.Y;
            return Dc1;
        }
        public static bool operator ==(DoubleCoordinates wc1, DoubleCoordinates wc2)
        {
            return wc1.Equals(wc2); //(wc1.X == wc2.X && wc1.Y == wc2.Y);
        }

        public static bool operator !=(DoubleCoordinates wc1, DoubleCoordinates wc2)
        {
            return !wc1.Equals(wc2);
        }
    }
    public struct WholeCoords
    {
        public Int16 X;
        public Int16 Y;

        public WholeCoords(Int16 X, Int16 Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public WholeCoords(int X, int Y)
        {
            this.X = (short)X;
            this.Y = (short)Y;
        }
        public static Point convertToPoint(WholeCoords Dc1)
        {
            return new Point((int)Dc1.X, (int)Dc1.Y);
        }
        public WholeCoords(Point coords)
        { 
            this.X = (short)coords.X;
            this.Y = (short)coords.Y;
        }
        public WholeCoords(DoubleCoordinates coords)
        {
            this.X = (short)coords.X;
            this.Y = (short)coords.Y;
        }
        public static WholeCoords operator *(WholeCoords Dc1, int dv)
        {
            Dc1.X *= (short)dv;
            Dc1.Y *= (short)dv;
            return Dc1;
        }
        public static DoubleCoordinates operator -(WholeCoords Dc1, DoubleCoordinates Dc2)
        {
            Dc2.X = (float)Dc1.X - Dc2.X;
            Dc2.Y = (float)Dc1.Y - Dc2.Y;
            return Dc2;
        }

        public static bool operator ==(WholeCoords wc1, WholeCoords wc2)
        {
            return wc1.Equals(wc2); //(wc1.X == wc2.X && wc1.Y == wc2.Y);
        }

        public static bool operator !=(WholeCoords wc1, WholeCoords wc2)
        {
            return !wc1.Equals(wc2);
        }
    }
    public class Text
    {
        public string text;
        public char[] chars;
        public Vector2[] coords;
        public int[] color;
        public Rectangle size;
        public int NumberOfLines;
        public int[] LinePerCharecter;


        public void DoText(string textword = "", int startindex = 0)
        {
            (chars, coords, color, size, LinePerCharecter, NumberOfLines) = Getcoordinateschar(text.Substring(startindex) + textword);
        }
        (Char[], Vector2[], int[], Rectangle, int[], int) Getcoordinateschar(string data)
        {
            char[] chararrayprivate = data.ToCharArray(), chararray;
            chararray = chararrayprivate;
            int length = 0, newlength = 0, height = 16;
            //string test = "", test2 = "";
            Vector2[] CoordinatesString = new Vector2[chararrayprivate.Length];
            int[] ColorString = new int[chararrayprivate.Length];
            int[] numberofchararray = new int[chararrayprivate.Length];
            int[] LinesPerCharecter = new int[chararrayprivate.Length];
            int newLine = 0, charectersinpreviousline = 0, TotalWeirdChar = 0, SpacesPerLine = 0, WeirdCharPerLine = 0;
            Boolean ColorChar = false;
            int ColorOfLetter = 0;
            //System.Windows.Forms.MessageBox.Show((index).ToString());
            for (int i = 0; i < chararrayprivate.Length; i++)
            {
                char c;
                c = chararrayprivate[i];
                numberofchararray[i] = c;
                chararray[i - newLine - TotalWeirdChar] = chararrayprivate[i];
                //test2 += i.ToString() + "'" + numberofchararray[i].ToString() + "', ";
                if (ColorChar)
                {
                    if (numberofchararray[i] > 96 && numberofchararray[i] < 122)
                        ColorOfLetter = numberofchararray[i] - 97;
                    else if (numberofchararray[i] == 122)//z
                        ColorOfLetter = 20;
                    TotalWeirdChar += 1;
                    WeirdCharPerLine += 1;
                    ColorChar = false;
                }
                else if (numberofchararray[i] == 10)//enter or newline
                {
                    //System.Array.Resize(ref charectersperline, newLine + 1);
                    //charectersperline[newLine] = i + 1 - charectersinpreviousline;
                    newLine += 1;
                    charectersinpreviousline = i + 1;
                    SpacesPerLine = 0;
                    WeirdCharPerLine = 0;
                    height += 16;
                    length = length < newlength ? newlength : length;
                    newlength = 0;
                }
                else if (numberofchararray[i] == 32) //space
                {
                    SpacesPerLine += 1;
                    TotalWeirdChar += 1;
                    newlength += 8;
                }
                else if (numberofchararray[i] == 937) //Ω
                {
                    ColorChar = true;
                    TotalWeirdChar += 1;
                    WeirdCharPerLine += 1;
                }
                else
                {

                    if (numberofchararray[i] > 128)
                    {
                        System.Windows.Forms.MessageBox.Show(((char)numberofchararray[i]).ToString());
                        WeirdCharPerLine += 1;
                        TotalWeirdChar += 1;
                    }
                    else if (Game1.Letters[numberofchararray[i]] == null)
                    {
                        numberofchararray[i] = Game1.rnd.Next(97, 123);
                        chararray[i - newLine - TotalWeirdChar] = (char)numberofchararray[i];
                        //System.Windows.Forms.MessageBox.Show(chararray[i - newLine - TotalWeirdChar].ToString());
                    }
                    newlength += 16;
                    CoordinatesString[i - newLine - TotalWeirdChar] = new Vector2((i - SpacesPerLine - WeirdCharPerLine) * 16 - charectersinpreviousline * 16 + SpacesPerLine * 8, 0 + newLine * 16);
                    ColorString[i - newLine - TotalWeirdChar] = ColorOfLetter;
                    LinesPerCharecter[i - newLine - TotalWeirdChar] = newLine;
                    //test += (i.ToString() + "'" + ((char)c).ToString() + "', ");
                }

            }
            //System.Array.Resize(ref charectersperline, newLine + 1);
            //charectersperline[newLine] = chararrayprivate.Length - charectersinpreviousline;
            length = length < newlength ? newlength : length;
            //System.Windows.Forms.MessageBox.Show(test2);
            System.Array.Resize(ref chararray, chararrayprivate.Length - newLine - TotalWeirdChar);
            Rectangle size = new Rectangle(0, 0, length, height);
            return (chararray, CoordinatesString, ColorString, size, LinesPerCharecter, newLine + 1);
        }

        public void draw(SpriteBatch _spriteBatch, int X = 0, int Y = 0)
        {
            for (int i = 0; i < chars.Length; i++)
                _spriteBatch.Draw(Game1.Letters[chars[i]], coords[i] + new Vector2(size.X + X, size.Y + Y), Game1.LetterColors[color[i]]);
        }
    }
    public class Button
    {
            public Rectangle Rectangle;
            public Text text;
            public Boolean Selected;
            public Boolean visible;
            public Boolean TextOnly;
            public int PositionAnchor;
            public Vector2 Position;
            public Color color = Color.OrangeRed;
            public Color Selectedcolor = Color.Red;
        public static Vector2[] PositionAnchors = new Vector2[9];
        public const int TOPLEFT = 0;
        public const int TOPMIDDLE = 1;
        public const int TOPRIGHT = 2;
        public const int MIDDLELEFT = 3;
        public const int MIDDLEMIDDLE = 4;
        public const int MIDDLERIGHT = 5;
        public const int BOTTOMLEFT = 6;
        public const int BOTTOMMIDDLE = 7;
        public const int BOTTOMRIGHT = 8;
        public static Button[] Buttons = new Button[0];
        //public const int PLAYBUTTON = 0;
        //public const int CREATENEWWORLDBUTTON = 1;
        //public const int DEBUGBUTTON = 2;
        //public const int SETTINGSBUTTON = 3;
        //public const int RESETBUTTON = 4;
        //public const int NEWWORLDBUTTON = 5;
        //public const int SAVEWORLDBUTTON = 6;
        //public const int LOADWORLDBUTTON = 7;
        //public const int WORLD1BUTTON = 8;
        //public const int ROTATEBUTTON = 9;
        //public const int CUTBUTTON = 10;
        //public const int MOVEBUTTON = 11;
        //public const int AMOUNT = 12;//The Total amount of buttons

        public event EventHandler Press;

        protected virtual void OnPress(EventArgs e)
        {
            Press?.Invoke(this, e);
        }
        public void Click()
        {
            Selected = false;
            OnPress(EventArgs.Empty);
        }
        public Button(string ButtonText, int PositionAnchor, int RecX, int RecY, EventHandler eventHandler, Boolean TextOnly = true, int RecWidth = 200, int RecHeight = 50, Boolean ButtonVisible = false, Nullable<Color> color = null, Nullable<Color> Selectedcolor = null)
        {
            
           int buttonNum = Buttons.Length;
            Array.Resize(ref Buttons, buttonNum + 1);
            //Buttons[buttonNum] = new Button();
            this.Position = new Vector2(RecX, RecY);
            this.PositionAnchor = PositionAnchor;
            this.TextOnly = TextOnly;
            this.Rectangle = new Rectangle(RecX, RecY, RecWidth, RecHeight);
            this.text = new Text();

            this.visible = ButtonVisible;
            if (color != null)
                this.color = (Color)color;
            if (Selectedcolor != null)
                this.Selectedcolor = (Color)Selectedcolor;
            Press += eventHandler;
            Buttons[buttonNum] = this;
            ButtonChangeText(ButtonText);
            ResetButtonPosition(buttonNum);
        }

        void ButtonChangeText(string ButtonText)
        {
            

            text.text = ButtonText;
            text.DoText();
            if (TextOnly)
            {
                int RecWidth = text.size.Width;
                int RecHeight = text.size.Height;
               Rectangle.Size = new Point(RecWidth, RecHeight);
            }
        }
        static void ResetButtonPosition(int buttonNum)
        {
            
            if (Buttons[buttonNum] != null)
            {

                Vector2 PositionModifier = PositionAnchors[Buttons[buttonNum].PositionAnchor];
                if (Buttons[buttonNum].PositionAnchor == TOPRIGHT || Buttons[buttonNum].PositionAnchor == MIDDLERIGHT || Buttons[buttonNum].PositionAnchor == BOTTOMRIGHT)
                    PositionModifier.X -= Buttons[buttonNum].Rectangle.Width;
                Buttons[buttonNum].Rectangle = new Rectangle(Convert.ToInt32(PositionModifier.X + Buttons[buttonNum].Position.X), Convert.ToInt32(PositionModifier.Y + Buttons[buttonNum].Position.Y), Buttons[buttonNum].Rectangle.Width, Buttons[buttonNum].Rectangle.Height);

            }
        }
        public static void ResetAnchors(int ViewWidth, int ViewHeight)
        {
           

            int row = 0;
            for (int i = 0; i < PositionAnchors.Count(); i++)
            {
                PositionAnchors[i] = new Vector2((i - row * 3) * ViewWidth / 2, row * ViewHeight / 2);
                row += i - row * 3 == 3 ? 1 : 0;
            }
            //for (buttons)
            for (int b = 0; b < Buttons.Count(); b++)
                ResetButtonPosition(b);
        }
        //public static void Click(int buttonNum)
        //{
            
        //    Buttons[buttonNum].Selected = false;
        //    //if (buttonNum == PLAYBUTTON)//PLAY BUTTON
        //    //{
        //    //    Buttons[PLAYBUTTON].visible = false;
        //    //    //Buttons[CREATENEWWORLDBUTTON].visible = true;
        //    //    //Buttons[LOADWORLDBUTTON].visible = true;
        //    //    //Buttons[DEBUGBUTTON].visible = true;
        //    //    Level.initLevelSelector();
        //    //}
        //    //else if (buttonNum == CREATENEWWORLDBUTTON)//CREATE NEW WORLD BUTTON
        //    //{
        //    //    Buttons[CREATENEWWORLDBUTTON].visible = false;
        //    //    Buttons[DEBUGBUTTON].visible = false;
        //    //    Buttons[LOADWORLDBUTTON].visible = false;
        //    //    Game1.CurrentlyDisplaying = Game1.LOADING;

        //    //}
        //    //else if (buttonNum == LOADWORLDBUTTON)//LOAD WORLD BUTTON
        //    //{
        //    //    Buttons[CREATENEWWORLDBUTTON].visible = false;
        //    //    Buttons[DEBUGBUTTON].visible = false;
        //    //    Buttons[LOADWORLDBUTTON].visible = false;
        //    //    Buttons[WORLD1BUTTON].visible = true;
        //    //}
        //    //else if (buttonNum == WORLD1BUTTON)//LOAD WORLD1 BUTTON
        //    //{
        //    //    Buttons[WORLD1BUTTON].visible = false;


        //    //    Game1.CurrentlyDisplaying = Game1.LOADING;
        //    //}
        //    //else if (buttonNum == DEBUGBUTTON)//DEBUG BUTTON
        //    //{
        //    //    Buttons[CREATENEWWORLDBUTTON].visible = false;
        //    //    Buttons[DEBUGBUTTON].visible = false;
        //    //    Buttons[LOADWORLDBUTTON].visible = false;
        //    //    Game1.CurrentlyDisplaying = Game1.DEBUGLOADING;

        //    //}
        //    //else if (buttonNum == SETTINGSBUTTON)//SETTINGS BUTTON
        //    //{
        //    //    Buttons[NEWWORLDBUTTON].visible = !Buttons[NEWWORLDBUTTON].visible;
        //    //    Buttons[RESETBUTTON].visible = !Buttons[RESETBUTTON].visible;
        //    //    Buttons[SAVEWORLDBUTTON].visible = !Buttons[SAVEWORLDBUTTON].visible;

        //    //}
        //    //else if (buttonNum == RESETBUTTON)//RESET BUTTON
        //    //{
        //    //    Game1.CurrentlyDisplaying = Game1.LOADING;
        //    //}
        //    //else if (buttonNum == NEWWORLDBUTTON)//NEWWORLD BUTTON
        //    //{

        //    //    Game1.CurrentlyDisplaying = Game1.LOADING;
        //    //}
        //    //else if (buttonNum == MOVEBUTTON)//NEWWORLD BUTTON
        //    //{


        //    //}
        //    //else if (buttonNum == CUTBUTTON)//NEWWORLD BUTTON
        //    //{

        //    //}
        //    //else if (buttonNum == ROTATEBUTTON)//NEWWORLD BUTTON
        //    //{


        //    //}

        //}
    }

    public class Label
    {
        public Rectangle rectangle;
        public Point DistanceFromText = new Point(20,20);
        public Color color;
        public Text text = new Text();

        public void draw(SpriteBatch _spriteBatch, int X = 0, int Y = 0)
        {
            text.DoText();
            //System.Windows.Forms.MessageBox.Show(Inventory.SelectedSlotText.size.X.ToString());
            _spriteBatch.DrawRoundedRect(new Rectangle(X + rectangle.X, Y + rectangle.Y, text.size.Width + DistanceFromText.X * 2, text.size.Height + DistanceFromText.Y*2), Game1.circle, Game1.circle.Width / 2, Color.Red);

            text.draw(_spriteBatch, X + rectangle.X + DistanceFromText.X, Y + rectangle.Y + DistanceFromText.Y);

        }
    }

    class KeyBoardKey
    {
        public bool Pressed;
        public bool Holdable;
        public bool activateOnStateChange = false;
        public int Continue;
        public int MaxContinueTime;
        public int MaxContinueTimeOriginal;
        public int InitialWaitTime;
        public int InitialWaitTimeOriginal;
        public int MinWaitTime;
        public Keys Key;
        public void NewKeyBoardKey(int thisMaxContinueTime = 10, int thisMinWaitTime = 5, int thisInitialWaitTime = 25)
        {
            MaxContinueTimeOriginal = thisMaxContinueTime;
            MaxContinueTime = MaxContinueTimeOriginal;
            MinWaitTime = thisMinWaitTime;
            InitialWaitTimeOriginal = thisInitialWaitTime - (MaxContinueTime + MinWaitTime);
            InitialWaitTime = InitialWaitTimeOriginal;
            Holdable = true;
        }
        public Boolean CheckKeyPress(Boolean Overide = false)
        {
            bool Canactivate = false;
            if (Keyboard.GetState().IsKeyUp(Key) && (Pressed == true))
            {
                Pressed = false;
                if (Holdable)
                {
                    Continue = 0;
                    MaxContinueTime = MaxContinueTimeOriginal;
                    InitialWaitTime = InitialWaitTimeOriginal;
                }
                if (activateOnStateChange)
                    Canactivate = true;
            }

            else if ((Keyboard.GetState().IsKeyDown(Key)))
            {
                if (!Overide)
                {
                    if (Holdable)
                    {
                        Continue += 1;
                        if (Pressed == false || Continue > (MaxContinueTime + MinWaitTime + InitialWaitTime))
                        {
                            Canactivate = true;
                            Continue = 0;
                            if (MaxContinueTime > 0)
                                MaxContinueTime -= 1;
                            if (Pressed == true)
                                InitialWaitTime = 0;
                        }
                    }
                    else if (Pressed == false)
                        Canactivate = true;
                    Pressed = true;
                }
                else
                    Canactivate = true;

            }
            return Canactivate;
        }
    }



    class RectangleSprite
    {
        static Texture2D _pointTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }
    }

    public class Chat
    {
        public Text data = new Text();
        public const int LineLimit = 20;
        public const int TimePerLine = 150;
        //public int StartChar;
        public int TotalLines;
        public int Lines;
        public int[] TimeDisplayed;
        public string[] Texts;
        public int LengthOfFade = 25;
        public int maxLengthOfchat = 300;
        public float OpacityOfRectangle = 0.5F;


        public void Initialize()
        {
            TimeDisplayed = new int[maxLengthOfchat];
            Texts = new string[maxLengthOfchat];
        }

        public void NewLine(string Newtext, int TimeToDisplay, bool inDraw = false)//if in Draw mode disregarde line 
        {

            if (TotalLines + 1 >= maxLengthOfchat)
                TotalLines = 0;
            Texts[TotalLines] = "Ωa" + Newtext;
            TimeDisplayed[TotalLines] = TimeToDisplay;
            if (TotalLines == 0 && inDraw)
                Game1.Chat.RefreshText(Game1.time);
            TotalLines += 1;
        }
        public void RefreshText(int time)
        {
            Lines = 0;
            data.text = "";
            for (int i = TotalLines - 1; i >= 0; i--)
            {
                if (TimeDisplayed[i] <= time && time <= TimeDisplayed[i] + TimePerLine)
                {
                    string newline = "";
                    if (Lines > 0)
                        newline = "\n";
                    data.text = Texts[i] + newline + data.text;
                    Lines += 1;
                }
                else
                    break;
                if (Lines >= LineLimit)
                    break;
            }

        }
    }

    public static class SpriteBatchExtensions
    {
        public static void DrawRoundedRect(this SpriteBatch spriteBatch, Rectangle destinationRectangle, Texture2D texture, int border, Color color)
        {
            // Top left
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location, new Point(border)),
                new Rectangle(0, 0, border, border),
                color);

            // Top
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(border, 0),
                    new Point(destinationRectangle.Width - border * 2, border)),
                new Rectangle(border, 0, texture.Width - border * 2, border),
                color);

            // Top right
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(destinationRectangle.Width - border, 0), new Point(border)),
                new Rectangle(texture.Width - border, 0, border, border),
                color);

            // Middle left
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(0, border), new Point(border, destinationRectangle.Height - border * 2)),
                new Rectangle(0, border, border, texture.Height - border * 2),
                color);

            // Middle
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(border), destinationRectangle.Size - new Point(border * 2)),
                new Rectangle(border, border, texture.Width - border * 2, texture.Height - border * 2),
                color);

            // Middle right
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(destinationRectangle.Width - border, border),
                    new Point(border, destinationRectangle.Height - border * 2)),
                new Rectangle(texture.Width - border, border, border, texture.Height - border * 2),
                color);

            // Bottom left
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(0, destinationRectangle.Height - border), new Point(border)),
                new Rectangle(0, texture.Height - border, border, border),
                color);

            // Bottom
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + new Point(border, destinationRectangle.Height - border),
                    new Point(destinationRectangle.Width - border * 2, border)),
                new Rectangle(border, texture.Height - border, texture.Width - border * 2, border),
                color);

            // Bottom right
            spriteBatch.Draw(
                texture,
                new Rectangle(destinationRectangle.Location + destinationRectangle.Size - new Point(border), new Point(border)),
                new Rectangle(texture.Width - border, texture.Height - border, border, border),
                color);
        }

        public static void DrawVertices(this SpriteBatch spriteBatch, Texture2D pixel, Vector2[] TestVectrices, Vector2 position, Vector2 middleposition, Color color)
        {
            int next = 0;
            for (int current = 0; current < TestVectrices.Length; current++)
            {
                next = current + 1;
                if (next == TestVectrices.Length) next = 0;
                Vector2 vc = TestVectrices[current];    // c for "current"
                Vector2 vn = TestVectrices[next];       // n for "next"
                double pente = (vn.Y - vc.Y) / (vn.X - vc.X);
                double b = vc.Y - pente * vc.X;
                double Forvalue1 = vc.X;
                double Forvalue2 = vn.X;
                bool ForLoopY = false;

                if (Math.Abs(vc.Y - vn.Y) > Math.Abs(vc.X - vn.X))
                {
                    Forvalue1 = vc.Y;
                    Forvalue2 = vn.Y;
                    ForLoopY = true;
                }
                if (Forvalue1 > Forvalue2)
                {
                    double savedvalue = Forvalue1;
                    Forvalue1 = Forvalue2;
                    Forvalue2 = savedvalue;
                }
                for (int i = Convert.ToInt32(Forvalue1); i < Convert.ToInt32(Forvalue2); i++)
                {
                    if (ForLoopY)
                    {
                        //pente = (Forvalue2 - Forvalue1) / (vc.X  - vn.X );
                        int speciealy;
                        if (pente > 1000000 || pente < -1000000)
                            speciealy = Convert.ToInt32(vn.X);
                        else
                            speciealy = Convert.ToInt32((i - b) / pente);


                        spriteBatch.Draw(pixel, new Vector2(speciealy, i) - position + middleposition, color);
                    }
                    else
                    {
                        int speciealx;
                        if (pente > 1000000 || pente < -1000000)
                            speciealx = Convert.ToInt32(vn.Y);
                        else
                            speciealx = Convert.ToInt32(pente * i + b);


                        spriteBatch.Draw(pixel, new Vector2(i, speciealx) - position + middleposition, color);

                    }

                }

            }
        }
        public static void DrawVertice(this SpriteBatch spriteBatch, Texture2D pixel, Vector2 StartPoint, Vector2 EndPoint, Vector2 position, Vector2 middleposition, Color color)
        {
            Vector2 vc = StartPoint;    // c for "current"
            Vector2 vn = EndPoint;       // n for "next"
            double pente = (vn.Y - vc.Y) / (vn.X - vc.X);
            double b = vc.Y - pente * vc.X;
            double Forvalue1 = vc.X;
            double Forvalue2 = vn.X;
            bool ForLoopY = false;

            if (Math.Abs(vc.Y - vn.Y) > Math.Abs(vc.X - vn.X))
            {
                Forvalue1 = vc.Y;
                Forvalue2 = vn.Y;
                ForLoopY = true;
            }
            if (Forvalue1 > Forvalue2)
            {
                double savedvalue = Forvalue1;
                Forvalue1 = Forvalue2;
                Forvalue2 = savedvalue;
            }
            for (int i = Convert.ToInt32(Forvalue1); i < Convert.ToInt32(Forvalue2); i++)
            {
                if (ForLoopY)
                {
                    //pente = (Forvalue2 - Forvalue1) / (vc.X  - vn.X );
                    int speciealy;
                    if (pente > 1000000 || pente < -1000000)
                        speciealy = Convert.ToInt32(vn.X);
                    else
                        speciealy = Convert.ToInt32((i - b) / pente);


                    spriteBatch.Draw(pixel, new Vector2(speciealy, i) - position + middleposition, color);
                }
                else
                {
                    int speciealx;
                    if (pente > 1000000 || pente < -1000000)
                        speciealx = Convert.ToInt32(vn.Y);
                    else
                        speciealx = Convert.ToInt32(pente * i + b);


                    spriteBatch.Draw(pixel, new Vector2(i, speciealx) - position + middleposition, color);

                }



            }
        }

        public static Texture2D Resize(GraphicsDevice _GraphicsDevice, Texture2D texture, double ResizeamountX, double ResizeamountY = 0)
        {
            if (ResizeamountY == 0) ResizeamountY = ResizeamountX;
            Texture2D newtexture = new Texture2D(_GraphicsDevice, Convert.ToInt32(texture.Width * ResizeamountX), Convert.ToInt32(texture.Height * ResizeamountY));
            Color[] newcolorData = new Color[Convert.ToInt32(texture.Width * ResizeamountX) * Convert.ToInt32(texture.Height * ResizeamountY)];

            Color[] colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);
            for (int x = 0; x < texture.Height; x++)
                for (int y = 0; y < texture.Width; y++)
                {
                    int index = x * texture.Width + y;
                    for (int newx = Convert.ToInt32(x * ResizeamountY); newx < Convert.ToInt32(x * ResizeamountY + ResizeamountY); newx++)
                        for (int newy = Convert.ToInt32(y * ResizeamountX); newy < Convert.ToInt32(y * ResizeamountX + ResizeamountX); newy++)
                        {
                            int newindex = Convert.ToInt32(newx * newtexture.Width + newy);
                            newcolorData[newindex] = colorData[index];
                        }
                }

            newtexture.SetData(newcolorData);
            return newtexture;
        }
    }

    public static class CreateVertice
    {
        public static (Texture2D, Vector2) NewTexture(GraphicsDevice _GraphicsDevice, Point PointA, Point PointB)
        {
            double m = ((PointA.X - PointB.X) == 0) ? 1000 : (double)(PointA.Y - PointB.Y) / (double)(PointA.X - PointB.X);
            //int extraY = PointA.Y < PointB.Y ? PointA.Y : PointB.Y;
            //int extraX = PointA.X < PointB.X ? PointA.X : PointB.X;
            int width = Math.Abs(PointA.X - PointB.X) == 0 ? 1 : Math.Abs(PointA.X - PointB.X);
            int height = Math.Abs(PointA.Y - PointB.Y) == 0 ? 1 : Math.Abs(PointA.Y - PointB.Y);
            int b = m < 0 ? height - 1 : 0;
            Texture2D image = new Texture2D(_GraphicsDevice, width, height);
            Color[] colorData = new Color[image.Width * image.Height];
            if (height > width)
                for (int y = 0; y < image.Height; y++)
                {
                    //for(int x = -2; x < 2 + 1; x++)
                    //{
                    //    if((x + (int)((y - b) / m) >= 0 ) && (x + (int)((y - b) / m)) < width)
                    //        {
                    int index = y * image.Width + (int)((y - b) / m);// + x);
                    colorData[index] = Color.White;// * (float)((double)1/(1 + Math.Abs(x) ));

                    //        }

                    //}


                }
            else
                for (int x = 0; x < image.Width; x++)
                //for (int y = 0; y < image.Width; y++)
                {

                    int index = (int)((m * x + b)) * image.Width + x;
                    colorData[index] = Color.White;

                }


            image.SetData(colorData);
            Vector2 reletivepos = new Vector2(PointA.X > PointB.X ? width : 0, PointA.Y > PointB.Y ? height : 0);
            return (image, reletivepos);
        }
    }
    public static class FadeDraw
    {
        public static fadeDrawStruct[] items = new fadeDrawStruct[0];

        public static void newItem(Texture2D image, Vector2 position, int lastingTime, bool isPlayerDependant = true)
        {
            Array.Resize(ref items, items.Length + 1);
            items[items.Length - 1].texture = image;
            items[items.Length - 1].position = position;
            items[items.Length - 1].startTime = Game1.time;
            items[items.Length - 1].endTime = Game1.time + lastingTime;
            items[items.Length - 1].isPlayerDependant = isPlayerDependant;
        }
        public static void draw(SpriteBatch spriteBatch, Vector2 middlepos)
        {
            int done = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (done > 0)
                {
                    items[i - done] = items[i];
                }
                if (items[i - done].endTime > items[i - done].startTime && items[i - done].endTime > Game1.time || items[i - done].endTime < items[i - done].startTime && items[i - done].endTime < Game1.time)
                {
                    Vector2 playerpos = items[i - done].isPlayerDependant ? Game1.position : new Vector2(0, 0);
                    spriteBatch.Draw(items[i - done].texture, items[i - done].position - playerpos + middlepos, Color.White * (float)((double)(items[i - done].endTime - Game1.time) / (double)(items[i - done].endTime - items[i - done].startTime)));
                }
                else
                {
                    done += 1;
                }
            }
            if (done > 0)
                Array.Resize(ref items, items.Length - done);

        }
        public static void clear()
        {
            Array.Resize(ref items, 0);

        }
    }
    public struct fadeDrawStruct
    {
        public Texture2D texture;
        public Vector2 position;
        public int endTime;
        public int startTime;
        public bool isPlayerDependant;
    }
}