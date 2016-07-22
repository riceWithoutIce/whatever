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
    public enum ePlayerStatus
    {
        eSTATUS_IDLE = 0,
        eSTATUS_ROTATE,
        eSTATUS_JUMP,
        eSTATUS_MOVE,
        eSTATUS_NONE,
    }
}