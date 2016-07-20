using System.Collections;

namespace ToolsSpace
{
    public class Tools
    {
        #region Singleton
        static private Tools m_instance = null;
        private Tools()
        {

        }

        static public Tools GetInstance()
        {
            if (m_instance == null)
                m_instance = new Tools();
            return m_instance;
        }
        #endregion


    }
}


