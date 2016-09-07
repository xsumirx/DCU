using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
/*
In order to use this class in your program, just declare the varialble and hook up into HookEvent:
HookKeys hook = new HookKeys();
hook.HookEvent += new HookKeys.HookEventHandler(HookEvent);
hook.Start();
*/
public static class HookKeys
{
    private const int WM_KEYDOWN = 0x0100;
#region delegates
    public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
    public delegate void HookEventHandler(HookEventArgs e, KeyBoardInfo keyBoardInfo);
    public static event HookEventHandler HookEvent;
#endregion
#region fields
    private static HookProc hookDeleg;
    private static int hHook = 0;
#endregion


    #region public methods
    ///
    /// Starts the hook
    ///
    public static void Start()
    {
        if (hHook != 0)
        {
            //Unhook the previouse one
            Stop();
        }
        hookDeleg = new HookProc(HookProcedure);
        hHook = SetWindowsHookEx(WH_KEYBOARD_LL, hookDeleg, GetModuleHandle(null), 0);
        if (hHook == 0)
        {
            throw new SystemException("Failed acquiring of the hook.");
        }
        AllKeys(true);
    }
    ///
    /// Stops the hook
    ///
    public static void Stop()
    {
        UnhookWindowsHookEx(hHook);
        AllKeys(false);
    }
    #endregion
    #region protected and private methods
    public static void OnHookEvent(HookEventArgs hookArgs, KeyBoardInfo keyBoardInfo)
    {
        if (HookEvent != null)
        {
            
            HookEvent(hookArgs, keyBoardInfo);
        }
    }
 
    private static int HookProcedure(int code, IntPtr wParam, IntPtr lParam)
    {
       KBDLLHOOKSTRUCT hookStruct =  (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
       if (code < 0)
           return 1; //CallNextHookEx(hookDeleg, code, wParam, lParam);
       if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN)
       {
           // Let clients determine what to do
           HookEventArgs e = new HookEventArgs();
           e.Code = code;
           e.wParam = wParam;
           e.lParam = lParam;
           KeyBoardInfo keyInfo = new KeyBoardInfo(0);
           keyInfo.vkCode = hookStruct.vkCode;
           keyInfo.scanCode = hookStruct.scanCode;
           //MessageBox.Show("");
           OnHookEvent(e, keyInfo);
       }
       // Yield to the next hook in the chain
       return 1;//CallNextHookEx(hookDeleg, code, wParam, lParam);
   }
   #endregion
   #region P/Invoke declarations
   
   [DllImport("coredll.dll")]
   private static extern int AllKeys(bool bEnable);

   [DllImport("coredll.dll")]
   private static extern int SetWindowsHookEx(int type, HookProc hookProc, IntPtr hInstance, int m);
   [DllImport("coredll.dll")]
   private static extern IntPtr GetModuleHandle(string mod);
   [DllImport("coredll.dll")]
   private static extern int CallNextHookEx(
           HookProc hhk,
           int nCode,
           IntPtr wParam,
           IntPtr lParam
           );
   [DllImport("coredll.dll")]
   private static extern int GetCurrentThreadId();
   [DllImport("coredll.dll", SetLastError = true)]
   private static extern int UnhookWindowsHookEx(int idHook);
   private struct KBDLLHOOKSTRUCT
   {
       public int vkCode;
       public int scanCode;
       public int flags;
       public int time;
       public IntPtr dwExtraInfo;
   }
   const int WH_KEYBOARD_LL = 20;
   #endregion
}
#region event arguments
  
    public class HookEventArgs : EventArgs
    {
        public int Code;    // Hook code
        public IntPtr wParam;   // WPARAM argument
        public IntPtr lParam;   // LPARAM argument
    }
    public class KeyBoardInfo
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;

        public KeyBoardInfo(int vk) {
            vkCode = vk;
        }
    }
#endregion
