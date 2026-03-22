using UnityEngine;

namespace DLSample.Gameplay.Behaviours
{
    public abstract class GameplayObject : MonoBehaviour
    {
        private bool _isDestroyed = false;

        private void Awake()
        {
            GameplayEntry.Instance.RegisterGameplayObject(this);

            OnInit();
        }
        private void OnDestroy()
        {
            if(!_isDestroyed)
                OnExit();

            _isDestroyed = true;
        }

        public void DoStart() => OnStart();

        /// <summary>
        /// 在这里进行模块创建等自身初始化工作
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// 此时GameplayEntry已完成准备工作,其余模块已完成创建和服务注册，在这里可以安全执行模块管理器注册和访问其他模块等工作
        /// </summary>
        protected virtual void OnStart() { }

        /// <summary>
        /// 在这里做关闭操作
        /// </summary>
        protected virtual void OnExit() { }
    }
}
