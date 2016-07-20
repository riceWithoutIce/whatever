using System.Collections;

namespace GlobalSpace
{
    public class Global
    {
        #region Singleton
        static private Global m_instance = null;
        private Global()
        {

        }

        static public Global GetInstance()
        {
            if (m_instance == null)
                m_instance = new Global();
            return m_instance;
        }
        #endregion
    }
}

