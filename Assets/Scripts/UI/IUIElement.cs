namespace UI
{
    public interface IUIElement<in T1>
    {
        public void Show(T1 item);
        public void Hide();
    }
}