﻿using Emblazon;
using XF = Xamarin.Forms;

namespace Blaxamarin.Framework.Elements.Handlers
{
    public class ContentViewHandler : TemplatedViewHandler
    {
        public ContentViewHandler(EmblazonRenderer renderer, XF.ContentView contentViewControl) : base(renderer, contentViewControl)
        {
            ContentViewControl = contentViewControl ?? throw new System.ArgumentNullException(nameof(contentViewControl));
        }

        public XF.ContentView ContentViewControl { get; }
    }
}
