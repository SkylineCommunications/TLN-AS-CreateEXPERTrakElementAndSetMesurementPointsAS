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

		private ICollection<IDma> _agents;

		public ElementCreationPresenter(ElementCreationView elementSelectionView, ElementCreationModel elementModel)
		{
			view = elementSelectionView;
			model = elementModel;

			view.Create.Pressed += OnNextButtonPressed;
			LoadFromModel();
		}

		public event EventHandler<EventArgs> Create;

		public void OnNextButtonPressed(object sender, EventArgs e)
		{
			StoreToModel();
			Create?.Invoke(this, EventArgs.Empty);
		}

		public void StoreToModel()
		{
			string elementName = view.ElementName.Text;
			string ipAddress = view.ScriptName.Text;
			string scriptName = view.ScriptName.Text;

			model.SetModelData(elementName, ipAddress, scriptName);
			model.CreateElement();
		}

		public void LoadFromModel()
		{
			_agents = model.GetAgents();

			view.Agents.SetOptions(_agents.Select(e => e.HostName));
			view.Agents.Selected = $"{model.SelectedAgent}";
		}
	}
}
