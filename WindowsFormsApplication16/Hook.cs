using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using System.Reflection;
namespace Hooks
{
    public delegate void GetKeysCodeEventHandler(int keyCode);
    public class Hook
    {
        //键盘Hook结构函数 
        [StructLayout(LayoutKind.Sequential)]
       public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        //委托 
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        static int hHook = 0;
        public const int WH_KEYBOARD_LL = 13;
        //LowLevel键盘截获，如果是WH_KEYBOARD＝2，并不能对系统键盘截取，Acrobat Reader会在你截取之前获得键盘。 
        public  static HookProc KeyBoardHookProcedure;
        public GetKeysCodeEventHandler GetKeysCode;

        #region [DllImport("user32.dll")]
        //设置钩子 
        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
       
        //抽掉钩子 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //调用下一个钩子 
        [DllImport("user32.dll")]
         public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);
    
        //IsWindowVisible
        #endregion

        #region 安装键盘钩子
        /// <summary>
        /// 安装键盘钩子
        /// </summary>
        public  void Hook_Start()
        {
            if (hHook == 0)
            {
                KeyBoardHookProcedure = new HookProc(KeyBoardHookProc);
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyBoardHookProcedure, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //如果设置钩子失败. 
                if (hHook == 0)
                {
                    Hook_Clear();
                }
            }
        }
        #endregion

        #region 取消钩子事件
        /// <summary>
        /// 取消钩子事件
        /// </summary>
        public  void Hook_Clear()
        {
            bool retKeyboard = true;
            if (hHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hHook);
                hHook = 0;
            }
        }
        #endregion

   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
    
            if (nCode >= 0)
            {
                KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                if (GetKeysCode != null)
                {
                    GetKeysCode(kbh.vkCode);
                }
                return 1;
            }
           return CallNextHookEx(hHook, nCode, wParam, lParam);
        }
  

    }
}
