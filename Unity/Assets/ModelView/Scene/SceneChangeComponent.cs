﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET
{
    
    public class SceneChangeComponentUpdateSystem: UpdateSystem<SceneChangeComponent>
    {
        public override void Update(SceneChangeComponent self)
        {
            if (self.loadMapOperation.isDone)
            {
                self.tcs.SetResult();
            }
           
        }
    }
	
    
    public class SceneChangeComponentDestroySystem: DestroySystem<SceneChangeComponent>
    {
        public override void Destroy(SceneChangeComponent self)
        {
            self.loadMapOperation = null;
            self.tcs = null;
        }
    }

    public class SceneChangeComponent: Entity
    {
        public AsyncOperation loadMapOperation;
        public ETTaskCompletionSource tcs;

        public async ETTask ChangeSceneAsync(string sceneName)
        {
            this.tcs = new ETTaskCompletionSource();
            // 加载map
            this.loadMapOperation = SceneManager.LoadSceneAsync(sceneName);
            //this.loadMapOperation.allowSceneActivation = false;
            await this.tcs.Task;
        }

        public int Process
        {
            get
            {
                if (this.loadMapOperation == null)
                {
                    return 0;
                }
                return (int)(this.loadMapOperation.progress * 100);
            }
        }

        public void Finish()
        {
            this.tcs.SetResult();
        }
    }
}