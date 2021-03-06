﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const int NUMBER_OF_COLORS = 4; //representing the colours as a number instead of a string
        const int COLOR1 = 1; //green
        const int COLOR2 = 2; //red
        const int COLOR3 = 3; //yellow
        const int COLOR4 = 4; //blue
        const int MAX = 50; //max sequence allowed (50 animatons in a row)

        Random r;

        Boolean userTurn = true; //specify if the user can guess or not
        Boolean gameRunning = false; //specify if the game is over or not
        Boolean guessCorrect = false; //check if the user's guess is correct
        Boolean inAnimation = false;

        int[] generatedColorSequence; //array to store the generated sequence
        int[] usersGuessSequence; //array to store the users input

        int generatedColorSequenceIndex = 0; //need to keep track of index to play animation
        int usersGuessSequenceIndex = 0; //keep track of users input to compare to generated

        int sequenceLength = 0; //current size of sequence to be guessed

        //int mi = 0;

        public MainPage()
        {
            this.InitializeComponent();

            r = new Random(); //seeded random number based on time

            txtTurnNumber.Visibility = Visibility.Collapsed; //set counter to collapsed when application starts

            generatedColorSequence = new int[MAX]; //set to max to generate random number
            usersGuessSequence = new int[MAX];

           createColorSequence(); //generate a sequence when they start the application
        }

        private void blueRect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //animate opacity
            if (userTurn == true)
            {
                storyboardBlueRectUser.Begin(); //if its the users guess start the animation

                if (gameRunning == true) //if the game is not over (only time gameRunning will be false)
                {
                    usersGuessSequence[usersGuessSequenceIndex++] = COLOR4;//move to the next sequence (after first one runs)
                    userTurn = false;
                    compareGuess();
                }
            }
        }

        private void yellowRect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //txtRandom.Text = generatedColorSequence[mi].ToString();
            //mi++;
            //storyboardYellowRect.Begin(); //start animation when shape is tapped/clicked
            if (userTurn == true)
            {
                storyboardYellowRectUser.Begin();//start animation when shape is tapped/clicked

                if (gameRunning == true)//if the game is not over (only time gameRunning will be false)
                {
                    usersGuessSequence[usersGuessSequenceIndex++] = COLOR3;//move to the next sequence (after first one runs)
                    userTurn = false;
                    compareGuess();
                }
            }
        }

        private void greenRect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //storyboardGreenRect.Begin(); //start animation when shape is tapped/clicked
            if (userTurn == true)
            {
                storyboardGreenRectUser.Begin();

                if (gameRunning == true)//if the game is not over (only time gameRunning will be false)
                {
                    usersGuessSequence[usersGuessSequenceIndex++] = COLOR1;//move to the next sequence (after first one runs)
                    userTurn = false;
                    compareGuess();
                }
            }
        }

        private void redRect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //storyboardRedRect.Begin(); //start animation when shape is tapped/clicked
            if (userTurn == true)
            {
                storyboardRedRectUser.Begin();

                if (gameRunning == true)//if the game is not over (only time gameRunning will be false)
                {
                    usersGuessSequence[usersGuessSequenceIndex++] = COLOR2; //move to the next sequence (after first one runs)
                    userTurn = false;
                    compareGuess();
                }
            }
        }

        private void Tapped_txtStart(object sender, TappedRoutedEventArgs e)
        {
            //txtStart.Visibility = Visibility.Collapsed;
            //start game
            if (txtStart.Text == "Tap to Start")
            {
                if (txtStart.Visibility != Visibility.Collapsed) //if the game has not already been started
                {
                    //txtRandom.Text = "yay!!";
                    play(); //start the game
                }
            }
            else if (txtStart.Text != "Tap to Start") //which means its equal to winner
            {
                txtStart.Text = "Tap to Start";
                gameOver();
            }
            //will change text to winner if hits max.. then txtStart wont equal Tap to start
            else
            {
            }
        }
        private void play()
        {
            //initialise variables here
            sequenceLength = 0; //has to be 0 at start of game
            txtTurnNumber.Text = sequenceLength.ToString(); //set the turn number to current sequence length                                                      //createColorSequence(); //do this incase they have started a new game

            txtTurnNumber.Visibility = Visibility.Visible; //set counter to Visible when the game starts
            txtStart.Visibility = Visibility.Collapsed; //set Start text to collapsed when the game starts

            gameRunning = true;
            usersGuessSequenceIndex = 0; //set users index back to 0
            createColorSequence(); //do this incase they have started a new game
            userTurn = false;
            //commented out for testing
            //userTurn = false;


            compareGuess();
        }

        private void compareGuess()
        {
            guessCorrect = true;
            for (int i = 0; i < usersGuessSequenceIndex; i++) //get to current sequence index
            {
                if (generatedColorSequence[i] == usersGuessSequence[i]) //compare generated guess to users guess
                {
                    //if they are the same we dont have to do anything
                }
                else
                {
                    guessCorrect = false; //otherwise they have guessed in correctly and we set the guess to false
                    break;
                }
            }

            if (guessCorrect == true) //if the guess was correct
            {
                if (usersGuessSequenceIndex >= sequenceLength) //move to the next item
                {
                    sequenceLength++;
                    if (sequenceLength < MAX) //if we arent at the max number
                    {
                        audioShinyDing.Play();
                        txtTurnNumber.Text = sequenceLength.ToString(); //update the turn number
                    }
                    else // turn number must be equal to MAX so they have won!
                    {
                        txtStart.Text = "Winner";
                        gameOver();
                    }
                }
                else //the color index is less than the round number so we keep going
                {
                    userTurn = true;
                }
            }
            else //when the guess is not correct change the txtStart message to let them try again
            {
                txtStart.Text = "Tap to Start";
                //txtRandom.Text = "gg!";
                gameOver();
            }
        }

        private void gameOver()
        {
            //initialise variables here
            gameRunning = false;
            txtTurnNumber.Visibility = Visibility.Collapsed; //set counter to collapsed when the game ends
            txtStart.Visibility = Visibility.Visible; //set Start text to visible when the game ends
            userTurn = true;
        }

        private void storyboardGreenRectAnimationFin(object sender, object e)
        {
            if (generatedColorSequenceIndex < sequenceLength) //if user is done and sequence is correct , proceed with sequence
            {
                playGeneratedSequence();
            }
            else
            {
                inAnimation = false; //not doing an animation
                userTurn = true; //users turn to guess
                generatedColorSequenceIndex = 0; //set back to first index on each to compare guesses
                usersGuessSequenceIndex = 0;
            }
        }
        private void storyboardRedRectAnimationFin(object sender, object e)
        {
            if (generatedColorSequenceIndex < sequenceLength) //if user is done and sequence is correct , proceed with sequence
            {
                playGeneratedSequence();
            }
            else
            {
                inAnimation = false;//not doing an animation
                userTurn = true;//users turn to guess
                generatedColorSequenceIndex = 0;//set back to first index on each to compare guesses
                usersGuessSequenceIndex = 0;
            }
        }
        private void storyboardYellowRectAnimationFin(object sender, object e)
        {
            if (generatedColorSequenceIndex < sequenceLength) //if user is done and sequence is correct , proceed with sequence
            {
                playGeneratedSequence();
            }
            else
            {
                inAnimation = false;//not doing an animation
                userTurn = true;//users turn to guess
                generatedColorSequenceIndex = 0;//set back to first index on each to compare guesses
                usersGuessSequenceIndex = 0;
            }
        }
        private void storyboardBlueRectAnimationFin(object sender, object e)
        {
            if (generatedColorSequenceIndex < sequenceLength) //if user is done and sequence is correct , proceed with sequence
            {
                playGeneratedSequence();
            }
            else
            {
                inAnimation = false;//not doing an animation
                userTurn = true;//users turn to guess
                generatedColorSequenceIndex = 0;//set back to first index on each to compare guesses
                usersGuessSequenceIndex = 0;
            }
        }

        private void createColorSequence()
        {
            //generate a random number to represent colours
            for (int i = 0; i < MAX; i++)
            {
                //generating entire sequence
                generatedColorSequence[i] = r.Next(1, NUMBER_OF_COLORS); //generate random number and store it in the generatedSeq array
                usersGuessSequence[i] = 0;
                //txtRandom.Text = "generated!";
            }
        }

        private void playGeneratedSequence()
        {
            if (generatedColorSequence[generatedColorSequenceIndex] == 1) //if generated sequence is 1 play greenRect Animation
            {
                storyboardGreenRect.Begin(); //start the animation to show user sequence
                inAnimation = true; //doing animation, so everything has to wait
            }
            else if (generatedColorSequence[generatedColorSequenceIndex] == 2)//if generated sequence is 1 play redRect Animation
            {
                storyboardRedRect.Begin(); //start the animation to show user sequence
                inAnimation = true; //doing animation, so everything has to wait
            }
            else if (generatedColorSequence[generatedColorSequenceIndex] == 3)//if generated sequence is 1 play yellowRect Animation
            {
                storyboardYellowRect.Begin(); //start the animation to show user sequence
                inAnimation = true; //doing animation, so everything has to wait
            } 
            else if (generatedColorSequence[generatedColorSequenceIndex] == 4)//if generated sequence is 1 play blueRect Animation
            {
                storyboardBlueRect.Begin(); //start the animation to show user sequence
                inAnimation = true; //doing animation, so everything has to wait
            }
            generatedColorSequenceIndex++; //increase the index to move to next number in the sequence
        }

        private void audioShinyDing_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (gameRunning)
            {
                playGeneratedSequence();
            }
        }

    }
}
