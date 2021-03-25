namespace Events
{
    public class CanNotAffordUIEvent
    {
        public readonly string Text;

        public CanNotAffordUIEvent(string text) => this.Text = text;
    }
}