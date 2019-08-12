using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
	public class BufferedTreeView : TreeView
	{
		private const int TVM_SETEXTENDEDSTYLE = 4396;

		private const int TVM_GETEXTENDEDSTYLE = 4397;

		private const int TVS_EX_DOUBLEBUFFER = 4;

		private IContainer components;

		public BufferedTreeView()
		{
			this.InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			BufferedTreeView.SendMessage(base.Handle, 4396, (IntPtr)4, (IntPtr)4);
			base.OnHandleCreated(e);
		}

		[DllImport("user32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Auto, ExactSpelling=false)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
	}
}