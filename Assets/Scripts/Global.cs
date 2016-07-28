using System.Collections;
using Framework;

public class Global : SingletonFramework<Global>
{
    #region cons
    private Global()
    {

    }

    #endregion

    //player status
    public enum ePlayerState
    {
        eSTATUS_IDLE = 0,
        eSTATUS_MOVE_FRONT,
        eSTATUS_MOVE_BACK,
        eSTATUS_MOVE_RIGHT,
        eSTATUS_MOVE_LEFT,
        eSTATUS_ROTATE,
        eSTATUS_JUMP,
        eSTATUS_NONE,
    }
}