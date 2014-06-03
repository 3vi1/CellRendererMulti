using System;
using Gtk;
using EternalDusk;

public enum Columns
{
	Attribute = 0,
	Value = 1,
	CellType = 2
}

[TreeNode (ListOnly=true)]
public class MyTreeNode : TreeNode {

	[TreeNodeValue (Column=(int)Columns.Attribute)]
	public string MyAttribute;

	[TreeNodeValue (Column=(int)Columns.Value)]
	public string MyValue;

	// Note: This is a hidden column (not added to view).  --3vi1
	[TreeNodeValue (Column=(int)Columns.CellType)]
	public CellType MyValueCellType;

	public MyTreeNode (string myAttribute, string myValue, CellType myValueCellType)
	{
		MyAttribute = myAttribute;
		MyValue = myValue;
		MyValueCellType = myValueCellType;
	}
}

public partial class MainWindow: Gtk.Window
{
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();

		// Add our Attribute column.
		nodeView.AppendColumn(
			"Attribute",
			new CellRendererText(),
			"text",
			(int) Columns.Attribute
		);

		// Define some test types that will be in the dropdown combo.
		ListStore storeTypes = new ListStore (typeof(string));
		storeTypes.AppendValues("test1");
		storeTypes.AppendValues("test2");
		storeTypes.AppendValues("test3");

		// Add our Value Column
		//CellRendererCombo myRenderer = new CellRendererCombo();
		CellRendererMulti myRenderer = new CellRendererMulti();
		myRenderer.Model = storeTypes;
		myRenderer.TextColumn = 0;
		myRenderer.Editable = true;
		myRenderer.Edited += new EditedHandler(TypeEdited);
		TreeViewColumn column = nodeView.AppendColumn (
			"Value",
			myRenderer,
			"text",
			(int)Columns.Value
		);
		column.SetCellDataFunc(myRenderer, MyCellDataFunc);

		// Note:  Do not add the hidden column that determines cell type. --3vi1

		// Attach the view to the data model/store.
		nodeView.NodeStore = Store;
	}

	// Our data store.
	NodeStore store;
	NodeStore Store {
		get {
			if (store == null) {
				store = new NodeStore (typeof (MyTreeNode));
				store.AddNode (new MyTreeNode ("Text 1", "Combo 1", CellType.Combo));
				store.AddNode (new MyTreeNode ("Text 2", "Combo 2", CellType.Combo));
				store.AddNode (new MyTreeNode ("Text 3", "Text 4", CellType.Text));
			}
			return store;
		}
	}

	private void MyCellDataFunc(
		TreeViewColumn column,
		CellRenderer cell,
		TreeModel model,
		TreeIter iter
	) {
		string cellValue = (string) model.GetValue (iter, (int)Columns.Value);
		CellType cellType = (CellType) model.GetValue (iter, (int)Columns.CellType);
		(cell as CellRendererMulti).CellType = cellType;
		(cell as CellRendererMulti).Text = cellValue;
		(cell as CellRendererMulti).Update();
		//column.SetAttributes(cell, "text", (int)Columns.Value);
		//column.PackStart(cell, true);
	}

	protected void TypeEdited (object sender, EditedArgs args)
	{
		TreeIter iter;
		/*if (store.GetIterFromString (out iter, args.Path)) {
			foreach (string name in Enum.GetNames (typeof (TriggerType))) {
				if (args.NewText == name) {
					store.SetValue (iter, colTypeIndex, args.NewText);
					EmitContentChanged ();
					return;
				}
			}
			string oldText = store.GetValue (iter, colTypeIndex) as string;
			(sender as CellRendererText).Text = oldText;
		}*/
	}
	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}
}
