using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VisualNovel.Managers;
using VisualNovel.Screens;
using Microsoft.Xna.Framework.Audio;
namespace VisualNovel
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameScreen mainMenu;
        Song backgroundMusic;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();
            GameManager.screenDimensions = new Vector2(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            GameManager.aspectRatio = (GameManager.screenDimensions.Y < 1080) ? GameManager.screenDimensions.Y / 1080:
                1;
            
            GameManager.screens = new Dictionary<string, GameScreen>();
            
            GameManager.renderManager = new RenderManager();
            GameManager.screenManager = new ScreenManager();
            
         
            Texture2D logo = Content.Load<Texture2D>("UIElements/main_logo");
            Texture2D startButton = Content.Load<Texture2D>("UIElements/main_start");

            backgroundMusic = Content.Load<Song>("SFX/bgmed");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1.0f;
            MediaPlayer.Play(backgroundMusic);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Guide.SimulateTrialMode = false;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameManager.screenManager.spriteBatch = spriteBatch;

            GameManager.game1 = this;  
            
            GameManager.screenManager.content = this.Content;

            GameManager.soundManager = new SoundManager();
            GameManager.soundManager.Add("ButtonPushed", Content.Load<SoundEffect>("SFX/button-pushed"));
            GameManager.soundManager.Add("OptionPushed", Content.Load<SoundEffect>("SFX/option-pushed"));
            GameManager.soundManager.Add("ChoicePushed", Content.Load<SoundEffect>("SFX/choice-screen"));
            GameManager.soundManager.Add("Laugh", Content.Load<SoundEffect>("SFX/laugh"));
            GameManager.soundManager.Add("Sigh", Content.Load<SoundEffect>("SFX/sigh"));
            
            mainMenu = new GameScreen();
            mainMenu.LoadContent();
            mainMenu.background = Content.Load<Texture2D>("Backgrounds/home5");
            mainMenu.controllerManager.a += start;
            GameManager.screens.Add("MainMenu", mainMenu);
            GameManager.screenManager.AddScreen(mainMenu);

            Texture2D messageBubble = GameManager.screenManager.content.Load<Texture2D>("UIElements/SpeechBubbles/message_bubble");
            Texture2D talkBubble = GameManager.screenManager.content.Load<Texture2D>("UIElements/SpeechBubbles/talk_bubble");
            Texture2D personalBubble = GameManager.screenManager.content.Load<Texture2D>("UIElements/SpeechBubbles/robbie_talk_bubble");

            Vector2 bubblePos = new Vector2(650 * GameManager.aspectRatio,  0);

            GameManager.speechBubbles = new Dictionary<string, UIElement>();
            GameManager.speechBubbles.Add("MessageBubble", new UIElement(new Vector2(140, 690), messageBubble, false));
            GameManager.speechBubbles.Add("TalkBubble", new UIElement(new Vector2(970, 180), talkBubble, false));
            GameManager.speechBubbles.Add("PersonalBubble", new UIElement(new Vector2(800, 640), personalBubble, false));

            Texture2D junkohappy = Content.Load<Texture2D>("UIElements/SideImages/junkohappy");
            Texture2D junkoconcerned = Content.Load<Texture2D>("UIElements/SideImages/junkoconcerned");
            Texture2D junkoembarrassed = Content.Load<Texture2D>("UIElements/SideImages/junkoembarrassed");
            Texture2D junkoextremelyhappy = Content.Load<Texture2D>("UIElements/SideImages/junkoextremelyhappy");
            
            GameManager.sideImages = new Dictionary<string, UIElement>();
            GameManager.sideImages.Add("junkohappy", new UIElement(Vector2.Zero, junkohappy, false));
            GameManager.sideImages.Add("junkoconcerned", new UIElement(Vector2.Zero, junkoconcerned, false));
            GameManager.sideImages.Add("junkoembarrassed", new UIElement(Vector2.Zero, junkoembarrassed, false));
            GameManager.sideImages.Add("junkoextremelyhappy", new UIElement(Vector2.Zero, junkoextremelyhappy, false));

            Texture2D marikoconcerned = Content.Load<Texture2D>("UIElements/SideImages/marikoconcerned");
            Texture2D marikoembarrassed = Content.Load<Texture2D>("UIElements/SideImages/marikoembarrassed");
            Texture2D marikoextremelyhappy = Content.Load<Texture2D>("UIElements/SideImages/marikoextremelyhappy");
            Texture2D marikohappy = Content.Load<Texture2D>("UIElements/SideImages/marikohappy");


            GameManager.sideImages.Add("marikohappy", new UIElement(Vector2.Zero, marikohappy, false));
            GameManager.sideImages.Add("marikoconcerned", new UIElement(Vector2.Zero, marikoconcerned, false));
            GameManager.sideImages.Add("marikoembarrassed", new UIElement(Vector2.Zero, marikoembarrassed, false));
            GameManager.sideImages.Add("marikoextremelyhappy", new UIElement(Vector2.Zero, marikoextremelyhappy, false));

            Texture2D aiconcerned = Content.Load<Texture2D>("UIElements/SideImages/aiconcerned");
            Texture2D aiembarrassed = Content.Load<Texture2D>("UIElements/SideImages/aiembarrassed");
            Texture2D aiextremelyhappy = Content.Load<Texture2D>("UIElements/SideImages/aiextremelyhappy");
            Texture2D aihappy = Content.Load<Texture2D>("UIElements/SideImages/aihappy");

            GameManager.sideImages.Add("aiconcerned", new UIElement(Vector2.Zero, aiconcerned, false));
            GameManager.sideImages.Add("aiembarrassed", new UIElement(Vector2.Zero, aiembarrassed, false));
            GameManager.sideImages.Add("aiextremelyhappy", new UIElement(Vector2.Zero, aiextremelyhappy, false));
            GameManager.sideImages.Add("aihappy", new UIElement(Vector2.Zero, aihappy, false));

            Texture2D sidegirlhappy = Content.Load<Texture2D>("UIElements/SideImages/sidegirl");
            Texture2D oldmanangry = Content.Load<Texture2D>("UIElements/SideImages/oldman");
            
            GameManager.sideImages.Add("sidegirlhappy", new UIElement(Vector2.Zero, sidegirlhappy, false));
            GameManager.sideImages.Add("oldmanangry", new UIElement(Vector2.Zero, oldmanangry, false));

            GameManager.backgrounds = new Dictionary<string, Texture2D>();

            Texture2D baths = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/baths");
            GameManager.backgrounds.Add("baths", baths);

           Texture2D changingRoom= Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/changing-room");
           Texture2D changingRoomDoor = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/changing-room-door");
            GameManager.backgrounds.Add("changingRoom", changingRoom);
            GameManager.backgrounds.Add("changingRoomDoor", changingRoomDoor);
            
            Texture2D hallway, hallway2, hallway3;

            hallway = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/hallway-interior-optional");
            hallway2 = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/hallway-2");
            hallway3 = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/hallway3");
            GameManager.backgrounds.Add("hallway", hallway);
            GameManager.backgrounds.Add("hallway2", hallway2);
            GameManager.backgrounds.Add("hallway3", hallway3);

            Texture2D hotelInterior, hotelOutside;
            hotelInterior = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/hotel-interior");
            hotelOutside = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/hotel-outside");
            GameManager.backgrounds.Add("hotelInterior", hotelInterior);
            GameManager.backgrounds.Add("hotelOutside", hotelOutside);

            Texture2D reception, reception2;
            reception = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/reception");
            reception2 = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/reception2");
            GameManager.backgrounds.Add("reception", reception);
            GameManager.backgrounds.Add("reception2", reception2);

            Texture2D roomBathroom, roomDaytime, roomDaytime2, roomFront, roomNight, roomNight2;
            roomBathroom = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/room-bathroom");
            roomDaytime = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/room-daytime2");
            roomDaytime2 = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/room-daytime2");
            roomFront = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/room-front");
            roomNight = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/room-night");
            roomNight2 = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/room-night2");

            GameManager.backgrounds.Add("roomBathroom", roomBathroom);
            GameManager.backgrounds.Add("roomDaytime", roomDaytime);
            GameManager.backgrounds.Add("roomDaytime2", roomDaytime2);
            GameManager.backgrounds.Add("roomFront", roomFront);
            GameManager.backgrounds.Add("roomNight", roomNight);
            GameManager.backgrounds.Add("roomNight2", roomNight2);

         
            Texture2D train = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/train");
            GameManager.backgrounds.Add("train", train);

            Texture2D woods = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/woods-outside-hotel");
            GameManager.backgrounds.Add("woods", woods);

            Texture2D stars = Content.Load<Texture2D>("Backgrounds/SceneBackgrounds/stars");
            GameManager.backgrounds.Add("stars", stars);

            
        }

        public void start()
        {
            StoryScreen nextScreen = new StoryScreen();
            GameManager.screenManager.ClearScreens();
            GameManager.screenManager.AddScreen(nextScreen);
            nextScreen.LoadFromXML(@"Scenes/Scene" + GameManager.currentScene.ToString() + ".xml");
            GameManager.soundManager.GetSong("OptionPushed").Play();

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            GameManager.screenManager.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

           
            GameManager.screenManager.Draw();
           // spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
           // spriteBatch.End(); 

            base.Draw(gameTime);
        }
    }
}
