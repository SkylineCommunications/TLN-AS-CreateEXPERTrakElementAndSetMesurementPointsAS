namespace TLN_AS_CreateEXPERTrakElementAndSetMesurementPointsAS_1.ElementCreationPackage
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

	public class ElementCreationView : Dialog
	{
		public ElementCreationView(IEngine engine) : base(engine)
		{
			Label elementNameLabel = new Label("Element Name");
			Label elementIpAddress = new Label("IP Address");
			Label elementScript = new Label("Script to execute");
			Create = new Button("Create");
			ElementName = new TextBox
			{
				PlaceHolder = "Element Name",
			};
			IpAddress = new TextBox { PlaceHolder = "IP Address"};
			ScriptName = new TextBox("Default Script");

			AddWidget(elementNameLabel, 0, 0);
			AddWidget(ElementName, 0, 1);
			AddWidget(elementIpAddress, 1, 0);
			AddWidget(IpAddress, 1, 1);
			AddWidget(elementScript, 2, 0);
			AddWidget(ScriptName, 2, 1);
			AddWidget(Create, 3, 0);
		}

		public TextBox ElementName { get; private set; }

		public TextBox IpAddress { get; private set; }

		public TextBox ScriptName { get; private set; }

		public Button Create { get; private set; }
	}
}
