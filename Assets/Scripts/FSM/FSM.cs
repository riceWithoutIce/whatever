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
        public class FSMState
        {
            private T m_tState;
            //存储对应事件跳转
            private Dictionary<T, FSMTranslation> m_dicTranslation = new Dictionary<T, FSMTranslation>();

            #region getset
            public T State
            {
                get { return m_tState; }
                set { m_tState = value; }
            }

            public Dictionary<T, FSMTranslation> DicTranslation
            {
                get { return m_dicTranslation; }
            }

            #endregion

            public FSMState(T tState)
            {
                m_tState = tState;
            }
        }

        //跳转类
        public class FSMTranslation
        {
            private T m_tFromState;
            private T m_tCurrState;
            private T m_tToState;
            //回调函数
            private FSMCallfunc m_callfunc;

            #region getset
            public T FromeState
            {
                get { return m_tFromState; }
                set { m_tFromState = value; }
            }

            public T CurrState
            {
                get { return m_tCurrState; }
                set { m_tCurrState = value; }
            }

            public T ToState
            {
                get { return m_tToState; }
                set { m_tToState = value; }
            }

            public FSMCallfunc Callfunc
            {
                get { return m_callfunc; }
                set { m_callfunc = value; }
            }

            #endregion

            public FSMTranslation(T tFromState, T tCurrState, T tToState, FSMCallfunc callfunc)
            {
                this.m_tFromState = tFromState;
                this.m_tCurrState = tCurrState;
                this.m_tToState = tToState;
                this.m_callfunc = callfunc;
            }
        }

        //当前状态
        private T m_tCurrState;
        private Dictionary<T, FSMState> m_dicState = new Dictionary<T, FSMState>();

        #region getset
        public T CurrState
        {
            get { return m_tCurrState; }
        }

        public Dictionary<T, FSMState> DicState
        {
            get { return m_dicState; }
        }

        #endregion

        //添加状态
        public void AddState(T tState)
        {
            m_dicState[tState] = new FSMState(tState);
        }

        //添加跳转
        public void AddTranslation(T tFromState, T tCurrState, T tToState, FSMCallfunc callfunc)
        {
            m_dicState[tFromState].DicTranslation[tCurrState] = new FSMTranslation(tFromState, tCurrState, tToState, callfunc);
        }

        //启动状态机
        public void Start(T tState)
        {
            m_tCurrState = tState;
        }

        //处理事件
        public void HandleEvent(T tState)
        {
            if (m_tCurrState != null && m_dicState[m_tCurrState].DicTranslation.ContainsKey(tState))
            {
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                FSMTranslation transTmp = m_dicState[m_tCurrState].DicTranslation[tState];
                transTmp.Callfunc();

                m_tCurrState = transTmp.ToState;
                watch.Stop();
            }
        }

        public void Clear()
        {
            foreach (KeyValuePair<T, FSMState> state in m_dicState)
            {
                state.Value.DicTranslation.Clear();
            }
            DicState.Clear();
        }
    }
}
