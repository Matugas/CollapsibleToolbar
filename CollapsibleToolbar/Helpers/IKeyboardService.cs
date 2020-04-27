using System;

namespace CollapsibleToolbar.Helpers
{
    public interface IKeyboardService
    {
        event EventHandler KeyboardIsShown;
        event EventHandler KeyboardIsHidden;
    }
}
