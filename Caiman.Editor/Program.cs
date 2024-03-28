using System.Diagnostics;
using Caiman.Editor;
using ConsoleTraceListener = Caiman.Editor.ConsoleTraceListener;


Trace.Listeners.Add(new ConsoleTraceListener());
var editor = new Editor();
editor.Run();
