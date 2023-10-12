namespace TLN_AS_CreateEXPERTrakElementAndSetMesurementPointsAS_1.ElementCreationPackage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Library.Automation;
	using Skyline.DataMiner.Library.Common;
	using Skyline.DataMiner.Net.DMSState.Agents;
	using Skyline.DataMiner.Net.Helper;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	public class ElementCreationModel
	{
		private readonly IEngine engine;
		private readonly IDms ds;
		private readonly ICollection<IDma> agents;
		private IDma selectedAgent;
		private string elementName;
		private string elementIpAddress;
		private string elementAutomationScript;

		public ElementCreationModel(IEngine engine)
		{
			this.engine = engine;
			ds = engine.GetDms();
			agents = ds.GetAgents();
		}

		public IDma SelectedAgent { get => selectedAgent; set => selectedAgent = value; }

		public void SetModelData(string elementName, string elementIpAddress, string elementAutomationScript)
		{
			this.elementName = elementName;
			this.elementIpAddress = elementIpAddress;
			this.elementAutomationScript = elementAutomationScript;
		}

		public bool CreateElement()
		{
			if (!ValidateElementName() || !ValidateIP())
			{
				return false;
			}

			selectedAgent.CreateElement();

			return true;
		}

		public ICollection<IDma> GetAgents()
		{
			return agents;
		}

		private bool ValidateIP()
		{
			if (elementIpAddress.IsNotNullOrEmpty())
			{
				return ValidateIPv4(elementIpAddress);
			}
			else
			{
				return false;
			}
		}

		private bool ValidateIPv4(string ip)
		{
			string[] nums = ip.Split('.');
			if (nums.Length == 4 && nums.All(x => byte.TryParse(x, out _)))
			{
				return true;
			}

			return false;
		}

		private bool ValidateElementName()
		{
			var elements = ds.GetElements();

			foreach (var element in elements)
			{
				if (element.Name.Equals(elementName))
				{
					return false;
				}
			}

			return true;
		}
	}
}
