﻿using Microsoft.Maui.Graphics.Native;

namespace Microsoft.Maui.Handlers
{
	public partial class GraphicsViewHandler : ViewHandler<IGraphicsView, NativeGraphicsView>
	{
		protected override NativeGraphicsView CreateNativeView()
		{
			return new NativeGraphicsView(Context);
		}

		public static void MapDrawable(GraphicsViewHandler handler, IGraphicsView graphicsView)
		{
			handler.NativeView?.UpdateDrawable(graphicsView);
		}

		public static void MapInvalidate(GraphicsViewHandler handler, IGraphicsView graphicsView, object? arg)
		{
			handler.NativeView?.Invalidate();
		}
	}
}