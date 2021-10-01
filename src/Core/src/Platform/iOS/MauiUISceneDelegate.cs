﻿using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace Microsoft.Maui
{
    public class MauiUISceneDelegate : UIWindowSceneDelegate
    {
		WeakReference<IWindow>? _virtualWindow;
		internal IWindow? VirtualWindow
		{
			get
			{
				IWindow? window = null;
				_virtualWindow?.TryGetTarget(out window);
				return window;
			}
			set
			{
				if (value != null)
				{
					if (_virtualWindow == null)
						_virtualWindow = new WeakReference<IWindow>(value);
					else
						_virtualWindow.SetTarget(value);
				}
			}
		}

		public override UIWindow? Window { get; set; }
		
		public override void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
		{
			if (session.Configuration.Name != "__MAUI_DEFAULT_SCENE_CONFIGURATION__")
				return;

			if (Window == null)
			{
				var currentUIAppDelegate = MauiUIApplicationDelegate.Current;

				var dicts = new List<NSDictionary>();

				// Find any userinfo/dictionaries we might pass into the activation state
				if (scene?.UserActivity?.UserInfo != null)
					dicts.Add(scene.UserActivity.UserInfo);
				if (session.UserInfo != null)
					dicts.Add(session.UserInfo);
				if (session.StateRestorationActivity?.UserInfo != null)
					dicts.Add(session.StateRestorationActivity.UserInfo);

				var w = MauiUIApplicationDelegate.Current.CreateNativeWindow(scene as UIWindowScene, dicts.ToArray());
				Window = w.nativeWIndow;
				VirtualWindow = w.virtualWindow;
			}
			
			Window?.MakeKeyAndVisible();
		}

		public override NSUserActivity? GetStateRestorationActivity(UIScene scene)
		{
			var virtualWindow = MauiUIApplicationDelegate.Current.VirtualWindow;

			if (virtualWindow == null)
			{
				Console.WriteLine("VirtualWindow null, no state to save...");
				return null;
			}

			var userActivity = new NSUserActivity(virtualWindow.GetType().FullName!);
			var persistedState = new PersistedState();

			virtualWindow.Backgrounding(persistedState);

			var mutableUserInfo = new NSMutableDictionary();

			foreach (var kvp in persistedState.State)
				mutableUserInfo.SetValueForKey(new NSString(kvp.Value), new NSString(kvp.Key));

			userActivity.UserInfo = mutableUserInfo;

			return userActivity;
		}
	}
}
