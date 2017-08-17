using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {        
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        // Read Memory
        const int PROCESS_VM_READ = 0x0010;        

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        // Write Memory
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;
        

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        public Int32 _address;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            _address = Convert.ToInt32(txtAddr.Text, 16);

            Process process = Process.GetProcessesByName("WindowsFormsApplication2")[0];
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

            int bytesRead = 0;
            byte[] buffer = new byte[22]; //'Hello World' takes 10*2 bytes because of Unicode            

            // 0x0005513E is the address where I found the string from OllyDbg
            ReadProcessMemory((int)processHandle, _address, buffer, buffer.Length, ref bytesRead);

            textBox1.Text = Encoding.Unicode.GetString(buffer) + " (" + bytesRead.ToString() + " bytes)";    
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            _address = Convert.ToInt32(txtAddr.Text, 16);

            Process process = Process.GetProcessesByName("WindowsFormsApplication2")[0];
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

            int bytesWritten = 0;
            byte[] buffer = Encoding.Unicode.GetBytes(textBox2.Text + "\0");// '\0' marks the end of string

            WriteProcessMemory((int)processHandle, _address, buffer, buffer.Length, ref bytesWritten);

        }
        
    }
}
