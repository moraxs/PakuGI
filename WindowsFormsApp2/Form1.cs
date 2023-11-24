using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public class MouseControl
    {
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        public const uint MOUSEEVENTF_MOVE = 0x0001;

        private bool isRunning;

        public void Start()
        {
            isRunning = true;

            Console.WriteLine("说明:按住“-”键以高速旋转，该程序使用“mouse_event”模拟鼠标移动，没有注入内存，理论不会封号，如果不放心，可以使用云原神.\nWritten by Morax");

            // 启动后台线程来处理键盘输入
            Thread inputThread = new Thread(HandleKeyboardInput);
            inputThread.Start();
        }

        public void Stop()
        {
            isRunning = false;
        }

        private void HandleKeyboardInput()
        {
            while (isRunning)
            {
                if ((GetKeyState(0xBD) & 0x8000) != 0) // VK_OEM_MINUS
                {
                    mouse_event(MOUSEEVENTF_MOVE, 800, 0, 0, 0);
                }

                Thread.Sleep(1);
            }
        }
    }

    public partial class Form1 : Form
    {
        private MouseControl mouseControl;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            KeyPreview = true; // 启用键盘事件预览
        }

        private void InitializeUI()
        {
            // 在这里可以初始化窗体上的控件和其他UI元素
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = true; // 确保窗体可见
            mouseControl = new MouseControl();
            mouseControl.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mouseControl.Stop();
        }
    }
}
