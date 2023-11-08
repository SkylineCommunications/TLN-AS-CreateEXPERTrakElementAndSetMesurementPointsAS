namespace TLN_AS_CreateEXPERTrakElementAndSetMesurementPointsAS_1
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Runtime.Remoting.Channels;
	using System.Text;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;
	using TLN_AS_CreateEXPERTrakElementAndSetMesurementPointsAS_1.ElementCreationPackage;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			InteractiveController controller;
			ElementCreationModel model;
			ElementCreationView view;
			ElementCreationPresenter presenter;

			// DO NOT REMOVE THIS COMMENTED-OUT CODE OR THE SCRIPT WON'T RUN!
			// DataMiner evaluates if the script needs to launch in interactive mode.
			// This is determined by a simple string search looking for "engine.ShowUI" in the source code.
			// However, because of the toolkit NuGet package, this string cannot be found here.
			// So this comment is here as a workaround.
			//// engine.ShowUI();

			try
			{
				controller = new InteractiveController(engine);
				model = new ElementCreationModel(engine);
				view = new ElementCreationView(engine);
				presenter = new ElementCreationPresenter(view, model);

				presenter.Create += (sender, args) =>
				{
					presenter.StoreToModel();
				};
				controller.Run(view);
			}
			catch (Exception ex)
			{
				if (ex.Message != "Automation Script Closed.")
				{
					if (ex.Message.Equals("Element created!"))
					{
						engine.GenerateInformation(ex.Message);
						engine.ExitSuccess(ex.Message);
					}
					else
					{
						engine.GenerateInformation(ex.Message);
						engine.ExitFail(ex.Message);
					}
				}
			}
		}
		}
	}