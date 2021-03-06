using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace PetriNetSimulator2
{
	/// <summary>
	/// Summary description for PlaceOutput.
	/// </summary>
	[Serializable]
	public class PlaceOutput : Place, ICloneable, ISerializable, IMetafileModel
	{
		public PlaceOutput() : base()
		{
			InitializeComponent();
		}

		#region public PlaceOutput(SerializationInfo info, StreamingContext context) : base(info, context)
		public PlaceOutput(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			//
			// PlaceOutput
			//
			this.Name = "PlaceOutput";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PlaceOutput_Paint);
		}
		#endregion


		#region public new void GetObjectData(SerializationInfo info, StreamingContext context)
		public new void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
		#endregion


		#region private void PlaceOutput_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		private void PlaceOutput_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// Draws Place object
			Graphics g = e.Graphics;

			if (PetriNetDocument.AntiAlias == true)
				g.SmoothingMode = SmoothingMode.AntiAlias;

			PetriNetEditor pne = (PetriNetEditor)this.Parent;

			Rectangle r = this.ClientRectangle;
			LinearGradientBrush lgb = new LinearGradientBrush(r, Color.White, this.cBackgroundColor, LinearGradientMode.ForwardDiagonal);

			// Draw on background a letter which describes PlaceType
			StringFormat sfo = new StringFormat();
			sfo.LineAlignment = StringAlignment.Center;
			sfo.Alignment = StringAlignment.Center;

			Font fo = new Font(this.Parent.Font.FontFamily, pne.Zoom * 10f,  FontStyle.Bold);

			Pen pSign = new Pen(Color.Black, 3 * pne.Zoom);
			Point[] pa = new Point[4]
			{
				new Point((int)(64 * pne.Zoom), (int)(30 * pne.Zoom)),
				new Point((int)(54 * pne.Zoom), (int)(30 * pne.Zoom)),
				new Point((int)(54 * pne.Zoom), (int)(44 * pne.Zoom)),
				new Point((int)(64 * pne.Zoom), (int)(44 * pne.Zoom)),
			};
			g.DrawLines(pSign, pa);

			pSign.CustomEndCap = new AdjustableArrowCap(4 * pne.Zoom, 3 * pne.Zoom, true);
			pSign.Width = (int)(2 * pne.Zoom);
			g.DrawLine(pSign, new Point((int)(57 * pne.Zoom), (int)(37 * pne.Zoom)), new Point((int)(67 * pne.Zoom), (int)(37 * pne.Zoom)));

			g.FillRectangle(lgb, r);

			Pen pBlack = new Pen(Color.Black, pne.Zoom * 7);
			g.DrawEllipse(pBlack, r);

			Brush bBlack = new SolidBrush(Color.Black);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			Font f = new Font(this.Parent.Font.FontFamily, pne.Zoom * 7f,  FontStyle.Bold);

			g.DrawString("P" + sIndex, f, bBlack, new RectangleF(new PointF(0f, this.Height - pne.Zoom * 23f), new SizeF(this.Width, pne.Zoom * 20f)), sf);

			sf.LineAlignment = StringAlignment.Center;
			g.DrawString(this.sName, f, bBlack, new RectangleF(new PointF(0f, pne.Zoom * 6f), new SizeF(this.Width, pne.Zoom * 20f)), sf);

			sf.LineAlignment = StringAlignment.Center;
			RectangleF rTokens = new RectangleF(new PointF(0f, 0f), new SizeF(this.Width, this.Height));
			this.DrawTokens(g, bBlack, rTokens, sf);
		}
		#endregion

		#region public override object Clone()
		public override object Clone()
		{
			PlaceOutput po = new PlaceOutput();
			po.Location = this.Location;
			po.NameID = this.NameID;
			po.Tokens = this.Tokens;
			return po;
		}
		#endregion

		#region public override string ToString()
		public override string ToString()
		{
			if (this.NameID != null && this.NameID != "")
				return "P" + this.sIndex + " - " + this.NameID + " (Output)";
			else
				return "P" + this.sIndex + " (Output)";
		}
		#endregion


        public override string GetShortString()
        {
                if (this.NameID != null && this.NameID != "")
                        return this.NameID;
                else
                        return "P" + this.sIndex;
        }

        #region public string GetXMLString()
        public override string GetXMLString()
        {
            Point pt = this.Location;
            string s = "\t<place id=\"" + this.GetShortString() + "\">\n";

            s += "\t\t<name>\n";
            s += "\t\t\t<graphics><position x=\""+pt.X+"\" y=\""+pt.Y+"\" /></graphics>\n";
            s += "\t\t\t<text>" + this.GetShortString () + "</text>\n";
            s += "\t\t</name>\n";

            if(this.Tokens != 0)
                s += "\t\t<initialMarking><text>"+ this.Tokens  +"</text></initialMarking>\n";

            s += "<toolspecific tool=\"PNE\"><type><text>O</text></type></toolspecific>\n";

            s += "\t</place>\n";
            return s;
        }
        #endregion


		#region public override bool CanConnectTo(object o)
		public override bool CanConnectTo(object o)
		{
			return false;
		}
		#endregion

		#region IMetafileModel Members

		public void DrawModel(Graphics g)
		{
			Point pt = this.Location;

			PetriNetEditor pne = (PetriNetEditor)this.Parent;
			Rectangle r = new Rectangle(pt, this.Size);
			Brush bFill = new LinearGradientBrush(r, Color.White, Color.Red, LinearGradientMode.ForwardDiagonal);

			g.FillEllipse(Brushes.White, r);

			// Draw on background a letter which describes PlaceType
			StringFormat sfo = new StringFormat();
			sfo.LineAlignment = StringAlignment.Center;
			sfo.Alignment = StringAlignment.Center;

			Font fo = new Font(this.Parent.Font.FontFamily, pne.Zoom * 10f,  FontStyle.Bold);

			Pen pSign = new Pen(Color.LightGray, 2 * pne.Zoom);
			Point[] pa = new Point[4]
			{
				new Point(pt.X + (int)(64 * pne.Zoom), pt.Y + (int)(30 * pne.Zoom)),
				new Point(pt.X + (int)(54 * pne.Zoom), pt.Y + (int)(30 * pne.Zoom)),
				new Point(pt.X + (int)(54 * pne.Zoom), pt.Y + (int)(44 * pne.Zoom)),
				new Point(pt.X + (int)(64 * pne.Zoom), pt.Y + (int)(44 * pne.Zoom)),
			};
			g.DrawLines(pSign, pa);

			pSign.CustomEndCap = new AdjustableArrowCap(4 * pne.Zoom, 3 * pne.Zoom, true);
			pSign.Width = (int)(2 * pne.Zoom);
			g.DrawLine(pSign, new Point(pt.X + (int)(57 * pne.Zoom), pt.Y + (int)(37 * pne.Zoom)), new Point(pt.X + (int)(67 * pne.Zoom), pt.Y + (int)(37 * pne.Zoom)));

			Pen pBlack = new Pen(Color.Black, pne.Zoom * 4);
			g.DrawEllipse(pBlack, r);

			Brush bBlack = new SolidBrush(Color.Black);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			Font f = new Font(this.Parent.Font.FontFamily, pne.Zoom * 7f,  FontStyle.Bold);

			g.DrawString("P" + iIndex.ToString(), f, bBlack, new RectangleF(new PointF(pt.X, pt.Y + this.Height - pne.Zoom * 23f), new SizeF(this.Width, pne.Zoom * 20f)), sf);

			sf.LineAlignment = StringAlignment.Center;
			g.DrawString(this.sName, f, bBlack, new RectangleF(new PointF(pt.X, pt.Y + pne.Zoom * 6f), new SizeF(this.Width, pne.Zoom * 20f)), sf);

			sf.LineAlignment = StringAlignment.Center;
			RectangleF rTokens = new RectangleF(new PointF(pt.X, pt.Y), new SizeF(this.Width, this.Height));
			this.DrawTokens(g, bBlack, rTokens, sf);
		}

		#endregion
	}
}
