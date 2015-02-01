using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using System.Threading;
using VisualNovel.Managers;
namespace VisualNovel.Screens
{
    class StoryScreen : GameScreen
    {
        private List<XElement> StoryElements;
        private List<XElement> interactionOptions;
        private List<string[]> StoryElementText;
        private XDocument doc;
        private Vector2 dialoguePosition = Vector2.Zero;
        private Vector2 choicePosition1, choicePosition2;
        private Vector2 button1Pos, button2Pos;
        private Vector2 bubblePos = Vector2.Zero, textPos;
        private int dialogueIndex, optionSelected = 0;
        private UIElement button1, button2;
        private UIElement chooseElement;
        private UIElement speechBubble, sideImage;
        private UIElement cornerButton;
        private Texture2D cornerButtonTexture;
       // private Texture2D background;
        private SpriteFont interactionText, messageText, characterText, currentText, headerText;
        private bool displayHeader = true;
        private string currentElementType = "";
        Color headerColor = Color.Black;

        public Vector2 cornerButtonPos
        {
            get { return cornerButton.position; }
            set
            {
               
                if(speechBubble.texture != null)
                {
                    switch (currentElementType)
                    {
                        case "MessageBubble":
                            {
                                cornerButton.position = new Vector2((GameManager.screenDimensions.X - cornerButtonTexture.Width), (GameManager.screenDimensions.Y - cornerButtonTexture.Height));
                                value = cornerButton.position;
                                break;
                            }

                        case "PersonalBubble":
                            {
                                cornerButton.position = new Vector2(0, GameManager.screenDimensions.Y - cornerButtonTexture.Height);
                                value = cornerButton.position;
                                break;
                            }

                        case "TalkBubble":
                            {
                                cornerButton.position = new Vector2((GameManager.screenDimensions.X - cornerButtonTexture.Width), (GameManager.screenDimensions.Y - cornerButtonTexture.Height));
                                value = cornerButton.position;
                                break;
                            }

                    }
                }
            }
        }
        public StoryScreen()
            
        {
            Thread.Sleep(150);
        }


        public override void LoadContent()
        {
            base.LoadContent();
            controllerManager.x += nextDialogue;

            StoryElements = new List<XElement>();
            dialogueIndex = 0;
            dialoguePosition = new Vector2(600, 300);
            Texture2D button1Texture = GameManager.screenManager.content.Load<Texture2D>("UIElements/button_1");
            button1Pos = new Vector2(720, 600 * GameManager.aspectRatio);
            button1 = new UIElement(button1Pos, button1Texture, false);

            Texture2D button2Texture = GameManager.screenManager.content.Load<Texture2D>("UIElements/button_2");
            button2Pos = new Vector2(720, button1Pos.Y + button1Texture.Height + 20);
            button2 = new UIElement(button2Pos, button2Texture, false);

            Texture2D choiceGraphic = GameManager.screenManager.content.Load<Texture2D>("UIElements/choose_screen");
            chooseElement = new UIElement(Vector2.Zero, choiceGraphic, 2.0f);


            choicePosition1 = new Vector2((button1Pos.X + (155 * GameManager.aspectRatio)), (button1Pos.Y + (50 * GameManager.aspectRatio)));
            choicePosition2 = new Vector2((button2Pos.X + (155 * GameManager.aspectRatio)), (button2Pos.Y + (50 * GameManager.aspectRatio)));

            cornerButtonTexture = GameManager.screenManager.content.Load<Texture2D>("UIElements/corner_button");
            
            
            cornerButton = new UIElement(new Vector2(), cornerButtonTexture, false);
            elements.Add(cornerButton);

            controllerManager.b += backtoMenu;

            interactionText = GameManager.screenManager.content.Load<SpriteFont>("InteractionText");
            messageText = GameManager.screenManager.content.Load<SpriteFont>("MessageText");
            characterText = GameManager.screenManager.content.Load<SpriteFont>("ElementText");
            headerText = GameManager.screenManager.content.Load<SpriteFont>("HeaderText");
        }

