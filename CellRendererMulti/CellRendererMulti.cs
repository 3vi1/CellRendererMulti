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

		public bool Editable
		{
			get {
				return (Renderer as CellRendererText).Editable;
			}

			set {
				// Set all subrenderers to value
				textRenderer.Editable = value;
				comboRenderer.Editable = value;

				if (value == true)
				{
					Mode = CellRendererMode.Editable;
				}
				else
				{
					Mode = CellRendererMode.Activatable & CellRendererMode.Inert;
				}
			}
		}

		private event EditedHandler edited;
		public event EditedHandler Edited
		{ 
			add 	{ edited += value; }
			remove	{ edited -= value; } 
		}

		[GLib.Property ("model")]
		public TreeModel Model
		{
			get {
				return comboRenderer.Model;
			}

			set {
				comboRenderer.Model = value;
			}
		}

		[GLib.Property ("text-column")]
		public int TextColumn
		{
			get {
				return comboRenderer.TextColumn;
			}

			set {
				comboRenderer.TextColumn = value;
			}
		}


		[GLib.Property ("text")]
		public string Text 		
		{ 
			get {
				return (Renderer as CellRendererText).Text; 
			}
			set {
				textRenderer.Text = value;
				comboRenderer.Text = value;
			}
		}

		public override void GetSize (
			Widget widget,
			ref Rectangle cell_area,
			out int x_offset,
			out int y_offset,
			out int width,
			out int height)
		{
			base.GetSize (widget, ref cell_area, out x_offset, out y_offset, out width, out height);
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

		private void EditedCallback (object sender, EditedArgs args)
		{
			edited(sender, args);
		}
	}
}

