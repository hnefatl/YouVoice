using System;
using System.Collections.Generic;
using System.Threading;

using System.Speech.Recognition;
using SpeechLib;

namespace YouVoice
{
    public delegate void ListenerEvent();

    public class Listener
    {
        #region Members

        protected SpeechRecognitionEngine Recog;
        protected List<Grammar> Grammars;

        protected bool Listening;
        protected bool Running;

        protected Thread ListeningThread;

        #endregion Members

        #region Events
        public event ListenerEvent OnPlay;
        public event ListenerEvent OnPause;
        public event ListenerEvent OnNext;
        public event ListenerEvent OnPrevious;
        public event ListenerEvent OnExit;
        public event ListenerEvent OnStartListening;
        public event ListenerEvent OnStopListening;
        #endregion

        public Listener()
        {
            // Give base values to variables
            Grammars = new List<Grammar>();
            Listening = false;
            Running = false;

            // Set up OnExit clause to stop thread
            OnExit += () =>
            {
                Running = false;
                Listening = false;
            };
            OnStartListening += () =>
            {
                Listening = true;
            };
            OnStopListening += () =>
            {
                Listening = false;
            };
        }

        public bool Initialise()
        {
            #region Initialising Grammars
            Grammars.Add(new Grammar(new GrammarBuilder(new Choices(new List<string>() { "Play", "Start", "Begin" }.ToArray())) { Culture = System.Globalization.CultureInfo.CurrentCulture } ));
            Grammars[0].Name = "Play";

            Grammars.Add(new Grammar(new GrammarBuilder(new Choices(new List<string>() { "Stop", "Halt", "Pause" }.ToArray())) { Culture = System.Globalization.CultureInfo.CurrentCulture }));
            Grammars[1].Name = "Pause";

            Grammars.Add(new Grammar(new GrammarBuilder(new Choices(new List<string>() { "Next", "Skip", "Forward", "Skip Forward" }.ToArray())) { Culture = System.Globalization.CultureInfo.CurrentCulture }));
            Grammars[2].Name = "Next";

            Grammars.Add(new Grammar(new GrammarBuilder(new Choices(new List<string>() { "Previous", "Back", "Skip Back" }.ToArray())) { Culture = System.Globalization.CultureInfo.CurrentCulture }));
            Grammars[3].Name = "Previous";

            Grammars.Add(new Grammar(new GrammarBuilder(new Choices(new List<string>() { "Exit", "Quit", "Close" }.ToArray())) { Culture = System.Globalization.CultureInfo.CurrentCulture }));
            Grammars[4].Name = "Exit";

            Grammars.Add(new Grammar(new GrammarBuilder(new Choices(new List<string>() { "Start Listening" }.ToArray())) { Culture = System.Globalization.CultureInfo.CurrentCulture }));
            Grammars[5].Name = "StartListening";

            Grammars.Add(new Grammar(new GrammarBuilder(new Choices(new List<string>() { "Stop Listening" }.ToArray())) { Culture = System.Globalization.CultureInfo.CurrentCulture }));
            Grammars[6].Name = "StopListening";
            #endregion

            // Initialise the recognition engine
            Recog = new SpeechRecognitionEngine(System.Globalization.CultureInfo.CurrentCulture);
            Recog.SetInputToDefaultAudioDevice();

            // Load all grammars
            for (int x = 0; x < Grammars.Count; x++)
            {
                Recog.LoadGrammar(Grammars[x]);
                if (!Grammars[x].Loaded)
                {
                    return false;
                }
            }

            // Initialise the Listening thread
            ListeningThread = new Thread(new ThreadStart(Listen));

            return true;
        }

        public void Start()
        {
            if (ListeningThread.ThreadState != ThreadState.Running)
            {
                Running = true;
                ListeningThread.Start();
            }
        }
        public void Stop()
        {
            if (ListeningThread.ThreadState != ThreadState.Stopped)
            {
                Running = false;
            }
        }

        protected void Listen()
        {
            OnStartListening();
            while (Running)
            {
                // Recognise a string and get the grammar it was found from (effectively getting the command,
                // regardless of which variant of it was used)
                string GrammarName = string.Empty;
                try
                {
                    GrammarName = Recog.Recognize(TimeSpan.Parse("1")).Grammar.Name;
                }
                catch(NullReferenceException)
                {
                    // Timeout, no matter. Re-loop
                    continue;
                }

                // Match the grammar, fire the relevant event
                #region Grammar Checking

                // If we're meant to be receiving commands
                if (Listening)
                {
                    // Run checks on the commands
                    switch (GrammarName)
                    {
                        case "Play":        OnPlay();           break;
                        case "Pause":       OnPause();          break;
                        case "Next":        OnNext();           break;
                        case "Previous":    OnPrevious();       break;
                        case "Exit":        OnExit();           break;
                    }
                }

                // See if we need to change whether or not we are listening
                switch (GrammarName)
                {
                    case "StartListening":  OnStartListening(); break;
                    case "StopListening":   OnStopListening();  break;
                }
                
                #endregion
            }
        }
    }
}
