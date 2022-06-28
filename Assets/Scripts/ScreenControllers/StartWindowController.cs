using eggsgd.UiFramework.Examples.Utils;
using eggsgd.UiFramework.Window;

namespace eggsgd.UiFramework.Examples.ScreenControllers
{
    public class StartDemoSignal : ASignal
    {
    }

    public class StartWindowController : AWindowController
    {
        public void UI_Start()
        {
            Signals.Get<StartDemoSignal>().Dispatch();
        }
    }
}