// by Stephen Toub
// from https://msdn.microsoft.com/en-us/magazine/cc163450.aspx
// under Microsoft Limited Public License (https://msdn.microsoft.com/en-us/cc300389.aspx)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Pop.Cs
{
    public static class InUseDetection
    {
        public static IList<Process> GetProcessesUsingFiles(IEnumerable<string> filePaths)
        {
            var filePathsList = filePaths.ToList();

            uint sessionHandle;
            List<Process> processes = new List<Process>();

            // Create a restart manager session
            int rv = RmStartSession(out sessionHandle, 0, Guid.NewGuid().ToString("N"));
            if (rv != 0) throw new Win32Exception();
            try
            {
                // Let the restart manager know what files we're interested in
                string[] pathStrings = new string[filePathsList.Count];
                filePathsList.CopyTo(pathStrings, 0);
                rv = RmRegisterResources(sessionHandle,
                    (uint)pathStrings.Length, pathStrings,
                    0, null, 0, null);
                if (rv != 0) throw new Win32Exception();

                // Ask the restart manager what other applications are using those files
                const int ERROR_MORE_DATA = 234;
                uint pnProcInfoNeeded = 0, pnProcInfo = 0, lpdwRebootReasons = RmRebootReasonNone;
                rv = RmGetList(sessionHandle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);
                if (rv == ERROR_MORE_DATA)
                {
                    // Create an array to store the process results
                    RM_PROCESS_INFO[] processInfo = new RM_PROCESS_INFO[pnProcInfoNeeded];
                    pnProcInfo = (uint)processInfo.Length;

                    // Get the list.  
                    rv = RmGetList(sessionHandle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
                    if (rv == 0)
                    {
                        // Enumerate all of the results and add them to the list to be returned
                        for(int i=0; i<pnProcInfo; i++)
                        {
                            try
                            {
                                processes.Add(Process.GetProcessById(processInfo[i].Process.dwProcessId));
                            }
                            catch (ArgumentException) { } // in case process is no longer running
                        }
                    }
                        // It's possible that the number of processes could have changed between previous call to RmGetList
                        // and this one, in which case our array might not be big enough and rv could be
                        // ERROR_MORE_DATA.  In that case, we could retry, but for now we'll just throw an exception.
                    else throw new Win32Exception();
                }
                else if (rv != 0) throw new Win32Exception();
            }
            finally
            {
                // Close the resource manager
                RmEndSession(sessionHandle);
            }
            return processes;
        }

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        static extern int RmEndSession(uint pSessionHandle);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        static extern int RmRegisterResources(uint pSessionHandle, UInt32 nFiles, string[] rgsFilenames,
            UInt32 nApplications, RM_UNIQUE_PROCESS[] rgApplications, UInt32 nServices, string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll")]
        static extern int RmGetList(uint dwSessionHandle,
            out uint pnProcInfoNeeded, ref uint pnProcInfo,
            [In, Out] RM_PROCESS_INFO[] rgAffectedApps,
            ref uint lpdwRebootReasons);

        private const int RmRebootReasonNone = 0;
        private const int CCH_RM_MAX_APP_NAME = 255;
        private const int CCH_RM_MAX_SVC_NAME = 63;

        [StructLayout(LayoutKind.Sequential)]
        private struct RM_UNIQUE_PROCESS
        {
            public int dwProcessId;
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct RM_PROCESS_INFO
        {
            public RM_UNIQUE_PROCESS Process;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            public string strAppName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            public string strServiceShortName;
            public RM_APP_TYPE ApplicationType;
            public uint AppStatus;
            public uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }

        private enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }
    }
}