using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using PopOpen.Contracts;

namespace PopOpen
{
    public class PopOpener
    {
        private readonly IStartProcess _processStarter;
        private readonly IFindProcess _processFinder;
        private readonly IPeekaboo _peekaboo;

        public PopOpener(IStartProcess processStarter, IFindProcess processFinder, IPeekaboo peekaboo)
        {
            _processStarter = processStarter;
            _processFinder = processFinder;
            _peekaboo = peekaboo;
        }

        public PopProcess Open(string filePath)
        {
            _processStarter.Start(filePath);
            
            Thread.Sleep(7000);
            
            var process = _processFinder.Find(filePath);
            var foregroundWindowHandle = _processFinder.FindForegroundWindow();

            if (process.MainWindowHandle != foregroundWindowHandle)
            {
                _peekaboo.Minimise(process.MainWindowHandle);    
            }
            
            _peekaboo.Restore(process.MainWindowHandle);

            return process;
        }
    }
}
