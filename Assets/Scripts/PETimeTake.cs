using System;

/// <summary>
/// 时间定时
/// </summary>
public class PETimeTake
{
    // 全局id，方便取消定时事件
    public int tid;

    // 目标时间
    public double destTime;

    // 事件间隔时间
    public double delayTime;

    // 回调事件
    public Action callback;

    // 回调次数
    public int count;
}


/// <summary>
/// 帧定时
/// </summary>
public class PEFrameTake
{
    // 全局id，方便取消定时事件
    public int tid;

    // 目标帧
    public int destFrame;

    // 事件间隔帧
    public int delayFrame;

    // 回调事件
    public Action callback;

    // 回调次数
    public int count;
}



/// <summary>
/// 传入时间类型
/// </summary>
public enum PETimeUnit
{
    Millisecond,// 毫秒
    Second,// 秒
    Minute,// 分钟
    Hour,// 小时
    Day,// 天
}
