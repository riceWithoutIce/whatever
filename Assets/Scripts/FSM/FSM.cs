using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class FSM<T> where T : FSM<T>
    {
        //定义函数指针
        public delegate void FSMCallfunc();

        //状态类
        public class FSMStatus
        {
            private T m_tStatus;
            //存储对应事件跳转
            private Dictionary<T, FSMTranslation> m_dicTranslation = new Dictionary<T, FSMTranslation>();

            #region getset
            public T Status
            {
                get { return m_tStatus; }
                set { m_tStatus = value; }
            }

            public Dictionary<T, FSMTranslation> DicTranslation
            {
                get { return m_dicTranslation; }
            }

            #endregion

            public FSMStatus(T tStatus)
            {
                m_tStatus = tStatus;
            }
        }

        //跳转类
        public class FSMTranslation
        {
            private T m_tFromStatus;
            private T m_tCurrStatus;
            private T m_tToStatus;
            //回调函数
            private FSMCallfunc m_callfunc;

            #region getset
            public T FromeStatus
            {
                get { return m_tFromStatus; }
                set { m_tFromStatus = value; }
            }

            public T CurrStatus
            {
                get { return m_tCurrStatus; }
                set { m_tCurrStatus = value; }
            }

            public T ToStatus
            {
                get { return m_tToStatus; }
                set { m_tToStatus = value; }
            }

            public FSMCallfunc Callfunc
            {
                get { return m_callfunc; }
                set { m_callfunc = value; }
            }

            #endregion

            public FSMTranslation(T tFromStatus, T tCurrStatus, T tToStatus, FSMCallfunc callfunc)
            {
                this.m_tFromStatus = tFromStatus;
                this.m_tCurrStatus = tCurrStatus;
                this.m_tToStatus = tToStatus;
                this.m_callfunc = callfunc;
            }
        }

        //当前状态
        private T m_tCurrStatus;
        private Dictionary<T, FSMStatus> m_dicStatus = new Dictionary<T, FSMStatus>();

        #region getset
        public T CurrStaus
        {
            get { return m_tCurrStatus; }
        }

        public Dictionary<T, FSMStatus> DicStatus
        {
            get { return m_dicStatus; }
        }

        #endregion

        //添加状态
        public void AddStatus(T tStatu)
        {
            m_dicStatus[tStatu] = new FSMStatus(tStatu);
        }

        //添加跳转
        public void AddTranslation(T tFromStatus, T tCurrStatus, T tToStatus, FSMCallfunc callfunc)
        {
            m_dicStatus[tFromStatus].DicTranslation[tCurrStatus] = new FSMTranslation(tFromStatus, tCurrStatus, tToStatus, callfunc);
        }

        //启动状态机
        public void Start(T tStatus)
        {
            m_tCurrStatus = tStatus;
        }

        //处理事件
        public void HandleEvent(T tStatus)
        {
            if (m_tCurrStatus != null && m_dicStatus[m_tCurrStatus].DicTranslation.ContainsKey(tStatus))
            {
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                FSMTranslation transTmp = m_dicStatus[m_tCurrStatus].DicTranslation[tStatus];
                transTmp.Callfunc();

                m_tCurrStatus = transTmp.ToStatus;
                watch.Stop();
            }
        }

        public void Clear()
        {
            foreach (KeyValuePair<T, FSMStatus> status in m_dicStatus)
            {
                status.Value.DicTranslation.Clear();
            }
            DicStatus.Clear();
        }
    }
}
