namespace UI
{
    public interface IUIElement
    {
        public void Hide();
    }

    public interface IUIElement<in T1> : IUIElement
    {
        public void Show(T1 item);
    }
}