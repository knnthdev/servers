using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veecar
{
    public class Pragma
    {
        public class Instant
        {
            public Instant(int countlines)
            {
                _lines = new string[countlines];
            }
            public Instant(int countlines, string nameinstant) : this(countlines) { _name = nameinstant; }
            public Instant(string nameinstant) : this(1) { _name = nameinstant; }
            public Instant() : this(1, "Instant") { }

            private string _name;
            private string[] _lines;

            public void log(object msg)
            {
                if (Program.islib)
                    _lines.Push($"{_name}->{msg}\n");
                else
                    Program.log($"{_name}->{msg}");
                OnLogger(Channel);
            }

            private void OnLogger(string msg)
            {
                OnLog?.Invoke(msg);
            }

            public string Channel { get => _lines.Concat().Append("\n"); }
            public string[] Lines { get => _lines; }

        }
        private static string[] _lines = new string[50];

        public static void log(object msg)
        {

            _lines.Push($"{msg}\n");
            OnLogger(Channel);
        }

        private static void OnLogger(string msg)
        {
            OnLog?.Invoke(msg);
        }

        public static event Action<string> OnLog;

        public static string Channel { get => _lines.Concat(); }
        public static string[] Lines { get => _lines; }

        public new string ToString()
        {
            return _lines.Concat();
        }
    }
}
