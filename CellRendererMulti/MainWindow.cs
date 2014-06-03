using System;
using Gtk;
using EternalDusk;

public enum Columns
{
	Attribute = 0,
	Value = 1,
	CellType = 2,
	ComboStore = 3
}

[TreeNode (ListOnly=true)]
public class MyTreeNode : TreeNode {

	[TreeNodeValue (Column=(int)Columns.Attribute)]
	public string MyAttribute;

	[TreeNodeValue (Column=(int)Columns.Value)]
	public string MyValue;

	// Note: The following are hidden columns (not added to view).  --3vi1
	[TreeNodeValue (Column=(int)Columns.CellType)]
	public CellType MyValueCellType;

	[TreeNodeValue (Column=(int)Columns.ComboStore)]
	public ListStore comboStore;

	public MyTreeNode (
		string myAttribute,
		string myValue,
		CellType myValueCellType,
		ListStore comboStore
	) {
		MyAttribute = myAttribute;
		MyValue = myValue;
		MyValueCellType = myValueCellType;
		this.comboStore = comboStore;
	}
}

public partial class MainWindow: Gtk.Window
{
	ListStore makeChoices, modelChoices;

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

		// Define some test types that will be in the dropdown combos.
		makeChoices = new ListStore (typeof(string));
		makeChoices.AppendValues("Chevy");
		makeChoices.AppendValues("Ford");
		makeChoices.AppendValues("Plymouth");

		modelChoices = new ListStore (typeof(string));
		modelChoices.AppendValues("Camaro");
		modelChoices.AppendValues("Fury");
		modelChoices.AppendValues("Mustang");

		// Add our Value Column
		CellRendererMulti myRenderer = new CellRendererMulti();
		myRenderer.TextColumn = 0;
		myRenderer.Editable = true;
		myRenderer.Edited += new EditedHandler(TypeEdited);
		TreeViewColumn column = nodeView.AppendColumn (
			"Value",
			myRenderer,
			"text",
			(int)Columns.Value
		);
		column.SetCellDataFunc(myRenderer, MultiCellDataFunc);

		// Note:  Do not add the hidden columns. --3vi1

		// Attach the view to the data model/store.
		nodeView.NodeStore = Store;
	}

	// Our data store.
	NodeStore store;
	NodeStore Store {
		get {
			if (store == null) {
				store = new NodeStore (typeof (MyTreeNode));
				store.AddNode (
					new MyTreeNode ("Car", "Christine", CellType.Text, null)
				);
				store.AddNode (
					new MyTreeNode ("Make", "Plymouth", CellType.Combo, makeChoices)
				);
				store.AddNode (
					new MyTreeNode ("Model", "Fury", CellType.Combo, modelChoices)
				);
			}
			return store;
		}
	}

	private void MultiCellDataFunc(
		TreeViewColumn column,
		CellRenderer cell,
		TreeModel model,
		TreeIter iter
	) {
		string cellValue = (string) model.GetValue (iter, (int)Columns.Value);
		CellType cellType = (CellType) model.GetValue (iter, (int)Columns.CellType);
		(cell as CellRendererMulti).CellType = cellType;
		if(cellType == CellType.Combo)
		{
			ListStore comboStore = (ListStore) model.GetValue(iter, (int) Columns.ComboStore);
			(cell as CellRendererMulti).Model = comboStore;
		}
		(cell as CellRendererMulti).Text = cellValue;
	}

	protected void TypeEdited (object sender, EditedArgs args)
	{
		MyTreeNode node = Store.GetNode(new Gtk.TreePath(args.Path)) as MyTreeNode;
		node.MyValue = args.NewText;
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}
}
