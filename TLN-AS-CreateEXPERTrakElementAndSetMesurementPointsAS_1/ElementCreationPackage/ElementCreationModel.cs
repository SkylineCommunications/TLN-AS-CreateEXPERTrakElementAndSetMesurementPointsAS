﻿namespace TLN_AS_CreateEXPERTrakElementAndSetMesurementPointsAS_1.ElementCreationPackage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Net.DMSState.Agents;
	using Skyline.DataMiner.Net.Helper;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	public class ElementCreationModel
	{
		private readonly IEngine engine;
		private readonly IDms ds;
		private readonly IDma elementAgent;
		private readonly int defaultPort = 443;
		private readonly string elementProtocolName = "Viavi Solutions XPERTrak";
		private readonly string defaultScriptsFolder = "Measurements Points Scripts";
		private readonly string defaultViewName = "XPERTrak Monitoring";
		private readonly int rootViewId = -1;
		private string elementProtocolVersion;
		private string elementName;
		private string elementIpAddress;
		private string elementAutomationScript;

		public ElementCreationModel(IEngine engine)
		{
			this.engine = engine;
			ds = engine.GetDms();
			elementAgent = ds.GetAgent(Engine.SLNetRaw.ServerDetails.AgentID);
		}

		public void SetModelData(string elementName, string elementIpAddress, string elementAutomationScript, string version)
		{
			this.elementName = elementName;
			this.elementIpAddress = elementIpAddress;
			this.elementAutomationScript = elementAutomationScript;
			this.elementProtocolVersion = version;
		}

		public List<IDmsProtocol> LoadFromModel()
		{
			var protocols = ds.GetProtocols();
			var protocol = protocols.Where(x => x.Name == elementProtocolName).ToList();
			return protocol;
		}

		public List<string> LoadAvailableScripts()
		{
			var availableScripts = ds.GetScripts().Where(x => x.Folder == defaultScriptsFolder).ToList();
			return availableScripts.Select(x => x.Name).ToList();
		}

		public void CreateElement()
		{
			if (!ValidateElementName())
			{
				engine.ExitFail("Element not created - Element with this name already exists!");
			}

			if (!ValidateIP())
			{
				engine.ExitFail("Element not created - IP not valid!");
			}

			if (!ds.ViewExists(defaultViewName))
			{
				ds.CreateView(new ViewConfiguration(defaultViewName, ds.GetView(rootViewId)));
			}

			ElementConfiguration configuration = CreateElementConfiguration();
			if (ValidateAutomationScript())
			{
				configuration.Views.Add(ds.GetView(defaultViewName));
				elementAgent.CreateElement(configuration);
				SubScriptOptions subScript = engine.PrepareSubScript(elementAutomationScript);
				subScript.StartScript();
				if(subScript.HadError)
				{
					engine.ExitSuccess("Element created! - Error with subscript, check logging for more information!");
				}

				engine.ExitSuccess("Element created!");
			}
			else
			{
				engine.ExitFail("Elemenet created - Automation script not found!");
			}
		}

		private static bool ValidateIPv4(string ip)
		{
			string[] nums = ip.Split('.');
			if (nums.Length == 4 && nums.All(x => byte.TryParse(x, out _)))
			{
				return true;
			}

			return false;
		}

		private ElementConfiguration CreateElementConfiguration()
		{
			IDmsProtocol elementProtocol;

			elementProtocol = ds.GetProtocol(elementProtocolName, elementProtocolVersion);

			if (elementProtocol == null)
			{
				throw new ArgumentException("Protocol not found");
			}

			List<IElementConnection> connection = CreateElementConnection();

			ElementConfiguration config = new ElementConfiguration(ds, elementName, elementProtocol, connection);
			return config;
		}

		private List<IElementConnection> CreateElementConnection()
		{
			ITcp tcp = new Tcp(elementIpAddress, defaultPort);
			HttpConnection httpConnection = new HttpConnection(tcp);
			List<IElementConnection> connection = new List<IElementConnection> { httpConnection };
			return connection;
		}

		private bool ValidateAutomationScript()
		{
			List<IDmsAutomationScript> scripts = ds.GetScripts().ToList();

			foreach (IDmsAutomationScript script in scripts)
			{
				if (script.Name == elementAutomationScript)
				{
					return true;
				}
			}

			return false;
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

		private bool ValidateElementName()
		{
			var elements = ds.GetElements();
			if (!elements.Any(x => x.Name.Equals(elementName)))
			{
				return true;
			}

			return false;
		}
	}
}
