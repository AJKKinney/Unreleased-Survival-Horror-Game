namespace Lamplight.Input
{
    public static class InputProvider
    {
        public static PlayerActions playerActions;

        static InputProvider()
        {
            playerActions = new PlayerActions();
            playerActions.Enable();
        }
    }
}
