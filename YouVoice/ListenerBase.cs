using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using System.Speech.Recognition;
using SpeechLib;

namespace YouVoice
{
    public delegate void ListenerEvent();

    public abstract class ListenerBase
    {
        #region Members

        protected SpeechRecognitionEngine Recog { get; set; }

        public bool Running { get; protected set; }

        protected Thread ListeningThread { get; set; }

        public List<Command> Commands { get; protected set; }

        #endregion

        public ListenerBase()
        {
            // Give base values to variables
            Recog = new SpeechRecognitionEngine(System.Globalization.CultureInfo.CurrentCulture);
            Commands = new List<Command>();

            // Initialise the Listening thread
            ListeningThread = new Thread(new ThreadStart(Listen));

            Running = false;
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
                Running = false;
                while (ListeningThread.ThreadState != ThreadState.Stopped)
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void Enable()
        {
            lock (Recog)
            {
                Recog.SetInputToDefaultAudioDevice();
            }
        }
        public void Disable()
        {
            lock (Recog)
            {
                Recog.SetInputToNull();
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
        public void UnloadCommand(int Index)
        {
            Commands.RemoveAt(Index);
        }
        public void UnloadCommand(string Command)
        {
            for (int x = 0; x < Commands.Count; x++)
            {
                if (Commands[x].Name == Command)
                {
                    UnloadCommand(x);
                    return;
                }
            }
        }
        public void UnloadCommand(Command CommandToUnload)
        {
            Commands.Remove(CommandToUnload);
        }

        protected abstract void Listen();
    }
}
