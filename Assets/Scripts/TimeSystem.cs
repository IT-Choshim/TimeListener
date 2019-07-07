using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : ITimeSystem
{
    private static TimeSystem inst;
    public static TimeSystem Inst
    {
        get
        {
            if(inst == null)
            {
                inst = new TimeSystem();
            }
            return inst;
        }
    }

    // 线程锁
    private static readonly string lockObj = "lock";
    private int tid;

    // tid List
    private List<int> tids = new List<int>();

    // tid 将要删除的缓存list
    private List<int> tidCaches = new List<int>();

    // 缓存时间事件列表
    private List<PETimeTake> cacheTimeTakes = new List<PETimeTake>();

    // 时间事件列表
    private List<PETimeTake> timeTakes = new List<PETimeTake>();

    // 帧的总数
    private int FrameCount = 0;

    // 缓存帧事件列表
    private List<PEFrameTake> cacheFrameTakes = new List<PEFrameTake>();

    // 帧事件列表
    private List<PEFrameTake> frameTakes = new List<PEFrameTake>();
    

    // Update is called once per frame
    public void Update()
    {
        CheckTimeTake();

        CheckFrameTake();

        if (tidCaches.Count >= 0)
        {
            for(int i = 0; i < tidCaches.Count; i++)
            {
                int tid = tidCaches[i];
                for(int j = 0; j < tids.Count; j++)
                {
                    if(tid == tids[i])
                    {
                        tids.RemoveAt(j);
                        break;
                    }
                }
            }
            tidCaches.Clear();
        }
    }

    #region TimeTake
    // Update CheckTimeTake
    private void CheckTimeTake()
    {
        for (int i = 0; i < cacheTimeTakes.Count; i++)
        {
            timeTakes.Add(cacheTimeTakes[i]);
        }
        cacheTimeTakes.Clear();

        for (int i = timeTakes.Count - 1; i >= 0; i--)
        {
            PETimeTake take = timeTakes[i];
            if (take.destTime > Time.realtimeSinceStartup * 1000)
            {
                continue;
            }
            Action ac = take.callback;
            // 添加异常捕获机制
            try
            {
                if (ac != null)
                {
                    ac();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Exception:" + e.ToString());
            }

            if (take.count == 1)
            {
                timeTakes.RemoveAt(i);
                tidCaches.Add(take.tid);
                //tids.Remove(take.tid); // 清除tid
            }
            else
            {
                if (take.count != 0)
                {
                    take.count--;
                }
                take.destTime += take.delayTime;
            }
        }

    }


    /// <summary>
    /// 添加时间回调事件
    /// </summary>
    /// <param name="destTime">Destination time.</param>
    /// <param name="callback">Callback.</param>
    /// <param name="count">count = 0表示一直循环.</param>
    public int AddTimeTake(float destTime, float delayTime, Action callback = null ,PETimeUnit unit = PETimeUnit.Millisecond ,int count = 1)
    {
        float time = 0;
        switch (unit)
        {
            case PETimeUnit.Millisecond:
                time = destTime;
                break;
            case PETimeUnit.Second:
                time = destTime * 1000;
                break;
            case PETimeUnit.Minute:
                time = destTime * 1000 * 60;
                break;
            case PETimeUnit.Hour:
                time = destTime * 1000 * 60 * 60;
                break;
            case PETimeUnit.Day:
                time = destTime * 1000 * 60 * 60 * 24;
                break;
            default:
                Debug.Log("Unit Error...");
                break;
        }

        float dest = Time.realtimeSinceStartup * 1000 + time;
        int tid = GetTid();
        PETimeTake take = new PETimeTake
        {
            tid = tid,
            destTime = dest,
            delayTime = delayTime,
            callback = callback,
            count = count,
        };
        cacheTimeTakes.Add(take);

        return tid;
    }


    /// <summary>
    /// 删除时间回调事件
    /// </summary>
    /// <param name="_tid">Tid.</param>
    public bool DelteTimeTake(int _tid)
    {
        bool isExit = false;
        for(int i = 0;i < timeTakes.Count; i++)
        {
            if(timeTakes[i].tid == _tid)
            {
                isExit = true;
                timeTakes.RemoveAt(i);
                break;
            }
        }
        tids.Remove(_tid);
        return isExit;
    }


    /// <summary>
    /// 替换时间回调事件
    /// </summary>
    public bool ReplaceTimeTake(int tid, float destTime, float delayTime, Action callback = null, PETimeUnit unit = PETimeUnit.Millisecond, int count = 1)
    {
        bool isReplace = false;
        float time = 0;
        switch (unit)
        {
            case PETimeUnit.Millisecond:
                time = destTime;
                break;
            case PETimeUnit.Second:
                time = destTime * 1000;
                break;
            case PETimeUnit.Minute:
                time = destTime * 1000 * 60;
                break;
            case PETimeUnit.Hour:
                time = destTime * 1000 * 60 * 60;
                break;
            case PETimeUnit.Day:
                time = destTime * 1000 * 60 * 60 * 24;
                break;
            default:
                Debug.Log("Unit Error...");
                break;
        }

        float dest = Time.realtimeSinceStartup * 1000 + time;
        PETimeTake take = new PETimeTake
        {
            tid = tid,
            destTime = dest,
            delayTime = delayTime,
            callback = callback,
            count = count,
        };
        for(int i = 0; i < timeTakes.Count; i++)
        {
            if(timeTakes[i].tid == tid)
            {
                timeTakes[i] = take;
                isReplace = true;
            }
        }

        return isReplace;
    }
    #endregion


    #region FrameTake
    // Update CheckFrameTake
    private void CheckFrameTake()
    {
        FrameCount++;
        for (int i = 0; i < cacheFrameTakes.Count; i++)
        {
            frameTakes.Add(cacheFrameTakes[i]);
        }
        cacheFrameTakes.Clear();

        for (int i = frameTakes.Count - 1; i >= 0; i--)
        {
            PEFrameTake take = frameTakes[i];
            if (take.destFrame > FrameCount)
            {
                continue;
            }
            Action ac = take.callback;
            // 添加异常捕获机制
            try
            {
                if (ac != null)
                {
                    ac();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Exception:" + e.ToString());
            }

            if (take.count == 1)
            {
                frameTakes.RemoveAt(i);
                tidCaches.Add(take.tid);
            }
            else
            {
                if (take.count != 0)
                {
                    take.count--;
                }
                take.destFrame += take.delayFrame;
            }
        }

    }


    /// <summary>
    /// 添加时间回调事件
    /// </summary>
    /// <param name="destTime">Destination time.</param>
    /// <param name="callback">Callback.</param>
    /// <param name="count">count = 0表示一直循环.</param>
    public int AddFrameTake(int destFrame, int delayFrame, Action callback = null, int count = 1)
    {
        int tid = GetTid();
        PEFrameTake take = new PEFrameTake
        {
            tid = tid,
            destFrame = FrameCount +destFrame,
            delayFrame = delayFrame,
            callback = callback,
            count = count,
        };
        cacheFrameTakes.Add(take);

        return tid;
    }


    /// <summary>
    /// 删除时间回调事件
    /// </summary>
    /// <param name="_tid">Tid.</param>
    public bool DelteFrameTake(int _tid)
    {
        bool isExit = false;
        for (int i = 0; i < frameTakes.Count; i++)
        {
            if (frameTakes[i].tid == _tid)
            {
                isExit = true;
                frameTakes.RemoveAt(i);
                break;
            }
        }
        tids.Remove(_tid);
        return isExit;
    }


    /// <summary>
    /// 替换时间回调事件
    /// </summary>
    public bool ReplaceFrameTake(int tid, int destFrame, int delayFrame, Action callback = null, int count = 1)
    {
        bool isReplace = false;
        PEFrameTake take = new PEFrameTake
        {
            tid = tid,
            destFrame = FrameCount + destFrame,
            delayFrame = delayFrame,
            callback = callback,
            count = count,
        };
        for (int i = 0; i < frameTakes.Count; i++)
        {
            if (frameTakes[i].tid == tid)
            {
                frameTakes[i] = take;
                isReplace = true;
            }
        }

        return isReplace;
    }
    #endregion


    /// <summary>
    /// 获取回调事件tid
    /// </summary>
    /// <returns>The tid.</returns>
    private int GetTid()
    {
        lock (lockObj)
        {
            tid++;

            // 安全代码，以防tid大于int最大值
            if(tid > int.MaxValue)
            {
                tid = 0;
            }
            while (true)
            {
                bool isDrag = false;
                for (int i = 0; i < tids.Count; i++)
                {
                    if (tid == tids[i])
                    {
                        tid++;
                        isDrag = true;
                        break;
                    }
                }
                if (!isDrag)
                {
                    break;
                }
            }
        }
        return tid;
    }
}