        public void LoadFromXML(String directory)
        {
            StoryElementText = new List<string[]>();
            doc = XDocument.Load(directory);
            if (doc != null && StoryElements.Count == 0)
            {
               
                foreach (XElement element in doc.Root.Elements())
                {
                    if (element.Element("Bubble") != null && element.Element("Bubble").Value != "")
                    {
                        if (element.Element("Bubble").Value == "MessageBubble")
                        {
                            currentText = messageText;
                        }

                        else
                        {
                            currentText = characterText;
                        }
                    }
                        StoryElements.Add(element);
                        if (element.Elements(element.Name).Count() == 1)
                        {
                            string[] text = new string[1];
                            text[0] = parseText(element.Element(element.Name).Value, element.Element("Bubble"));
                            StoryElementText.Add(text);
                        }

                        else if (element.Elements(element.Name).Count() == 2)
                        {

                            string[] text = new string[2];
                            try
                            {
                            text[0] = parseText(element.Elements(element.Name).ElementAt(0).Value, 
                                  element.Elements("Bubble").ElementAt(0));
                            text[1] = parseText(element.Elements(element.Name).ElementAt(1).Value,
                                  element.Elements("Bubble").ElementAt(1));

                            }

                            catch
                            {
                                if (element.Name == "StoryInteraction")
                                {
                                    text[0] = parseText(element.Elements(element.Name).ElementAt(0).Value, 610);
                                    text[1] = parseText(element.Elements(element.Name).ElementAt(1).Value, 610);
                               
                                }

                                else
                                {
                                   
                                    string s = element.Elements(element.Name).ElementAt(0).Value;
                                    s.ToString();
                                    text[0] = parseText(element.Elements(element.Name).ElementAt(0).Value,
                                        element.Elements("Bubble").ElementAt(0));
                                    text[1] = parseText(element.Elements(element.Name).ElementAt(1).Value, 
                                        element.Elements("Bubble").ElementAt(0));
                                }
                            }
                            StoryElementText.Add(text);
                        }
                        else
                        {
                            string[] text = { " " };
                            StoryElementText.Add(text);
                        }
                         
                }

                headerColor = (StoryElements[0].Element("Brightness").Value == "Dark") ? Color.Black :
                    Color.White;
            }

          
            InteractionCleanup();
            if (StoryElements.ElementAt(dialogueIndex).Element("Bubble") != null)
            {
                speechBubble = GameManager.speechBubbles[StoryElements.ElementAt(dialogueIndex).Element("Bubble").Value];

                if (StoryElements.ElementAt(dialogueIndex).Element("Bubble").Value == "MessageBubble")
                {
                    currentText = messageText;
                }

                else
                {
                    currentText = characterText;
                }
            }
          /*      
            
            if (StoryElements.ElementAt(dialogueIndex).Element("SideImage") != null && 
                StoryElements.ElementAt(dialogueIndex).Element("SideImage").Value != "")
            {
                sideImage = GameManager.sideImages[StoryElements.ElementAt(dialogueIndex).Element("SideImage").Value];
            }*/
        }

        public void backtoMenu()
        {
            //request player input in future

            GameManager.screenManager.ClearScreens();
            GameManager.screenManager.AddScreen(GameManager.screens["MainMenu"]);
        }
        public void InteractionCleanup()
        {
            if (StoryElements.ElementAt(dialogueIndex).Name == "Header")
            {
                dialogueIndex++;
            }
           
            if (StoryElements.ElementAt(dialogueIndex).Name == "Background")
            {
                background = GameManager.backgrounds[StoryElements.ElementAt(dialogueIndex).Value.ToString()];
                dialogueIndex++;
            }

            
            if (StoryElements.ElementAt(dialogueIndex).Name == "StoryInteraction")
            {
                elements.Remove(cornerButton);
                interactionOptions = StoryElements.ElementAt(dialogueIndex).Elements().ToList();

                elements.Add(button1);
                elements.Add(button2);
                elements.Add(chooseElement);

                controllerManager.y += nextDialogueSecondButtonPress;
                sideImage = null;
                speechBubble = null;

                
            }

            else
            {
                
                if (!elements.Contains(cornerButton))
                {
                    elements.Add(cornerButton);
                    
                    
                }
                if (StoryElements.ElementAt(dialogueIndex - 1) != null)
                {
                    if (StoryElements.ElementAt(dialogueIndex - 1).Name == "StoryInteraction")
                    {
                        GameManager.soundManager.GetSong("OptionPushed").Play();
                    }
                }
                if (controllerManager.y != null)
                {
                    controllerManager.y = null;
                }
                if (elements.Contains(button1) || elements.Contains(button2) || elements.Contains(chooseElement))
                {
                    elements.Remove(button1);
                    elements.Remove(button2);
                    elements.Remove(chooseElement);

                }

                assignSpeechBubble();
                assignSideImage();
                
                cornerButtonPos = new Vector2();
                    
                    
                }
            }
        
        
       public void nextDialogue()
       {
           if (StoryElements != null)
           {
               try
               {

                   if (StoryElements.ElementAt(dialogueIndex + 1) != null)
                   {

                      
                       if (displayHeader == true && StoryElements[dialogueIndex].Name != "Header" && StoryElements[dialogueIndex].Name != "Background")
                       {
                           displayHeader = false;
                       }
                       
                        if (!displayHeader)
                       {
                           dialogueIndex++;
                       }
                       
                       optionSelected = 0;
                       InteractionCleanup();
                   } 
                   
               }
               catch
               {
                   evauluateEndOfScene();
               }
           }

       }

