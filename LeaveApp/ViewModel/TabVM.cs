namespace LeaveApp.ViewModel
{
    public class TabVM
    {
        public Tab ActiveTab { get; set; }
    }

    public enum Tab
    {
        Employee,
        Department,
        Level
    }
}
