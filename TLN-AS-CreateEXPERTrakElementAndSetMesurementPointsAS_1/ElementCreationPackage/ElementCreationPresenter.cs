namespace TLN_AS_CreateEXPERTrakElementAndSetMesurementPointsAS_1.ElementCreationPackage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.DataMiner.Library.Common;
	using Skyline.DataMiner.Net.APIDeployment.Url.Blocks;

	public class ElementCreationPresenter
	{
		private readonly ElementCreationView view;
		private readonly ElementCreationModel model;

		public ElementCreationPresenter(ElementCreationView elementSelectionView, ElementCreationModel elementModel)
		{
			view = elementSelectionView;
			model = elementModel;
			var protocols = elementModel.LoadFromModel();
			view.ProtocolVersions.SetOptions(protocols.Select(protocol => protocol.Version));

			view.Create.Pressed += OnNextButtonPressed;
		}

		public event EventHandler<EventArgs> Create;

		public void OnNextButtonPressed(object sender, EventArgs e)
		{
			Create?.Invoke(this, EventArgs.Empty);
		}

		public void StoreToModel()
		{
			string elementName = view.ElementName.Text;
			string ipAddress = view.IpAddress.Text;
			string scriptName = view.ScriptName.Text;
			string protocolVersion = view.ProtocolVersions.Selected;

			model.SetModelData(elementName, ipAddress, scriptName, protocolVersion);
			model.CreateElement();
		}
	}
}