        public void nextDialogueSecondButtonPress()
        {
            try
            {
                if (StoryElements[dialogueIndex + 1] != null)
                {
                    GameManager.soundManager.GetSong("OptionPushed").Play();
                    dialogueIndex++;

                    optionSelected = 1;
                    InteractionCleanup();
                }
            }
            catch
            {
                evauluateEndOfScene();
            }
        }

        public void evauluateEndOfScene()
        {
            
            if (GameManager.currentScene < GameManager.totalScenes)
            {
                /*if ((!Guide.IsTrialMode || !Guide.SimulateTrialMode )
                    || (Guide.IsTrialMode && Guide.SimulateTrialMode 
                    && GameManager.currentScene < 2))
                {*/
                    StoryScreen nextScreen = new StoryScreen();
                    GameManager.screenManager.ClearScreens();
                    GameManager.screenManager.AddScreen(nextScreen);
                    GameManager.currentScene++;
                    nextScreen.LoadFromXML(@"Scenes/Scene" + GameManager.currentScene.ToString() + ".xml");
               // }
              /*  else
                {
                    GameManager.screenManager.ClearScreens();
                    GameManager.screenManager.AddScreen(GameManager.screens["MainMenu"]);
                    GameManager.currentScene = 1;
                }*/
            }

            else
            {
                GameManager.screenManager.ClearScreens();
                GameManager.screenManager.AddScreen(GameManager.screens["MainMenu"]);
                GameManager.currentScene = 1;
            }
                 
        }

        public void assignSpeechBubble()
        {
            if (StoryElements.ElementAt(dialogueIndex).Element("Bubble") != null && StoryElements.ElementAt(dialogueIndex).Element("Bubble").Value != "")
            {
                if (StoryElements.ElementAt(dialogueIndex).Element("Bubble").Value == "MessageBubble")
                {
                    currentText = messageText;
                }

                else
                {
                    currentText = characterText;
                }

                if (StoryElements.ElementAt(dialogueIndex).Elements("Bubble").Count() <= 1)
                {
                    speechBubble = GameManager.speechBubbles[StoryElements.ElementAt(dialogueIndex).Element("Bubble").Value];
                    textPos = GameManager.speechBubbles[StoryElements[dialogueIndex].Element("Bubble").Value.ToString()].position;
                    currentElementType = StoryElements.ElementAt(dialogueIndex).Element("Bubble").Value;
                }
                else
                {
                    speechBubble = GameManager.speechBubbles[StoryElements.ElementAt(dialogueIndex).Elements("Bubble").ElementAt(optionSelected).Value.ToString()];
                    textPos = GameManager.speechBubbles[StoryElements[dialogueIndex].Elements("Bubble").ElementAt(optionSelected).Value].position;
                    currentElementType = StoryElements.ElementAt(dialogueIndex).Elements("Bubble").ElementAt(optionSelected).Value;
                }

            }
            else
            {
                speechBubble = null;
            }

        }

        public void assignSideImage()
        {
            sideImage = null;
            if (StoryElements.ElementAt(dialogueIndex).Elements("SideImage") != null)
            {
                if (StoryElements.ElementAt(dialogueIndex).Elements("SideImage").Count() == 1)
                {
                    if (StoryElements.ElementAt(dialogueIndex).Element("SideImage").Value != null &&
                        StoryElements.ElementAt(dialogueIndex).Element("SideImage").Value != "")
                    {
                        sideImage = GameManager.sideImages[StoryElements.ElementAt(dialogueIndex).Element("SideImage").Value];
                    }
                }
                if (StoryElements.ElementAt(dialogueIndex).Elements("SideImage").Count() > 1)
                {


                    if (StoryElements.ElementAt(dialogueIndex).Elements("SideImage").ElementAt(optionSelected).Value != null)
                    {
                        if (StoryElements.ElementAt(dialogueIndex).Elements("SideImage").ElementAt(optionSelected).Value != "" &&
                            StoryElements.ElementAt(dialogueIndex).Elements("SideImage").ElementAt(optionSelected).Value != "")
                        {

                            sideImage = GameManager.sideImages[StoryElements.ElementAt(dialogueIndex).
                                Elements("SideImage").ElementAt(optionSelected).Value];

                        }

                    }
                }

            }
        }
        public override void Update()
        {
            base.Update();
        }

