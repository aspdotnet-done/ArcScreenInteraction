/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02
{
    public class ItemData
    {
        public string Message { get; }
        public Texture2D texture;
        public ItemData(string message)
        {
            Message = message;
        }
        public ItemData(string message, Texture2D texture)
        {
            Message = message;
            this.texture = texture;
        }

    }
}
