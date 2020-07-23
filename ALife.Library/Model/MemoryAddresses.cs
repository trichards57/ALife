namespace ALife.Model
{
    public enum MemoryAddresses
    {
        MoveUp = 1,
        MoveDown = 2,
        MoveLeft = 3,
        MoveRight = 4,
        TurnLeft = 5,
        TurnRight = 6,
        Speed = 20,
        SpeedForward = 21,
        SpeedRight = 22,
        EyeFirst = 30,
        Eye1 = EyeFirst + 1,
        Eye2 = EyeFirst + 2,
        Eye3 = EyeFirst + 3,
        Eye4 = EyeFirst + 4,
        Eye5 = EyeFirst + 5,
        Eye6 = EyeFirst + 6,
        Eye7 = EyeFirst + 7,
        EyeLast = Eye7,
        MyEyeRefCount = 40,
        FocusBotSpeed = 50,
        FocusBotSpeedForward = 51,
        FocusBotSpeedRight = 52,
        FocusBotDistance = 53,
        FocusBotEyeRefCount = 54,
    }
}