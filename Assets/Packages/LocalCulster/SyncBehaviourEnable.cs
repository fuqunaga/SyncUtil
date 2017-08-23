﻿using UnityEngine;

namespace LocalClustering
{
    public class SyncBehaviourEnable : SyncObjectBool<Behaviour>
    {
        protected override bool Get(Behaviour target)
        {
            return target.enabled;
        }

        protected override void Set(Behaviour target, bool value)
        {
            target.enabled = value;
        }
    }


    public abstract class SyncObjectBool<T> : SyncObjectBool
        where T : Object
    {
        public T _target;
        protected string key { get { return _target.name + "/" + _target.GetType().ToString() + "/syncObjectBool"; } }

        protected abstract bool Get(T target);
        protected abstract void Set(T target, bool value);

        void Update()
        {
            var mgr = SyncParamManager.instance;
            if (mgr != null)
            {
                if (LocalCluster.isServer)
                {
                    mgr.UpdateParam(key, Get(_target));
                }

                if (LocalCluster.isSlaver)
                {
                    var obj = mgr.GetParam(key, _mode == Mode.Trigger);
                    if (obj != null)
                    {
                        Set(_target, (bool)obj);
                    }
                }
            }
        }
    }

    public abstract class SyncObjectBool : MonoBehaviour
    {
        public enum Mode
        {
            Sync,
            Trigger
        }

        public Mode _mode;
    }
}