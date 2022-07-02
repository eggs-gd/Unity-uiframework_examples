using eggsgd.Signals;
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
            Signals.SignalBus.Get<StartDemoSignal>().Dispatch();
        }
    }
}