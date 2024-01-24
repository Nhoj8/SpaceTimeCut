using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;
//using System.Drawing;
//using System.Windows.Forms;
using System.Globalization;
using System.Linq;
//using CellGame;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace SpaceTimeCut
{

    public class Game1 : Game
    {

        public static GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;

        public static System.Random rnd = new System.Random();
        public static Rectangle GraphicsRectangle;

        public static bool testToggle;

        

        public static Color colorclear = Color.White;
        ColorRGB Rainbow;

        MouseState mouseState;
        Point MousePosition;
        public static Boolean Paused = true;
            bool Infomode = false, screensizechanged = false, MouseLeftPressed = false, MoveOneFrame = false, ChatOn = true, ButtonSelected; 

        KeyBoardKey NewMonsters = new KeyBoardKey(), KillMonsters = new KeyBoardKey();
        KeyBoardKey InfoKey = new KeyBoardKey(), EscapeKey = new KeyBoardKey(), DebugButton1 = new KeyBoardKey(), DebugButton2 = new KeyBoardKey(), ChatButton = new KeyBoardKey(), OpenInventory = new KeyBoardKey();
        KeyBoardKey Shoot = new KeyBoardKey(), Switch = new KeyBoardKey();
        KeyBoardKey timeMoveForward = new KeyBoardKey(), timeUndo = new KeyBoardKey(); //, MoveLeft = new KeyBoardKey();
        KeyBoardKey moveRight = new KeyBoardKey(), moveLeft = new KeyBoardKey(), moveDown = new KeyBoardKey(), moveUp = new KeyBoardKey(), ToggleMap = new KeyBoardKey();
        int SelectedBlockType = 0;

        public static Texture2D[] Letters;
        public static Color[] LetterColors;
        String[] ColorString;
        Text Coordinates = new Text(), Extratext = new Text(), Info = new Text();

        float[] FrameDisplayTime = new float[60];
        float FPS;
        int currentFrame;

        bool Loadworld;
        public static Vector2 middleposition;
        public static int time = 0;
        public static Vector2 position;

        public static Chat Chat = new Chat();

        Texture2D pixel;

        public static Texture2D circle, circletransparent;
        //public static Button[] Buttons;
        Button playButton;
        Button settingsButton;
        Button resetButton;
        Button levelSelectButton;
        //Position Anchors
        Texture2D[] Blocks8x8;
        public static Texture2D[] BlocksTexture;
        //public static Texture2D[] PlayerTexture;
        public static Texture2D[][] EntityTextures;
        //public static Texture2D[] PushableEntityTexture;
        public static Texture2D[] BarrierTexture;
        //public static Level[] grids;
        public static int blocksize;
        //public static System.Windows.Forms.Cursor Hand { get; }
        Level level;


        Boolean MainMenuInitilised = false;
        public const int MAINMENU = 0;
        public const int LOADING = 1;
        public const int GAME = 2;
        public const int DEBUGLOADING = 3;
        public const int DEBUG = 4;
        public static int CurrentlyDisplaying = MAINMENU;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //_graphics.ToggleFullScreen();
            _graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void Initialize()
        {
            
            // TODO: Add your initialization logic here


            //playerposition = new Vector2(-playersize / 2, -playersize);
            NewMonsters.NewKeyBoardKey();
            NewMonsters.Key = Keys.N;
            KillMonsters.NewKeyBoardKey();
            KillMonsters.Key = Keys.B;
            InfoKey.Key = Keys.F3;
            EscapeKey.Key = Keys.Escape;
            DebugButton1.NewKeyBoardKey();
            DebugButton1.Key = Keys.C;
            DebugButton2.NewKeyBoardKey();
            DebugButton2.Key = Keys.V;
            Shoot.Key = Keys.Space;
            ChatButton.Key = Keys.OemQuestion;
            OpenInventory.Key = Keys.E;
            timeMoveForward.Key = Keys.Right;
            timeUndo.Key = Keys.Left;
            Switch.Key = Keys.L;
            moveRight.Key = Keys.D;
            //moveRight.activateOnStateChange = true;
            moveLeft.Key = Keys.A;
           // moveLeft.activateOnStateChange = true;
            moveUp.Key = Keys.W;
            //moveUp.activateOnStateChange = true;
            moveDown.Key = Keys.S;
            //moveDown.activateOnStateChange = true;
            ToggleMap.Key = Keys.M;

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

            
            //MyExtension().run;

            Letters = new Texture2D[128];
            for (int i = 48; i < 58; i++)
            {
                Letters[i] = Content.Load<Texture2D>("Numbers/" + (i - 48).ToString());
            }
            for (int i = 65; i < 91; i++)
            {
                Letters[i] = Content.Load<Texture2D>("Letters/Letters" + ((Char)i).ToString());
            }
            for (int i = 97; i < 123; i++)
            {
                Letters[i] = Content.Load<Texture2D>("Letters/Letters_" + ((Char)i).ToString());
            }

            string test = "";
            for (int i = 32; i < 129; i++)
            {
                test += (i.ToString() + "'" + ((char)i).ToString() + "', ");

            }
            System.Windows.Forms.MessageBox.Show(test);



            circle = SpriteBatchExtensions.Resize(GraphicsDevice, Getborders(CreateCircle(_spriteBatch.GraphicsDevice,16), 1, Color.Black, 0, false), 2, 2);
            circletransparent = Getborders(CreateCircle(_spriteBatch.GraphicsDevice,16), 1, Color.White,0, true);
            pixel = new Texture2D(this.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[1] { Color.White });

            LetterColors = new Color[21];
            LetterColors[0] = Color.White;
            LetterColors[1] = Color.Red;
            LetterColors[2] = Color.Yellow;
            LetterColors[3] = Color.Lime;
            LetterColors[4] = Color.Cyan;
            LetterColors[5] = Color.Blue;
            LetterColors[6] = Color.Magenta;
            LetterColors[7] = Color.Black;
            LetterColors[8] = Color.DarkGray;
            LetterColors[9] = Color.DarkRed;
            LetterColors[10] = Color.DarkOrange;
            LetterColors[11] = Color.Green;
            LetterColors[12] = Color.DarkCyan;
            LetterColors[13] = Color.DarkBlue;
            LetterColors[14] = Color.DarkMagenta;
            LetterColors[15] = Color.DimGray;

            ColorString = new string[21];
            ColorString[0] = "Ωa";
            ColorString[1] = "Ωb";
            ColorString[2] = "Ωc";
            ColorString[3] = "Ωd";
            ColorString[4] = "Ωe";
            ColorString[5] = "Ωf";
            ColorString[6] = "Ωg";
            ColorString[7] = "Ωh";
            ColorString[8] = "Ωi";
            ColorString[9] = "Ωj";
            ColorString[10] = "Ωk";
            ColorString[11] = "Ωl";
            ColorString[12] = "Ωm";
            ColorString[13] = "Ωn";
            ColorString[14] = "Ωo";
            ColorString[15] = "Ωp";
            //ColorRGB Rainbow = new ColorRGB();
            Rainbow.R = 255;
            Rainbow.G = 0;
            Rainbow.B = 0;
            Rainbow.A = 255;
            LetterColors[20] = Rainbow.col;
            ColorString[20] = "Ωz";
            InitializeMainMenu();
            RedoScreenVars();
        }

        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your Update logic here

            if (screensizechanged)
                RedoScreenVars();
            
            mouseState = Mouse.GetState();
            MousePosition = new Point(mouseState.X, mouseState.Y);

            ref Button[] Buttons = ref Button.Buttons;
            if (CurrentlyDisplaying == MAINMENU)
            {
                if (!MainMenuInitilised)
                    InitializeMainMenu();

                if (mouseState.LeftButton == ButtonState.Released && MouseLeftPressed)
                    MouseLeftPressed = false;
                if (mouseState.LeftButton == ButtonState.Pressed && !MouseLeftPressed)
                {
                    
                    MouseLeftPressed = true;
                    for (int b = 0; b < Buttons.Count(); b++)
                    {
                        if (Buttons[b] != null &&Buttons[b].Selected)
                        {
                            Buttons[b].Click();
                           
                        }
                    }
                }

            }

            else if (CurrentlyDisplaying == LOADING)
            {
                blocksize = 32;

                level = new Level(blocksize);
                //times[0].Initilize();
                Blocks8x8 = new Texture2D[6];
                Blocks8x8[1] = Content.Load<Texture2D>("NewBlocks/Block");
                Blocks8x8[2] = Content.Load<Texture2D>("NewBlocks/Air0");//actually 16x16
                Blocks8x8[3] = Content.Load<Texture2D>("NewBlocks/Player0");
                Blocks8x8[4] = Content.Load<Texture2D>("NewBlocks/Goal");
                Blocks8x8[5] = Content.Load<Texture2D>("NewBlocks/Entity0");

                Texture2D[] PlayerTexture = new Texture2D[4];
                for (int i = 0; i< 4; i++)
                {
                    PlayerTexture[i] = SpriteBatchExtensions.Resize(GraphicsDevice, Content.Load<Texture2D>("NewBlocks/Player"+i.ToString()), Convert.ToDouble(blocksize) / 16);
                }
                Texture2D[] PushableBlockTexture = new Texture2D[4];
                for (int i = 0; i < 4; i++)
                {
                    PushableBlockTexture[i] = SpriteBatchExtensions.Resize(GraphicsDevice, Content.Load<Texture2D>("NewBlocks/Block"), Convert.ToDouble(blocksize) / 16);
                }
                Texture2D[] EntityTexture = new Texture2D[4];
                for (int i = 0; i < 4; i++)
                {
                    EntityTexture[i] = SpriteBatchExtensions.Resize(GraphicsDevice, Content.Load<Texture2D>("NewBlocks/Entity" + i.ToString()), Convert.ToDouble(blocksize) / 16);
                }
                Texture2D[] PushableEntityTexture = new Texture2D[4];
                for (int i = 0; i < 4; i++)
                {
                    PushableEntityTexture[i] = SpriteBatchExtensions.Resize(GraphicsDevice, Content.Load<Texture2D>("NewBlocks/PushableEntity" + i.ToString()), Convert.ToDouble(blocksize) / 16);
                }
                Texture2D[][] NewEntityTextures = { PlayerTexture,PushableBlockTexture, EntityTexture, PushableEntityTexture };
                EntityTextures = NewEntityTextures;
                //Button.NewButton(Button.MOVEBUTTON, "M", Button.TOPMIDDLE, -75, 75, false, 50, 50,true,Color.Green,Color.DarkOliveGreen);
                //Button.NewButton(Button.ROTATEBUTTON, "R", Button.TOPMIDDLE, -25, 75, false, 50, 50, true, Color.Green, Color.DarkOliveGreen);
                //Button.NewButton(Button.CUTBUTTON, "C", Button.TOPMIDDLE, 25, 75, false, 50, 50, true, Color.Green, Color.DarkOliveGreen);



                BlocksTexture = new Texture2D[Blocks8x8.Length + 16];
                for (int i = 1; i < Blocks8x8.Length; i++)
                    BlocksTexture[i] = SpriteBatchExtensions.Resize(GraphicsDevice, Blocks8x8[i], Convert.ToDouble(blocksize) / 16);

                for (int i = 0; i < 16; i++)
                {
                    //BlocksTexture[i+6] = Content.Load<Texture2D>("Barriers/" + (i).ToString().PadLeft(2, '0'));
                    BlocksTexture[i + 6] = SpriteBatchExtensions.Resize(GraphicsDevice, Content.Load<Texture2D>("Barriers/" + (i).ToString().PadLeft(2, '0')), Convert.ToDouble(blocksize) / 16);
                }
                settingsButton.visible = true;
                //Buttons[Button.SETTINGSBUTTON].visible = true;//SETTINGS BUTTON
                Chat.Initialize();
                Chat.NewLine("You Joined The World", time);
                CurrentlyDisplaying = GAME;
                RedoScreenVars();
            }
            else if (CurrentlyDisplaying == GAME)
            {
                if (EscapeKey.CheckKeyPress())
                {
                    PauseGame();

                }

                if (InfoKey.CheckKeyPress())
                    Infomode = !Infomode;
                if(OpenInventory.CheckKeyPress())
                {
                    //PlayerHotbar.Open();
                }


                if (ChatButton.CheckKeyPress())
                    ChatOn = !ChatOn;
                //Exit();

                if (DebugButton1.CheckKeyPress())
                {


                }

                if (DebugButton2.CheckKeyPress())
                {

                }
                if (timeUndo.CheckKeyPress())
                {

                }
                if (Switch.CheckKeyPress())
                {
                    Level.IsGrabbing = !Level.IsGrabbing;
                    Level.IsCutting = !Level.IsCutting;
                }
                if (Paused == false || MoveOneFrame)
                {
                    MoveOneFrame = false;
                    level.MoveTime();//INdex out of bounds of array wtf?

                    time++;
                }
                else
                {

                    if (timeMoveForward.CheckKeyPress())
                        MoveOneFrame = true;

                }


                //position.X += Convert.ToInt32(true == moveRight.CheckKeyPress())*10;
                //position.X -= Convert.ToInt32(true == moveLeft.CheckKeyPress()) * 10;
                //position.Y += Convert.ToInt32(true == moveDown.CheckKeyPress()) * 10;
                //position.Y -= Convert.ToInt32(true == moveUp.CheckKeyPress()) * 10;

                //WARNING CAN GO THROUGH DIAGONAL GAPS IN WALLS
                Point movement = new Point(0, 0);
                movement.X += Convert.ToInt32(moveRight.CheckKeyPress());
                movement.X -= Convert.ToInt32(moveLeft.CheckKeyPress());
                movement.Y += Convert.ToInt32(moveDown.CheckKeyPress());
                movement.Y -= Convert.ToInt32(moveUp.CheckKeyPress());
                if (movement != new Point(0, 0))
                {
                   level.MoveWithKEYS(movement);
                }

                level.Update(MousePosition);






                if (mouseState.LeftButton == ButtonState.Released && MouseLeftPressed)
                {
                    level.Release();

                    MouseLeftPressed = false;

                }
                if (mouseState.LeftButton == ButtonState.Pressed && !MouseLeftPressed)
                {
                    //System.Windows.Forms.MessageBox.Show("test");
                    bool isOnbutton = false;
                    for (int b = 0; b < Buttons.Count(); b++)
                    {
                        if (Buttons[b] != null && Buttons[b].Selected)
                        {
                            
                            Buttons[b].Click();
                            isOnbutton = true;
                        }
                    }
                    if (!isOnbutton)
                        level.Press(MousePosition);

                    MouseLeftPressed = true;
                }


            }

            ButtonSelected = false;
            for (int b = 0; b < Buttons.Count(); b++)
            {
                if (Buttons[b] != null && Buttons[b].visible)
                {
                    if (Buttons[b].Rectangle.Contains(MousePosition))
                    {
                        Buttons[b].Selected = true;
                        ButtonSelected = true;
                    }
                    else
                        Buttons[b].Selected = false;
                }
            }
            DoLettersOnScreen();



            // Last thing, do not move
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            _spriteBatch.Begin();

            ref Button[] Buttons = ref Button.Buttons;
            if (CurrentlyDisplaying == MAINMENU)
            {


            }
            else if (CurrentlyDisplaying == LOADING)
            {
                GraphicsDevice.Clear(Color.Gray);
            }
            else if (CurrentlyDisplaying == GAME)
            {
                //Point coords = position.ToPoint();
                level.Draw(_spriteBatch,colorclear);



                FadeDraw.draw(_spriteBatch, middleposition);
                Coordinates.draw(_spriteBatch, GraphicsRectangle.Width/2-100, 25);
            }
            else if (CurrentlyDisplaying == DEBUGLOADING)
            {

            }
            else if (CurrentlyDisplaying == DEBUG)
            {
               




            }

            Extratext.draw(_spriteBatch, 50, GraphicsRectangle.Height - 50);

            for (int b = 0; b < Buttons.Count(); b++)
            
                if (Buttons[b] != null && Buttons[b].visible)
                {
                    Color LetterColor = colorclear;
                    if (Buttons[b].TextOnly == false)
                    {
                        _spriteBatch.DrawRoundedRect(Buttons[b].Rectangle, // The coordinates of the Rectangle to be drawn
circle, // Texture for the whole rounded rectangle
circle.Width / 2, // Distance from the edges of the texture to the "middle" patch
Buttons[b].Selected ? Buttons[b].Selectedcolor : Buttons[b].color);
                    }
                    else
                    {
                        LetterColor = Buttons[b].Selected ? Color.Red : colorclear;
                    }
                    for (int i = 0; i < Buttons[b].text.chars.Length; i++)
                    {
                        if (LetterColor == colorclear)
                            LetterColor = LetterColors[Buttons[b].text.color[i]];
                        _spriteBatch.Draw(Letters[Buttons[b].text.chars[i]], Buttons[b].text.coords[i] + new Vector2(Buttons[b].Rectangle.X + Buttons[b].Rectangle.Width / 2 - Buttons[b].text.size.Width / 2, Buttons[b].Rectangle.Y + Buttons[b].Rectangle.Height / 2 - Buttons[b].text.size.Height / 2), LetterColor);
                    }


                

            }

            if (Infomode)
                Info.draw(_spriteBatch, 25, 100);

            
            if (ChatOn)
            {
                DrawRectangle(_spriteBatch, new Rectangle(25, GraphicsRectangle.Height - 50 - Chat.Lines * 16, Chat.data.size.Width, Chat.data.size.Height), Color.Black * Chat.OpacityOfRectangle);
                for (int i = 0; i < Chat.data.chars.Length; i++)
                    _spriteBatch.Draw(Letters[Chat.data.chars[i]], Chat.data.coords[i] + new Vector2(25, GraphicsRectangle.Height - 50 - Chat.Lines * 16), LetterColors[Chat.data.color[i]] * (float)(Convert.ToDouble(Chat.TimeDisplayed[Chat.TotalLines - (Chat.Lines - Chat.data.LinePerCharecter[i])] + Chat.TimePerLine - time) / Chat.LengthOfFade));

            }

            _spriteBatch.End();

            FPS = 0;
            float CurrentlyDisplayTime = DateTime.Now.Minute * 60 + DateTime.Now.Second + (float)DateTime.Now.Millisecond / 1000;
            FrameDisplayTime[currentFrame] = CurrentlyDisplayTime - FrameDisplayTime[currentFrame];
            for (int i = 0; i < FrameDisplayTime.Length; i++)
            {

                FPS += FrameDisplayTime[i];
            }
            //int oldFrame = currentFrame;
            if (currentFrame >= 59)
                currentFrame = 0;
            else
                currentFrame += 1;
            FrameDisplayTime[currentFrame] = CurrentlyDisplayTime;


            base.Draw(gameTime);
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            screensizechanged = true;
        }

        void DoLettersOnScreen()
        {





            Extratext.text = "Ωa0Ωb1Ωc2Ωd3Ωe4Ωf5Ωg6Ωh7\nΩi8Ωj9Ωk0Ωl1Ωm2Ωn3Ωo4Ωp5Ωz5  " + "Ωa" + Rainbow.R.ToString() + " " + Rainbow.G.ToString() + " " + Rainbow.B.ToString() + " " + Rainbow.A.ToString();
            Extratext.DoText();

            Chat.RefreshText(time);
            Chat.data.DoText();


            if (CurrentlyDisplaying != MAINMENU)
            {
                if (CurrentlyDisplaying == GAME)
                {
                    Coordinates.text = Level.totalCuts.ToString() + " Cuts out of " + Level.currentMaxCut.ToString();//"Ωg" + position.X.ToString() + ", " + position.Y.ToString();
                    Coordinates.text += "\n" + Level.totalRotations.ToString() + " Rotations out of " + Level.currentMaxRotations.ToString();
                        Coordinates.DoText();
                }

                if (Infomode)
                {
                    Info.text = "";
                    Info.text += "\nFPS: " + (60 / FPS).ToString();

                    Info.text +=
                        "\n\nMOUSE:" +
                        "\n  Position X : " + MousePosition.X.ToString() + " Y: " + MousePosition.Y.ToString();
                    if (CurrentlyDisplaying == GAME)
                        Info.text +=
                         "\n\nOriginalLocationOfMouse:" +
                        "\n  Position X : " + Level.mouseOriginalPosition.X.ToString() + " Y: " + Level.mouseOriginalPosition.Y.ToString();


                    Info.text +=
                        "\n\nΩbTIME: " + time.ToString();

                    Info.DoText();
                }
            }
                if (Rainbow.R == 255 && Rainbow.G == 0 && Rainbow.B > 0)
                    Rainbow.B -= 5;
                else if (Rainbow.R == 255 && Rainbow.G < 255 && Rainbow.B == 0)
                    Rainbow.G += 5;
                else if (Rainbow.R > 0 && Rainbow.G == 255 && Rainbow.B == 0)
                    Rainbow.R -= 5;
                else if (Rainbow.R == 0 && Rainbow.G == 255 && Rainbow.B < 255)
                    Rainbow.B += 5;
                else if (Rainbow.R == 0 && Rainbow.G > 0 && Rainbow.B == 255)
                    Rainbow.G -= 5;
                else if (Rainbow.R < 255 && Rainbow.G == 0 && Rainbow.B == 255)
                    Rainbow.R += 5;
                else
                {
                    Rainbow.R = 255;
                    Rainbow.A = 255;
                }
            LetterColors[20] = Rainbow.col;
        }
        void RedoScreenVars()
        {
            GraphicsRectangle = new Rectangle(this.GraphicsDevice.Viewport.X, this.GraphicsDevice.Viewport.Y, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            middleposition = new Vector2(GraphicsRectangle.Width / 2, GraphicsRectangle.Height / 2);
            screensizechanged = false;

            Button.ResetAnchors(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        }


        void PauseGame()
        {
            Paused = !Paused;
            if (Paused)
                settingsButton.visible = true;
                //Buttons[Button.SETTINGSBUTTON].visible = true;
            else
            {
                settingsButton.visible = false;
                //Buttons[Button.SETTINGSBUTTON].visible = false;
                resetButton.visible = false;
                levelSelectButton.visible = false;

                //Buttons[Button.RESETBUTTON].visible = false;
                //Buttons[Button.NEWWORLDBUTTON].visible = false;
                //Buttons[Button.SAVEWORLDBUTTON].visible = false;
            }
        }



        
        


        void InitializeMainMenu()
        {
            playButton = new Button("Play", Button.TOPMIDDLE, -100, 200, PlayButtonClick, false, 200, 50,true);
            settingsButton = new Button("Settings", Button.TOPRIGHT, -20, 20,SettingsButtonClick);
            resetButton = new Button("Reset", Button.TOPRIGHT, -20, 50,ResetButtonClick);
            levelSelectButton = new Button("Level select", Button.TOPRIGHT, -20, 70, LevelSelectButtonClick);
            //Buttons = new Button[Button.AMOUNT];  //change when adding new button
            //Button.NewButton(Button.PLAYBUTTON, "Play", Button.TOPMIDDLE, -100, 200, false, 200, 50);
            //Button.NewButton(Button.CREATENEWWORLDBUTTON, "Create New World", Button.TOPMIDDLE, -150, 100, false, 300, 50);
            //Button.NewButton(Button.LOADWORLDBUTTON, "Load World", Button.TOPMIDDLE, -150, 200, false, 300, 50);
            //Button.NewButton(Button.DEBUGBUTTON, "Debug", Button.TOPMIDDLE, -150, 300, false, 300, 50);
            //Button.NewButton(Button.WORLD1BUTTON, "World", Button.TOPMIDDLE, -150, 200, false, 300, 100);
            //Button.NewButton(Button.SETTINGSBUTTON, "Settings", Button.TOPRIGHT, -20, 20);
            //Button.NewButton(Button.RESETBUTTON, "Reset", Button.TOPRIGHT, -20, 50);
            //Button.NewButton(Button.NEWWORLDBUTTON, "New World", Button.TOPRIGHT, -20, 70);
            //Button.NewButton(Button.SAVEWORLDBUTTON, "Save World", Button.TOPRIGHT, -20, 90);


            //Buttons[Button.PLAYBUTTON].visible = true;
            MainMenuInitilised = true;
        }
        void PlayButtonClick(object sender, EventArgs e)
        {
            playButton.visible = false;
      
            Level.initLevelSelector();
        }
        void SettingsButtonClick(object sender, EventArgs e)
        {
            resetButton.visible = !resetButton.visible;
            levelSelectButton.visible = !levelSelectButton.visible;
        }
        void ResetButtonClick(object sender, EventArgs e)
        {
            Game1.CurrentlyDisplaying = Game1.LOADING;
        }
        void LevelSelectButtonClick(object sender, EventArgs e)
        {

                for(int i = 0; i < Level.levelSelectors.Length; i++)
            {
                Level.levelSelectors[i].button.visible = true;
            }
                Level.cutButton.visible = false;
            Level.rotateButton.visible = false;
            Level.moveButton.visible = false;
            Game1.CurrentlyDisplaying = Game1.MAINMENU;
        }
        public static Texture2D CreateCircle(GraphicsDevice graphicsDevice, int diameter)
        {
            Texture2D texture = new Texture2D(graphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radius = diameter / 2f;
            float radiussq = radius * radius;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    Vector2 pos2 = new Vector2(x - radius + 1, y - radius);
                    Vector2 pos3 = new Vector2(x - radius, y - radius + 1);
                    Vector2 pos4 = new Vector2(x - radius + 1, y - radius + 1);
                    if (pos.LengthSquared() <= radiussq || pos2.LengthSquared() <= radiussq || pos3.LengthSquared() <= radiussq || pos4.LengthSquared() <= radiussq)
                    {
                            colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
            texture.SetData(colorData);
            return texture;
        }

        public static Texture2D Getborders(Texture2D texture, int bordersize, Color bordercolor, int bordersizeOverImage, bool onlyBorder = false)
        {
            Color actualbordercolor = bordercolor;
            if (onlyBorder) 
            {
                bordercolor = Color.Black;
            }

            //Texture2D newtexture;
            Color[] colorData = new Color[(texture.Width) * (texture.Height)];
            texture.GetData(colorData);
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                {
                    int index = x * texture.Width + y;
                    if (colorData[index] != Color.Transparent)
                        for (int neighborX = x - bordersize; neighborX <= x + bordersize; neighborX++)
                            for (int neighborY = y - bordersize; neighborY <= y + bordersize; neighborY++)
                                if (neighborX != x || neighborY != y)
                                {
                                    int indexNeighbor = neighborX * texture.Width + neighborY;
                                    if (indexNeighbor < 0 || indexNeighbor >= texture.Width * texture.Height || neighborY < 0 || neighborY >= texture.Height)
                                        colorData[index] = bordercolor;
                                    else if (colorData[indexNeighbor] == Color.Transparent)
                                    {
                                        colorData[index] = bordercolor;
                                    }
                                }
                }
            if (onlyBorder)
                for (int x = 0; x < texture.Width; x++)
                    for (int y = 0; y < texture.Height; y++)
                    {
                        int index = x * texture.Width + y;
                        if (colorData[index] == Color.White)
                            colorData[index] = Color.Transparent;
                        else if (colorData[index] == Color.Black)
                            colorData[index] = actualbordercolor;
                    }


            Texture2D TextureFinished = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
            TextureFinished.SetData(colorData);
            return TextureFinished;
        }



        private static Texture2D rect;

        public static void DrawRectangle(SpriteBatch _spriteBatch, Rectangle coords, Color color)
        {
            if (rect == null)
            {
                rect = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
                rect.SetData(new[] { Color.White });
            }
            _spriteBatch.Draw(rect, coords, color);
        }

        public struct ColorRGB
        {
            internal Microsoft.Xna.Framework.Color col;

            public byte R
            {
                get { return col.R; }
                set { col.R = value; }
            }
            public byte G
            {
                get { return col.G; }
                set { col.G = value; }
            }
            public byte B
            {
                get { return col.B; }
                set { col.B = value; }
            }
            public byte A
            {
                get { return col.A; }
                set { col.A = value; }
            }
        }


    }



}




