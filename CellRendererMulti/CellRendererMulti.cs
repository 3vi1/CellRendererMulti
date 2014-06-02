using System;
using System.Diagnostics;

using Gdk;
using Gtk;

namespace EternalDusk
{
	public class CellRendererMulti : CellRenderer
	{
		public CellType CellType	{ get; set; }
		public bool Editable 		{ get; set; }
		public EditedHandler Edited	{ get; set; }
		public TreeModel Model 		{ get; set; }
		public int TextColumn		{ get; set; }

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
		}

		public override CellEditable StartEditing(
			Event editEvent,
			Widget widget,
			string path,
			Rectangle background,
			Rectangle cellArea,
			CellRendererState flags
		) {
			return Renderer.StartEditing(editEvent, widget, path, background, cellArea, flags);
		}

		private CellRenderer Renderer
		{
			get 
			{
				if (CellType == CellType.Combo)
				{
					return comboRenderer;
				}

				return textRenderer; 
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
			// Build Combo Options
			comboChoices = new ListStore (typeof(string));
			comboChoices.AppendValues("Choice A");
			comboChoices.AppendValues("Choice B");
			comboChoices.AppendValues("Choice C");

			Model = comboChoices;
			TextColumn = 0;
			Mode = CellRendererMode.Editable;
		}

		public void Update()
		{
			switch (CellType)
			{
			case CellType.Combo:
				comboRenderer = new CellRendererCombo();
				comboRenderer.Model = Model;
				comboRenderer.TextColumn = TextColumn;
				comboRenderer.Editable = Editable;
				foreach (EditedHandler handler in Edited.GetInvocationList())
				{
					comboRenderer.Edited += handler;
				}
				comboRenderer.Text = Text;
				break;
			case CellType.Text:
				textRenderer = new CellRendererText();
				textRenderer.Editable = Editable;
				foreach (EditedHandler handler in Edited.GetInvocationList())
				{
					textRenderer.Edited += handler;
				}
				textRenderer.Text = Text;
				break;
			}
			//Renderer.Mode = CellRendererMode.Activatable;
		}
	}
}

