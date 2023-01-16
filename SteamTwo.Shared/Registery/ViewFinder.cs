namespace SteamTwo.Shared.Registery
{
    public static class ViewFinder
    {
        private static object? _view;
        public static void SetView(object view)
        {
            _view = view;
        }

        public static object? GetView()
        {
            return _view;
        }
    }
}
