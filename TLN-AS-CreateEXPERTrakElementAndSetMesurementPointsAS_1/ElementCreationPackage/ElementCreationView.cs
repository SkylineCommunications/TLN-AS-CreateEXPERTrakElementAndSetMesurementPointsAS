namespace TLN_AS_CreateEXPERTrakElementAndSetMesurementPointsAS_1.ElementCreationPackage
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

	public class ElementCreationView : Dialog
	{
		public ElementCreationView(IEngine engine) : base(engine)
		{
			Label = new Label("Create new Element");
			Create = new Button("Create");
			ElementName = new TextBox("Element name");
			IpAddress = new TextBox("IP Address");
			ScriptName = new TextBox("Script to be executed");
			Agents = new DropDown();

			AddWidget(Label, 0, 0);
			AddWidget(ElementName, 0, 1);
			AddWidget(IpAddress, 0, 2);
			AddWidget(ScriptName, 0, 3);
			AddWidget(Agents, 1, 0);
			AddWidget(Create, 2, 0);
		}

		public TextBox ElementName { get; private set; }

		public TextBox IpAddress { get; private set; }

		public TextBox ScriptName { get; private set; }

		public Button Create { get; private set; }

		public DropDown Agents { get; private set; }

		public Label Label { get; private set; }
	}
}
