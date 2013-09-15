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

        public bool Listening;
        public bool Running;

        protected Thread ListeningThread;

        public List<Command> Commands;

        #endregion

        public Listener(bool UseBabbleGuard = true)
        {
            // Give base values to variables
            Recog = new SpeechRecognitionEngine(System.Globalization.CultureInfo.CurrentCulture);
            Commands = new List<Command>();

            // Initialise the Listening thread
            ListeningThread = new Thread(new ThreadStart(Listen));

            Listening = false;
            Running = false;

            if (UseBabbleGuard)
            {
                // Implement a babble guard - protection from background noise
                AddCommand(new Command("Babble", () => { }, new List<string>() { "..." }));
            }
        }

        public void AddCommand(Command NewCommand)
        {
            Commands.Add(NewCommand);
            Recog.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(NewCommand.Commands.ToArray())) { Culture = System.Globalization.CultureInfo.CurrentCulture }) { Name = NewCommand.Name });
        }
        public void AddCommand(List<Command> Commands)
        {
            foreach (Command c in Commands)
            {
                AddCommand(c);
            }
        }

        public void UnloadCommands()
        {
            Recog.UnloadAllGrammars();
            Commands.Clear();
        }

        public void Start()
        {
            if (ListeningThread.ThreadState != ThreadState.Running)
            {
                Running = true;
                ListeningThread.Start();
                Enable();
            }
        }
        public void Stop()
        {
            if (ListeningThread.ThreadState != ThreadState.Stopped)
            {
                Listening = false;
                Running = false;
                while (ListeningThread.ThreadState != ThreadState.Stopped)
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void Enable()
        {
            Recog.SetInputToDefaultAudioDevice();
        }
        public void Disable()
        {
            Recog.SetInputToNull();
        }

        protected void Listen()
        {
            Listening = true;
            while (Running)
            {
                // Recognise a string and get the grammar it was found from (effectively getting the command,
                // regardless of which variant of it was used)
                string GrammarName = string.Empty;
                try
                {
                    // Recognise a command, store the name of the command used
                    GrammarName = Recog.Recognize().Grammar.Name;

                    // Locate the specific command
                    for (int x = 0; x < Commands.Count; x++)
                    {
                        if (Commands[x].Name == GrammarName)
                        {
                            // Fire the command event
                            Commands[x].OnCommanded();
                            break;
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    // Timeout, no matter. Re-loop
                    continue;
                }
            }
        }
    }
}