        public String parseText(string text, XElement bubbleElement)
        {
            String returnString = String.Empty;
            String line = String.Empty;
            String _word;
            String newText = text.Replace("\n", String.Empty);
            String[] wordArray = newText.Split("* *".ToCharArray());
            foreach (String word in wordArray)
            {
                _word = word;
                _word = cleanStringOfInvalidCharacters(_word);
                try
                {
                    if (currentText.MeasureString(line + _word).Length() > determineBubbleSize(bubbleElement) 
                        * GameManager.aspectRatio)
                    {
                        returnString = returnString + line + "\n";
                        line = String.Empty;
                    }
                    line = line + _word + ' ';
                }

                catch
                {
                        continue;   
                }
            }

            return returnString + line;
        }

        public String parseText(string text, int cutOffPoint)
        {
            String returnString = String.Empty;
            String line = String.Empty;
            String[] wordArray = text.Split("* *".ToCharArray());

            String _word;
            foreach (String word in wordArray)
            {
                _word = word;
                _word = cleanStringOfInvalidCharacters(_word);
                try
                {
                    if (interactionText.MeasureString(line + _word).Length() > cutOffPoint
                        * GameManager.aspectRatio)
                    {
                        returnString = returnString + line + "\n";
                        line = String.Empty;
                    }
                    line = line + _word + ' ';
                }

                catch
                {
                    continue;
                }
            }

            return returnString + line;
        }
        public int determineBubbleSize(XElement bubble)
        {
            int bubbleSize = (bubble.Value == "MessageBubble") ? 900 :
                (bubble.Value == "PersonalBubble") ? 1000 :
                600;

            
            return bubbleSize;
        }
        public string cleanStringOfInvalidCharacters(string _word)
        {
            string returnString = _word;
            if (_word.Contains("’"))
            {
                _word = _word.Replace("’", "'");
            }

            if (_word.Contains("\t"))
            {
                _word = _word.Replace("\t", "");
            }
            returnString = _word;
            return returnString;
        }
        public override void Draw()
        {
            base.Draw();
            SpriteBatch spriteBatch = GameManager.screenManager.spriteBatch;

            if(StoryElements != null && StoryElements.Count > 0)
            {
                if (displayHeader == true)
                {
                    spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    spriteBatch.DrawString(headerText,
                             StoryElements[0].Element("Header").Value.ToString(),
                                 new Vector2(180 * GameManager.aspectRatio, 0), headerColor);
                    spriteBatch.End();
                }
                try
                {
                    if (StoryElements[dialogueIndex] != null)
                    {
                        if (speechBubble != null)
                        {
                            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                            spriteBatch.Draw(speechBubble.texture, Vector2.Zero,
                                new Rectangle(0, 0, speechBubble.texture.Width,
                                    speechBubble.texture.Height), Color.White, 0, Vector2.Zero,
                                        GameManager.aspectRatio, SpriteEffects.None, 0);
                            spriteBatch.End();

                        }
                        if (StoryElements[dialogueIndex].Name == "StoryElement")
                        {
                            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                            spriteBatch.DrawString(currentText,
                                StoryElementText.ElementAt(dialogueIndex)[0], //StoryElements[dialogueIndex].Element("StoryElement").Value.ToString(),
                                    textPos * GameManager.aspectRatio, Color.Black);
                            string s = StoryElementText.ElementAt(dialogueIndex)[0].ToString();
                            s.ToString();
                            spriteBatch.End();
                        }

                        if (StoryElements[dialogueIndex].Name == "StoryInteraction")
                        {
                            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                            spriteBatch.DrawString(interactionText,
                             StoryElementText.ElementAt(dialogueIndex)[0],
                                 choicePosition1, Color.White);
                            spriteBatch.End();

                            spriteBatch.Begin();
                            spriteBatch.DrawString(interactionText,
                             StoryElementText.ElementAt(dialogueIndex)[1], 
                             choicePosition2, Color.White);
                            spriteBatch.End();

                        }

                        if (StoryElements[dialogueIndex].Name == "StoryResponse")
                        {
                            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                            spriteBatch.DrawString(currentText,
                                StoryElementText.ElementAt(dialogueIndex)[optionSelected],//StoryElements.ElementAt(dialogueIndex).Elements("StoryResponse").
                                      textPos * GameManager.aspectRatio, 
                                         Color.Black);
                            spriteBatch.End();
                        }

                     

                        if (sideImage != null)
                        {
                            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                            spriteBatch.Draw(sideImage.texture, Vector2.Zero,
                                new Rectangle(0, 0, sideImage.texture.Width, sideImage.texture.Height), Color.White, 0, Vector2.Zero,
                                        GameManager.aspectRatio, SpriteEffects.None, 0);
                            spriteBatch.End(); 
                        }
                    }
                }
            
                catch
                {
                    spriteBatch.End();
                }
                
            }
            
        }
    }
}
