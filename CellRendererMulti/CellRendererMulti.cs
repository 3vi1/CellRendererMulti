using System;
using System.Diagnostics;

using Gdk;
using Gtk;

namespace EternalDusk
{
	/// <summary>
	/// These are the types of cells our renderer supports.
	/// </summary>
	public enum CellType
	{
		Combo,
		Text
	}


	public class CellRendererMulti : CellRenderer
	{
		public CellType CellType		{ get; set; }
		public bool Editable 			{ get; set; }

		private event EditedHandler edited;
		public event EditedHandler Edited
		{ 
			add 	{ edited += value; }
			remove	{ edited -= value; } 
		}

		public TreeModel Model 			{ get; set; }
		public int TextColumn			{ get; set; }

		[GLib.Property ("text")]
		public string Text 		{ get; set; }

		public override void GetSize (
			Widget widget,
			ref Rectangle cell_area,
			out int x_offset,
			out int y_offset,
			out int width,
			out int height)
		{
			base.GetSize (widget, ref cell_area, out x_offset, out y_offset, out width, out height);
			width = 150;
			// TODO:  Autogrow
		}

		public override CellEditable StartEditing(
			Event editEvent,
			Widget widget,
			string path,
			Rectangle background,
			Rectangle cellArea,
			CellRendererState flags
		) {
			return Renderer.StartEditing(
				editEvent,
				widget,
				path,
				background,
				cellArea,
				flags
			);
		}

		private CellRenderer Renderer
		{
			get 
			{
				switch(CellType)
				{
				case CellType.Combo:
					return comboRenderer;
				case CellType.Text:
					return textRenderer;
				default:
					return null;
				}
			}
		}

		protected override void Render(
			Gdk.Drawable window,		// Note the type! --3vi1
			Widget widget,
			Rectangle background_area,
			Rectangle cell_area,
			Rectangle expose_area,
			CellRendererState flags)
		{
			// Cast window to correct type, or you won't call correct renderer! --3vi1
			Renderer.Render(
				(Gdk.Window) window,
				widget,
				background_area,
				cell_area,
				expose_area,
				flags
			);
		}

		private CellRendererText textRenderer;
		private CellRendererCombo comboRenderer;

		public ListStore comboChoices;

		public CellRendererMulti() : base()
		{
			comboRenderer = new CellRendererCombo();
			comboRenderer.Edited += new EditedHandler(EditedCallback);

			textRenderer = new CellRendererText();
			textRenderer.Edited += new EditedHandler(EditedCallback);
		}

		protected void EditedCallback (object sender, EditedArgs args)
		{
			edited(sender, args);
		}

		public void Update()
		{
			switch (CellType)
			{
			case CellType.Combo:
				comboRenderer.Model = Model;
				comboRenderer.TextColumn = TextColumn;
				comboRenderer.Editable = Editable;
				comboRenderer.Text = Text;
				break;
			case CellType.Text:
				textRenderer.Editable = Editable;
				textRenderer.Text = Text;
				break;
			}
			if (Editable)
			{
				Mode = CellRendererMode.Editable;
			}
		}
	}
}

