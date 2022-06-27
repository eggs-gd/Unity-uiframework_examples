using deVoid.Utils;
using eggsgd.UiFramework.Window;

namespace deVoid.UIFramework.Examples
{
    public class StartDemoSignal : ASignal { }

    public class StartWindowController : AWindowController
    {
        public void UI_Start() {
            Signals.Get<StartDemoSignal>().Dispatch();
        }
    }
}
